SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL';

DROP SCHEMA IF EXISTS `flowManager` ;
CREATE SCHEMA IF NOT EXISTS `flowManager` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
USE `flowManager` ;

-- -----------------------------------------------------
-- Table `flowManager`.`Relation`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `flowManager`.`Relation` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `owner` INT NOT NULL ,
  `origin` INT NOT NULL ,
  `destination` INT NOT NULL ,
  PRIMARY KEY (`id`) ,
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `flowManager`.`Element`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `flowManager`.`Element` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `owner` INT NOT NULL ,
  `name` VARCHAR(120) NOT NULL ,
  `enabled` TINYINT(1) NOT NULL ,
  `elementType` VARCHAR(80) NOT NULL ,
  `left` INT NOT NULL ,
  `top` INT NOT NULL ,
  `width` INT NOT NULL ,
  `height` INT NOT NULL ,
  PRIMARY KEY (`id`) ,
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `flowManager`.`Workflow`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `flowManager`.`Workflow` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `name` VARCHAR(120) NOT NULL ,
  `startElement` INT NOT NULL ,
  `finishElement` INT NOT NULL ,
  PRIMARY KEY (`id`) ,
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) )
ENGINE = InnoDB;

USE `flowManager` ;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
