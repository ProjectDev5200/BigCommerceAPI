# TTPD Server - FinditParts API Integration
## Technical Architecture Documentation

---

## 1. System Overview

**TTPD Server** (Avrhil.BigCommerce.API) is a .NET 9.0 middleware application that integrates BigCommerce e-commerce platform with FinditParts automotive parts supplier API, enabling automated parts catalog synchronization, inventory management, and order processing.

---

## 2. Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                        TTPD Server (.NET 9.0)                   │
│                   (Avrhil.BigCommerce.API)                      │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │              API Gateway Layer                            │  │
│  │  - Rate Limiting Middleware                               │  │
│  │  - Request Throttling                                     │  │
│  │  - Authentication/Authorization                           │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          ↓                                       │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │              Service Layer                                │  │
│  │  ┌────────────────────┐  ┌──────────────────────────┐    │  │
│  │  │ FinditPartsApiClient│  │ BigCommerceApiClient     │    │  │
│  │  │ - SearchParts       │  │ - GetProducts            │    │  │
│  │  │ - GetPartById       │  │ - CreateProduct          │    │  │
│  │  │ - CreateOrder       │  │ - UpdateInventory        │    │  │
│  │  │ - GetInventory      │  │ - ProcessOrders          │    │  │
│  │  └────────────────────┘  └──────────────────────────┘    │  │
│  │                                                              │  │
│  │  ┌────────────────────┐  ┌──────────────────────────┐    │  │
│  │  │ MappingHandler      │  │ CsvHandler               │    │  │
│  │  │ - ProductMapper     │  │ - Import/Export          │    │  │
│  │  └────────────────────┘  └──────────────────────────┘    │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          ↓                                       │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │              Data Models Layer                            │  │
│  │  - FinditPartsModels  - BigCommerceModels                │  │
│  │  - OAuthModels        - ProductMapper                    │  │
│  └──────────────────────────────────────────────────────────┘  │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
                    ↓                           ↓
    ┌───────────────────────────┐   ┌──────────────────────────┐
    │   FinditParts API         │   │   BigCommerce API        │
    │   (External Service)      │   │   (External Service)     │
    │                           │   │                          │
    │   - Parts Catalog         │   │   - Product Management   │
    │   - Inventory             │   │   - Order Processing     │
    │   - Pricing               │   │   - OAuth 2.0            │
    │   - Orders                │   │                          │
    └───────────────────────────┘   └──────────────────────────┘
```

---

## 3. Component Architecture

### 3.1 Core Components

| Component | Responsibility | Technology |
|-----------|---------------|------------|
| **FinditPartsApiClient** | Handles all FinditParts API communication | HttpClient, JSON |
| **BigCommerceApiClient** | Manages BigCommerce API interactions | HttpClient, OAuth 2.0 |
| **BigCommerceOAuthService** | OAuth authentication flow | OAuth 2.0 |
| **ProductMapper** | Maps FinditParts data to BigCommerce format | C# LINQ |
| **MappingHandler** | Business logic for data transformation | C# |
| **CsvHandler** | Bulk import/export operations | CSV Processing |

### 3.2 Data Flow

```
FinditParts API → FinditPartsApiClient → MappingHandler → ProductMapper 
                                              ↓
                                    BigCommerceApiClient → BigCommerce Store
```

---

## 4. API Integration Details

### 4.1 FinditParts API Endpoints

| Endpoint | Method | Purpose | Rate Limit |
|----------|--------|---------|------------|
| `/v1/parts/search` | GET | Search parts by criteria | TBD |
| `/v1/parts/{id}` | GET | Get part details | TBD |
| `/v1/parts/{id}/price` | GET | Get current pricing | TBD |
| `/v1/parts/{id}/inventory` | GET | Check stock levels | TBD |
| `/v1/vehicles` | GET | Vehicle compatibility | TBD |
| `/v1/orders` | POST | Create order | TBD |
| `/v1/orders/{id}` | GET | Order status | TBD |

### 4.2 Authentication

- **FinditParts**: API Key (X-API-Key header)
- **BigCommerce**: OAuth 2.0 with access tokens

---

## 5. Environment Configuration

### 5.1 Test Environment

```json
{
  "FinditParts": {
    "BaseUrl": "https://api-test.finditparts.com/v1",
    "ApiKey": "<TEST_API_KEY>",
    "Timeout": 30,
    "RetryAttempts": 3
  },
  "BigCommerce": {
    "StoreHash": "<TEST_STORE_HASH>",
    "ClientId": "<TEST_CLIENT_ID>",
    "ClientSecret": "<TEST_CLIENT_SECRET>",
    "AccessToken": "<TEST_ACCESS_TOKEN>"
  },
  "RateLimiting": {
    "RequestsPerSecond": 2,
    "RequestsPerMinute": 100,
    "BurstSize": 5
  }
}
```

### 5.2 Production Environment

```json
{
  "FinditParts": {
    "BaseUrl": "https://api.finditparts.com/v1",
    "ApiKey": "<PROD_API_KEY>",
    "Timeout": 30,
    "RetryAttempts": 3
  },
  "BigCommerce": {
    "StoreHash": "<PROD_STORE_HASH>",
    "ClientId": "<PROD_CLIENT_ID>",
    "ClientSecret": "<PROD_CLIENT_SECRET>",
    "AccessToken": "<PROD_ACCESS_TOKEN>"
  },
  "RateLimiting": {
    "RequestsPerSecond": 5,
    "RequestsPerMinute": 250,
    "BurstSize": 10
  }
}
```

---

## 6. Rate Limiting & Throttling Strategy

### 6.1 Implementation Approach

```
┌─────────────────────────────────────────────────────────┐
│           Rate Limiting Architecture                    │
├─────────────────────────────────────────────────────────┤
│                                                          │
│  Request → Token Bucket → Retry Queue → API Call       │
│              Algorithm                                   │
│                                                          │
│  ┌──────────────────────────────────────────────┐      │
│  │  Token Bucket Parameters:                    │      │
│  │  - Capacity: 100 tokens                      │      │
│  │  - Refill Rate: 5 tokens/second              │      │
│  │  - Burst Allowance: 10 tokens                │      │
│  └──────────────────────────────────────────────┘      │
│                                                          │
│  ┌──────────────────────────────────────────────┐      │
│  │  Exponential Backoff:                        │      │
│  │  - Initial Delay: 1 second                   │      │
│  │  - Max Delay: 60 seconds                     │      │
│  │  - Multiplier: 2x                            │      │
│  └──────────────────────────────────────────────┘      │
│                                                          │
└─────────────────────────────────────────────────────────┘
```

### 6.2 Rate Limit Error Handling

| HTTP Status | Error Type | Retry Strategy | Action |
|-------------|-----------|----------------|--------|
| **429** | Too Many Requests | Exponential backoff | Wait & retry (max 3 attempts) |
| **503** | Service Unavailable | Linear backoff | Wait 5s, retry (max 5 attempts) |
| **504** | Gateway Timeout | Immediate retry | Retry once, then fail |
| **401** | Unauthorized | No retry | Refresh token & retry |

### 6.3 Throttling Configuration

```csharp
// Per-endpoint throttling
FinditParts API:
  - Search: 10 req/sec
  - Details: 20 req/sec
  - Inventory: 15 req/sec
  - Orders: 5 req/sec

BigCommerce API:
  - Products: 20 req/sec
  - Orders: 10 req/sec
  - Catalog: 15 req/sec
