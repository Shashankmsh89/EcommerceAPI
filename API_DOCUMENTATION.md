# EcommerceAPI – API Documentation

## Authentication
- `POST /api/Auth/register` — register customer
- `POST /api/Auth/login` — login; returns access/refresh tokens
- `POST /api/v1/Auth/refresh` — rotate refresh token

## Products
- `GET /api/v1/Product`
- `GET /api/v1/Product/{id}`
- `POST /api/v1/Product`
- `PUT /api/v1/Product/{id}`
- `DELETE /api/v1/Product/{id}`
- `POST /api/v1/Product/bulk`

Product GET supports `search`, `categoryId`, `brandId`, `minPrice`, `maxPrice`, `minRating`, `sortBy`, `sortOrder`, `page`, `pageSize`.

## Categories
- `GET /api/Category`
- `GET /api/Category/{id}`

## Prices
- `GET /api/ProductPrice`
- `GET /api/ProductPrice/{id}`

## Availability
- `GET /api/ProductAvailability/{productId}`
- `GET /api/ProductAvailability/validate?productIds=1,3,5,8`

## Inventory
- `PUT /api/Inventory`
- `PUT /api/Inventory/bulk`
Admin authorization required.

## Cart
Customer authorization required.
- `GET /api/v1/Cart/{customerId}`
- `GET /api/v1/Cart/{customerId}/subtotal`
- `GET /api/v1/Cart/{customerId}/count`
- `POST /api/v1/Cart`
- `PUT /api/v1/Cart/{customerId}/{productId}`
- `DELETE /api/v1/Cart/{customerId}/{productId}`
- `DELETE /api/v1/Cart/{customerId}/clear`

## Checkout
- `GET /api/v1/Checkout/shipping-methods`
- `GET /api/v1/Checkout/summary?shippingMethodId={shippingMethodId}`
- `POST /api/v1/Checkout`

Checkout validates shipping data, cart, quantities, prices, inventory and totals. Customer identity comes from JWT.

## Orders
- `GET /api/v1/Order/{orderId}`
- `GET /api/v1/Order/{orderId}/items`

## Order History
- `GET /api/OrderHistory/{customerId}?page=1&pageSize=10`
- `GET /api/OrderHistory/{customerId}?orderStatus=Pending&page=1&pageSize=10`
- `POST /api/OrderHistory/reorder`

## Addresses
- `GET /api/v1/Address`
- `GET /api/v1/Address/{id}`
- `POST /api/v1/Address`
- `PUT /api/v1/Address/{id}`
- `DELETE /api/v1/Address/{id}`

## Product Images
- `POST /api/v1/Product/{productId}/image` — multipart form-data field `file`
- `GET /api/v1/Product/{productId}/image`

## Cross-cutting
Rate limit: 100 requests/minute/user or IP, returning 429 when exceeded.

Product caching supports invalidation and ETags/conditional GET.

Errors use ProblemDetails-style responses.

Order tracking statuses: `Processing`, `Shipped`, `Delivered`, `Cancelled`.

Protected requests use:
`Authorization: Bearer <access-token>`
