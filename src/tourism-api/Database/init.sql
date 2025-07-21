BEGIN TRANSACTION;
DROP TABLE IF EXISTS "KeyPoints";
CREATE TABLE "KeyPoints" (
	"Id"	INTEGER,
	"OrderPosition"	INTEGER,
	"Name"	TEXT,
	"Description"	TEXT,
	"ImageUrl"	TEXT,
	"Latitude"	REAL,
	"Longitude"	REAL,
	PRIMARY KEY("Id" AUTOINCREMENT)
);
DROP TABLE IF EXISTS "Meals";
CREATE TABLE "Meals" (
	"Id"	INTEGER,
	"OrderPosition"	INTEGER,
	"Name"	TEXT,
	"Price"	REAL,
	"Ingredients"	TEXT,
	"ImageUrl"	TEXT,
	"RestaurantId"	INTEGER,
	PRIMARY KEY("Id" AUTOINCREMENT),
	FOREIGN KEY("RestaurantId") REFERENCES "Restaurants"("Id") ON DELETE CASCADE
);
DROP TABLE IF EXISTS "RestaurantReservations";
CREATE TABLE "RestaurantReservations" (
	"Id"	INTEGER,
	"RestaurantId"	INTEGER NOT NULL,
	"UserId"	INTEGER NOT NULL,
	"ReservationDate"	DATE NOT NULL,
	"MealType"	TEXT NOT NULL CHECK("MealType" IN ('dorucak', 'rucak', 'vecera')),
	"NumberOfGuests"	INTEGER NOT NULL CHECK("NumberOfGuests" > 0),
	"Status"	TEXT NOT NULL DEFAULT 'pending' CHECK("Status" IN ('pending', 'confirmed', 'cancelled')),
	"CreatedAt"	DATETIME NOT NULL DEFAULT (datetime('now')),
	PRIMARY KEY("Id" AUTOINCREMENT),
	FOREIGN KEY("RestaurantId") REFERENCES "Restaurants"("Id") ON DELETE CASCADE,
	FOREIGN KEY("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);
DROP TABLE IF EXISTS "Restaurants";
CREATE TABLE "Restaurants" (
	"Id"	INTEGER,
	"Name"	TEXT,
	"Description"	TEXT,
	"Capacity"	INTEGER,
	"ImageUrl"	TEXT,
	"Latitude"	REAL,
	"Longitude"	REAL,
	"Status"	TEXT,
	"OwnerId"	INTEGER,
	PRIMARY KEY("Id" AUTOINCREMENT),
	FOREIGN KEY("OwnerId") REFERENCES "Users"("Id") ON DELETE CASCADE
);
DROP TABLE IF EXISTS "TourFeedbacks";
CREATE TABLE "TourFeedbacks" (
	"Id"	INTEGER,
	"TourId"	INTEGER,
	"UserId"	INTEGER,
	"UserRating"	INTEGER CHECK("UserRating" >= 1 AND "UserRating" <= 5),
	"UserComment"	TEXT,
	"PostedOn"	TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT),
	FOREIGN KEY("TourId") REFERENCES "Tours"("Id") ON DELETE CASCADE,
	FOREIGN KEY("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);
DROP TABLE IF EXISTS "TourKeypoints";
CREATE TABLE "TourKeypoints" (
	"KeypointId"	INTEGER NOT NULL,
	"TourId"	INTEGER NOT NULL,
	PRIMARY KEY("KeypointId","TourId"),
	FOREIGN KEY("KeypointId") REFERENCES "KeyPoints"("Id") ON DELETE CASCADE,
	FOREIGN KEY("TourId") REFERENCES "Tours"("Id") ON DELETE CASCADE
);
DROP TABLE IF EXISTS "TourReservations";
CREATE TABLE "TourReservations" (
	"Id"	INTEGER,
	"TourId"	INTEGER,
	"UserId"	INTEGER,
	"NumberOfGuests"	INTEGER,
	"CreatedOn"	TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT),
	FOREIGN KEY("TourId") REFERENCES "Tours"("Id") ON DELETE CASCADE,
	FOREIGN KEY("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);
DROP TABLE IF EXISTS "Tours";
CREATE TABLE "Tours" (
	"Id"	INTEGER,
	"Name"	TEXT,
	"Description"	TEXT,
	"DateTime"	TEXT,
	"MaxGuests"	INTEGER,
	"Status"	TEXT,
	"GuideId"	INTEGER,
	PRIMARY KEY("Id" AUTOINCREMENT),
	FOREIGN KEY("GuideId") REFERENCES "Users"("Id") ON DELETE CASCADE
);
DROP TABLE IF EXISTS "Users";
CREATE TABLE "Users" (
	"Id"	INTEGER,
	"Username"	TEXT,
	"Password"	TEXT,
	"Role"	TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT)
);
INSERT INTO "KeyPoints" ("Id","OrderPosition","Name","Description","ImageUrl","Latitude","Longitude") VALUES (4,1,'Petrovaradinska Tvrđava','Tvrđava sa prelepim pogledom na Novi Sad','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/red-arrow.jpg',45.2461,19.8446),
 (5,2,'Zlatibor Priroda','Netaknuta priroda i staze na Zlatiboru','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/red-arrow.jpg',43.7284,19.6089),
 (6,3,'Centar Novog Sada','Centar Novog Sada sa odličnim restoranima i prodavnicama','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/red-arrow.jpg',45.2671,19.8335),
 (7,1,'Niška Tvrđava','Tvrđava iz rimskog doba smeštena u Nišu','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/rocket-launch.jpg',43.3147,21.8954),
 (8,2,'Crkva Svetog Save','Najveća crkva u Nišu','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/example.jpg',43.3172,21.8961),
 (9,3,'Čele Kula','Spomenik žrtvama Prvog srpskog ustanka','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/rocket-launch.jpg',43.3125,21.8958),
 (10,1,'Zemunska Obala','Uživajte u mirnom pogledu na reku sa Zemunske obale','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/rocket-launch.jpg',44.9862,20.4068),
 (11,2,'Gardos Toranj','Srednjovekovni toranj sa prelepim pogledom na reku','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/example.jpg',44.9925,20.4031),
 (12,3,'Zemunska Crkva','Stara crkva sa bogatom istorijom','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/example.jpg',44.9867,20.406),
 (13,1,'Centar Subotice','Prelepa arhitektura i istorijske zgrade u centru Subotice','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/rocket-launch.jpg',46.1003,19.6669),
 (14,2,'Palićko Jezero','Mirno jezero idealno za odmor','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/example.jpg',46.0705,19.7119),
 (15,3,'Subotička Sinagoga','Najveća sinagoga u Srbiji','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/example.jpg',46.1003,19.6669),
 (23,1,'Bazen Maglic','Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum ','https://backipetrovacvesti.com/wp-content/uploads/2024/07/bazeni-backi-maglic-kompleks-bazen-696x387-1.jpg',34.0,34.0),
 (24,1,'Kej Novi Sad','Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum ','https://backipetrovacvesti.com/wp-content/uploads/2024/07/bazeni-backi-maglic-kompleks-bazen-696x387-1.jpg',45.0,67.0),
 (25,1,'Petrovaradinska Tvrđava','Tvrđava sa prelepim pogledom na Novi Sad','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/rocket-launch.jpg',45.2461,19.8446),
 (26,2,'Zlatibor Priroda','Netaknuta priroda i staze na Zlatiboru','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/rocket-launch.jpg',43.7284,19.6089),
 (27,3,'Centar Novog Sada','Centar Novog Sada sa odličnim restoranima i prodavnicama','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/rocket-launch.jpg',45.2671,19.8335),
 (28,1,'Niška Tvrđava','Tvrđava iz rimskog doba smeštena u Nišu','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/example.jpg',43.3147,21.8954),
 (29,2,'Crkva Svetog Save','Najveća crkva u Nišu','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/rocket-launch.jpg',43.3172,21.8961),
 (30,3,'Čele Kula','Spomenik žrtvama Prvog srpskog ustanka','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/example.jpg',43.3125,21.8958),
 (31,1,'Zemunska Obala','Uživajte u mirnom pogledu na reku sa Zemunske obale','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/rocket-launch.jpg',44.9862,20.4068),
 (32,2,'Gardos Toranj','Srednjovekovni toranj sa prelepim pogledom na reku','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/example.jpg',44.9925,20.4031),
 (33,3,'Zemunska Crkva','Stara crkva sa bogatom istorijom','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/rocket-launch.jpg',44.9867,20.406),
 (34,1,'Centar Subotice','Prelepa arhitektura i istorijske zgrade u centru Subotice','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/rocket-launch.jpg',46.1003,19.6669),
 (35,2,'Palićko Jezero','Mirno jezero idealno za odmor','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/example.jpg',46.0705,19.7119),
 (36,3,'Subotička Sinagoga','Najveća sinagoga u Srbiji','https://e10b802d9a492d15f222.b-cdn.net/wp-content/uploads/2023/10/rocket-launch.jpg',46.1003,19.6669),
 (37,1,'Bazen Maglic','Lorem Ipsum ...','https://backipetrovacvesti.com/wp-content/uploads/2024/07/bazeni-backi-maglic-kompleks-bazen-696x387-1.jpg',34.0,34.0),
 (39,1,'test999','Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum ','https://backipetrovacvesti.com/wp-content/uploads/2024/07/bazeni-backi-maglic-kompleks-bazen-696x387-1.jpg',32.0,45.0);
INSERT INTO "Meals" ("Id","OrderPosition","Name","Price","Ingredients","ImageUrl","RestaurantId") VALUES (1,1,'Pljeskavica',10.5,'Govedina, svinjetina, začini','https://example.com/pljeskavica.jpg',1),
 (2,2,'Ćevapi',8.5,'Mlevena govedina, jagnjetina, začini','https://example.com/cevapi.jpg',1),
 (3,1,'Ćevapi',7.5,'Mlevena govedina sa začinima','https://example.com/cevapi2.jpg',2),
 (4,2,'Sarma',10.0,'Kupus, svinjetina, mleveno meso','https://example.com/sarma2.jpg',2),
 (5,3,'Grilovana Riba',15.0,'Sveža riba sa povrćem','https://example.com/grilled_fish.jpg',2),
 (6,1,'Grilovana Riba',15.0,'Sveža riba sa povrćem','https://example.com/grilled_fish.jpg',3),
 (7,2,'Vegan Salata',7.0,'Lettuce, paradajz, krastavci, maslinovo ulje','https://example.com/vegan_salad.jpg',3),
 (8,1,'Zucchini Fritters',8.0,'Tikvice, brašno, začini','https://example.com/zucchini_fritters.jpg',4),
 (9,2,'Prženi Karp',14.5,'Sveže prženi šaran sa salatom','https://example.com/fried_carp.jpg',4),
 (10,3,'Burek',9.5,'Pita sa mesom ili sirom','https://example.com/burek.jpg',4),
 (11,1,'Pašticada',16.0,'Goveđi gulaš sa povrćem i vinom','https://example.com/pasticada.jpg',5),
 (12,2,'Pržena Piletina',10.5,'Hrskava pržena piletina sa pomfritom','https://example.com/fried_chicken.jpg',5),
 (13,3,'Gulaš',12.0,'Govedina, povrće, paprika','https://example.com/goulash.jpg',5);
INSERT INTO "RestaurantReservations" ("Id","RestaurantId","UserId","ReservationDate","MealType","NumberOfGuests","Status","CreatedAt") VALUES (1,1,1,'2025-07-15','rucak',4,'confirmed','2025-07-09 14:30:00'),
 (2,2,2,'2025-07-16','dorucak',2,'confirmed','2025-07-09 09:15:00'),
 (3,3,3,'2025-07-17','vecera',6,'confirmed','2025-07-09 16:45:00'),
 (4,1,4,'2025-07-18','rucak',3,'cancelled','2025-07-09 11:20:00'),
 (5,2,5,'2025-07-19','dorucak',2,'confirmed','2025-07-09 08:30:00'),
 (6,3,6,'2025-07-20','vecera',4,'confirmed','2025-07-09 17:00:00'),
 (7,4,1,'2025-07-21','rucak',5,'confirmed','2025-07-09 12:15:00'),
 (8,5,2,'2025-07-22','dorucak',2,'confirmed','2025-07-09 07:45:00'),
 (9,1,3,'2025-07-23','vecera',3,'cancelled','2025-07-09 18:30:00'),
 (10,2,4,'2025-07-24','rucak',4,'confirmed','2025-07-09 13:20:00');
INSERT INTO "Restaurants" ("Id","Name","Description","Capacity","ImageUrl","Latitude","Longitude","Status","OwnerId") VALUES (1,'Restoran Kalemegdan','Tradicionalna srpska hrana sa pogledom na grad',100,'https://example.com/kalemegdan_restaurant.jpg',44.8176,20.4633,'objavljeno',7),
 (2,'Restoran Novi Sad','Sveži lokalni sastojci i moderna srpska kuhinja',80,'https://example.com/novi_sad_restaurant.jpg',45.2671,19.8335,'objavljeno',8),
 (3,'Restoran Niš','Tradicionalna jela u srcu Niša',60,'https://example.com/nis_restaurant.jpg',43.3147,21.8954,'objavljeno',9),
 (4,'Restoran Zemun','Prijatna atmosfera sa odličnom srpskom hranom',50,'https://example.com/zemun_restaurant.jpg',44.9897,20.4067,'objavljeno',7),
 (5,'Restoran Subotica','Sveži lokalni sastojci sa modernim twistom',70,'undefined',46.1003,19.6669,'objavljeno',8);
INSERT INTO "TourKeypoints" ("KeypointId","TourId") VALUES (4,3),
 (5,3),
 (6,3),
 (4,4),
 (5,4),
 (8,4),
 (10,4),
 (7,7),
 (5,7),
 (4,7),
 (11,7),
 (12,7),
 (39,7);
INSERT INTO "TourReservations" ("Id","TourId","UserId","NumberOfGuests","CreatedOn") VALUES (6,2,1,5,'2025-07-15 20:15:23.112'),
 (8,3,1,3,'2025-07-16 08:54:45.541'),
 (9,3,1,3,'2025-07-16 08:56:04.837'),
 (10,3,1,1,'2025-07-16 08:57:27.371'),
 (11,3,1,1,'2025-07-16 09:35:13.475'),
 (12,3,1,10,'2025-07-16 11:26:17.735'),
 (13,3,1,4,'2025-07-16 11:41:23.27'),
 (15,2,1,1,'2025-07-16 15:44:50.921');
INSERT INTO "Tours" ("Id","Name","Description","DateTime","MaxGuests","Status","GuideId") VALUES (2,'Niška Istorijska Tura','Šetnja kroz istoriju Niša','2025-04-03 11:00:00',10,'objavljeno',12),
 (3,'Zemunski Obala Tura','Scenski obilazak uz reku u Zemunu Scenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u ZemunuScenski obilazak uz reku u Zemunu','2025-07-31 15:02:00',26,'objavljeno',10),
 (4,'Subotica Kulturna Tura','Istražite kulturnu baštinu Subotice Istražite kulturnu baštinu SuboticeIstražite kulturnu baštinu SuboticeIstražite kulturnu baštinu SuboticeIstražite kulturnu baštinu SuboticeIstražite kulturnu baštinu SuboticeIstražite kulturnu baštinu SuboticeIstražite kulturnu baštinu Subotice','2025-08-21 00:03:00',12,'objavljena',11),
 (7,'Nova tura','Lorem ipsum tralalaLorem ipsum tralalaLorem ipsum tralalaLorem ipsum tralalaLorem ipsum tralalaLorem ipsum tralalaLorem ipsum tralalaLorem ipsum tralalaLorem ipsum tralalaLorem ipsum tralalaLorem ipsum tralalaLorem ipsum tralalaLorem ipsum tralalaLorem ipsum tralala','2025-08-09 22:46:00',12,'objavljeno',11);
INSERT INTO "Users" ("Id","Username","Password","Role") VALUES (1,'turista1','turista1','turista'),
 (2,'turista2','turista2','turista'),
 (3,'turista3','turista3','turista'),
 (4,'turista4','turista4','turista'),
 (5,'turista5','turista5','turista'),
 (6,'turista6','turista6','turista'),
 (7,'vlasnik1','vlasnik1','vlasnik'),
 (8,'vlasnik2','vlasnik2','vlasnik'),
 (9,'vlasnik3','vlasnik3','vlasnik'),
 (10,'vodic1','vodic1','vodic'),
 (11,'vodic2','vodic2','vodic'),
 (12,'vodic3','vodic3','vodic');
COMMIT;
