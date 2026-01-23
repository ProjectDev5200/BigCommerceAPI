# TTPD Server Architecture - Visual Diagrams

## 1. High-Level System Architecture

```mermaid
graph TB
    subgraph "External Systems"
        FP[FinditParts API<br/>Parts Supplier]
        BC[BigCommerce API<br/>E-commerce Platform]
    end
    
    subgraph "TTPD Server - Avrhil.BigCommerce.API"
        subgraph "API Gateway Layer"
            RL[Rate Limiter]
            AUTH[Authentication]
            THROTTLE[Throttling Manager]
        end
        
        subgraph "Service Layer"
            FPC[FinditPartsApiClient]
            BCC[BigCommerceApiClient]
            OAUTH[BigCommerceOAuthService]
            MAP[MappingHandler]
            CSV[CsvHandler]
        end
        
        subgraph "Data Layer"
            MODELS[Data Models<br/>FinditParts & BigCommerce]
            MAPPER[ProductMapper]
        end
    end
    
    FP -->|API Key Auth| RL
    RL --> THROTTLE
    THROTTLE --> FPC
    FPC --> MAP
    
    BC -->|OAuth 2.0| AUTH
    AUTH --> BCC
    BCC --> MAP
    
    MAP --> MAPPER
    MAPPER --> MODELS
    
    CSV -.->|Bulk Operations| FPC
    CSV -.->|Bulk Operations| BCC
    
    OAUTH -.->|Token Management| BCC
```

## 2. Data Flow Architecture

```mermaid
sequenceDiagram
    participant User
    participant TTPD as TTPD Server
    participant RL as Rate Limiter
    participant FP as FinditParts API
    participant Mapper as Product Mapper
    participant BC as BigCommerce API
    
    User->>TTPD: Request Part Sync
    TTPD->>RL: Check Rate Limit
    
    alt Rate Limit OK
        RL->>FP: Search Parts
        FP-->>RL: Parts Data
        RL->>TTPD: Return Data
        TTPD->>Mapper: Transform Data
        Mapper->>BC: Create/Update Product
        BC-->>TTPD: Success Response
        TTPD-->>User: Sync Complete
    else Rate Limit Exceeded
        RL-->>TTPD: 429 Too Many Requests
        TTPD->>TTPD: Exponential Backoff
        TTPD->>RL: Retry Request
    end
```

## 3. Rate Limiting Flow

```mermaid
flowchart TD
    START([API Request]) --> CHECK{Check Token<br/>Bucket}
    
    CHECK -->|Tokens Available| CONSUME[Consume Token]
    CHECK -->|No Tokens| QUEUE[Add to Retry Queue]
    
    CONSUME --> CALL[Make API Call]
    CALL --> RESPONSE{Response<br/>Status}
    
    RESPONSE -->|200 OK| SUCCESS([Success])
    RESPONSE -->|429 Rate Limit| BACKOFF[Exponential Backoff]
    RESPONSE -->|5xx Error| LINEAR[Linear Backoff]
    RESPONSE -->|4xx Error| FAIL([Fail])
    
    BACKOFF --> RETRY1{Retry Count<br/>< 3?}
    RETRY1 -->|Yes| WAIT1[Wait 2^n seconds]
    RETRY1 -->|No| FAIL
    WAIT1 --> QUEUE
    
    LINEAR --> RETRY2{Retry Count<br/>< 5?}
    RETRY2 -->|Yes| WAIT2[Wait 5 seconds]
    RETRY2 -->|No| FAIL
    WAIT2 --> QUEUE
    
    QUEUE --> REFILL{Token<br/>Refilled?}
    REFILL -->|Yes| CONSUME
    REFILL -->|No| WAIT3[Wait]
    WAIT3 --> REFILL
```

## 4. Component Interaction Diagram

