# CourseAPI
Perform CRUD operations on Course list through endpoints

# Connect to Database
The application uses an In-Memory database by default.

If you want to use an SQL Server database instead, kindly change do the following in appsettings.json
1. Change the value of "UseInMemoryDb" property to false.
2. Add your SQL Server DB connection string against the key "CourseDetailDB".
