# SampleRestAPI
Sample Rest API with Entity Framework Core, In-Memory Database, Mocking, Unit Tests, and Swagger

## Running the project
**Since we use an in-memory database we need to create our data at the start of any sample run.**

Postman allows you to set variables for collections etc so now that we have multiple requests we can pull out the base URL of https://localhost:7221 into a variable
so our API calls are made to locations like {{basehttpsURL}}/api/customers instead of the full name. It makes it easier to keep them all updated if something changes.

## Sample API calls

For an interactive UI with sample API calls, check the Swagger UI at: https://localhost:7221/swagger/index.html
```