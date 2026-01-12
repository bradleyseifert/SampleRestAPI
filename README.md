# SampleRestAPI
Setting up a sample Rest API just to showcase some basic C#/Github knowledge.

## Initial Guide

Starting with the guide on https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-9.0&tabs=visual-studio

Instead of primarily using the built-in *.http file Visual Studio provides I lean toward using Postman, but both work fine.
In my experience you end up in Postman eventually anyway, so I find it easier to start with and maintain.

## Running the project
I've deviated some from the API walkthrough in order to create customers and orders as a more real world scenario.

**Since we use an in-memory database we need to create our data at the start of any sample run.**

Postman allows you to set variables for collections etc so now that we have multiple requests we can pull out the base URL of https://localhost:7221 into a variable
so our API calls are made to locations like {{basehttpsURL}}/api/customers instead of the full name. It makes it easier to keep them all updated if something changes.

### Sample INSERT call

For a sample data-inserting call, the API setup is as follows:

```
Variable: basehttpsURL = https://localhost:7221
URL: {{basehttpsURL}}/api/customers
Request Type: POST, making sure the header for content-type is set to application/json
```

The sample json provided for the request body is:

```json
{
  "name": "JOHN SMITH",
  "Orders": [
    {
        "OrderNumber": 12345,
        "ProductName": "Doodad"
    },
    {
        "OrderNumber": 67890,
        "ProductName": "Thingamajig"
    }
  ]
}
```

Response for the above:
```json
{
    "customerId": 1,
    "name": "JOHN SMITH",
    "orders": [
        {
            "orderNumber": 12345,
            "productName": "Doodad"
        },
        {
            "orderNumber": 67890,
            "productName": "Thingamajig"
        }
    ]
}
```

When you send this request you can see something like the following in the console window indicating the request worked:
info: Microsoft.EntityFrameworkCore.Update[30100]
      Saved 1 entities to in-memory store.

From there we can make some follow-up requests to modify the data, search for it etc.

### Sample GET call

```
Variable: basehttpsURL = https://localhost:7221
URL: {{basehttpsURL}}/api/customers
Request Type: GET, making sure the header for content-type is set to application/json
```

Response for the above:
```json
[
    {
        "customerId": 1,
        "name": "JOHN SMITH",
        "orders": [
            {
                "orderNumber": 12345,
                "productName": "Doodad"
            },
            {
                "orderNumber": 67890,
                "productName": "Thingamajig"
            }
        ]
    }
]
```