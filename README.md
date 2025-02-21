# Documentation

This project is a Web API built using .NET 8 that provides CRUD operations for managing contacts, companies, and countries. It follows the Onion Architecture approach with Domain-Driven Design (DDD) and integrates with a SQL database using Entity Framework Core (EF Core) with automatic migrations.

**Note**: Although it is recommended to use MinimalApi, I used traditional Controllers for personal convenience.

## Features

- Written in C# using .NET 8
- CRUD API operations for each table (Company, Country, Contact).
- `GetAllContacts()` API returns each contact with the company that is working and the country name.
- `FilterContacts(int countryId, int companyId)` API that filters Contacts by `countryId` and/or `companyId`.
- Swagger for the APU UI.
- Implemented Factory Design Pattern for service creation
- `GetCompanyStatisticsByCountryId(int id)` API that displays all the companies that are related to the given country and the number of contacts that are connected to each of the companies
- Use of lambda expressions (mainly in `FilterContacts()` and `GetCompanyStatisticsByCountryId()`)
- Logging and Error Handling

## Testing
- To run unit tests, execute the following command in the root directory: `dotnet test`
    
