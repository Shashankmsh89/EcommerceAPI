# EcommerceAPI – Assignment 4

ASP.NET Core e-commerce REST API using SQL Server stored procedures and ADO.NET.

## Assignment 4
- Customer, Address and Order Tracking entities
- Cross-field/business validation
- JWT authentication + refresh tokens
- Claims: `CanManageProducts`, `CanManageOrders`
- Product caching, cache invalidation and ETags
- Rate limiting: 100 requests/minute/user or IP; HTTP 429
- Bulk product creation, bulk inventory update and bulk availability validation
- Search by name/description/category/brand with combined filters
- Order tracking: Processing, Shipped, Delivered, Cancelled
- Reorder
- Postman + Newman automation

## Technology
ASP.NET Core, C#, SQL Server, ADO.NET, Stored Procedures, JWT, BCrypt, AutoMapper, API Versioning, Postman/Newman.

## Base URL
`https://localhost:7062`

## Main endpoints
| Area | Endpoint |
|---|---|
| Auth | `POST /api/Auth/login` |
| Products | `GET /api/v1/Product` |
| Categories | `GET /api/Category` |
| Prices | `GET /api/ProductPrice` |
| Availability | `GET /api/ProductAvailability/1` |
| Inventory | `PUT /api/Inventory` |
| Cart | `GET /api/v1/Cart/{customerId}` |
| Checkout | `POST /api/v1/Checkout` |
| Orders | `GET /api/v1/Order/{orderId}` |
| Order History | `GET /api/OrderHistory/{customerId}` |
| Address | `GET /api/v1/Address` |
| Images | `POST /api/v1/Product/{productId}/image` |

## Newman
Install:
```bash
npm install -g newman newman-reporter-htmlextra
```

Run final collection:
```bat
newman run "EcommerceAPI_Assignment4_Newman_FINAL_BOOM_v3.postman_collection.json" -r cli,htmlextra -k --env-var "adminEmail=YOUR_ADMIN_EMAIL" --env-var "adminPassword=YOUR_ADMIN_PASSWORD" --reporter-htmlextra-export "NewmanReport.html"
```

Final verified execution: **49 requests, 49 assertions, 0 failures**.

## Architecture
Controllers → Services/Repositories → SQL Stored Procedures → SQL Server.

## Deliverables
GitHub repository, Postman collection, Newman report, API documentation and README.
