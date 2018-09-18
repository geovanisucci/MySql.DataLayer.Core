# MySql.DataLayer.Core
Data Layer that provides a basic CRUD operations using Dapper with .Net Core

[ ![Codeship Status for sajoratosg/MySql.DataLayer.Core](https://app.codeship.com/projects/495340d0-871f-0136-d478-3e27c1ec70f0/status?branch=master)](https://app.codeship.com/projects/302596)

Welcome to `MySql.DataLayer.Core`

This library provides the database access layer to MySql using Dapper and .Net Core.
Also, it provides the base repository with basic CRUD operations, calls Stored Procedures and Views. Provides the configuration to Data Model, mapping the entities to your database.
Also, can map the Entity Data Model to Data Transfer Object (DTO) using Agile Mapper.

- [Installation](#Installation)
- [Setup Connection Factory](#Setup-Connection-Factory)
- [Using MySqlConnection](#Using-MySql-Connection)
- [Using MySqlConnection and Dapper functions](#Using-MySql-Connection-Dapper-functions)

---

## Installation
To install this library you will need to run the following Nuget Package command:
```dotnet add package MySqlDataLayer.Core --version 1.1.0```

---
## Setup Connection Factory
To use the data methods, you should to register the dependency for `IMySqlConnectionFactory` in `ConfigureServices` using `IServiceCollection`. E.g.:

```services.AddSingleton<IMySqlConnectionFactory>(new MySqlConnectionFactory("yourConnectionString"));```

---
## Using MySqlConnection
To get the connection you should to call the `GetAsync` (from `IMySqlConnectionFactory`). This function will open the connection from a connection pool.
E.g.:

```
using (MySqlConnection connection = await connectionFactory.GetAsync())
{
     //Use the connection here...  
}
 ```
 ---
 ## Using MySqlConnection and Dapper functions
 To use the Dapper functions and methods, you should open the connection with `GetAsync` method (from `IMySqlConnectionFactory`) and add  a `using` reference from `Dapper`. And then, you can access all Dapper methods to use as you prefer.
 E.g.:
 
 ```
 using Dapper;
 public class Foo
 {
    using (MySqlConnection connection = await connectionFactory.GetAsync())
    {
        //Use the connection and Dapper method here...
        
        connection.Query("your sql query here"); // for example
        
        //like this, you have all Dapper methods in connection object here.
    }  
 }
 ```
 
 ---
 
 
