CREATE DATABASE StudentDB

CREATE TABLE Students(
		StudentID INT PRIMARY KEY IDENTITY(1,1),
		FirstName VARCHAR(50),
		LastName VARCHAR(50),
		Age INT,
		Course VARCHAR(50),
);