```

---

## 7. Critical Questions for Test & Production

### 7.1 FinditParts API Questions

#### Authentication & Security
1. What is the API key rotation policy?
2. Are there separate API keys for test and production?
3. Is IP whitelisting required?
4. What SSL/TLS versions are supported?
5. Are there webhook capabilities for real-time updates?

#### Rate Limits & Throttling
6. What are the exact rate limits per endpoint?
7. Is rate limiting per API key or per IP?
8. What is the time window for rate limits (per second/minute/hour)?
9. Are there different rate limits for test vs production?
10. What headers indicate remaining rate limit quota?
11. Is there a burst allowance for short spikes?
12. Are rate limits shared across all endpoints or per-endpoint?

#### Data & Performance
13. What is the maximum page size for search results?
14. Is pagination supported? What is the mechanism?
15. What is the average response time SLA?
16. What is the API uptime guarantee?
17. Are there scheduled maintenance windows?
18. What is the data refresh frequency for inventory?

#### Error Handling
19. What error codes and messages are returned?
20. Is there a retry-after header in 429 responses?
21. What is the timeout recommendation?
22. Are there circuit breaker requirements?

#### Business Logic
23. How are price updates communicated?
24. What is the inventory reservation mechanism?
25. How long are inventory holds valid?
26. What is the order confirmation workflow?
27. Are partial shipments supported?
28. What is the return/cancellation policy?

### 7.2 Infrastructure Questions

#### Test Environment
29. What is the test environment URL?
30. Is test data automatically reset?
31. Are there test credit cards/payment methods?
32. Can we create unlimited test orders?
33. What monitoring is available in test?

#### Production Environment
34. What is the production cutover process?
35. What monitoring and alerting is required?
36. What are the backup and disaster recovery procedures?
37. What logging level is recommended?
38. Are there compliance requirements (PCI, GDPR)?
39. What is the incident response procedure?
40. What are the SLA commitments?

#### Scalability
41. What is the expected transaction volume?
42. Are there peak traffic periods?
43. What horizontal scaling strategy is recommended?
44. Is caching recommended? What TTL?
45. Should we implement a queue for async processing?

---

## 8. Error Handling Matrix

| Scenario | Detection | Response | Logging |
|----------|-----------|----------|---------|
| Rate limit exceeded | HTTP 429 | Exponential backoff + retry | WARN |
| API timeout | No response in 30s | Retry with circuit breaker | ERROR |
| Invalid API key | HTTP 401 | Alert + stop processing | CRITICAL |
| Part not found | HTTP 404 | Skip + continue | INFO |
| Inventory mismatch | Business logic | Sync + notify | WARN |
| Order creation failure | HTTP 500 | Retry 3x + manual review | ERROR |

---

## 9. Monitoring & Observability

### 9.1 Key Metrics

```
Performance Metrics:
- API response time (p50, p95, p99)
- Request success rate
- Rate limit hit frequency
- Retry attempt count

Business Metrics:
- Parts synced per hour
- Inventory sync accuracy
- Order processing time
- Failed transaction count

System Metrics:
- CPU/Memory usage
- Network latency
- Error rate by endpoint
- Queue depth (if async)
```

### 9.2 Alerting Thresholds

| Metric | Warning | Critical |
|--------|---------|----------|
| Error Rate | > 5% | > 10% |
| Response Time | > 2s | > 5s |
| Rate Limit Hits | > 10/min | > 50/min |
| Failed Orders | > 1 | > 5 |

---

## 10. Deployment Checklist

### Pre-Production
- [ ] All API credentials configured
- [ ] Rate limiting tested with load tests
- [ ] Error handling verified for all scenarios
- [ ] Logging configured and tested
- [ ] Monitoring dashboards created
- [ ] Alerting rules configured
- [ ] Backup and rollback plan documented
- [ ] Security scan completed
- [ ] Performance baseline established

### Production
- [ ] Blue-green deployment strategy
- [ ] Gradual traffic ramp-up (10% → 50% → 100%)
- [ ] Real-time monitoring active
- [ ] On-call rotation established
- [ ] Incident response runbook ready
- [ ] Rollback procedure tested

---

## 11. Technology Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| Runtime | .NET | 9.0 |
| Language | C# | 12.0 |
| HTTP Client | HttpClient | Built-in |
| JSON | Newtonsoft.Json | 13.0.4 |
| Authentication | OAuth 2.0 | - |
| Hosting | TBD | - |

---

## 12. Security Considerations

1. **API Key Management**: Store in Azure Key Vault or AWS Secrets Manager
2. **Token Refresh**: Implement automatic OAuth token refresh
3. **HTTPS Only**: Enforce TLS 1.2+ for all communications
4. **Input Validation**: Sanitize all user inputs
5. **Audit Logging**: Log all API calls with timestamps
6. **PII Protection**: Mask sensitive data in logs

---

## Document Version
- **Version**: 1.0
- **Last Updated**: 2024
- **Author**: TTPD Development Team
