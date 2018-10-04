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
- [Using the BaseMySqlRepository](#Using-The-BaseMySqlRepository)
- [Using the DTO translation](#Using-the-DTO-translation)
---

## Installation
To install this library you will need to run the following Nuget Package command:

```Install-Package MySqlDataLayer.Core -Version 1.3.0```

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
## Using the BaseMySqlRepository
The `BaseMySqlRepository`provides de CRUD (Create, Read, Update, Delete) operations to your entity (to configure your class model see **Configuring your entities** section), provides the call to Stored Procedures (to confiure your stored procedure class model see **Configuring your Stored Procedures** section) and provides the call to Views (feature is avaliable soon).

To use this you should inherit your class as `BaseMySqlRepository` and pass your `IDataEntity` as parameter. As it, you will be able to overwrite the methods if you prefer.
If you prefer, you can inherit your interface as `IMySqlDataRepository` and pass your `IDataEntity` as parameter.

**- Interface**
```
 public interface IFooBarRepository:IMySqlDataRepository<FooBarModel>
 {       
 }
```

**- Class: Pass the IMySqlConnectionFactory as Dependency Injection (to configure see  Setup Connection Factory section)**
```
 public class FooBarRepository : BaseMySqlRepository<FooBarModel>, IFooBarRepository
 {
    private readonly IMySqlConnectionFactory _connectionFactory;
    
    public FooBarRepository(IMySqlConnectionFactory connectionFactory) : base(connectionFactory)
    {
       _connectionFactory = connectionFactory;
    }
 }
```
### Examples

#### GetAllAsync
Provides the Select operation to get a list for mapped entity.

```
List<FooBarModel> result = await _fooBarRepository.GetAllAsync();
```

##### GetAllAsync with Condition

```
  ConditionSearch[] conditions = new ConditionSearch[]
  {
      new ConditionSearch(){
                            ParameterName = "id", 
                            ParameterValue = Guid.Parse("b10cfad7-eb3a-4016-85d0-b5a307a2c8a1"), 
                            Condition = Condition.Where,
                            Operator = ConditionOperator.Equal
                           },

      new ConditionSearch(){
                             ParameterName = "name", 
                             ParameterValue = "Example", 
                             Condition = Condition.And,
                             Operator = ConditionOperator.Diff
                            },
                
      };

List<FooBarModel> result = await _repository.GetAllAsync(conditions);

```

##### GetAllAsync selecting Columns 
Create the custom model that will represents your result:

```
  public class FooBarResult 
  {
        public string Name { get; set; }
  }
```

And then, call the `GetAllAsync` passing the column(s):

```
 ColumnTable[] columns = new ColumnTable[]
 {
   //Pass the string column name as parameter.
   new ColumnTable("name").As("Name") //If you prefer, you can pass an alias string name using "As"
 };
 
  List<FooBarResult> result = await _repository.GetAllAsync<FooBarResult>(columns);
```

##### GetAllAsync selecting Columns with conditions

```
 ConditionSearch[] conditions = new ConditionSearch[]
  {
      new ConditionSearch(){
                            ParameterName = "id", 
                            ParameterValue = Guid.Parse("b10cfad7-eb3a-4016-85d0-b5a307a2c8a1"), 
                            Condition = Condition.Where,
                            Operator = ConditionOperator.Equal
                           },

      new ConditionSearch(){
                             ParameterName = "name", 
                             ParameterValue = "Example", 
                             Condition = Condition.And,
                             Operator = ConditionOperator.Diff
                            },
                
      };
      
  ColumnTable[] columns = new ColumnTable[]
  {
   //Pass the string column name as parameter.
   new ColumnTable("name").As("Name") //If you prefer, you can pass an alias string name using "As"
  };
 
  List<FooBarResult> result = await _repository.GetAllAsync<FooBarResult>(columns, conditions);
  
```

#### GetAsync
Provides the Select operation to get a single mapped data.

```
FooBarModel resultFirstOrDefault = await _repository.GetAsync(Guid.Parse("b10cfad7-eb3a-4016-85d0-b5a307a2c8a1"));
```

#### CreateAsync
Provides the insert statement.

```
 int rowsAfected = await _repository.CreateAsync(new FooBarModel(){
                Id = Guid.NewGuid(),
                Name = "Example Create Async"
            });
```

**NOTE:** if the PK is auto increment, just pass `true` in the second parameter:

```
int rowsAfected = await _repository.CreateAsync(new FooBarModel(){
                Name = "Example Create Async"
            }, true);
```

#### UpdateAsync
Provides the update statement.
Pass the object with the values. You should pass all values, including the PK value:

```
 int rowsAfected = await _repository.UpdateAsync(new FooBarModel(){
                 Id = Guid.Parse("b10cfad7-eb3a-4016-85d0-b5a307a2c8a1"),
                 Name = "Example Update Async"
             });

```

#### RemoveAsync
Provides the delete statement.

```
int rowsAfected = await _repository.RemoveAsync(Guid.Parse("b10cfad7-eb3a-4016-85d0-b5a307a2c8a1"));
```

#### ExecuteStoredProcedure
Provides the execution of a StoredProcedure to get a list for mapped data

:exclamation: Configure your StoredProcedure model class (see Configuring your Stored Procedures section).

```
  List<FooStoredProcedure> storedProcedureResult = await _repository.ExecuteStoredProcedure<FooStoredProcedure>();
```

##### Passing parameters

```
QueryParameter[] queryParameters = new QueryParameter[]
            {
                new QueryParameter(){
                    ParameterName = "limitToSelect",
                    ParameterValue = 50
                }
            };
 List<FooStoredProcedure> storedProcedureResult = await _repository.ExecuteStoredProcedure<FooStoredProcedure>(queryParameters);
 ```
 
 #### Using Dapper functions
 You can use Dapper functions and create your custom queries. You can invoke the Dapper functions using by `GetAsync` (from `IMySqlConnectionFactory`, see **Using MySqlConnection and Dapper functions** section).

---

## Using the DTO translation
You can translate the `IDataEntity` to your custom class model (like a DTO model).
This function is based on `AgileMapper` framework and the use is very simple.

### Translate `IDataEntity` to `DTO` model

```
FooDTO dto = Map.FromDataEntity<FooDTO, FooBarModel>(_fooBarModel);
```

### Translate `DTO` model to `IDataEntity`

```
 FooBarModel entity = Map.ToEntity<FooDTO, FooBarModel>(_fooDTO); 
```

---

:email: **Email me for any questions and suggestions. Collaborate with this Framework.**

**Skype**: sajorato.sg

**E-mail**: geovani@sajorato.com

:beer:
:metal:
