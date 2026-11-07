-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: chudoobuv
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `chudoobuv`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `chudoobuv` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `chudoobuv`;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `idCat` int NOT NULL AUTO_INCREMENT,
  `catName` text,
  PRIMARY KEY (`idCat`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'Детская обувь'),(2,'Женская обувь'),(3,'Мужская обувь');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `manufacrures`
--

DROP TABLE IF EXISTS `manufacrures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `manufacrures` (
  `idManuf` int NOT NULL AUTO_INCREMENT,
  `ManufName` text,
  PRIMARY KEY (`idManuf`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `manufacrures`
--

LOCK TABLES `manufacrures` WRITE;
/*!40000 ALTER TABLE `manufacrures` DISABLE KEYS */;
INSERT INTO `manufacrures` VALUES (1,'Малыш-Спорт'),(2,'Топ-Топ'),(3,'Лапушка'),(4,'Тёплые ножки'),(5,'Северята'),(6,'Весна'),(7,'Здоровый шаг'),(8,'Карапуз'),(9,'Маленький франт'),(10,'Барбари'),(11,'Твой комфорт'),(12,'Стиль и комфорт'),(13,'Кожевенные традиции'),(14,'Модный силуэт'),(15,'Азимут'),(16,'Берестов');
/*!40000 ALTER TABLE `manufacrures` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `idOrder` int NOT NULL AUTO_INCREMENT,
  `orderDate` datetime DEFAULT NULL,
  `userId` int DEFAULT NULL,
  PRIMARY KEY (`idOrder`),
  KEY `fk_UserIdOrd_idx` (`userId`),
  CONSTRAINT `fk_UserIdOrd` FOREIGN KEY (`userId`) REFERENCES `users` (`idUser`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,'2026-04-02 00:00:00',3),(2,'2026-04-05 00:00:00',17),(3,'2026-04-08 00:00:00',7),(4,'2026-04-12 00:00:00',15),(5,'2026-04-18 00:00:00',12),(6,'2026-04-22 00:00:00',9),(7,'2026-04-25 00:00:00',14),(8,'2026-04-28 00:00:00',20),(9,'2026-05-05 00:00:00',5),(10,'2026-05-08 00:00:00',17);
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ordersitem`
--

