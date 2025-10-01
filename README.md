# CourseAPI
A simple yet production-style ASP.NET Core Web API for managing courses.
This project demonstrates clean architecture, environment-based configuration, and database flexibility (In-Memory or SQL Server).
It also includes a Dockerfile for containerized deployment.

# Features:
1. CRUD operations for managing course details
2. In-Memory database for quick local testing
3. SQL Server integration for production-like setup
4. Configurable via appsettings.json
5. RESTful endpoints with proper HTTP verbs and status codes
6. Swagger UI for API documentation
7. Dockerized for easy deployment
8. Azure Key Vault Integration
9. XUnit test cases


# Tech Stack:
1. Backend : ASP.Net Core 9.0
2. Database : EF Core (In-Memory/SQL Server)
3. Language : C#
4. Tools : Swagger UI for API Documentation

# Project Structure:

CourseAppAPI/
 ├── Controllers/
 ├── DAL/
 ├── DTO/
 ├── Models/
 ├── Repository/
 ├── Services/
 ├── appsettings.json
 ├── CourseAppAPI.http
 ├── Dockerfile
 ├── Program.cs
 README.md

 # Setup and Run:
1. Clone the Repository

 git clone https://github.com/BindiyaBKumar/CourseAPI.git
 cd CourseAPI

2. Configure the database

By default, the API uses In-Memory Database.
To use SQL Server:

    1. Open appsettings.json

    2. Set:

    "UseInMemoryDb": false
    "ConnectionStrings": {
        "CourseDetailDB": "Your SQL Server connection string"
    }

3. Run the API

Using .NET CLI :
    dotnet restore
    dotnet run

Using Visual Studio :
    1. Open the .sln file
    2. Set the project as startup
    3. Press F5

Once running, navigate to :
    http://localhost:<port>/swagger

    Note : The port can be set in CourseAppAPI.http file 


# Sample endpoints:

POST /api/Auth/Login → Get Bearer token to authorize other endpoints

GET /api/GetCourseList → Get all courses

GET api/Course/GetCourseList?page=1&pageSize=2&sort=createdAt&status=Active&tutor=Mr.%20X&q=tuitorial → Apply filters and sorting on the data

GET /api/GetCourseBy/{id} → Get course by Id

POST /api/AddCourse → Add a new course

POST /api/UpdateCourse/{id} → Update a course

DELETE /api/Course/{id} → Delete a course


# Query Parameters:
page : Accepts the page number (Default value is 1)
pageSize : Accepts the number of data items that a page must contain. (Default value is 20)
sort : Accepts one of the following values to sort data:
       createdAt - sorts data in ascending order based on createdAt
       -createdAt - sorts data in descending order based on createdAt
       name - sorts data in ascending order based on name
       -name - sorts data in descending order based on name
       Id - sorts data in ascending order based on Id (default value)
       -Id - sorts data in descending order based on Id 
status : Accepts status to filter data based on it. (Status can be Active/Inactive)
tutor : Accepts tutor name to filter data based on it. (Eg : Mr. X)
q : Accepts a string and returns data where COurse name or Course description contains this string


# Sample Request:

POST /api/AddCourse

{
  "courseNumber": "X01",
  "courseName": "Docker Fundamentals",
  "courseDescription": "Introductory Docker concepts",
  "courseDuration": 10,
  "courseTutor": "Max",
  "courseStatus": "Active",
  "createdAt" : null

}

# Authentication:
The endpoints in this Web API uses JWT (JSON Web Token) Bearer Authentication to secure endpoints.

When using the CRUD endpoints,

1. Send valid credentials to the /api/auth/login endpoint to receive a JWT token.
2. Pass the token in the Authorization header when calling protected endpoints: (Header -> Authorization: Bearer <your_token_here>)

In case of Swagger UI, 
1. Send valid credentials to the /api/auth/login endpoint to receive a JWT token.
2. Click on 'Authorize' button and provide <your_token_here>
3. Provide <your_token_here> and click on 'Authorize' to authorize all endpoints. Then click on close.
4. Try any other CRUD endpoint. It should get automatically authorized.

Note : Token expiration is configured in appsettings.json under the Jwt section.


# Sample Login Request:
POST /api/auth/login
Content-Type: application/json

{
  "username": "user",
  "password": "password"
}

# Secret Management with Azure Key Vault (Optional):
The app has optional provision to store and use secrets via Azure Key Vault.

In order to enable the feature, follow the below steps:
1. Open your Azure dashboard and create a key vault to store secrets
Bash command : az keyvault create --name MyCourseApiVault --resource-group MyResourceGroup --location eastus
2. From Azure Dashboard --> Go to Resources --> Open the new key vault (MyCourseApiVault) --> Select Access Control (IAM) from left Menu --> Click 'Add Role Assignment' --> Select desired role( eg: Key Vault Administrator) --> Click on Next and under Select Members, choose your user --> Review and assign role.

3. Add The secrets for DB Connection string, JWT Token, Audience and Issuer values in the vault manually or using below commands
Bash command :
az keyvault secret set --vault-name MyCourseApiVault --name "CourseApi-DbConnection" --value "<your-connection-string>"
az keyvault secret set --vault-name MyCourseApiVault --name "CourseApi-JwtKey" --value "<your-jwt-secret>"
az keyvault secret set --vault-name MyCourseApiVault --name "CourseApi-Issuer" --value "<your-issuer-secret>"
az keyvault secret set --vault-name MyCourseApiVault --name "CourseApi-Audience" --value "<your-audience-secret>"

4. Open appsettings.json in the .Net solution.
5. Change the value of the key "useKeyVault" to true .
6. Replace the placeholder "<azure-key-vault-name>" with the name of your key vault (Note : DO NOT USE THE ENTIRE URI. USE ONLY THE KEYVAULT NAME. Eg: "MyCourseApiVault").
Note : In case you have used a different secret or key, ensure that it is properly mapped in the middlewares in Program.cs. In case it is a JWT secret, ensure it is used in JWTTokenService.cs as well.
7. Connect to your Azure account via Visual Studio (Tools --> Options --> Azure Service Authentication).
8. Once you execute the application locally, it should fetch values from Azure Key Vault.


# Caching:

The app uses In-Memory Caching for the filtered list of Courses. The duration/expiry is configured in Program.cs


# Future improvements:
1. Add more Xunit test cases
2. Try to integrate Prometheus/Kubernetes



