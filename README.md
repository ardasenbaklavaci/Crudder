# CrudderWithHtml
![image](https://github.com/user-attachments/assets/1d803f53-926f-4dc7-89fb-dff8f34d2c6e)

**Description:**
CrudderWithHtml, creates CRUD Operations with HTML and CSHTML (codebehind) Files for your specified MSSQL Database... 

**Features:**
- Creating Entity Models, Page Directories for each table, within name of each table... For implementing CRUD Operations, and setting a base implementation for a project scaffolded from any database... 

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
   git clone https://github.com/ardasenbaklavaci/Crudder.git

2. Create a new sample database or use an existing one... Get Connection String:
   ```sh
   "Server=myServerName\\myInstanceName;Database=myDataBase;User Id=myUsername;Password=myPassword;"
   ```
   And add connection string to your Crudder project's appsettings.json
   ```sh
   "ConnectionStrings": {
     "Server=myServerName\\myInstanceName;Database=myDataBase;User Id=myUsername;Password=myPassword;"
   }
   ```
3. Launch Crudder Project... If you get no any errors, you will realize your CRUD Pages, Models, Datacontext created inside CoreProject directory. 
   
## Info

![image](https://github.com/user-attachments/assets/586b040b-9097-4e44-bdb0-7d449e8c5b28)

In this Solution we have 2 projects. Crudder Project is the tool for Scaffolding... It gets database information,tables,columns etc... Generates Models, Datacontext, Pages...Copies them to CoreProject directory. Lets you have a ready to run scaffolded Solution at CoreProject .NET Core 8.0 Project!!! Since ``` Program.cs ``` has set and ``` appsettings.json ``` have set, the project is ready to run... 

![image](https://github.com/user-attachments/assets/48a4abd6-17ce-49e5-8afd-421129744291)

Right now it created datacontext as ``` NewDbContext.cs ```  in ``` Data ``` Directory... Created ``` Customers.cs ``` and ``` Products.cs ``` in ``` Models ``` Directory... Shortly we scaffolded datacontext and models... 

![image](https://github.com/user-attachments/assets/9f56e7ab-2a04-4523-b6c0-df02d0b0d5af)

In our sample db, we have 2 tables... ``` Customers ``` and ``` Products ``` . So our Solutions created  ``` Customers ``` and ``` Products ``` directories for CRUD Pages... 

![image](https://github.com/user-attachments/assets/85eb60b8-7b67-4b6b-852d-c72bdcfaf74a)


## Roadmap

- [x] Creating Directories with each table's name 
- [x] Creating Models of each table using Entity Framework Core Scaffolding
- [ ] Creating Index,Add,Details and Delete (CRUD) Pages for each model...
- [ ] Creating Codebehind .cshtml for each model's CRUD Pages
- [x] Creating pages at another project inside this solution (Another .NET Core Project in this solution)
   

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
![image](https://github.com/user-attachments/assets/9de1fe77-bd94-4e83-bf94-4c6a73320145)
