-- -----------------------------------------------------
-- Schema AccessControl
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema AccessControl
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `AccessControl` ;
USE `AccessControl` ;

-- -----------------------------------------------------
-- Table `AccessControl`.`PartnerAddresses`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `AccessControl`.`PartnerAddresses` (
  `Id` CHAR(36) NOT NULL,
  `PartnerId` CHAR(36) NOT NULL,
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
  INDEX `fk_PartnerAddresses_Partners_idx` (`PartnerId` ASC),
  CONSTRAINT `fk_PartnerAddresses_Partners`
    FOREIGN KEY (`PartnerId`)
    REFERENCES `AccessControl`.`Partners` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
 ;


-- -----------------------------------------------------
-- Table `AccessControl`.`PartnerSystems`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `AccessControl`.`PartnerSystems` (
  `Id` CHAR(36) NOT NULL,
  `PartnerId` CHAR(36) NOT NULL,
  `Name` VARCHAR(150) NOT NULL,
  `Active` TINYINT(1) NOT NULL,
  `Deleted` TINYINT(1) NOT NULL,
  `CreatedAt` DATETIME NOT NULL,
  `UpdatedAt` DATETIME NULL,
  `DeletedAt` DATETIME NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_PartnerSystems_Partners_idx` (`PartnerId` ASC),
  CONSTRAINT `fk_PartnerSystems_Partners`
    FOREIGN KEY (`PartnerId`)
    REFERENCES `AccessControl`.`Partners` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
 ;