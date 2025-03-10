-- Creación de la base de datos
CREATE DATABASE LoginSystem;
GO

USE LoginSystem;
GO

-- Creación de tablas
-- Tabla Rol
CREATE TABLE Rol (
    RolId INT PRIMARY KEY IDENTITY(1,1),
    RolName VARCHAR(50) NOT NULL,
    AuditCreateUser INT NULL,
    AuditCreateDate DATETIME NULL,
    AuditUpdateUser INT NULL,
    AuditUpdateDate DATETIME NULL,
    AuditDeleteUser INT NULL,
    AuditDeleteDate DATETIME NULL
);
GO

-- Tabla RolOpciones
CREATE TABLE RolOptions (
    OptionId INT PRIMARY KEY IDENTITY(1,1),
    OptionName VARCHAR(50) NOT NULL,
    AuditCreateUser INT NULL,
    AuditCreateDate DATETIME NULL,
    AuditUpdateUser INT NULL,
    AuditUpdateDate DATETIME NULL,
    AuditDeleteUser INT NULL,
    AuditDeleteDate DATETIME NULL
);
GO

-- Tabla Persona
CREATE TABLE Person (
    PersonId INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(80) NOT NULL,
    LastName VARCHAR(80) NOT NULL,
    IdentityCard VARCHAR(10) NOT NULL UNIQUE,
    BirthDate DATE NULL,
    AuditCreateUser INT NULL,
    AuditCreateDate DATETIME NULL,
    AuditUpdateUser INT NULL,
    AuditUpdateDate DATETIME NULL,
    AuditDeleteUser INT NULL,
    AuditDeleteDate DATETIME NULL
);
GO

-- Tabla Usuarios
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(20) NOT NULL UNIQUE,
    Password VARCHAR(MAX) NOT NULL,
    Mail VARCHAR(120) NOT NULL UNIQUE,
    SessionActive CHAR(1) DEFAULT '0',
    PersonId INT NULL,
    Status VARCHAR(20) DEFAULT 'Active',
    FailedAttempts INT DEFAULT 0,
    AuditCreateUser INT NULL,
    AuditCreateDate DATETIME NULL,
    AuditUpdateUser INT NULL,
    AuditUpdateDate DATETIME NULL,
    AuditDeleteUser INT NULL,
    AuditDeleteDate DATETIME NULL,
    CONSTRAINT FK_Users_Person FOREIGN KEY (PersonId) REFERENCES Person(PersonId),
	CONSTRAINT UQ_Users_PersonId UNIQUE (PersonId)
);
GO

-- Tabla Sesiones_usuarios
CREATE TABLE User_sessions (
    SessionId INT PRIMARY KEY IDENTITY(1,1),
    EntryDate DATETIME NOT NULL,
    CloseDate DATETIME NULL,
    UserId INT NOT NULL,
    AuditCreateUser INT NULL,
    AuditCreateDate DATETIME NULL,
    AuditUpdateUser INT NULL,
    AuditUpdateDate DATETIME NULL,
    AuditDeleteUser INT NULL,
    AuditDeleteDate DATETIME NULL,
    CONSTRAINT FK_UserSessions_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
GO

-- Tabla relacional rol_usuarios
CREATE TABLE User_rol (
    RolId INT NOT NULL,
    UserId INT NOT NULL,
    AuditCreateUser INT NULL,
    AuditCreateDate DATETIME NULL,
    AuditUpdateUser INT NULL,
    AuditUpdateDate DATETIME NULL,
    AuditDeleteUser INT NULL,
    AuditDeleteDate DATETIME NULL,
    PRIMARY KEY (RolId, UserId),
    CONSTRAINT FK_UserRol_Rol FOREIGN KEY (RolId) REFERENCES Rol(RolId),
    CONSTRAINT FK_UserRol_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
GO

-- Tabla relacional rol_rolOpciones
CREATE TABLE Rol_rolOptions (
    RolId INT NOT NULL,
    OptionId INT NOT NULL,
    AuditCreateUser INT NULL,
    AuditCreateDate DATETIME NULL,
    AuditUpdateUser INT NULL,
    AuditUpdateDate DATETIME NULL,
    AuditDeleteUser INT NULL,
    AuditDeleteDate DATETIME NULL,
    PRIMARY KEY (RolId, OptionId),
    CONSTRAINT FK_RolRolOptions_Rol FOREIGN KEY (RolId) REFERENCES Rol(RolId),
    CONSTRAINT FK_RolRolOptions_RolOptions FOREIGN KEY (OptionId) REFERENCES RolOptions(OptionId)
);
GO

CREATE PROCEDURE StartUserSession
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar si el usuario ya tiene una sesión activa
    IF EXISTS (SELECT 1 FROM User_sessions WHERE UserId = @UserId AND CloseDate IS NULL)
    BEGIN
        -- Si ya tiene una sesión activa, devolver un error
        RAISERROR ('El usuario ya tiene una sesión activa.', 16, 1);
        RETURN;
    END

    -- Insertar nueva sesión
    INSERT INTO User_sessions (EntryDate, UserId, AuditCreateDate)
    VALUES (GETDATE(), @UserId, GETDATE());

    -- Marcar al usuario como activo
    UPDATE Users SET SessionActive = '1' WHERE UserId = @UserId;
END;
GO

CREATE PROCEDURE CloseUserSession
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Cerrar la sesión activa del usuario
    UPDATE User_sessions
    SET CloseDate = GETDATE(), AuditUpdateDate = GETDATE()
    WHERE UserId = @UserId AND CloseDate IS NULL;

    -- Marcar al usuario como inactivo
    UPDATE Users SET SessionActive = '0' WHERE UserId = @UserId;
END;
GO 

CREATE PROCEDURE GetActiveSessions
AS
BEGIN
    SELECT U.UserId, U.Username, U.SessionActive, 
           S.EntryDate, S.CloseDate
    FROM Users U
    LEFT JOIN User_sessions S ON U.UserId = S.UserId AND S.CloseDate IS NULL;
END;

GO
INSERT INTO Rol (RolName, AuditCreateDate)
VALUES ('ADMIN', GETDATE()),
       ('USER', GETDATE());


GO
DECLARE @PersonId INT, @UserId INT, @RolId INT;

-- Insertar la persona en la tabla Person
INSERT INTO Person (FirstName, LastName, IdentityCard, BirthDate, AuditCreateUser, AuditCreateDate)
VALUES ('Sebastian Ricardo', 'Rodriguez', '1231231231', '1990-01-01', 1, GETDATE());

-- Obtener el ID de la persona recién insertada
SET @PersonId = SCOPE_IDENTITY();

-- Insertar el usuario en la tabla Users
INSERT INTO Users (Username, Password, Mail, SessionActive, PersonId, Status, FailedAttempts, AuditCreateUser, AuditCreateDate)
VALUES ('Administrador100', '$2a$11$HLGFRyxb8MMYLl.Ohbiquep96IpdEfieT.jX.YgdYt0sbZzp6d.DO', 'srodriguez@mail.com', NULL, @PersonId, 'Active', NULL, 1, GETDATE());

-- Obtener el ID del usuario recién insertado
SET @UserId = SCOPE_IDENTITY();

-- Obtener el ID del rol ADMIN
SELECT @RolId = RolId FROM Rol WHERE RolName = 'ADMIN';

-- Asignar el rol ADMIN al usuario
INSERT INTO User_rol (RolId, UserId, AuditCreateUser, AuditCreateDate)
VALUES (@RolId, @UserId, 1, GETDATE());

GO


