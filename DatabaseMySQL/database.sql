CREATE DATABASE fs12ApelMusic;

USE fs12ApelMusic;

CREATE TABLE MsUser (

    Id VARCHAR(40) PRIMARY KEY DEFAULT (UUID()),
    Name VARCHAR(250) NOT NULL,
    Email VARCHAR(250) NOT NULL UNIQUE,
    Password VARCHAR(250) NOT NULL,  
    IsDeleted bool DEFAULT 0,
    IsActivated bool DEFAULT 0,
    CreatedAt TIMESTAMP
);

CREATE TABLE MsCategory (

    Id VARCHAR(50) PRIMARY KEY DEFAULT (UUID()),
    Name VARCHAR(250) NOT NULL,
    Title VARCHAR(250) NOT NULL,
    Description TEXT NOT NULL,
    Image VARCHAR(250),
    HeaderImage VARCHAR(250)
);

CREATE TABLE MsCourse (

    Id VARCHAR(50) PRIMARY KEY DEFAULT (UUID()),
    Name VARCHAR(250) NOT NULL,
    Description TEXT NOT NULL,
    Image VARCHAR(250),
    Price DOUBLE NOT NULL,
    CategoryId VARCHAR(50) NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES MsCategory(Id)
);

CREATE TABLE MsPaymentMethod (

    Id VARCHAR(50) PRIMARY KEY DEFAULT (UUID()),
    Name VARCHAR(250) NOT NULL,
    Image VARCHAR(250)
);

CREATE TABLE TsOrder(

    Id VARCHAR(50) PRIMARY KEY DEFAULT (UUID()),
    UserId VARCHAR(50) NOT NULL,
    PaymentId VARCHAR(50),
    InvoiceNo VARCHAR(250) UNIQUE,
    OrderDate TIMESTAMP,
    IsPaid bool DEFAULT 0,
    FOREIGN KEY (PaymentId) REFERENCES MsPaymentMethod(Id)
);

CREATE TABLE TsOrderDetail (

    Id VARCHAR(40) PRIMARY KEY DEFAULT (UUID()),
    OrderId VARCHAR(40) NOT NULL,
    CourseId VARCHAR(40) NOT NULL,
    IsActivated bool DEFAULT 0,
    FOREIGN KEY (OrderId) REFERENCES TsOrder(Id),
    FOREIGN KEY (CourseId) REFERENCES MsCourse(Id)
);