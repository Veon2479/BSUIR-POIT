CREATE PROCEDURE `updateExpiringContracts` ()
BEGIN
	UPDATE Contract 
		SET isActive = 1 
        WHERE isActive = 0 AND dateExpiring < CURDATE();
END