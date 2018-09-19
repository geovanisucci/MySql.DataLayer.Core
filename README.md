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
- [Configuring your Entities](#Configuring-your-entities)
- [Configuring your Stored Procedures](#Configuring-your-Stored-Procedures)
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
    public async void MyMethod()
    {
         using (MySqlConnection connection = await connectionFactory.GetAsync())
         {
             //Use the connection and Dapper method here...

             connection.Query("your sql query here"); // for example

             //like this, you have all Dapper methods in connection object here.
         }
    }
 }
 ```
 
 ---
 ## Configuring your entities
 You can map your `C#` model classes with your tables in your database.
 
 :exclamation: The `MySql.DataLayer.Core` is not a **ORM**. It just map the `C#` model class to tables in database for using the basic CRUD operations in `Repository` that will be explain in this `README`.
 
 To configure your `C#` model classes you should tag some attributes and tag your model class as `IDataEntity`.
 
 E.g.:
 
 ```
    [TableName("foo_bar_table")] //the string that contains the name of your database table.
    public class FooBarModel : IDataEntity
    {
        [PK("id")] // PK attribute who tags the property to represents a PK and tags the column name of your database table.
        public Guid Id { get; set; }
        [ColumnName("name")] // ColumnName attribute who tags the column name of your database table.
        public string Name { get; set; }
    }
    
 ```

---
## Configuring your Stored Procedures
You can map your `C#` model stored procedures classes with your stored procedures in your database.

To configure you should tag an attribute and tag your model stored procedure class as `IDataStoredProcedure`.

E.g.:

```
    [StoredProcedureName("sp_foo")] //StoredProcedureName attribute who tags the stored procedure name in your database.
    public class FooStoredProcedure : IDataStoredProcedure
    {
        public Guid id { get; set; }
        public string Description { get; set; }
    }
```

---
