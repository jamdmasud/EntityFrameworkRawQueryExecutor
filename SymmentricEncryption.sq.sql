 CREATE MASTER KEY ENCRYPTION BY PASSWORD = '141'
 
 CREATE CERTIFICATE TodoCertification
WITH SUBJECT = 'TodoTestCertificate'


CREATE SYMMETRIC KEY TodoSymmetricKey WITH
IDENTITY_VALUE = 'sa',
ALGORITHM = AES_256,
KEY_SOURCE = '141'
ENCRYPTION BY CERTIFICATE TodoCertification

CREATE PROCEDURE OpenKeys
AS
BEGIN 
    BEGIN TRY
        OPEN SYMMETRIC KEY TodoSymmetricKey
        DECRYPTION BY CERTIFICATE TodoCertification
    END TRY
    BEGIN CATCH
        -- Handle non-existant key here
    END CATCH
END

CREATE PROCEDURE ColseKeys
AS
BEGIN 
    BEGIN TRY
        CLOSE SYMMETRIC KEY TodoSymmetricKey
    END TRY
    BEGIN CATCH
        -- Handle non-existant key here
    END CATCH
END

CREATE FUNCTION Encrypt
(
    @ValueToEncrypt nvarchar(max)
)
RETURNS varbinary(max)
AS
BEGIN
    -- Declare the return variable here
    DECLARE @Result varbinary(max)

    SET @Result = EncryptByKey(Key_GUID('TodoSymmetricKey'), N''+@ValueToEncrypt+'')

    -- Return the result of the function
    RETURN @Result
END

CREATE FUNCTION Decrypt
(
    @ValueToDecrypt varbinary(max)
)
RETURNS nvarchar(max)
AS
BEGIN

    DECLARE @Result nvarchar(max)

    SET @Result = DecryptByKey(@ValueToDecrypt)

    RETURN @Result
END
 
EXEC OpenKeys
select dbo.Encrypt('second way you can do so is via a database transaction that essentially')
select dbo.Decrypt(dbo.Encrypt('second way you can do so is via a database transaction that essentially'))
EXEC ColseKeys 