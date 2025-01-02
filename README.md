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
   

## Sample MSSQL Query Script For This Project 
   ```sh
   -- Create Customers table
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Email NVARCHAR(100)
);

-- Insert sample data into Customers table
INSERT INTO Customers (FirstName, LastName, Email) VALUES
('John', 'Doe', 'john.doe@example.com'),
('Jane', 'Smith', 'jane.smith@example.com'),
('Michael', 'Brown', 'michael.brown@example.com'),
('Emily', 'Davis', 'emily.davis@example.com');

-- Create Products table
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100),
    Price DECIMAL(10, 2),
    Stock INT
);

-- Insert sample data into Products table
INSERT INTO Products (ProductName, Price, Stock) VALUES
('Laptop', 799.99, 10),
('Smartphone', 499.99, 20),
('Headphones', 29.99, 50),
('Tablet', 299.99, 15);

   ```
