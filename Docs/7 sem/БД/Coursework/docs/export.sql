-- MySQL Script generated by MySQL Workbench
-- Sun Dec 17 21:19:40 2023
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema guitars_db
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `guitars_db` DEFAULT CHARACTER SET utf8 COLLATE utf8_bin ;
USE `guitars_db` ;

-- -----------------------------------------------------
-- Table `guitars_db`.`GuitarManufacturer`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`GuitarManufacturer` (
  `idGuitarManufacturer` INT NOT NULL,
  `manufacturerInfo` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idGuitarManufacturer`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`Guitars`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`Guitars` (
  `idGuitars` INT NOT NULL,
  `guitarType` VARCHAR(45) NOT NULL,
  `stringCount` VARCHAR(45) NOT NULL,
  `bodyShape` VARCHAR(45) NOT NULL,
  `topDeckMaterial` VARCHAR(45) NOT NULL,
  `bodyMaterial` VARCHAR(45) NOT NULL,
  `colour` VARCHAR(45) NOT NULL,
  `lacquerCoating` TINYINT(1) NOT NULL,
  `idManufacturer` INT NOT NULL,
  `modelName` VARCHAR(45) NOT NULL,
  `averageRating` INT NOT NULL,
  `ratingVotes` INT NOT NULL,
  `iconUrl` VARCHAR(45) NULL,
  `photosUrl` VARCHAR(45) NULL,
  PRIMARY KEY (`idGuitars`),
  INDEX `idManufacturer_idx` (`idManufacturer` ASC) VISIBLE,
  INDEX `idxGuitarType` (`guitarType` ASC) VISIBLE,
  INDEX `idxModelName` (`modelName` ASC) INVISIBLE,
  INDEX `idxRating` (`averageRating` ASC) VISIBLE,
  INDEX `idxStringCount` (`stringCount` ASC) INVISIBLE,
  INDEX `idxBodyShape` (`bodyShape` ASC) INVISIBLE,
  INDEX `idxTopDeckMaterial` (`topDeckMaterial` ASC) INVISIBLE,
  INDEX `idxBodyMaterial` (`bodyMaterial` ASC) INVISIBLE,
  INDEX `idxColour` (`colour` ASC) INVISIBLE,
  INDEX `idxCoating` (`lacquerCoating` ASC) INVISIBLE,
  INDEX `idxRatingVotes` (`ratingVotes` ASC) VISIBLE,
  CONSTRAINT `Guitars_idManufacturer`
    FOREIGN KEY (`idManufacturer`)
    REFERENCES `guitars_db`.`GuitarManufacturer` (`idGuitarManufacturer`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`AccountRoles`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`AccountRoles` (
  `idRole` INT NOT NULL,
  `roleName` VARCHAR(45) NOT NULL,
  `roleTable` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idRole`),
  UNIQUE INDEX `roleName_UNIQUE` (`roleName` ASC) VISIBLE,
  UNIQUE INDEX `roleTable_UNIQUE` (`roleTable` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`Accounts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`Accounts` (
  `idAccount` INT NOT NULL,
  `idAccountRole` INT NOT NULL,
  `login` VARCHAR(45) NOT NULL,
  `passwordHash` VARCHAR(256) NOT NULL,
  `email` VARCHAR(45) NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idAccount`),
  INDEX `idxLogin` (`login` ASC) INVISIBLE,
  INDEX `idxEmail` (`email` ASC) VISIBLE,
  UNIQUE INDEX `login_UNIQUE` (`login` ASC) VISIBLE,
  UNIQUE INDEX `email_UNIQUE` (`email` ASC) VISIBLE,
  INDEX `accounts_idRole_idx` (`idAccountRole` ASC) VISIBLE,
  CONSTRAINT `accounts_idRole`
    FOREIGN KEY (`idAccountRole`)
    REFERENCES `guitars_db`.`AccountRoles` (`idRole`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`CustomersAccountInfo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`CustomersAccountInfo` (
  `idAccount` INT NOT NULL,
  `comments` VARCHAR(200) NOT NULL,
  `registrationDate` DATE NOT NULL,
  PRIMARY KEY (`idAccount`),
  CONSTRAINT `CustomersAccountInfo_idAccount`
    FOREIGN KEY (`idAccount`)
    REFERENCES `guitars_db`.`Accounts` (`idAccount`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`EmployeeRoles`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`EmployeeRoles` (
  `idRole` INT NOT NULL,
  `roleName` VARCHAR(45) NOT NULL,
  `roleTable` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idRole`),
  UNIQUE INDEX `roleName_UNIQUE` (`roleName` ASC) VISIBLE,
  UNIQUE INDEX `roleTable_UNIQUE` (`roleTable` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`EmployeeAccountInfo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`EmployeeAccountInfo` (
  `idAccount` INT NOT NULL,
  `employeeRole` INT NOT NULL,
  `rate` INT NOT NULL,
  `salary` BIGINT NOT NULL,
  PRIMARY KEY (`idAccount`),
  INDEX `idRole_idx` (`employeeRole` ASC) VISIBLE,
  CONSTRAINT `EmployeeAccountInfo_idAccount`
    FOREIGN KEY (`idAccount`)
    REFERENCES `guitars_db`.`Accounts` (`idAccount`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT,
  CONSTRAINT `EmployeeAccountInfo_idRole`
    FOREIGN KEY (`employeeRole`)
    REFERENCES `guitars_db`.`EmployeeRoles` (`idRole`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`CouriersAccountInfo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`CouriersAccountInfo` (
  `idAccount` INT NOT NULL,
  PRIMARY KEY (`idAccount`),
  CONSTRAINT `CouriersAccountInfo_idAccount`
    FOREIGN KEY (`idAccount`)
    REFERENCES `guitars_db`.`EmployeeAccountInfo` (`idAccount`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`Region`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`Region` (
  `idRegion` INT NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idRegion`),
  INDEX `idxName` (`name` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`City`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`City` (
  `idCity` INT NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  `idRegion` INT NOT NULL,
  PRIMARY KEY (`idCity`),
  INDEX `idRegion_idx` (`idRegion` ASC) VISIBLE,
  INDEX `idxName` (`name` ASC) VISIBLE,
  CONSTRAINT `City_idRegion`
    FOREIGN KEY (`idRegion`)
    REFERENCES `guitars_db`.`Region` (`idRegion`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`Street`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`Street` (
  `idStreet` INT NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idStreet`),
  INDEX `idxName` (`name` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`Address`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`Address` (
  `idAddress` INT NOT NULL,
  `idRegion` INT NOT NULL,
  `idCity` INT NULL,
  `idStreet` INT NOT NULL,
  `buildingNumber` INT NOT NULL,
  PRIMARY KEY (`idAddress`),
  INDEX `idxRegion` (`idRegion` ASC) VISIBLE,
  INDEX `idCity_idx` (`idCity` ASC) VISIBLE,
  INDEX `idStreet_idx` (`idStreet` ASC) VISIBLE,
  CONSTRAINT `Address_idRegion`
    FOREIGN KEY (`idRegion`)
    REFERENCES `guitars_db`.`Region` (`idRegion`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `Address_idCity`
    FOREIGN KEY (`idCity`)
    REFERENCES `guitars_db`.`City` (`idCity`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `Address_idStreet`
    FOREIGN KEY (`idStreet`)
    REFERENCES `guitars_db`.`Street` (`idStreet`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`CustomerOrders`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`CustomerOrders` (
  `idOrder` INT NOT NULL AUTO_INCREMENT,
  `idCustomer` INT NOT NULL,
  `idCourier` INT NOT NULL,
  `status` ENUM('not confirmed', 'confirmed', 'paid', 'delivered') NOT NULL,
  `comment` VARCHAR(200) NULL,
  `paymentType` ENUM('upon receipt', 'upon order') NOT NULL,
  `paymentMean` ENUM('card', 'cash') NOT NULL,
  `idAddress` INT NOT NULL,
  `date` DATE NOT NULL,
  PRIMARY KEY (`idOrder`),
  INDEX `idCustomer_idx` (`idCustomer` ASC) VISIBLE,
  INDEX `idCurier_idx` (`idCourier` ASC) VISIBLE,
  INDEX `idAddress_idx` (`idAddress` ASC) VISIBLE,
  INDEX `idxStatus` (`status` ASC) VISIBLE,
  INDEX `idxDate` (`date` ASC) VISIBLE,
  CONSTRAINT `CustomerOrders_idCustomer`
    FOREIGN KEY (`idCustomer`)
    REFERENCES `guitars_db`.`CustomersAccountInfo` (`idAccount`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT,
  CONSTRAINT `CustomerOrders_idCurier`
    FOREIGN KEY (`idCourier`)
    REFERENCES `guitars_db`.`CouriersAccountInfo` (`idAccount`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `CustomerOrders_idAddress`
    FOREIGN KEY (`idAddress`)
    REFERENCES `guitars_db`.`Address` (`idAddress`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`FavouriteGuitars`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`FavouriteGuitars` (
  `idAccount` INT NOT NULL,
  `idGuitar` INT NOT NULL,
  PRIMARY KEY (`idAccount`, `idGuitar`),
  INDEX `idGuitar_idx` (`idGuitar` ASC) VISIBLE,
  CONSTRAINT `FavouriteGuitars_idAccount`
    FOREIGN KEY (`idAccount`)
    REFERENCES `guitars_db`.`CustomersAccountInfo` (`idAccount`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT,
  CONSTRAINT `FavouriteGuitars_idGuitar`
    FOREIGN KEY (`idGuitar`)
    REFERENCES `guitars_db`.`Guitars` (`idGuitars`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`CustomerShoppingBasket`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`CustomerShoppingBasket` (
  `idAccount` INT NOT NULL,
  `idGuitar` INT NOT NULL,
  `guitarCount` INT NOT NULL,
  PRIMARY KEY (`idAccount`, `idGuitar`),
  INDEX `idGuitars_idx` (`idGuitar` ASC) VISIBLE,
  CONSTRAINT `CustomerShoppingBasket_idAccount`
    FOREIGN KEY (`idAccount`)
    REFERENCES `guitars_db`.`CustomersAccountInfo` (`idAccount`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT,
  CONSTRAINT `CustomerShoppingBasket_idGuitars`
    FOREIGN KEY (`idGuitar`)
    REFERENCES `guitars_db`.`Guitars` (`idGuitars`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`GuitarProviders`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`GuitarProviders` (
  `idProvider` INT NOT NULL,
  `organizationName` VARCHAR(45) NOT NULL,
  `comments` VARCHAR(200) NULL,
  PRIMARY KEY (`idProvider`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`ProvidedGuitars`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`ProvidedGuitars` (
  `idGuitar` INT NOT NULL,
  `idProvider` INT NOT NULL,
  `guitarCount` INT NOT NULL,
  `guitarPrice` BIGINT NOT NULL,
  PRIMARY KEY (`idGuitar`, `idProvider`),
  INDEX `idProvider_idx` (`idProvider` ASC) VISIBLE,
  CONSTRAINT `providerGuitars_idProvider`
    FOREIGN KEY (`idProvider`)
    REFERENCES `guitars_db`.`GuitarProviders` (`idProvider`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT,
  CONSTRAINT `providerGuitars_idGuitar`
    FOREIGN KEY (`idGuitar`)
    REFERENCES `guitars_db`.`Guitars` (`idGuitars`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`AdministratorsAccountInfo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`AdministratorsAccountInfo` (
  `idAccount` INT NOT NULL,
  PRIMARY KEY (`idAccount`),
  CONSTRAINT `AdministratorsAccountInfo_idAccount`
    FOREIGN KEY (`idAccount`)
    REFERENCES `guitars_db`.`EmployeeAccountInfo` (`idAccount`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`DirectorsAccountInfo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`DirectorsAccountInfo` (
  `idAccount` INT NOT NULL,
  PRIMARY KEY (`idAccount`),
  CONSTRAINT `DirectorsAccountInfo_idAccount`
    FOREIGN KEY (`idAccount`)
    REFERENCES `guitars_db`.`EmployeeAccountInfo` (`idAccount`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`ProvidersOrders`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`ProvidersOrders` (
  `idProviderOrder` INT NOT NULL,
  `date` DATE NOT NULL,
  `idProvider` INT NOT NULL,
  `totalPrice` BIGINT NOT NULL,
  `idAdministratorAuthor` INT NOT NULL,
  `idDirectorThatConfirmed` INT NULL,
  `status` ENUM('planning', 'confirmed', 'paid', 'delivered') NOT NULL,
  PRIMARY KEY (`idProviderOrder`),
  INDEX `idProvider_idx` (`idProvider` ASC) VISIBLE,
  INDEX `idAdministrator_idx` (`idAdministratorAuthor` ASC) VISIBLE,
  INDEX `idDirector_idx` (`idDirectorThatConfirmed` ASC) VISIBLE,
  INDEX `idxStatus` (`status` ASC) INVISIBLE,
  INDEX `idxDate` (`date` ASC) VISIBLE,
  CONSTRAINT `ProvidersOrders_idProvider`
    FOREIGN KEY (`idProvider`)
    REFERENCES `guitars_db`.`GuitarProviders` (`idProvider`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT,
  CONSTRAINT `ProvidersOrders_idAdministrator`
    FOREIGN KEY (`idAdministratorAuthor`)
    REFERENCES `guitars_db`.`AdministratorsAccountInfo` (`idAccount`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `ProvidersOrders_idDirector`
    FOREIGN KEY (`idDirectorThatConfirmed`)
    REFERENCES `guitars_db`.`DirectorsAccountInfo` (`idAccount`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`ProvidersOrdersList`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`ProvidersOrdersList` (
  `idProvidersOrders` INT NOT NULL,
  `idGuitar` INT NOT NULL,
  `count` INT NOT NULL,
  PRIMARY KEY (`idProvidersOrders`),
  INDEX `idGuitar_idx` (`idGuitar` ASC) VISIBLE,
  CONSTRAINT `ProvidersOrdersList_idGuitar`
    FOREIGN KEY (`idGuitar`)
    REFERENCES `guitars_db`.`Guitars` (`idGuitars`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `ProvidersOrdersList_idProvidersOrders`
    FOREIGN KEY (`idProvidersOrders`)
    REFERENCES `guitars_db`.`ProvidersOrders` (`idProviderOrder`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`EmployeeFines`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`EmployeeFines` (
  `idFine` INT NOT NULL,
  `idEmployee` INT NOT NULL,
  `fine` BIGINT NOT NULL,
  `reason` VARCHAR(45) NOT NULL,
  `date` DATE NOT NULL,
  PRIMARY KEY (`idFine`),
  INDEX `idEmployee_idx` (`idEmployee` ASC) VISIBLE,
  INDEX `idxDate` (`date` ASC) VISIBLE,
  CONSTRAINT `EmployeeFines_idEmployee`
    FOREIGN KEY (`idEmployee`)
    REFERENCES `guitars_db`.`EmployeeAccountInfo` (`idAccount`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`EmployeePremiums`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`EmployeePremiums` (
  `idPremium` INT NOT NULL,
  `idEmployee` INT NOT NULL,
  `premium` BIGINT NOT NULL,
  `reason` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idPremium`),
  INDEX `idEmployee_idx` (`idEmployee` ASC) VISIBLE,
  CONSTRAINT `EmployeePremiums_idEmployee`
    FOREIGN KEY (`idEmployee`)
    REFERENCES `guitars_db`.`EmployeeAccountInfo` (`idAccount`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`Passport`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`Passport` (
  `idPassport` INT NOT NULL,
  `idEmployeeAccount` INT NOT NULL,
  `firstName` VARCHAR(45) NOT NULL,
  `surname` VARCHAR(45) NULL,
  `patronymic` VARCHAR(45) NULL,
  `birthDate` DATE NOT NULL,
  `serialNumber` VARCHAR(45) NOT NULL,
  `idNumber` VARCHAR(45) NOT NULL,
  `isActive` TINYINT(1) NOT NULL,
  PRIMARY KEY (`idPassport`),
  INDEX `idEmployeeAccount_idx` (`idEmployeeAccount` ASC) VISIBLE,
  INDEX `idxIsActive` (`isActive` ASC) VISIBLE,
  CONSTRAINT `Passport_idEmployeeAccount`
    FOREIGN KEY (`idEmployeeAccount`)
    REFERENCES `guitars_db`.`EmployeeAccountInfo` (`idAccount`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`Reviews`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`Reviews` (
  `idGuitar` INT NOT NULL,
  `idAccount` INT NOT NULL,
  `text` VARCHAR(400) NOT NULL,
  `rating` INT NOT NULL,
  INDEX `idGuitar'_idx` (`idGuitar` ASC) VISIBLE,
  INDEX `idAccount_idx` (`idAccount` ASC) VISIBLE,
  PRIMARY KEY (`idGuitar`, `idAccount`),
  INDEX `idxRating` (`rating` ASC) VISIBLE,
  CONSTRAINT `Reviews_idGuitars`
    FOREIGN KEY (`idGuitar`)
    REFERENCES `guitars_db`.`Guitars` (`idGuitars`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `Reviews_idAccount`
    FOREIGN KEY (`idAccount`)
    REFERENCES `guitars_db`.`Accounts` (`idAccount`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`Storehouse`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`Storehouse` (
  `idGuitar` INT NOT NULL,
  `availableCount` INT NOT NULL,
  `soldCount` INT NOT NULL,
  `nextSupply` DATE NULL,
  `price` BIGINT NOT NULL,
  PRIMARY KEY (`idGuitar`),
  INDEX `idxCountGreaterZero` (`availableCount` ASC) VISIBLE,
  INDEX `idxAvailableCount` (`availableCount` ASC) VISIBLE,
  INDEX `idxPrice` (`price` ASC) VISIBLE,
  CONSTRAINT `Storehouse_idGuitar`
    FOREIGN KEY (`idGuitar`)
    REFERENCES `guitars_db`.`Guitars` (`idGuitars`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`CustomerOrderList`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`CustomerOrderList` (
  `idOrder` INT NOT NULL,
  `idGuitar` INT NOT NULL,
  `count` INT NOT NULL,
  `guitarPrice` INT NOT NULL,
  `guaranteeExpireDate` DATE NOT NULL,
  PRIMARY KEY (`idOrder`, `idGuitar`),
  INDEX `idGuitar_idx` (`idGuitar` ASC) VISIBLE,
  INDEX `idxGuarantee` (`guaranteeExpireDate` ASC) VISIBLE,
  CONSTRAINT `CustomerOrderList_idOrder`
    FOREIGN KEY (`idOrder`)
    REFERENCES `guitars_db`.`CustomerOrders` (`idOrder`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT,
  CONSTRAINT `CustomerOrderList_idGuitar`
    FOREIGN KEY (`idGuitar`)
    REFERENCES `guitars_db`.`Guitars` (`idGuitars`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `guitars_db`.`OrderArchive`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`OrderArchive` (
  `idOrderArchive` INT NOT NULL AUTO_INCREMENT,
  `idGuitar` INT NOT NULL,
  `count` INT NOT NULL,
  `guitarPrice` INT NOT NULL,
  `date` DATE NOT NULL,
  PRIMARY KEY (`idOrderArchive`),
  INDEX `idxDate` (`date` ASC) VISIBLE,
  INDEX `idGuitar_idx` (`idGuitar` ASC) VISIBLE,
  CONSTRAINT `OrderArchive_idGuitar`
    FOREIGN KEY (`idGuitar`)
    REFERENCES `guitars_db`.`Guitars` (`idGuitars`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

USE `guitars_db` ;

-- -----------------------------------------------------
-- Placeholder table for view `guitars_db`.`TopGuitars`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `guitars_db`.`TopGuitars` (`idGuitars` INT, `modelName` INT, `averageRating` INT, `ratingVotes` INT);

-- -----------------------------------------------------
-- procedure CreateOrder
-- -----------------------------------------------------

DELIMITER $$
USE `guitars_db`$$
CREATE PROCEDURE CreateOrder(
    idAccount INT, 
    idCourier INT, 
    comment VARCHAR(200), 
    paymentType ENUM('upon receipt', 'upon order'), 
    paymentMean ENUM('card', 'cash'), 
    idAddress INT)
BEGIN
    INSERT INTO `CustomerOrders` (idCustomer, idCourier, status, comment, paymentType, paymentMean, idAddress, date)
        VALUES (idAccount, idCourier, 'not confirmed', comment, paymentType, paymentMean, idAddress, DATE(NOW()));
END;$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure FillOrderFromBasket
-- -----------------------------------------------------

DELIMITER $$
USE `guitars_db`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `FillOrderFromBasket`(orderID INT)
BEGIN
    DECLARE user INT DEFAULT 0;
    SELECT idCustomer
        INTO user
        FROM CustomerOrders
        WHERE `idOrder` = orderID;
    
    INSERT INTO `CustomerOrderList` (`idOrder`, `idGuitar`, `count`, `guitarPrice`, `guaranteeExpireDate`)
        SELECT orderID, storehouse.idGuitar, `guitarCount`, `price`, '9999-12-31'
            FROM `CustomerShoppingBasket`
            JOIN Storehouse ON CustomerShoppingBasket.idGuitar = Storehouse.idGuitar
            WHERE idAccount = user;
            
    DELETE FROM CustomerShoppingBasket
        WHERE idAccount = user;
END$$

DELIMITER ;

-- -----------------------------------------------------
-- View `guitars_db`.`TopGuitars`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `guitars_db`.`TopGuitars`;
USE `guitars_db`;
CREATE  OR REPLACE VIEW `TopGuitars` AS
SELECT idGuitars, modelName, averageRating, ratingVotes
FROM Guitars
ORDER BY averageRating DESC, ratingVotes DESC
LIMIT 10;
USE `guitars_db`;

DELIMITER $$
USE `guitars_db`$$
CREATE DEFINER = CURRENT_USER TRIGGER `guitars_db`.`Accounts_BEFORE_INSERT` BEFORE INSERT ON `Accounts` FOR EACH ROW
BEGIN
	IF LENGTH(NEW.login) < 8 OR (SHA2(NEW.login, 256) = NEW.passwordHash) THEN
		SIGNAL SQLSTATE '45000' 
			SET MESSAGE_TEXT = 'Invalid credentials';
    END IF;
END$$

USE `guitars_db`$$
CREATE DEFINER = CURRENT_USER TRIGGER `guitars_db`.`CustomerOrders_BEFORE_INSERT` BEFORE INSERT ON `CustomerOrders` FOR EACH ROW
BEGIN
	SET NEW.date = DATE(NOW());
END$$

USE `guitars_db`$$
CREATE DEFINER = CURRENT_USER TRIGGER `guitars_db`.`CustomerOrders_BEFORE_UPDATE` BEFORE UPDATE ON `CustomerOrders` FOR EACH ROW
BEGIN
	IF NEW.status = 'delivered' THEN
		UPDATE `CustomerOrderList` SET `guaranteeExpireDate` = DATE(NOW())
			WHERE `idOrder` = OLD.idOrder;
    END IF;
END$$

USE `guitars_db`$$
CREATE DEFINER = CURRENT_USER TRIGGER `guitars_db`.`CustomerOrders_BEFORE_DELETE` BEFORE DELETE ON `CustomerOrders` FOR EACH ROW
BEGIN
	DECLARE countUnexpired INT;

    SELECT COUNT(*) INTO countUnexpired
        FROM CustomerOrderList
        WHERE idOrder = OLD.idOrder AND guaranteeExpireDate >= DATE(NOW());
    
    IF countUnexpired > 0 THEN
		SIGNAL SQLSTATE "45000" 
			SET MESSAGE_TEXT = "Cannot delete user with unexpired guarantees";
    END IF;
    
    IF OLD.status = "delivered" THEN
		INSERT INTO OrderArchive (idGuitar, count, guitarPrice, date)
			SELECT idGuitar, count, guitarPrice, OLD.date
			FROM CustomerOrderList
			WHERE idOrder = OLD.idOrder;
	END IF;
END$$

USE `guitars_db`$$
CREATE DEFINER = CURRENT_USER TRIGGER `guitars_db`.`ProvidersOrders_BEFORE_INSERT` BEFORE INSERT ON `ProvidersOrders` FOR EACH ROW
BEGIN
	SET NEW.date = NOW();
END$$

USE `guitars_db`$$
CREATE DEFINER = CURRENT_USER TRIGGER `guitars_db`.`EmployeeFines_BEFORE_INSERT` BEFORE INSERT ON `EmployeeFines` FOR EACH ROW
BEGIN
	SET NEW.date = NOW();
END$$

USE `guitars_db`$$
CREATE DEFINER = CURRENT_USER TRIGGER `guitars_db`.`Reviews_AFTER_INSERT` AFTER INSERT ON `Reviews` FOR EACH ROW
BEGIN
	DECLARE rating INT;
	DECLARE votes INT;
	
	SELECT averageRating, ratingVotes INTO rating, votes
        FROM Guitars
        WHERE idGuitars = NEW.idGuitar;
    
    SET rating = rating * votes;
    SET votes = votes + 1;
    
    SET rating = (rating + NEW.rating) / votes;
    
    UPDATE Guitars
        SET averageRating = rating, ratingVotes = votes
        WHERE idGuitars = NEW.idGuitar;
END$$

USE `guitars_db`$$
CREATE DEFINER = CURRENT_USER TRIGGER `guitars_db`.`OrderArchive_BEFORE_INSERT` BEFORE INSERT ON `OrderArchive` FOR EACH ROW
BEGIN
	IF NEW.date > NOW() THEN
		SIGNAL SQLSTATE '45000' 
			SET MESSAGE_TEXT = 'Cannot use specified date for done order';
    END IF;
END$$


DELIMITER ;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;


-- -----------------------------------------------------
-- Data for table `guitars_db`.`EmployeeRoles`
-- -----------------------------------------------------
START TRANSACTION;
USE `guitars_db`;
INSERT INTO `guitars_db`.`EmployeeRoles` (`idRole`, `roleName`, `roleTable`) VALUES (0, 'Director', 'DirectorsAccountInfo');
INSERT INTO `guitars_db`.`EmployeeRoles` (`idRole`, `roleName`, `roleTable`) VALUES (1, 'Administrator', 'AdministratorsAccountInfo');
INSERT INTO `guitars_db`.`EmployeeRoles` (`idRole`, `roleName`, `roleTable`) VALUES (2, 'Courier', 'CouriersAccountInfo');

COMMIT;

