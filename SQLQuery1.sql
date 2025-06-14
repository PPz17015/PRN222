Create database MyStore

use MyStore


CREATE TABLE AccountMember (
    MemberID NVARCHAR(50) NOT NULL PRIMARY KEY,
    MemberPassword VARCHAR(20) NOT NULL,
    FullName VARCHAR(50) NOT NULL,
    EmailAddress VARCHAR(100) NOT NULL,
    MemberRole INT NOT NULL
);
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CategoryName VARCHAR(15) NOT NULL
);

CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProductName VARCHAR(40) NOT NULL,
    CategoryID INT NOT NULL,
    UnitStock INT  ,
    UnitPrice MONEY ,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

INSERT INTO Categories (CategoryName) VALUES 
('Clothes'),
('Books'),
('Electronics'),
('Food');

INSERT INTO Products (ProductName, CategoryID, UnitStock, UnitPrice)
VALUES
    ('Laptop Pro X', 1, 50, 1200.00),
    ('Mechanical Keyboard RGB', 1, 150, 99.99),
    ('The Great Gatsby', 2, 200, 15.50),
    ('Sapiens: A Brief History of Humankind', 2, 120, 25.00),
    ('Organic Apples (1kg)', 3, 300, 3.99),
    ('Whole Grain Bread', 3, 100, 2.50),
    ('Mens T-Shirt (Large)', 4, 80, 29.00),
    ('Womens Jeans (Size 28)', 4, 60, 45.00),
    ('Smartwatch Series 7', 1, 75, 350.00),
    ('Database Design Principles', 2, 90, 40.00);


INSERT INTO AccountMember (MemberID, MemberPassword, FullName, EmailAddress, MemberRole)
VALUES
('admin', 'admin123', 'Administrator', 'admin@example.com', 1),
('user1', 'user123', 'Regular User', 'user1@example.com', 2);