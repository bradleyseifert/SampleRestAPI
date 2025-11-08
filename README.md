# SampleRestAPI
Setting up a sample Rest API just to showcase some basic C#/Github knowledge.

## Setting up the API code
Starting with the guide on https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-9.0&tabs=visual-studio

Generally speaking, we'll follow the instructions inside the guide without a lot of deviation.
Instead of primarily using the built-in *.http file Visual Studio provides I lean toward using Postman, but both work fine.

Initial API tempalte code provides a weather forecast method as it's default.
If we open up Postman we can put in a GET method with the following URL: https://localhost:7221/weatherforecast

This gives the following default response information as of my sample run:
```json
[
    {
        "date": "2025-11-06",
        "temperatureC": 11,
        "temperatureF": 51,
        "summary": "Balmy"
    },
    {
        "date": "2025-11-07",
        "temperatureC": 32,
        "temperatureF": 89,
        "summary": "Chilly"
    },
    {
        "date": "2025-11-08",
        "temperatureC": 5,
        "temperatureF": 40,
        "summary": "Hot"
    },
    {
        "date": "2025-11-09",
        "temperatureC": 29,
        "temperatureF": 84,
        "summary": "Sweltering"
    },
    {
        "date": "2025-11-10",
        "temperatureC": 2,
        "temperatureF": 35,
        "summary": "Chilly"
    }
]
```

After following the steps to update the PostTodoItem and creating the sample request for it we are ready to run another test in postman
This time the URL should be: https://localhost:7221/api/todoitems with a request type of POST, making sure the header for content-type is set to application/json
The sample json provided for the request body is:

```json
{
  "name": "walk dog",
  "isComplete": true
}
```

When you send this request you can see the following in the console window indicating the request worked:
info: Microsoft.EntityFrameworkCore.Update[30100]
      Saved 1 entities to in-memory store.

Also worth noting that Postman allows you to set variables for collections etc so now that we have multiple requests we can pull out the base URL of https://localhost:7221 into a variable
so our API calls are made to locations like {{basehttpsURL}}/api/todoitems instead of the full name. It makes it easier to keep them all updated if something changes.

We can also follow the instructions for testing out the GET methods as well, but since the information is held in memory for the walkthrough
we must POST our data first before we can GET it.

## Creation of the github repository
Fairly self explanatory, but worth indicating that adding a Readme.md (this file) and a .gitignore allow for a lot of nice features within github.
The readme does what you would expect and allows you to provide some quick documentation around necessary software, access, steps for building the solution, whatever you may need.

The .gitignore allows you to keep from cluttering up your change logs with binary files or anything else you don't actually want replicated to github.

All told it is a really quick, but solid example of some of the different features available for C# API code that allows for the user
to validate the way the code functions as well as ensure their request handling program (Visual Studio itself, Postman, or otherwise)
can communicate with the solution or not. I would honestly say that as a crash course in API usage within VS and C# you could probably get
the average prospective engineer to understand these fundamentals with this example within a very short amount of time.
