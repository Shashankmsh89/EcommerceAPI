# Newman Verification – Assignment 4

Final verified run:

- Collection: `EcommerceAPI_Assignment4_Newman_FINAL_BOOM_v3.postman_collection.json`
- Base URL: `https://localhost:7062`
- Iterations: 1
- Requests: 49
- Test scripts: 49
- Assertions: 49
- Failed assertions: 0
- Duration: 13.9 seconds

The verified run covered authentication/refresh, admin authorization, products, search/filtering, bulk products, categories, prices, availability, inventory, cart, checkout/payment, orders, order items, order history/reorder, addresses, cart cleanup and product image upload/download.

## Generate the HTML submission report

```bat
npm install -g newman newman-reporter-htmlextra
newman run "EcommerceAPI_Assignment4_Newman_FINAL_BOOM_v3.postman_collection.json" -r cli,htmlextra -k --env-var "adminEmail=YOUR_ADMIN_EMAIL" --env-var "adminPassword=YOUR_ADMIN_PASSWORD" --reporter-htmlextra-export "NewmanReport.html"
```

Keep the generated `NewmanReport.html` with the submission deliverables.
