-- -----------------------------------------------------
-- Schema AccessControl
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema AccessControl
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `AccessControl` ;
USE `AccessControl` ;

-- -----------------------------------------------------
-- Table `AccessControl`.`Partners`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `AccessControl`.`Partners` (
  `Id` CHAR(36) NOT NULL,
  `FantasyName` VARCHAR(150) NULL,
  `Identification` VARCHAR(50) NOT NULL,
  `LegalName` VARCHAR(150) NOT NULL,
  `StateRegistration` VARCHAR(30) NULL,
  `Active` TINYINT(1) NOT NULL,
  `Deleted` TINYINT(1) NOT NULL,
  `CreatedAt` DATETIME NOT NULL,
  `UpdatedAt` DATETIME NULL,
  `DeletedAt` DATETIME NULL,
  PRIMARY KEY (`Id`))
 ;

