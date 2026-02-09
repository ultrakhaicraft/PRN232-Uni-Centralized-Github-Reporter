CREATE DATABASE GithubReporter;
GO

USE GithubReporter;
GO

CREATE TABLE Account (
    AccountID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Status INT NOT NULL,
    Role INT NOT NULL,
    DateCreated DATETIME NOT NULL DEFAULT GETDATE(),
    GithubEmail NVARCHAR(200)
);

CREATE TABLE Supervisor (
    SupervisorID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	AccountID  UNIQUEIDENTIFIER NOT NULL,
    SupervisorCode NVARCHAR(25) NOT NULL,

    CONSTRAINT FK_Supervisor_Account
        FOREIGN KEY (AccountID)
        REFERENCES Account(AccountID)
);

CREATE TABLE Student (
    StudentID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	AccountID  UNIQUEIDENTIFIER NOT NULL,
    StudentCode NVARCHAR(25) NOT NULL,

    CONSTRAINT FK_Student_Account
        FOREIGN KEY (AccountID)
        REFERENCES Account(AccountID)
);

CREATE TABLE Project (
    ProjectID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    ProjectName NVARCHAR(200) NOT NULL,
    GithubLink TEXT,
    Description TEXT,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    DateCreated DATETIME NOT NULL DEFAULT GETDATE(),
    AccessToken TEXT,

    CONSTRAINT FK_Project_CreatedBy
        FOREIGN KEY (CreatedBy)
        REFERENCES Account(AccountID)
);

CREATE TABLE GroupTeam (
    GroupID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    GroupName NVARCHAR(100) NOT NULL,
    GroupCode NVARCHAR(25) NOT NULL,
    AccountID UNIQUEIDENTIFIER NOT NULL,
    GroupRole INT NOT NULL,
    ProjectID UNIQUEIDENTIFIER NOT NULL,
    SupervisorID UNIQUEIDENTIFIER NOT NULL,

    CONSTRAINT FK_Group_Account
        FOREIGN KEY (AccountID)
        REFERENCES Account(AccountID),

    CONSTRAINT FK_Group_Project
        FOREIGN KEY (ProjectID)
        REFERENCES Project(ProjectID),

    CONSTRAINT FK_Group_Supervisor
        FOREIGN KEY (SupervisorID)
        REFERENCES Supervisor(SupervisorID)
);

CREATE TABLE Report (
    ReportID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    ProjectID UNIQUEIDENTIFIER NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description TEXT,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    DateCreated DATETIME NOT NULL DEFAULT GETDATE(),
    CommitAmount INT NOT NULL,
    ProgressByNumber INT NOT NULL,
    SupervisorComment TEXT,

    CONSTRAINT FK_Report_Project
        FOREIGN KEY (ProjectID)
        REFERENCES Project(ProjectID),

    CONSTRAINT FK_Report_CreatedBy
        FOREIGN KEY (CreatedBy)
        REFERENCES Account(AccountID)
);

CREATE TABLE GradePerProject (
    GradePerProjectId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    StudentId UNIQUEIDENTIFIER NOT NULL,
    ProjectId UNIQUEIDENTIFIER NOT NULL,
    Grade INT NOT NULL,

    CONSTRAINT FK_GradePerProject_Student
        FOREIGN KEY (StudentId)
        REFERENCES Student(StudentID),

    CONSTRAINT FK_GradePerProject_Project
        FOREIGN KEY (ProjectId)
        REFERENCES Project(ProjectID),

    CONSTRAINT UQ_GradePerProject_Student_Project
        UNIQUE (StudentId, ProjectId)
);


CREATE INDEX IX_Project_CreatedBy ON Project(CreatedBy);
CREATE INDEX IX_Group_ProjectID ON GroupTeam(ProjectID);
CREATE INDEX IX_Report_ProjectID ON Report(ProjectID);
CREATE INDEX IX_GradeOn_ProjectID ON GradePerProject(ProjectID);
CREATE INDEX IX_GradeOn_StudentID ON GradePerProject(StudentID);
