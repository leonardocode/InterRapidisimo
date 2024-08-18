-- Crear la base de datos
CREATE DATABASE InterRapidisimo;
GO

USE InterRapidisimo;
GO



-- Tabla de Estudiantes
CREATE TABLE Students (
    StudentID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
	State bit default 1,
    RegistrationDate DATE NOT NULL DEFAULT GETDATE(),
	RegistrationUpdate DATE NOT NULL DEFAULT GETDATE()
);


-- Tabla de Profesores
CREATE TABLE Professors (
    ProfessorID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
	State bit default 1,
    RegistrationDate DATE NOT NULL DEFAULT GETDATE(),
	RegistrationUpdate DATE NOT NULL DEFAULT GETDATE()
);

-- Tabla de Materias
CREATE TABLE Subjects (
    SubjectID INT PRIMARY KEY IDENTITY(1,1),
    SubjectName NVARCHAR(100) NOT NULL,
    Credits INT NOT NULL DEFAULT 3,
    ProfessorID INT,
    FOREIGN KEY (ProfessorID) REFERENCES Professors(ProfessorID),
	State bit default 1,
    RegistrationDate DATE NOT NULL DEFAULT GETDATE(),
	RegistrationUpdate DATE NOT NULL DEFAULT GETDATE()
);

-- Tabla de Inscripciones
CREATE TABLE Enrollments (
    EnrollmentID INT PRIMARY KEY IDENTITY(1,1),
    StudentID INT,
    SubjectID INT,
    EnrollmentDate DATE NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID),
	State bit default 1,
    RegistrationDate DATE NOT NULL DEFAULT GETDATE(),
	RegistrationUpdate DATE NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Errores (
	ErrorId INT IDENTITY(1,1) PRIMARY KEY,
	ErrorMessage NVARCHAR(4000),
	ErrorSeverity INT,
	ErrorState INT,
	ErrorTime DATETIME
);

-- Insertar datos de ejemplo para profesores
INSERT INTO Professors (FirstName, LastName, Email) VALUES 
('John', 'Doe', 'john.doe@university.edu'),
('Jane', 'Smith', 'jane.smith@university.edu'),
('Robert', 'Johnson', 'robert.johnson@university.edu'),
('Emily', 'Brown', 'emily.brown@university.edu'),
('Michael', 'Davis', 'michael.davis@university.edu');

-- Insertar datos de ejemplo para materias
INSERT INTO Subjects (SubjectName, ProfessorID) VALUES 
('Mathematics', 1),
('Physics', 1),
('Chemistry', 2),
('Biology', 2),
('Computer Science', 3),
('History', 3),
('Literature', 4),
('Philosophy', 4),
('Economics', 5),
('Psychology', 5);

select * from Students
--materias
select * from Subjects
select * from Professors
--inscripciones
select * from Enrollments
select * from LogsErroresAplicacion
select * from Errores





-- Procedimiento almacenado para inscribir a un estudiante en una materia
CREATE PROCEDURE EnrollStudent
    @StudentID INT,
    @SubjectID INT
AS
BEGIN
    -- Verificar si el estudiante ya está inscrito en 3 materias
    IF (SELECT COUNT(*) FROM Enrollments WHERE StudentID = @StudentID) >= 3
    BEGIN
        RAISERROR('El estudiante ya está inscrito en el máximo de materias permitidas.', 16, 1)
        RETURN
    END

    -- Verificar si el estudiante ya tiene una clase con el mismo profesor
    IF EXISTS (
        SELECT 1 
        FROM Enrollments e
        JOIN Subjects s 
		ON e.SubjectID = s.SubjectID
        WHERE e.StudentID = @StudentID
        AND s.ProfessorID = (SELECT ProfessorID FROM Subjects WHERE SubjectID = @SubjectID)
    )
    BEGIN
        RAISERROR('El estudiante ya tiene una clase con este profesor.', 16, 1)
        RETURN
    END

    -- Inscribir al estudiante
    INSERT INTO Enrollments (StudentID, SubjectID)
    VALUES (@StudentID, @SubjectID)
END
