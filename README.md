# Avrhil BigCommerce API Client

## Overview
Centralized API client for BigCommerce API integration following Clean Architecture principles.

## Setup

### Prerequisites
- .NET 8 SDK
- BigCommerce Store Hash
- BigCommerce Access Token

### Configuration
Update `Program.cs` with your credentials:
```csharp
var storeHash = "your_store_hash";
var accessToken = "your_access_token";
```

### Run
```bash
cd Avrhil.BigCommerce.API
dotnet run
```

## Usage

### Initialize Client
```csharp
var apiClient = new BigCommerceApiClient(storeHash, accessToken);
```

### GET Request
```csharp
var products = await apiClient.GetAsync<ProductsResponse>("catalog/products");
var product = await apiClient.GetAsync<ProductResponse>("catalog/products/123");
```

### POST Request
```csharp
var newProduct = new Product { Name = "Test", Price = 29.99m };
var created = await apiClient.PostAsync<ProductResponse>("catalog/products", newProduct);
```

### PUT Request
```csharp
var updated = await apiClient.PutAsync<ProductResponse>("catalog/products/123", product);
```

### DELETE Request
```csharp
var deleted = await apiClient.DeleteAsync("catalog/products/123");
```

## API Endpoints

### Products
- `GET catalog/products` - List products
- `GET catalog/products/{id}` - Get product
- `POST catalog/products` - Create product
- `PUT catalog/products/{id}` - Update product
- `DELETE catalog/products/{id}` - Delete product

### Categories
- `GET catalog/categories`
- `POST catalog/categories`

### Orders
- `GET orders`
- `GET orders/{id}`

### Customers
- `GET customers`
- `POST customers`

## Architecture

```
Avrhil.BigCommerce.API/
├── Services/
│   └── BigCommerceApiClient.cs    # Centralized API client
├── Models/
│   └── BigCommerceModels.cs       # DTOs
└── Program.cs                      # Usage examples
```

## Features
- Centralized HTTP client
- Async/await pattern
- Generic type support
- Error handling
- JSON serialization