DROP TABLE IF EXISTS `ordersitem`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ordersitem` (
  `idOrderItem` int NOT NULL AUTO_INCREMENT,
  `orderId` int DEFAULT NULL,
  `prodId` int DEFAULT NULL,
  `sizeId` int NOT NULL,
  `quantity` int NOT NULL,
  `unitPrice` decimal(10,2) NOT NULL,
  PRIMARY KEY (`idOrderItem`),
  KEY `fk_OrderId_idx` (`orderId`),
  KEY `fk_ProdIdOrd_idx` (`prodId`),
  KEY `fk_OrderItemSize` (`sizeId`),
  CONSTRAINT `fk_OrderId` FOREIGN KEY (`orderId`) REFERENCES `orders` (`idOrder`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_OrderItemSize` FOREIGN KEY (`sizeId`) REFERENCES `sizes` (`idSize`) ON DELETE RESTRICT,
  CONSTRAINT `fk_ProdIdOrd` FOREIGN KEY (`prodId`) REFERENCES `products` (`idProd`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `ck_OrderItemPrice` CHECK (((`unitPrice` is null) or (`unitPrice` >= 0))),
  CONSTRAINT `ck_OrderItemQuantity` CHECK (((`quantity` is null) or (`quantity` > 0)))
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ordersitem`
--

LOCK TABLES `ordersitem` WRITE;
/*!40000 ALTER TABLE `ordersitem` DISABLE KEYS */;
INSERT INTO `ordersitem` VALUES (1,1,1,3,1,2190.00),(2,1,7,5,1,2790.00),(3,2,10,19,1,25000.00),(4,2,13,23,1,7500.00),(5,3,12,21,1,17000.00),(6,3,20,21,1,8565.00),(7,3,22,18,1,7795.00),(8,4,17,23,1,7757.00),(9,4,16,23,1,12340.00),(10,4,15,25,1,7130.00),(11,4,21,19,1,8765.00),(12,4,11,25,1,15000.00),(13,5,25,29,1,9567.00),(14,5,29,29,1,21500.00),(15,5,24,25,1,7890.00),(16,6,26,27,1,24569.00),(17,6,30,31,1,14567.00),(18,6,31,29,1,15670.00),(19,7,6,1,1,4290.00),(20,7,1,4,2,2190.00),(21,7,8,11,1,1690.00),(22,8,28,34,1,25625.00),(23,8,24,27,3,7890.00),(24,8,23,33,2,6550.00),(25,9,2,16,1,2490.00),(26,9,5,5,2,3190.00),(27,10,18,18,1,7550.00),(28,10,15,21,1,7130.00),(29,10,16,19,1,12340.00),(30,10,21,18,2,8765.00);
/*!40000 ALTER TABLE `ordersitem` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `idProd` int NOT NULL AUTO_INCREMENT,
  `CatId` int DEFAULT NULL,
  `subCatId` int DEFAULT NULL,
  `Image` text,
  `prodName` text,
  `manufId` int DEFAULT NULL,
  `descrip` text,
  `struct` text,
  `price` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`idProd`),
  KEY `fk_SubCatId_idx` (`subCatId`),
  KEY `fk_ManufId_idx` (`manufId`),
  KEY `fk_CatId_idx` (`CatId`),
  CONSTRAINT `fk_CatId` FOREIGN KEY (`CatId`) REFERENCES `categories` (`idCat`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_ManufId` FOREIGN KEY (`manufId`) REFERENCES `manufacrures` (`idManuf`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_SubCatId` FOREIGN KEY (`subCatId`) REFERENCES `subcategory` (`idSubCat`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (1,1,1,'IMG_KS_195217.png','Кроссовки детские «Звёздочка» экокожа',1,'Кроссовки детские «Звёздочка» экокожа. Производитель: ООО «1», г. Смоленск','Верх — экокожа (полиуретан на хлопковой основе), подкладка — хлопок 100%, подошва — ПВХ',2190.00),(2,1,1,'IMG_KS_195213.png','Кроссовки детские «Гонщик» экокожа',2,'Кроссовки детские «Гонщик» экокожа. Производитель: Фабрика «2», г. Киров','Верх — экокожа (микрофибра), подкладка — текстиль сетчатый (полиэстер), подошва — термопластичная резина (ТЭП)',2490.00),(3,1,1,'IMG_KS_195222.png','Кроссовки детские «Радуга» экокожа перфорированная',3,'Кроссовки детские «Радуга» экокожа перфорированная. Производитель: ИП Смирнова Л.В. (бренд «3»), г. Вологда','Верх — экокожа перфорированная, вставки — нейлон, стелька — ортопедическая с латексной подушкой, подошва — ЭВА',1990.00),(4,1,2,'IMG_KB_185501.png','Сапожки детские «Снежинка» зимние на меху',4,'Сапожки детские «Снежинка» зимние на меху. Производитель: ООО «4», г. Новосибирск','Верх — экокожа морозостойкая, утеплитель — искусственный мех (акрил 70%, полиэстер 30%), подкладка — флис, подошва — термоэластопласт (ТЭП)',3490.00),(5,1,2,'IMG_KB_164816.png','Сапожки детские «Медвежонок» демисезонные непромокаемые',5,'Сапожки детские «Медвежонок» демисезонные непромокаемые. Производитель: Компания «Северята», г. Архангельск','Верх — плащёвая ткань с водоотталкивающей пропиткой (полиэстер 100%), отделка — экокожа, подкладка — микрофибра, мембрана — AirTex, подошва — резина',3190.00),(6,1,2,'IMG_KB_164811.png','Сапожки детские «Принцесса» кожаные',6,'Сапожки детские «Принцесса» кожаные. Производитель: Мастерская «Весна», г. Тула','Верх — натуральная кожа (КРС), подкладка — шерсть натуральная (70%), байка (30%), стелька — войлочная, подошва — полиуретан',4290.00),(7,1,3,'IMG_KSd_195207.png','Сандалии детские «Пчёлка» ортопедические летние',7,'Сандалии детские «Пчёлка» ортопедические летние. Производитель: ООО «Здоровый шаг», г. Воронеж','Верх — нубук натуральный, подкладка — кожа растительного дубления, стелька — анатомическая с супинатором (латекс + пробка), подошва — каучук',2790.00),(8,1,3,'IMG_KSd_185508.png','Сандалии детские «Морячок» текстильные с фиксатором',8,'Сандалии детские «Морячок» текстильные с фиксатором. Производитель: ИП Кузнецов А.Д. (бренд «Карапуз»), г. Краснодар','Верх — джинсовая ткань (хлопок 100%), окантовка — искусственная кожа, подкладка — хлопок, подошва — ЭВА лёгкая',1690.00),(9,1,4,'IMG_KSho_185512.png','Туфли детские «Бантик» лаковые праздничные',9,'Туфли детские «Бантик» лаковые праздничные. Производитель: Дом обуви «Маленький франт», г. Москва','Верх — искусственная лаковая кожа (ПВХ), носок усиленный — термопластичный полиуретан, подкладка — кожа искусственная дышащая, стелька — анатомическая с амортизацией, подошва — полиуретан',2390.00),(10,2,5,'IMG_WB_170905.png','Сапоги зимние натуральная кожа',10,'Сапоги женские зимние, нескользящая, устойчивая подошва, натуральный мех. Производитель: Дом обуви «10», г. Новороссийск ','Верх — натуральная кожа, подкладка — евромех, натуральный мех, текстильный утеплитель, подошва — резина',25000.00),(11,2,5,'IMG_WB_170859.png','Сапоги осенние натуральная кожа',10,'Сапоги женские демисезонные из высококачественной натуральной кожи. Производитель: Дом обуви «10», г. Новороссийск ','Верх — натуральная кожа, подкладка — текстильный утеплитель, стелька — текстиль, подошва — ТПР',15000.00),(12,2,5,'IMG_WB_170906.png','Сапоги трубы демисезонные замшевые',11,'Сапоги женские демисезонные замшевые. Производитель: ОАО «Твой комфорт», г. Ярославль','Верх — натуральная замша, утепленные, тракторная подошва, стелька — байка, подошва — резина',17000.00),(13,2,3,'IMG_WSd_174332.png','Босоножки «Песчаный берег» коричневые',12,'Коричневая кожаная модель с открытым носком, боковой пряжкой и закрытым каблуком. Производитель: ИП «Стиль и комфорт», г. Санкт-Петербург','Верх — 100% козья кожа, подкладка — 100% кожа; подошва: 100% кожа',7500.00),(14,2,3,'IMG_WSd_185156.png','Женские босоножки «Черный кофе» со скульптурным каблуком',13,'Открытый носок и ремешок со сборной пряжкой, низкий каблук и резиновая подошва гарантируют комфорт. Производитель: ЗАО «Кожевенные традиции», г. Вятка','Верх — кожа собственного производств, подкладка — кожа; подошва: резина',5600.00),(15,2,3,'IMG_WSd_185551.png','Босоножки женские «Летний бриз»',14,'Босоножки женские кожаные белые, регулируемый ремешок, закрытый каблук и кожаная подошва. Производитель: ООО «Модный силуэт», г. Кострома','Верх — натуральная кожа, подкладка — натуральная кожа, подошва — кожа',7130.00),(16,2,4,'IMG_WSho_174409.png','Демисезонные туфли натуральная кожа',14,'Комфортные лаконичные лодочки, классический дизайна с современными акцентами. Производитель: ООО «Модный силуэт», г. Кострома','Верх — натуральная овечья кожа, подкладка и стелька — натуральная овечья кожа, подошва — резина',12340.00),(17,2,4,'IMG_WSho_185202.png','Туфли женские натуральная кожа с металлическими вставками',13,'Стильные туфли для весеннего и летнего сезона. Производитель: ЗАО «Кожевенные традиции», г. Вятка','Верх — натуральная телячьей кожа, металлическая пряжка, подкладка и стелька— натуральная овечья кожа, подошва — резина',7757.00),(18,2,4,'IMG_WSho_185545.png','Туфли классические лодочки натуральная кожа',12,'Модель из натуральной кожи, классический острый носок и изящная шпилька. Производитель: ИП «Стиль и комфорт», г. Санкт-Петербург','Верх — натуральная кожа, подкладка и стелька — натуральная кожа, подошва — тунит',7550.00),(19,2,1,'IMG_WS_174110.png','Кроссовки белые с оранжевым и красным',2,'Многослойный верх из кожи и текстиля. Производитель: Фабрика «2», г. Киров','Верх — 70% кожа, 30% текстиль, подкладка — текстиль, подошва — резина',9450.00),(20,2,1,'IMG_WS_185528.png','Кроссовки летние кожаные',2,'Уверенность на каждом шагу. Материал обеспечивает долговечность и комфорт при носке. Производитель: Фабрика «2», г. Киров','Верх — 87% кожа, 11% текстиль, 2% синтетика; подкладка: 100% текстиль; подошва: резина, пластик',8565.00),(21,2,1,'IMG_WS_185535.png','Кроссовки кожаные',12,'Кроссовки женские из натуральной кожи. Производитель: ИП «Стиль и комфорт», г. Санкт-Петербург','Верх — натуральная кожа, подкладка — кожа, подошва — полиуретан',8765.00),(22,2,1,'IMG_WS_185538.png','Кроссовки «Стиль и комфорт»',12,'Кроссовки женские из натуральной кожи и синтетических материалов, сетчатые вставки. Производитель: ИП «Стиль и комфорт», г. Санкт-Петербург','Верх — 70% кожа, 20% полиамид, 10% полиуретан; подкладка — текстиль, подошва — ТПУ',7795.00),(23,3,1,'IMG_MS_185531.png','Кроссовки для бега «Драйв»',15,'Сочетание стиля, комфорта и современных технологий. Производитель: Фабрика «Азимут», г. Санкт-Петербург','Верх —  сетка, подкладка — текстиль, подошва —  резина',6550.00),(24,3,1,'IMG_MS_185532.png','Кроссовки «Азимут Бег» ',15,'Универсальный выбор для повседневного использования. Производитель: Фабрика «Азимут», г. Санкт-Петербург','Верх — 6% кожа, 29% текстиль, 15% полимерные материалы; подкладка — 100% текстиль; подошва — полимерные материалы, резина',7890.00),(25,3,1,'IMG_MS_185533.png','Кроссовки',2,'Кроссовки для мужчин, идеальное сочетание стиля и комфорта. Производитель: Фабрика «2», г. Киров','Верх — 56% кожа, 29% текстиль, 15% полимерные материалы; подкладка — 100% текстиль; подошва — полимерные материалы, резина',9567.00),(26,3,4,'IMG_MSho_190514.png','Черные туфли в классическом стиле — база для деловых образов',10,'Модель выполнена из натуральной кожи, на резиновой подошве. Производитель: Дом обуви «10», г. Новороссийск ','Верх — натуральная кожа, подкладка — текстиль, стелька — натуральная кожа, подошва — резина',24569.00),(27,3,4,'IMG_MSho_190518.png','Туфли мужские светлые классика демисезонные',16,'Туфли на низком каблуке кожаные','Верх — натуральная кожа, подкладка — текстиль, стелька — натуральная кожа, подошва — ТЭП',10155.00),(28,3,4,'IMG_MSho_191612.png','Туфли из натуральной кожи',16,'Модель выполнена из натуральной кожи, на резиновой подошве, поддеживает классический и деловой стиль образов. Производитель: Дом обуви «Берестов», г. Москва','Верх — натуральная кожа, подкладка — натуральная кожа, стелька — натуральная кожа, подошва — резина',25625.00),(29,3,5,'IMG_MB_174349.png','Ботинки мужские демисезонные',10,'Ботинки из натуральной кожи темно-коричневого и черного цвета на шнурках. Производитель: Дом обуви «Берестов», г. Москва','Верх — натуральная кожа, подкладка — текстиль, подошва — искусственный материал',21500.00),(30,3,5,'IMG_MB_191200.png','Ботинки черные из натуральной кожи',13,'Ботинки представляют баланс традиций и современности. Выполнены из натуральной кожи насыщенного черного цвета. Производитель: ЗАО «Кожевенные традиции», г. Вятка','Верх — натуральная кожа, подкладка — ворсин, стелька — ворсин, подошва — тунит',14567.00),(31,3,5,'IMG_MB_191205.png','Ботинки зимние классические',10,'Выбор на каждый день — натуральная кожа, натуральный мех. Производитель: Дом обуви «10», г. Новороссийск ','Верх — натуральная кожа, подкладка — натуральный мех, стелька — натуральный мех, подошва — ТЭП',15670.00);
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `idRole` int NOT NULL AUTO_INCREMENT,
  `RoleName` text,
  PRIMARY KEY (`idRole`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES (1,'Администратор'),(2,'Менеджер'),(3,'Авторизованный пользователь');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sizes`
--

DROP TABLE IF EXISTS `sizes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sizes` (
  `idSize` int NOT NULL AUTO_INCREMENT,
  `Size` text,
  PRIMARY KEY (`idSize`)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sizes`
--

LOCK TABLES `sizes` WRITE;
/*!40000 ALTER TABLE `sizes` DISABLE KEYS */;
INSERT INTO `sizes` VALUES (1,'18'),(2,'19'),(3,'20'),(4,'21'),(5,'22'),(6,'23'),(7,'24'),(8,'25'),(9,'26'),(10,'27'),(11,'28'),(12,'29'),(13,'30'),(14,'31'),(15,'32'),(16,'33'),(17,'34'),(18,'35'),(19,'36'),(20,'36.5'),(21,'37'),(22,'37.5'),(23,'38'),(24,'38.5'),(25,'39'),(26,'39.5'),(27,'40'),(28,'40.5'),(29,'41'),(30,'41.5'),(31,'42'),(32,'42.5'),(33,'43'),(34,'44'),(35,'45');
/*!40000 ALTER TABLE `sizes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stockitems`
--

DROP TABLE IF EXISTS `stockitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stockitems` (
  `idStockItem` int NOT NULL AUTO_INCREMENT,
  `prodId` int NOT NULL,
  `SizeId` int NOT NULL,
  `quantity` int NOT NULL,
  PRIMARY KEY (`idStockItem`),
  UNIQUE KEY `uq_StockProductSize` (`prodId`,`SizeId`),
  KEY `fk_SizeId_idx` (`SizeId`),
  KEY `fk_ProdStockId_idx` (`prodId`),
  CONSTRAINT `fk_ProdStockId` FOREIGN KEY (`prodId`) REFERENCES `products` (`idProd`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_SizeId` FOREIGN KEY (`SizeId`) REFERENCES `sizes` (`idSize`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `ck_StockQuantity` CHECK (((`quantity` is null) or (`quantity` >= 0)))
) ENGINE=InnoDB AUTO_INCREMENT=96 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stockitems`
--

LOCK TABLES `stockitems` WRITE;
/*!40000 ALTER TABLE `stockitems` DISABLE KEYS */;
INSERT INTO `stockitems` VALUES (1,1,3,20),(2,1,4,20),(3,1,5,20),(4,1,6,20),(5,1,7,20),(6,1,8,20),(7,2,16,2),(8,2,17,2),(9,3,2,30),(10,3,3,30),(11,4,2,15),(12,4,3,15),(13,5,4,5),(14,5,5,5),(15,5,6,5),(16,6,1,4),(17,6,2,4),(18,6,3,4),(19,7,4,4),(20,7,5,4),(21,7,6,4),(22,7,7,4),(23,8,10,7),(24,8,11,7),(25,8,12,7),(26,8,13,7),(27,9,1,7),(28,9,2,7),(29,9,3,7),(30,10,19,12),(31,10,21,12),(32,11,25,2),(33,11,27,2),(34,12,21,5),(35,12,25,5),(36,12,27,5),(37,13,19,10),(38,13,21,10),(39,13,23,10),(40,13,25,10),(41,13,27,10),(42,14,18,3),(43,15,21,23),(44,15,25,23),(45,15,27,23),(46,16,19,25),(47,16,21,25),(48,16,23,25),(49,17,19,4),(50,17,23,4),(51,17,25,4),(52,17,27,4),(53,18,18,9),(54,18,23,9),(55,19,18,2),(56,19,19,2),(57,20,21,5),(58,20,22,5),(59,20,23,5),(60,20,24,5),(61,21,18,16),(62,21,19,16),(63,21,23,16),(64,21,25,16),(65,22,18,3),(66,22,23,3),(67,23,30,18),(68,23,31,18),(69,23,32,18),(70,23,33,18),(71,24,23,50),(72,24,24,50),(73,24,25,50),(74,24,26,50),(75,24,27,50),(76,24,29,50),(77,25,29,3),(78,25,31,3),(79,25,34,3),(80,26,27,23),(81,26,29,23),(82,26,33,23),(83,27,31,15),(84,27,33,15),(85,27,34,15),(86,28,34,3),(87,29,29,1),(88,29,31,1),(89,30,31,20),(90,30,33,20),(91,31,29,30),(92,31,31,30);
/*!40000 ALTER TABLE `stockitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subcategory`
--

DROP TABLE IF EXISTS `subcategory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subcategory` (
  `idSubCat` int NOT NULL AUTO_INCREMENT,
  `subCatName` text,
  PRIMARY KEY (`idSubCat`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subcategory`
--

LOCK TABLES `subcategory` WRITE;
/*!40000 ALTER TABLE `subcategory` DISABLE KEYS */;
INSERT INTO `subcategory` VALUES (1,'Кроссовки'),(2,'Сапоги'),(3,'Босоножки'),(4,'Туфли'),(5,'Ботинки');
/*!40000 ALTER TABLE `subcategory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `idUser` int NOT NULL AUTO_INCREMENT,
  `lastName` text,
  `firstName` text,
  `middleName` text,
  `login` text,
  `roleId` int DEFAULT NULL,
  PRIMARY KEY (`idUser`),
  KEY `fk_roleId_idx` (`roleId`),
  CONSTRAINT `fk_roleId` FOREIGN KEY (`roleId`) REFERENCES `roles` (`idRole`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'Иванов','Иван','Сергеевич','isivanov',1),(2,'Петров','Пётр','Алексеевич','papetrov',2),(3,'Сидорова','Анна','Дмитриевна','asidorova',3),(4,'Кузнецова','Екатерина','Андреевна','ekuznetsova',2),(5,'Смирнов','Дмитрий','Владимирович','dsmirnov',3),(6,'Попов','Алексей','Николаевич','apopov',1),(7,'Васильева','Ольга','Игоревна','ovasileva',3),(8,'Соколов','Павел','Викторович','psokolov',2),(9,'Михайлова','Татьяна','Юрьевна','tmikhailova',3),(10,'Новикова','Наталья','Сергеевна','nnovikova',2),(11,'Фёдоров','Андрей','Максимович','afedorov',1),(12,'Морозов','Сергей','Павлович','smorozov',3),(13,'Волкова','Юлия','Александровна','jvolkova',2),(14,'Алексеев','Артём','Евгеньевич','aalekseev',3),(15,'Лебедева','Ирина','Владимировна','ilebedeva',3),(16,'Егоров','Никита','Денисович','negorov',2),(17,'Павлова','Елена','Петровна','epavlova',3),(18,'Козлов','Роман','Олегович','rkozlov',1),(19,'Степанова','Виктория','Романовна','vstepanova',2),(20,'Николаев','Владимир','Иванович','vnikolaev',3);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-11-06 19:00:10
