# Projects

API
Exposes two endpoints:
* /getProductDetails
* /getProductListing
Testable via OpenAI/Swagger, use http://localhost:[port]/swagger/index.html

Catalog
Updates a catalog DTO from JSON data. Exposes that data.

CatalogRepo
Mock repository that serves up the JSON data from samples files. Some implementation of building a proper URL based on version.

Catalog.Tests
Various tests for Catalog and, in part, CatalogRepo.

Shared
Shared DTOSs.
