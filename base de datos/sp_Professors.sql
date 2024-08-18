CREATE OR ALTER PROCEDURE sp_Professors
    @opcion int,
    @id int = 0,
    @firstName varchar(100) = '',
    @lastName varchar(100) = '',
    @email varchar(100) = '',
    @state bit = 1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();

    BEGIN TRY
        BEGIN TRANSACTION;

        IF @opcion = 1
        BEGIN
            SELECT 
            ProfessorID,
            FirstName,
            LastName,
            Email,
            State
            FROM Professors

            SELECT 'OK' as Respuesta
        END

        IF @opcion = 2
        BEGIN
            IF NOT EXISTS(SELECT 1 FROM Professors WHERE ProfessorID = @id)
            BEGIN
                RAISERROR ('El profesor no existe en la base de datos', 16, 1);
                INSERT INTO Errores (ErrorMessage, ErrorSeverity, ErrorState, ErrorTime)
                VALUES ('El profesor no existe en la base de datos', @ErrorSeverity, @ErrorState, GETDATE());

                SELECT 'Error: El profesor no existe en la base de datos' as Respuesta
                RETURN;
            END
            ELSE
            BEGIN
                SELECT 
                ProfessorID,
                FirstName,
                LastName,
                Email,
                State
                FROM Professors
                WHERE ProfessorID = @id

                SELECT 'OK' as Respuesta
            END
        END

        IF @opcion = 3
        BEGIN
            IF EXISTS (SELECT * FROM Professors WHERE FirstName = @firstName AND LastName = @lastName AND Email = @email)
            BEGIN
                RAISERROR ('El profesor ya existe en nuestra base de datos', 16, 1);
                INSERT INTO Errores (ErrorMessage, ErrorSeverity, ErrorState, ErrorTime)
                VALUES ('El profesor ya existe en nuestra base de datos', @ErrorSeverity, @ErrorState, GETDATE());

                SELECT 'Error: El profesor ya existe en nuestra base de datos' as Respuesta
                RETURN;
            END

            INSERT INTO Professors(FirstName, LastName, Email, RegistrationDate) 
            VALUES (@firstName, @lastName, @email, GETDATE());

            SELECT 'OK' as Respuesta
        END

        IF @opcion = 4
        BEGIN
            IF NOT EXISTS (SELECT * FROM Professors WHERE ProfessorID = @id)
            BEGIN
                RAISERROR ('El profesor no existe en nuestra base de datos', 16, 1);
                INSERT INTO Errores (ErrorMessage, ErrorSeverity, ErrorState, ErrorTime)
                VALUES ('El profesor no existe en nuestra base de datos', @ErrorSeverity, @ErrorState, GETDATE());

                SELECT 'Error: El profesor no existe en nuestra base de datos' as Respuesta
                RETURN;
            END

            UPDATE Professors
            SET FirstName = @firstName,
                LastName = @lastName,
                Email = @email,
                State = @state,
                RegistrationUpdate = GETDATE()
            WHERE ProfessorID = @id;

            SELECT 'OK' as Respuesta
        END

        IF @opcion = 5
        BEGIN
            IF NOT EXISTS (SELECT * FROM Professors WHERE ProfessorID = @id)
            BEGIN
                RAISERROR ('El profesor no existe en nuestra base de datos', 16, 1);
                INSERT INTO Errores (ErrorMessage, ErrorSeverity, ErrorState, ErrorTime)
                VALUES ('El profesor no existe en nuestra base de datos', @ErrorSeverity, @ErrorState, GETDATE());

                SELECT 'Error: El profesor no existe en nuestra base de datos' as Respuesta
                RETURN;
            END

            UPDATE Professors
            SET State = 0,
                RegistrationUpdate = GETDATE()
            WHERE ProfessorID = @id;

            SELECT 'OK' as Respuesta
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        INSERT INTO Errores (ErrorMessage, ErrorSeverity, ErrorState, ErrorTime)
        VALUES (@ErrorMessage, @ErrorSeverity, @ErrorState, GETDATE());

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO