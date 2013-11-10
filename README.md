ROL
===

Simple data access layer and ORM generator for .NET

The generator is pretty useful - just point it at a database and it'll build the ROL / strongly types session objects / views etc. 

The only caveats are the all tables and views MUST have an ID column. 

Works for MySQL, Microsoft SQL Server and mysql (but easily cusomizable to add support for others)

You need the connection details in the web.config - like:

  <!-- Database Connection Settings  -->
  <add key="ConnectionType" value="oledb" /> 
  <add key="ConnectionServer" value="localhost" />
  <add key="ConnectionDataBase" value="Northwind" />
  <add key="ConnectionPort" value="3306" /> 
  <add key="ConnectionUserID" value="sa" />
  <add key="ConnectionPassword" value="mypassword" />
    
This should be used to populate a ConnectionSettings Object.

Use
===
Run the generator against your database, and add to your solution as a seperate project. 
Add a reference to your ROL in your web application

To load a specific record you can simply call:

var conn = new ConnectionSettings();
var object = new DatabaseTable(conn);
conn.Load(1);

To search:

var objects = new DatabaseTableList(conn);
objects.Search('Deleted = 1');

etc.

There is a built in query builder to strongly type your query creation.

var where = new Where(conn);
where.add('name','geoff');
var objects = new DatabaseTableList(conn);
objects.Search(where);

Each database operation returns a boolean if successful, and an error message static string if there is a problem.  
You are advised to check if operations are successful.

Database operations can be conducted via stored procedures (which can be generated), views or just SQL from the client.

