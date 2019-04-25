CREATE DATABASE IF NOT EXISTS `settlers` DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;
USE `settlers`;

CREATE TABLE IF NOT EXISTS Alapanyag
(
	ID INT AUTO_INCREMENT PRIMARY KEY
	,Nev VARCHAR(200)
	,IkonAzonosito VARCHAR(200)
    ,KezdoMennyiseg INT
);
insert into alapanyag (Nev,KezdoMennyiseg)
VALUES 
("Wood",30),
("Stone",30),
("Wheat",0),
("Water",0),
("Flour",0),
("Bread",0),
("Meat",0);

CREATE TABLE IF NOT EXISTS Tipus
(
	ID INT AUTO_INCREMENT PRIMARY KEY
	,Nev VARCHAR(200)
);
Insert into Tipus (Nev)
VALUES
("Termelo"),
("Gyarto"),
("Lakohaz");

CREATE TABLE IF NOT EXISTS EpuletTipus
(
	ID INT AUTO_INCREMENT PRIMARY KEY
	,Nev VARCHAR(200)
	,TipusID INT
	,KepAzonosito VARCHAR(200)
	,FolyamatbanKepAzonosito VARCHAR(200)
	,FOREIGN KEY (TipusID) REFERENCES Tipus (ID)
);
Insert into EpuletTipus (Nev,TipusID,KepAzonosito,FolyamatbanKepAzonosito)
Values
("Favago",1,"woodcutter","hower_woodcutter"),
("Kofarago",1,"stonequarry","hower_stonequarry"),
("Buzafarm",1,"wheatfarm","hower_wheatfarm"),
("Szelmalom",2,"windmill","hower_windmill"),
("Kut",1,"well","hower_well"),
("Pekseg",2,"bakery","hower_bakery"),
("Lakohaz",3,"house","hower_house"),
("Vadaszhaz",1,"hunter","hower_hunter");

CREATE TABLE IF NOT EXISTS Epulet
(
	ID INT AUTO_INCREMENT PRIMARY KEY
	,EpuletTipusID INT
	,Koordinata VARCHAR(200)
	,AutoGeneralas BIT
    ,Statusz INT
	,FOREIGN KEY (EpuletTipusID) REFERENCES EpuletTipus(ID)
);
CREATE TABLE IF NOT EXISTS EpuletTipusElkeszites
(
	ID INT AUTO_INCREMENT PRIMARY KEY
	,EpuletTipusID INT
	,AlapanyagID INT
	,Mennyiseg INT
	,FOREIGN KEY (EpuletTipusID) REFERENCES EpuletTipus(ID)
	,FOREIGN KEY (AlapanyagID) REFERENCES Alapanyag(ID)
);
Insert into EpuletTipusElkeszites (EpuletTipusID,AlapanyagID,Mennyiseg)
VAlues
(1,1,2),
(1,2,2),
(2,1,2),
(2,2,3),
(3,1,6),
(3,2,6),
(4,1,3),
(4,2,3),
(5,1,3),
(5,2,3),
(6,1,4),
(6,2,5),
(7,1,5),
(7,2,6),
(8,1,3),
(8,2,3);
create table IF NOT EXISTS Alapanyag_Gyartas       
(
	ID INT AUTO_INCREMENT PRIMARY KEY
	,KeszAlapanyagID INT
	,AlapanyagID INT
	,Mennyiseg INT
    ,EpuletTipusID INT
    ,Foreign KEY (EpuletTipusID) References EpuletTipus(ID)
	,FOREIGN KEY (KeszAlapanyagID) REFERENCES Alapanyag(ID)
	,FOREIGN KEY (AlapanyagID) REFERENCES Alapanyag(ID)
);
Insert into Alapanyag_Gyartas (KeszAlapanyagID,AlapanyagID,Mennyiseg,EpuletTipusID)
VALUES
(1,NULL,0,1),
(2,NULL,0,2),
(3,NULL,0,3),
(4,NULL,0,5),
(5,3,2,4),
(6,4,1,6),
(6,5,2,6),
(7,NULL,0,8);

create table IF NOT EXISTS Epulet_Queue
(
	EpuletID INT
	,AlapanyagID INT
	,Mennyiseg INT
	,FOREIGN KEY (EpuletID) REFERENCES Epulet(ID)
	,FOREIGN KEY (AlapanyagID) REFERENCES Alapanyag(ID)
);
CREATE TABLE IF NOT EXISTS Munkas
(
	ID INT AUTO_INCREMENT PRIMARY KEY
	,Nev VARCHAR(200)
);
Insert into Munkas (Nev)
Values
("favago"),
("kofarago"),
("farmer"),
("viz hordo"),
("molnar"),
("pek"),
("vadasz");

CREATE TABLE IF NOT EXISTS Epulet_Munkas
(
	EpuletID INT
	,MunkasID INT
	,FOREIGN KEY (EpuletID) REFERENCES Epulet(ID)
	,FOREIGN KEY (MunkasID) REFERENCES Munkas(ID)
);

Create table IF NOT EXISTS Mezok
(
ID INT PRIMARY KEY AUTO_INCREMENT,
sor varchar(1200)
);

Create table IF NOT EXISTS Mentett_Alapanyag
(
ID int primary key auto_increment,
alapanyag_ID int,
mennyiseg int,
foreign key (alapanyag_ID) REFERENCES Alapanyag(id)
);