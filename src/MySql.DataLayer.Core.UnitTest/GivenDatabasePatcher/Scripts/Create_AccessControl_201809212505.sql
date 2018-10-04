-- -----------------------------------------------------
-- Schema AccessControl
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema AccessControl
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `AccessControl` ;
USE `AccessControl` ;

-- -----------------------------------------------------
-- Table `AccessControl`.`Customers`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `AccessControl`.`Customers` (
  `Id` CHAR(36) NOT NULL,
  `FirstName` VARCHAR(150) NOT NULL,
  `LastName` VARCHAR(150) NOT NULL,
  `Gender` CHAR(1) NULL,
  `Birthday` DATETIME NOT NULL,
  `PhoneNumber` VARCHAR(20) NULL,
  `MobileNumber` VARCHAR(20) NOT NULL,
  `PictureUrl` VARCHAR(150) NULL,
  `Active` TINYINT(1) NOT NULL,
  `Deleted` TINYINT(1) NOT NULL,
  `CreatedAt` DATETIME NOT NULL,
  `UpdatedAt` DATETIME NULL,
  `DeletedAt` DATETIME NULL,
  PRIMARY KEY (`Id`))
 ;

-- -----------------------------------------------------
-- Table `AccessControl`.`CustomerAddresses`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `AccessControl`.`CustomerAddresses` (
  `Id` CHAR(36) NOT NULL,
  `CustomerId` CHAR(36) NOT NULL,
  `StreetName` VARCHAR(150) NOT NULL,
  `Number` VARCHAR(20) NOT NULL,
  `Neighborhood` VARCHAR(150) NOT NULL,
  `Complement` VARCHAR(45) NULL,
  `Zipcode` VARCHAR(20) NOT NULL,
  `CityId` CHAR(36) NOT NULL,
  `StateId` CHAR(36) NOT NULL,
  `CountryId` CHAR(36) NOT NULL,
  `Deleted` TINYINT(1) NOT NULL,
  `CreatedAt` DATETIME NOT NULL,
  `UpdatedAt` DATETIME NULL,
  `DeletedAt` DATETIME NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_CustomerAddresses_Customers_idx` (`CustomerId` ASC),
  CONSTRAINT `fk_CustomerAddresses_Customers`
    FOREIGN KEY (`CustomerId`)
    REFERENCES `AccessControl`.`Customers` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
 ;