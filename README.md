# ODABS

To run the program visual studio 2017 or above is needed.

To create the database open "Web.config" and modify the following line

 <connectionStrings>
    <add name="DefaultConnection" connectionString="Data Source= YOUR_SQLSERVER_NAME; Initial Catalog = ODABS; Integrated Security=True;" providerName="System.Data.SqlClient" />
  </connectionStrings>
  
  Replace YOUR_SQLSERVER_NAME with the connection string of SQL Server installed in your computer.
