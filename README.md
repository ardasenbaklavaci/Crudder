# CrudderWithHtml

**Description:**
CrudderWithHtml, creates CRUD Operations with HTML and CSHTML (codebehind) Files for your specified MSSQL Database... 

**Features:**
- Creating Page Directories for each table, within name of each table... 

**Technologies Used:**
- .NET Core
- Entity Framework Core
- MSSQL


## Getting Started

**Prerequisites:**
- Visual Studio, .NET 8.0 , Microsoft SQL Server (or any database you use...)
Nuget Packages:
```sh
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer
```
**Installation:**
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/your-repo-name.git

2. Create a new sample database or use an existing one... Get Connection String:
   ```sh
   "Server=myServerName\\myInstanceName;Database=myDataBase;User Id=myUsername;Password=myPassword;"
   ```
   And add connection string to your appsettings.json
   ```sh
   "ConnectionStrings": {
     "Server=myServerName\\myInstanceName;Database=myDataBase;User Id=myUsername;Password=myPassword;"
   }
   ```
3. Launch Solution...

## Roadmap

- [x] Creating Directories with each table's name 
- [x] Creating Models of each table using Entity Framework Core Scaffolding
- [ ] Creating Index,Add,Details and Delete (CRUD) Pages for each model...
- [ ] Creating Codebehind .cshtml for each model's CRUD Pages
- [ ] Creating pages at another project inside this solution (Another .NET Core Project in this solution)
   

