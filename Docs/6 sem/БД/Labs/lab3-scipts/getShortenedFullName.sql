CREATE FUNCTION `getShortenedFullName` (id INT) RETURNS VARCHAR(255)
BEGIN
	DECLARE full_name VARCHAR(255);
	SELECT CONCAT(surname, ' ', LEFT(firstName, 1), '. ', LEFT(patronymic, 1), '.') 
		INTO full_name 
        FROM Passport
        WHERE idPassport = id;
    RETURN full_name;
END