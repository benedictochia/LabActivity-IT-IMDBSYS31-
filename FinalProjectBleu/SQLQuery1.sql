CREATE DATABASE FoodOrderingDB;


CREATE TABLE Admins (
    AdminID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50),
    Password NVARCHAR(50)
);

CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50),
    Password NVARCHAR(50),
);

CREATE TABLE Menu (
    ItemID INT IDENTITY(1,1) PRIMARY KEY,
    ItemName NVARCHAR(100),
    Category NVARCHAR(50),
    Price DECIMAL(10,2)
);

CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID),
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(10,2)

);

CREATE TABLE OrderItems (
    OrderItemID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    ItemID INT FOREIGN KEY REFERENCES Menu(ItemID),
    Quantity INT,
    Subtotal DECIMAL(10,2)
);

INSERT INTO Admins (Username, Password) VALUES ('admin', '12345');
ALTER TABLE Orders
ADD Status NVARCHAR(50) DEFAULT 'Pending';
