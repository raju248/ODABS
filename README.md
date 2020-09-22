# ODABS

To run the program visual studio 2017 or above is needed.

To create the database open "Web.config" and modify the following line

```xml
 <connectionStrings>
    <add name="DefaultConnection" connectionString="Data Source= YOUR_SQLSERVER_NAME; Initial Catalog = ODABS; Integrated Security=True;" providerName="System.Data.SqlClient" />
  </connectionStrings>
  ```
  Replace YOUR_SQLSERVER_NAME with the Name of SQL Server installed in your computer.
  
  After first run database named "ODABS" will be created.
  Then insert the following data into AspNetRoles table of database.
  
  |Id|Name|
  |--|----|
  |1 |Admin|
  |2 |Doctor|
  |3 |Patient|
  
  After inserting data run the application again.
