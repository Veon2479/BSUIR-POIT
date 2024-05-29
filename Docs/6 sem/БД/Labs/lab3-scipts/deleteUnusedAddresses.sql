CREATE PROCEDURE `deleteUnusedAddresses` ()
BEGIN
	DELETE
		FROM Adress
        WHERE id
			NOT IN (SELECT DISTINCT idAddress FROM Passport UNION SELECT DISTINCT idAddress FROM Training);
END