```mermaid
graph LR
    subgraph "Client Layer"
        CLI[Console Application]
    end
    
    subgraph "TTPD Server Core"
        direction TB
        
        subgraph "Services"
            FPS[FinditPartsApiClient<br/>- SearchParts<br/>- GetPartById<br/>- GetInventory<br/>- CreateOrder]
            BCS[BigCommerceApiClient<br/>- GetProducts<br/>- CreateProduct<br/>- UpdateInventory]
            OAS[OAuthService<br/>- GetAuthUrl<br/>- ExchangeToken]
        end
        
        subgraph "Handlers"
            MH[MappingHandler<br/>Data Transformation]
            CH[CsvHandler<br/>Bulk Import/Export]
            PM[ProductMapper<br/>Field Mapping]
        end
    end
    
    CLI --> FPS
    CLI --> BCS
    CLI --> OAS
    
    FPS --> MH
    BCS --> MH
    MH --> PM
    
    CH --> FPS
    CH --> BCS
```

## 5. Error Handling Flow

```mermaid
stateDiagram-v2
    [*] --> RequestSent
    
    RequestSent --> Success: 200 OK
    RequestSent --> RateLimit: 429
    RequestSent --> ServerError: 5xx
    RequestSent --> ClientError: 4xx
    RequestSent --> Timeout: No Response
    
    Success --> [*]
    
    RateLimit --> ExponentialBackoff
    ExponentialBackoff --> CheckRetryCount
    CheckRetryCount --> RetryRequest: Count < 3
    CheckRetryCount --> LogAndFail: Count >= 3
    
    ServerError --> LinearBackoff
    LinearBackoff --> CheckRetryCount2
    CheckRetryCount2 --> RetryRequest: Count < 5
    CheckRetryCount2 --> LogAndFail: Count >= 5
    
    ClientError --> LogAndFail
    
    Timeout --> CircuitBreaker
    CircuitBreaker --> RetryOnce
    RetryOnce --> Success: OK
    RetryOnce --> LogAndFail: Failed
    
    RetryRequest --> RequestSent
    LogAndFail --> [*]
```

## 6. OAuth Authentication Flow

```mermaid
sequenceDiagram
    participant User
    participant TTPD as TTPD Server
    participant BC as BigCommerce
    
    User->>TTPD: Start OAuth Flow
    TTPD->>TTPD: Generate Auth URL
    TTPD-->>User: Display Auth URL
    
    User->>BC: Visit Auth URL
    BC-->>User: Login & Authorize
    BC->>TTPD: Redirect with Code
    
    TTPD->>BC: Exchange Code for Token
    BC-->>TTPD: Access Token + Store Hash
    
    TTPD->>TTPD: Store Token
    TTPD->>BC: API Request with Token
    BC-->>TTPD: API Response
    TTPD-->>User: Operation Complete
```

## 7. Deployment Architecture

```mermaid
graph TB
    subgraph "Production Environment"
        LB[Load Balancer]
        
        subgraph "TTPD Server Cluster"
            TTPD1[TTPD Instance 1]
            TTPD2[TTPD Instance 2]
            TTPD3[TTPD Instance 3]
        end
        
        subgraph "Shared Services"
            CACHE[Redis Cache<br/>Rate Limit State]
            QUEUE[Message Queue<br/>Async Processing]
            LOGS[Centralized Logging]
            METRICS[Metrics & Monitoring]
        end
    end
    
    subgraph "External APIs"
        FP[FinditParts API]
        BC[BigCommerce API]
    end
    
    LB --> TTPD1
    LB --> TTPD2
    LB --> TTPD3
    
    TTPD1 --> CACHE
    TTPD2 --> CACHE
    TTPD3 --> CACHE
    
    TTPD1 --> QUEUE
    TTPD2 --> QUEUE
    TTPD3 --> QUEUE
    
    TTPD1 --> LOGS
    TTPD2 --> LOGS
    TTPD3 --> LOGS
    
    TTPD1 --> METRICS
    TTPD2 --> METRICS
    TTPD3 --> METRICS
    
    TTPD1 --> FP
    TTPD2 --> FP
    TTPD3 --> FP
    
    TTPD1 --> BC
    TTPD2 --> BC
    TTPD3 --> BC
```

