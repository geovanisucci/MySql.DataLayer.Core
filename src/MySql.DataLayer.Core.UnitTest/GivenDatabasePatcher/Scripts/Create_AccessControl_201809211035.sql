-- -----------------------------------------------------
-- Schema AccessControl
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema AccessControl
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `AccessControl` ;
USE `AccessControl` ;


-- -----------------------------------------------------
-- Table `AccessControl`.`PartnerContacts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `AccessControl`.`PartnerContacts` (
  `Id` CHAR(36) NOT NULL,
  `PartnerId` CHAR(36) NOT NULL,
  `Name` VARCHAR(150) NOT NULL,
  `PhoneNumber` VARCHAR(20) NULL,
  `MobileNumber` VARCHAR(20) NOT NULL,
  `Deleted` TINYINT(1) NOT NULL,
  `CreatedAt` DATETIME NOT NULL,
  `UpdatedAt` DATETIME NULL,
  `DeletedAt` DATETIME NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_PartnerContacts_Partners_idx` (`PartnerId` ASC),
  CONSTRAINT `fk_PartnerContacts_Partners`
    FOREIGN KEY (`PartnerId`)
    REFERENCES `AccessControl`.`Partners` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
 ;
