## Coding Tracker


The coding tracker is a C# console application I'm building to log and manage the duration of code sessions.

I built this as part of the C# academy curriculum.


## What it does:

Log coding sessions with a start time, end time, and auto calculated duration

View all sessions in a formatted table

Insert, update, and delete sessions

Input validation enforcing a strict date/time format



## How to run in terminal:

cd Coding-Tracker

dotnet run

.NET 10 SDK is needed to run the program


## Tech stack:

C# — .NET 10 console app

SQLite — local file based database, packeged as Microsoft.Data.Sqlite

Dapper — Object relational mapper used for mapping query results to C# objects

Spectre.Console — Used to create visually appealing tables in the formatted table output in the terminal


## How it works:

When running the program the user will be greeted with a menu.

The menu presents the user with five options shown in the image below with the first option being to close the program.

The rest of the options are Create Read Update and Delete operations.


![Main Menu](images/Menu.png)


The First CRUD operation is to view the coding tracker records:

![Viewing a record](images/View.png)


The second CRUD operation is to insert a record:

![Inserting a record](images/Insert.png)


The third CRUD operation is to delete a record:

![Deleting a record](images/Delete.png)


The fourth and final operation is the update record feature:

![Updating a record](images/Update.png)


## Project structure:


Program.cs is the entry point, it creates the table and starts the menu by calling the CreateTable method which lives in CodingController and the GetUserInput method which lives in the UserInput class.

CodingSession.cs is the model, it holds the properties used throughout the program, it mirrors the database table.

CodingController.cs is for database operations, all CRUD methods live inside this class, Dapper is used to execute the raw SQLite queries.

UserInput.cs is for user interaction, this class contains the switch menu, input collection methods and contains methods that call the CodingController class.

Validation.cs is for input validation, this class holds the ValidateTime & ValidateId methods used for date format and ID validation.

appsettings.json is for configuration, this file stores the database connection string.


## Separation of concerns

Each class has one job.

This is the first time I used classes in a program, the project requirements made separation of concerns a must for this project.

I decided to split the program into four main classes:


UserInput talks to the user, displays menus, collects input, calls controller methods.

CodingController talks to the database, receives parameters, runs SQL.

Validation checks input, never interacts with the user directly beyond error messages.

CodingSession is the model, Containing the properties used throughout the program.


## Database Layout:




Id is of type int, this is the primary key of the data, it is set to auto-increment.



StartTime is of type string, the format of the start time is dd-MM-yyyy HH:mm.



EndTime	is of type string, it follows the same format of StartTime which is dd-MM-yyyy HH:mm.



Duration is of type string and is calculated from EndTime - StartTime, stored as string.



SQLite does not have set DateTime or TimeSpan types.



Dates are stored as strings and parsed in C#. 



Duration is calculated as a TimeSpan then stored as a string.



The table is created on startup if it does not already exist:





````
CREATE TABLE IF NOT EXISTS coding_tracker (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
StartTime TEXT,
EndTime TEXT,
Duration TEXT
)

````



## Duration calculation:

Duration is not collected from the user. It is a product of endTime minus startTime:

```
string duration = (endTime - startTime).ToString();
```
When a value of type DateTime is subtracted its result returns as a TimeSpan type.

This is converted to a string before being stored in SQLite.


## Validation:

Date input is validated with DateTime.TryParseExact, enforcing the format `dd-MM-yyyy HH:mm`.

The loop keeps asking until valid input is received:

```
while (!DateTime.TryParseExact(input, "dd-MM-yyyy HH:mm", null, DateTimeStyles.None, out result))
{
    Console.WriteLine("Invalid format. Try again (dd-MM-yyyy HH:mm): ");
    input = Console.ReadLine();
}
```

ID input is validated with int.TryParse, rejecting non numeric input and values less than or equal to zero.

## Rows affected: 

connection.Execute returns the number of rows affected. Used to detect when a user enters an ID that does not exist:



```
int rowsAffected = connection.Execute(sql, parameters);
if (rowsAffected == 0)
    Console.WriteLine("No record found with that ID.");
```



## Key concepts:

### Dapper:

Dapper sits on top of ADO.NET and removes the need to manually map database columns to the properties.

connection.Query<CodingSession>(sql) — runs a SELECT and maps each row to a CodingSession object.

connection.Execute(sql, parameters) — runs INSERT, UPDATE, DELETE operations.

Parameters are passed as anonymous objects and being assigned value: new { Id = id, StartTime = startTime }.

The property names on the anonymous object must match the `@placeholder` names in the SQL exactly or values won't be passed over and values will be null.


### Connection string:

The database connection string is stored in appsettings.json rather than being hardcoded, this allows us to reference it with ease throughout the program.

It' stored in appsettings.json and read at startup via ConfigurationBuilder.

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=Coding-Tracker.db"
  }
}
```

### Reading the connection string

The connection string needed to access the database is read into a static field using ConfigurationBuilder:


```
static string connectionString = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build()
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found.");
```

If the connection string is missing, GetConnectionString returns null. The ?? operator then throws an exception with a not found message so the program fails immediately instead of hitting a confusing null error later.


### Copying appsettings.json to the build output

```
<ItemGroup>
  <None Update="appsettings.json">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
  </None>
</ItemGroup>
```


## Limitations:

When running the program and viewing records I noticed that id's that were deleted affected the new id number, the pre-existing id numbers could not be used again.




