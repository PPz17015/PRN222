create database FUNewsManagement 
use FUNewsManagement 

CREATE TABLE Category (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL,
    CategoryDescription NVARCHAR(250),
    ParentCategoryID INT,
    Status BIT NOT NULL -- 1 = Active, 0 = Inactive
);

CREATE TABLE SystemAccount (
    AccountID INT PRIMARY KEY IDENTITY(1,1),
    AccountName NVARCHAR(100) NOT NULL,
    AccountEmail NVARCHAR(100) NOT NULL UNIQUE,
    AccountRole INT NOT NULL, -- 1: Staff, 2: Lecturer, 3: Admin
    AccountPassword NVARCHAR(100) NOT NULL
);

CREATE TABLE NewsArticle (
    NewsArticleID INT PRIMARY KEY IDENTITY(1,1),
    Headline NVARCHAR(200) NOT NULL,
    CreatedDate DATETIME NOT NULL,
    NewsContent NVARCHAR(MAX),
    NewsStatus BIT NOT NULL, -- 1 = Active, 0 = Inactive
    CategoryID INT NOT NULL,
    AccountID INT NOT NULL,
    UpdatedBy INT,
    UpdatedDate DATETIME,
    ModifiedDate DATETIME,
    FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID),
    FOREIGN KEY (AccountID) REFERENCES SystemAccount(AccountID)
);

INSERT INTO Category (CategoryName, CategoryDescription, ParentCategoryID, Status)
VALUES 
('Politics', 'News related to politics', NULL, 1),
('Technology', 'Latest in tech', NULL, 1),
('Entertainment', 'Movies and Celebs', NULL, 1);

-- Insert System Accounts
INSERT INTO SystemAccount (AccountName, AccountEmail, AccountRole, AccountPassword)
VALUES
('Nguyen Van A', 'a@news.com', 1, 'pass123'),
('Tran Thi B', 'b@news.com', 2, 'pass456')

-- Insert News Articles
INSERT INTO NewsArticle (Headline, CreatedDate, NewsContent, NewsStatus, CategoryID, AccountID, UpdatedBy, UpdatedDate, ModifiedDate)
VALUES
('Tech Trends 2025', GETDATE(), 'The future of AI and IoT', 1, 2, 1, NULL, NULL, NULL),
('Election Results', GETDATE(), 'Full report on the election outcome', 1, 1, 2, NULL, NULL, NULL);

