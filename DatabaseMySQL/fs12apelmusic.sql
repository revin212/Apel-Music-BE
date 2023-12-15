-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 11, 2023 at 04:37 PM
-- Server version: 10.4.27-MariaDB
-- PHP Version: 8.2.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `fs12apelmusic`
--

-- --------------------------------------------------------

--
-- Table structure for table `mscategory`
--

CREATE TABLE `mscategory` (
  `Id` varchar(50) NOT NULL DEFAULT uuid(),
  `Name` varchar(250) NOT NULL,
  `Title` varchar(250) NOT NULL,
  `Description` text NOT NULL,
  `Image` varchar(250) DEFAULT NULL,
  `HeaderImage` varchar(250) DEFAULT NULL,
  `IsActivated` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `mscategory`
--

INSERT INTO `mscategory` (`Id`, `Name`, `Title`, `Description`, `Image`, `HeaderImage`, `IsActivated`) VALUES
('5f9be1f3-884a-11ee-b59a-3c5282e16d0b', 'Drum', 'Drummer Class', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 11.png', '\r\n/images/image 3-1.png', 1),
('5fa20198-884a-11ee-b59a-3c5282e16d0b', 'Piano', 'Piano Class', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 12.png', NULL, 1),
('5fa202ee-884a-11ee-b59a-3c5282e16d0b', 'Gitar', 'Gitar Class', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 13.png', NULL, 1),
('5fa203ae-884a-11ee-b59a-3c5282e16d0b', 'Bass', 'Bass Class', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 13.png', NULL, 1),
('5fa204c4-884a-11ee-b59a-3c5282e16d0b', 'Biola', 'Biola Class', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 14.png', NULL, 1),
('5fa2057a-884a-11ee-b59a-3c5282e16d0b', 'Menyanyi', 'Singing Class', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 15.png', NULL, 1),
('5fa205e9-884a-11ee-b59a-3c5282e16d0b', 'Flute', 'Flute Class', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 16.png', NULL, 1),
('b2c3711b-884b-11ee-b59a-3c5282e16d0b', 'Saxophone', 'Saxophone Class', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 16.png', NULL, 1);

-- --------------------------------------------------------

--
-- Table structure for table `mscourse`
--

CREATE TABLE `mscourse` (
  `Id` varchar(50) NOT NULL DEFAULT uuid(),
  `Name` varchar(250) NOT NULL,
  `Description` text NOT NULL,
  `Image` varchar(250) DEFAULT NULL,
  `Price` double NOT NULL,
  `CategoryId` varchar(50) NOT NULL,
  `IsActivated` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `mscourse`
--

INSERT INTO `mscourse` (`Id`, `Name`, `Description`, `Image`, `Price`, `CategoryId`, `IsActivated`) VALUES
('b3e55d96-884a-11ee-b59a-3c5282e16d0b', 'Kursus Drummer Special Coach (Eno Netral)', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 12-7.png', 8500000, '5f9be1f3-884a-11ee-b59a-3c5282e16d0b', 1),
('b3e79c28-884a-11ee-b59a-3c5282e16d0b', '[Beginner] Guitar class for kids ', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 12-2.png', 1600000, '5fa202ee-884a-11ee-b59a-3c5282e16d0b', 1),
('b3e79d1f-884a-11ee-b59a-3c5282e16d0b', 'Biola Mid-Level Course ', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 12-3.png', 3000000, '5fa204c4-884a-11ee-b59a-3c5282e16d0b', 1),
('b3e79da3-884a-11ee-b59a-3c5282e16d0b', 'Drummer for kids (Level Basic/1)', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 12-10.png', 2200000, '5f9be1f3-884a-11ee-b59a-3c5282e16d0b', 1),
('b3e79e06-884a-11ee-b59a-3c5282e16d0b', 'Kursus Piano : From Zero to Pro (Full Package)', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 12-5.png', 11650000, '5fa20198-884a-11ee-b59a-3c5282e16d0b', 1),
('b3e79e7b-884a-11ee-b59a-3c5282e16d0b', 'Expert Level Saxophone', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 12-6.png', 7350000, 'b2c3711b-884b-11ee-b59a-3c5282e16d0b', 1),
('b3e79f03-884a-11ee-b59a-3c5282e16d0b', 'Expert Level Drummer Lessons', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 12-8.png', 5450000, '5f9be1f3-884a-11ee-b59a-3c5282e16d0b', 1),
('b3e79ff7-884a-11ee-b59a-3c5282e16d0b', 'From Zero to Professional Drummer (Complit Package)', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', '/images/Rectangle 12-9.png', 13000000, '5f9be1f3-884a-11ee-b59a-3c5282e16d0b', 1);

-- --------------------------------------------------------

--
-- Table structure for table `mspaymentmethod`
--

CREATE TABLE `mspaymentmethod` (
  `Id` varchar(50) NOT NULL DEFAULT uuid(),
  `Name` varchar(250) NOT NULL,
  `Image` varchar(250) DEFAULT NULL,
  `IsActivated` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `mspaymentmethod`
--

INSERT INTO `mspaymentmethod` (`Id`, `Name`, `Image`, `IsActivated`) VALUES
('3e544b7c-884a-11ee-b59a-3c5282e16d0b', 'Gopay', '/images/Gopay.png', 1),
('3e59cc0b-884a-11ee-b59a-3c5282e16d0b', 'OVO', '/images/OVO.png', 1),
('3e59cd02-884a-11ee-b59a-3c5282e16d0b', 'DANA', '/images/DANA.png', 1),
('3e59cd9f-884a-11ee-b59a-3c5282e16d0b', 'Mandiri', '/images/Mandiri.png', 1),
('3e59ce13-884a-11ee-b59a-3c5282e16d0b', 'BCA', '/images/BCA.png', 1),
('3e59ced4-884a-11ee-b59a-3c5282e16d0b', 'BNI', '/images/BNI.png', 1);

-- --------------------------------------------------------

--
-- Table structure for table `msrole`
--

CREATE TABLE `msrole` (
  `id` int(2) NOT NULL,
  `Name` varchar(20) NOT NULL DEFAULT ''
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `msrole`
--

INSERT INTO `msrole` (`id`, `Name`) VALUES
(1, 'Admin'),
(2, 'User');

-- --------------------------------------------------------

--
-- Table structure for table `msuser`
--

CREATE TABLE `msuser` (
  `Id` varchar(50) NOT NULL DEFAULT uuid(),
  `Name` varchar(250) NOT NULL,
  `Email` varchar(250) NOT NULL,
  `Password` varchar(250) NOT NULL,
  `Role` int(2) NOT NULL DEFAULT 2,
  `IsActivated` tinyint(1) DEFAULT 0,
  `CreatedAt` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `RefreshToken` varchar(250) DEFAULT NULL,
  `RefreshTokenExpires` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `msuser`
--

INSERT INTO `msuser` (`Id`, `Name`, `Email`, `Password`, `Role`, `IsActivated`, `CreatedAt`, `RefreshToken`, `RefreshTokenExpires`) VALUES
('4c6ff0c9-95e5-11ee-9ec7-7cef3d6236e7', 'firman', 'sco1oter@gmail.com', '$2a$11$Z.tm2Bml6S0AC/Naexgg4Of4J98RRKneh93kvCcDSangnp7P5ughy', 2, 1, '2023-12-11 15:06:57', 'UJBa+BUlrGWfWaT8gadJNku1JA6Ip+KFXb9ihEgGELfTsCzzQyH1P91o+GHMailx2qV/GadX9Novo7m2SAsvyA==', '2023-12-18 15:06:57'),
('4f20b97e-963a-11ee-828d-50faf7e69d5d', 'folkrum', 'countfolkrum@gmail.com', '$2a$11$g0brhyD8bBtRsLDzUqmb5Oj9COI1xtti1N4HJ12ZM0sj1Ct6vtjbG', 1, 1, '2023-12-11 15:06:14', '', '2023-12-10 15:06:14');

-- --------------------------------------------------------

--
-- Table structure for table `tsorder`
--

CREATE TABLE `tsorder` (
  `Id` int(50) NOT NULL,
  `UserId` varchar(50) NOT NULL,
  `PaymentId` varchar(50) DEFAULT NULL,
  `Course_count` int(50) NOT NULL DEFAULT 0,
  `InvoiceNo` varchar(250) DEFAULT NULL,
  `TotalHarga` double NOT NULL DEFAULT 0,
  `OrderDate` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `IsPaid` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tsorder`
--

INSERT INTO `tsorder` (`Id`, `UserId`, `PaymentId`, `Course_count`, `InvoiceNo`, `TotalHarga`, `OrderDate`, `IsPaid`) VALUES
(24, '4c6ff0c9-95e5-11ee-9ec7-7cef3d6236e7', '3e59cd9f-884a-11ee-b59a-3c5282e16d0b', 1, 'APM00024', 11650000, '2023-12-08 16:28:55', 1),
(25, '4c6ff0c9-95e5-11ee-9ec7-7cef3d6236e7', '3e59cd9f-884a-11ee-b59a-3c5282e16d0b', 1, 'APM00025', 8500000, '2023-12-11 07:27:52', 1),
(26, '4c6ff0c9-95e5-11ee-9ec7-7cef3d6236e7', '3e59ce13-884a-11ee-b59a-3c5282e16d0b', 2, 'APM00026', 15200000, '2023-12-11 15:07:08', 1);

-- --------------------------------------------------------

--
-- Table structure for table `tsorderdetail`
--

CREATE TABLE `tsorderdetail` (
  `Id` int(50) NOT NULL,
  `OrderId` int(50) NOT NULL,
  `CourseId` varchar(50) NOT NULL,
  `Jadwal` date NOT NULL,
  `Harga` double NOT NULL DEFAULT 0,
  `IsActivated` tinyint(1) DEFAULT 0,
  `IsSelected` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tsorderdetail`
--

INSERT INTO `tsorderdetail` (`Id`, `OrderId`, `CourseId`, `Jadwal`, `Harga`, `IsActivated`, `IsSelected`) VALUES
(30, 24, 'b3e79e06-884a-11ee-b59a-3c5282e16d0b', '2022-07-26', 11650000, 1, 1),
(31, 25, 'b3e55d96-884a-11ee-b59a-3c5282e16d0b', '2022-07-26', 8500000, 1, 1),
(32, 26, 'b3e79ff7-884a-11ee-b59a-3c5282e16d0b', '2022-07-26', 13000000, 1, 1),
(34, 26, 'b3e79da3-884a-11ee-b59a-3c5282e16d0b', '2023-12-21', 2200000, 1, 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `mscategory`
--
ALTER TABLE `mscategory`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `mscourse`
--
ALTER TABLE `mscourse`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `CategoryId` (`CategoryId`);

--
-- Indexes for table `mspaymentmethod`
--
ALTER TABLE `mspaymentmethod`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `msrole`
--
ALTER TABLE `msrole`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `msuser`
--
ALTER TABLE `msuser`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Email` (`Email`),
  ADD KEY `Role` (`Role`) USING BTREE;

--
-- Indexes for table `tsorder`
--
ALTER TABLE `tsorder`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `UserId` (`UserId`),
  ADD KEY `PaymentId` (`PaymentId`);

--
-- Indexes for table `tsorderdetail`
--
ALTER TABLE `tsorderdetail`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `CourseId` (`CourseId`),
  ADD KEY `OrderId` (`OrderId`) USING BTREE;

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `msrole`
--
ALTER TABLE `msrole`
  MODIFY `id` int(2) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `tsorder`
--
ALTER TABLE `tsorder`
  MODIFY `Id` int(50) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT for table `tsorderdetail`
--
ALTER TABLE `tsorderdetail`
  MODIFY `Id` int(50) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=35;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `mscourse`
--
ALTER TABLE `mscourse`
  ADD CONSTRAINT `mscourse_ibfk_1` FOREIGN KEY (`CategoryId`) REFERENCES `mscategory` (`Id`);

--
-- Constraints for table `msuser`
--
ALTER TABLE `msuser`
  ADD CONSTRAINT `role_constraint` FOREIGN KEY (`Role`) REFERENCES `msrole` (`id`);

--
-- Constraints for table `tsorder`
--
ALTER TABLE `tsorder`
  ADD CONSTRAINT `tsorder_ibfk_1` FOREIGN KEY (`PaymentId`) REFERENCES `mspaymentmethod` (`Id`),
  ADD CONSTRAINT `tsorder_ibfk_2` FOREIGN KEY (`UserId`) REFERENCES `msuser` (`Id`);

--
-- Constraints for table `tsorderdetail`
--
ALTER TABLE `tsorderdetail`
  ADD CONSTRAINT `tsorderdetail_ibfk_1` FOREIGN KEY (`OrderId`) REFERENCES `tsorder` (`Id`),
  ADD CONSTRAINT `tsorderdetail_ibfk_2` FOREIGN KEY (`CourseId`) REFERENCES `mscourse` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
