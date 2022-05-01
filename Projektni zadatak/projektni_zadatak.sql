-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3306
-- Generation Time: May 01, 2022 at 06:18 PM
-- Server version: 5.5.54
-- PHP Version: 7.4.26

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `projektni`
--

-- --------------------------------------------------------

--
-- Table structure for table `artikal`
--

DROP TABLE IF EXISTS `artikal`;
CREATE TABLE IF NOT EXISTS `artikal` (
  `artikal_id` int(11) NOT NULL AUTO_INCREMENT,
  `naziv_artikla` varchar(40) DEFAULT NULL,
  `vrsta_artikla` varchar(40) DEFAULT NULL,
  `cijena` double DEFAULT NULL,
  PRIMARY KEY (`artikal_id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `artikal`
--

INSERT INTO `artikal` (`artikal_id`, `naziv_artikla`, `vrsta_artikla`, `cijena`) VALUES
(1, 'Playstation 5', 'Konzola', 1200),
(2, 'Samsung Galaxy S20 FE', 'Mobitel', 690),
(3, 'Apple Airpods', 'Slušalice', 150),
(4, 'Logitech G220', 'Miš', 60),
(5, 'LG Ultrafine 5K', 'Monitor', 1000);

-- --------------------------------------------------------

--
-- Table structure for table `kupac`
--

DROP TABLE IF EXISTS `kupac`;
CREATE TABLE IF NOT EXISTS `kupac` (
  `kupac_id` int(11) NOT NULL AUTO_INCREMENT,
  `ime` varchar(40) DEFAULT NULL,
  `prezime` varchar(40) DEFAULT NULL,
  `grad` varchar(40) DEFAULT NULL,
  `adresa` varchar(40) DEFAULT NULL,
  `telefon` varchar(40) DEFAULT NULL,
  `username` varchar(40) DEFAULT NULL,
  `password` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`kupac_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `kupac`
--

INSERT INTO `kupac` (`kupac_id`, `ime`, `prezime`, `grad`, `adresa`, `telefon`, `username`, `password`) VALUES
(1, 'Ensar', 'Hodžić', 'Sarajevo', 'Trg nezavisnosti 18', '061919809', 'ensar.h', '123'),
(2, 'Emel', 'Hasanovic', 'Sarajevo', 'Sedrenik 13', '0617785', 'emelg', 'emel'),
(3, 'Enis', 'Tvrtkovic', 'Sarajevo', 'Briješce', '061454578', 'enki', '123'),
(4, 'Nermina', 'Salihovic', 'Sarajevo', 'Briješce 57', '06267456', 'nermica', 'sale');

-- --------------------------------------------------------

--
-- Table structure for table `narudzbenica`
--

DROP TABLE IF EXISTS `narudzbenica`;
CREATE TABLE IF NOT EXISTS `narudzbenica` (
  `narudzbenica_id` int(11) NOT NULL AUTO_INCREMENT,
  `kupac_id` int(30) DEFAULT NULL,
  `datum_narudzbe` date DEFAULT NULL,
  PRIMARY KEY (`narudzbenica_id`),
  KEY `kupac_id` (`kupac_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `narudzbenica`
--

INSERT INTO `narudzbenica` (`narudzbenica_id`, `kupac_id`, `datum_narudzbe`) VALUES
(2, 2, '2022-04-30');

-- --------------------------------------------------------

--
-- Table structure for table `skladiste`
--

DROP TABLE IF EXISTS `skladiste`;
CREATE TABLE IF NOT EXISTS `skladiste` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `artikal_id` int(11) DEFAULT NULL,
  `kolicina_stanje` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `artikal_id` (`artikal_id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `skladiste`
--

INSERT INTO `skladiste` (`id`, `artikal_id`, `kolicina_stanje`) VALUES
(1, 1, 54),
(2, 2, 31),
(3, 3, 39),
(4, 4, 75),
(5, 5, 20);

-- --------------------------------------------------------

--
-- Table structure for table `stavka_narudzbenice`
--

DROP TABLE IF EXISTS `stavka_narudzbenice`;
CREATE TABLE IF NOT EXISTS `stavka_narudzbenice` (
  `stavka_id` int(11) NOT NULL AUTO_INCREMENT,
  `narudzbenica_id` int(11) DEFAULT NULL,
  `artikal_id` int(11) DEFAULT NULL,
  `kolicina` int(11) DEFAULT NULL,
  PRIMARY KEY (`stavka_id`),
  KEY `artikal_id` (`artikal_id`),
  KEY `stavka_narudzbenice_ibfk_1` (`narudzbenica_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `stavka_narudzbenice`
--

INSERT INTO `stavka_narudzbenice` (`stavka_id`, `narudzbenica_id`, `artikal_id`, `kolicina`) VALUES
(1, 2, 1, 1);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `narudzbenica`
--
ALTER TABLE `narudzbenica`
  ADD CONSTRAINT `narudzbenica_ibfk_1` FOREIGN KEY (`kupac_id`) REFERENCES `kupac` (`kupac_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints for table `skladiste`
--
ALTER TABLE `skladiste`
  ADD CONSTRAINT `skladiste_ibfk_1` FOREIGN KEY (`artikal_id`) REFERENCES `artikal` (`artikal_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints for table `stavka_narudzbenice`
--
ALTER TABLE `stavka_narudzbenice`
  ADD CONSTRAINT `stavka_narudzbenice_ibfk_1` FOREIGN KEY (`narudzbenica_id`) REFERENCES `narudzbenica` (`narudzbenica_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `stavka_narudzbenice_ibfk_2` FOREIGN KEY (`artikal_id`) REFERENCES `artikal` (`artikal_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