## 8. Monitoring Dashboard Layout

```mermaid
graph TD
    subgraph "Monitoring Dashboard"
        subgraph "API Health"
            FPH[FinditParts API<br/>Response Time: 150ms<br/>Success Rate: 99.5%]
            BCH[BigCommerce API<br/>Response Time: 200ms<br/>Success Rate: 99.8%]
        end
        
        subgraph "Rate Limiting"
            RLM[Rate Limit Metrics<br/>Hits/Min: 45<br/>Throttled: 2<br/>Queue Depth: 5]
        end
        
        subgraph "Business Metrics"
            SYNC[Parts Synced: 1,250/hr]
            ORD[Orders Processed: 45/hr]
            ERR[Error Rate: 0.5%]
        end
        
        subgraph "System Health"
            CPU[CPU: 45%]
            MEM[Memory: 2.1GB]
            NET[Network: 15Mbps]
        end
        
        subgraph "Alerts"
            ALERT1[⚠️ Rate limit approaching]
            ALERT2[✅ All systems operational]
        end
    end
```

## 9. Data Transformation Pipeline

```mermaid
flowchart LR
    FP_DATA[FinditParts<br/>Part Data] --> VALIDATE{Validate<br/>Data}
    
    VALIDATE -->|Valid| TRANSFORM[Transform Fields]
    VALIDATE -->|Invalid| LOG_ERROR[Log Error]
    
    TRANSFORM --> MAP_FIELDS[Map Fields:<br/>PartNumber → SKU<br/>Description → Name<br/>Price → Price<br/>Quantity → Inventory]
    
    MAP_FIELDS --> ENRICH[Enrich Data:<br/>- Add Categories<br/>- Format Images<br/>- Set Metadata]
    
    ENRICH --> BC_FORMAT[BigCommerce<br/>Product Format]
    
    BC_FORMAT --> SEND[Send to<br/>BigCommerce API]
    
    LOG_ERROR --> RETRY_QUEUE[Add to<br/>Retry Queue]
```

## 10. Security Architecture

```mermaid
graph TB
    subgraph "Security Layers"
        subgraph "Network Security"
            HTTPS[HTTPS/TLS 1.2+]
            FW[Firewall Rules]
            IP[IP Whitelisting]
        end
        
        subgraph "Authentication"
            APIKEY[API Key Management]
            OAUTH[OAuth 2.0 Tokens]
            VAULT[Secret Vault<br/>Azure Key Vault/AWS Secrets]
        end
        
        subgraph "Authorization"
            RBAC[Role-Based Access]
            SCOPE[API Scopes]
        end
        
        subgraph "Data Protection"
            ENCRYPT[Data Encryption]
            MASK[PII Masking in Logs]
            AUDIT[Audit Logging]
        end
    end
    
    HTTPS --> APIKEY
    HTTPS --> OAUTH
    FW --> IP
    
    APIKEY --> VAULT
    OAUTH --> VAULT
    
    VAULT --> RBAC
    RBAC --> SCOPE
    
    SCOPE --> ENCRYPT
    ENCRYPT --> MASK
    MASK --> AUDIT
```

---

## Diagram Legend

- **Solid Lines**: Synchronous communication
- **Dashed Lines**: Asynchronous communication
- **Rectangles**: Services/Components
- **Diamonds**: Decision points
- **Circles**: Start/End points
- **Subgraphs**: Logical groupings

---

## How to View These Diagrams

These diagrams use Mermaid syntax and can be viewed in:
1. **GitHub/GitLab**: Automatically rendered
2. **VS Code**: Install "Markdown Preview Mermaid Support" extension
3. **Online**: https://mermaid.live/
4. **Documentation Tools**: Confluence, Notion, etc.
