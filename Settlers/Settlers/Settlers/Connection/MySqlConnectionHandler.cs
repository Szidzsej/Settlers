﻿using Microsoft.Xna.Framework;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    public class MySqlConnectionHandler : IMySqlConnectionHandler
    {
        private MySqlConnection connection;
        /// <summary>
        /// Az adatbázis kapcsolat létrehozása
        /// </summary>
        public MySqlConnectionHandler()
        {
            string connectionString = $"Datasource=127.0.0.1;Port=3306;Database=settlers;Username=root;Password=;CharSet=utf8;";

            connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Épület tipusának megadásával lekérdezzük az adott épülethez szükséges nyersanyagokat
        /// </summary>
        /// <param name="bEnum">Enumban tárolt épület tipus azonositója</param>
        /// <returns></returns>
        public int[] GetBuildingTypeCreate(BuildingTypeEnum bEnum)
        {
            int[] temp = new int[2];

            using (MySqlCommand command = new MySqlCommand("SELECT EpuletTipusID,AlapanyagID,Mennyiseg FROM EpuletTipusElkeszites", connection)) 
            {
                using (MySqlDataReader MySqlDataReader = command.ExecuteReader())
                {
                    while (MySqlDataReader.Read())
                    {
                        if ((int)bEnum == MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("EpuletTipusID")))
                        {
                            if (MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("AlapanyagID"))==1)
                            {
                                temp[0] = MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("Mennyiseg"));
                            }
                            else
                            {
                                temp[1] = MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("Mennyiseg"));
                            }
                        }
                    }
                }
            }
            return temp;
        }
        /// <summary>
        /// Kitörli az adatbázis épület táblájában lévő összes elemet
        /// </summary>
        public void DeleteBuilding()
        {
            using (MySqlCommand command = new MySqlCommand("DELETE FROM epulet",connection))
            {
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Mentéskor beszúrunk egy adott épületet az épület táblába
        /// </summary>
        /// <param name="building">Egy Building tipusú épület</param>
        public void InsertBuilding(Building building)
        {
            string helper = null;
            using (MySqlCommand command = new MySqlCommand("INSERT INTO epulet (ID,EpuletTipusID,Koordinata, Statusz) VALUES(@id,@bTID,@coor,@stat)",connection))
            {
                command.Parameters.AddWithValue("@id", building.ID);
                command.Parameters.AddWithValue("@bTID", (int)building.BuildingType);
                helper = building.Bounds.X + "," + building.Bounds.Y;
                command.Parameters.AddWithValue("@coor", helper);
                        
                command.Parameters.AddWithValue("@stat", (int)building.Status);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Lekérdezi az adatbázisban definiált nyersanyagokat, és a játék kezdetekor megkapott alap mennyiségét
        /// </summary>
        /// <returns>Dictionaryben elemenkét kapunk egy nyersanyagot és egy mennyiséget</returns>
        public Dictionary<BaseMaterial,int> GetBaseMaterial()
        {
            Dictionary<BaseMaterial, int> temp = new Dictionary<BaseMaterial, int>();
            using (MySqlCommand command = new MySqlCommand("SELECT ID,Nev,KezdoMennyiseg FROM Alapanyag", connection))
            {
                using (MySqlDataReader MySqlDataReader = command.ExecuteReader())
                {
                    while (MySqlDataReader.Read())
                    {
                        temp.Add(new Settlers.BaseMaterial(MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("ID")), MySqlDataReader.GetString(MySqlDataReader.GetOrdinal("Nev"))),MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("KezdoMennyiseg")));
                    }
                }
            }
            return temp;
        }
        /// <summary>
        /// Az adatbázisba lementett épületeket kérdezi le
        /// </summary>
        /// <returns>Vissza adja az épületeket egy listában</returns>
        public List<Building> GetBuilding()
        {
            List<Building> temp = new List<Building>();
            string[] helper = new string[2];
            using (MySqlCommand command = new MySqlCommand("SELECT ID,EpuletTipusID,Koordinata,Statusz FROM Epulet", connection))
            {
                using (MySqlDataReader MySqlDataReader = command.ExecuteReader())
                {
                    while (MySqlDataReader.Read())
                    {
                        helper = MySqlDataReader.GetString(MySqlDataReader.GetOrdinal("Koordinata")).Split(',');
                        temp.Add(new Building(MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("ID")),(BuildingTypeEnum)MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("EpuletTipusID")), new Rectangle(int.Parse(helper[0]),int.Parse(helper[1]),Globals.BUILDINGSIZE, Globals.BUILDINGSIZE), (BuildingStatus) MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("Statusz"))));
                    }
                }
            }
            return temp;
        }

        /// <summary>
        /// Kapcsolat tesztelése és megnyitása
        /// </summary>
        /// <returns></returns>
        public bool TryOpen()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        /// <summary>
        /// Lekérdezi a mezők tábla id-jait, és vissza adja hány darabot talált
        /// </summary>
        /// <returns>Vissza adja a mezők számát</returns>
        public int SelectTiles()
        {
            int temp = 0;
            using (MySqlCommand command = new MySqlCommand("SELECT id FROM Mezok", connection))
            {
                using (MySqlDataReader MySqlDataReader = command.ExecuteReader())
                {
                    while (MySqlDataReader.Read())
                    {
                        temp++;
                    }
                }
            }
            return temp;
        }

        /// <summary>
        /// Kitörli a mezők tábla elemeit
        /// </summary>
        public void DeleteTiles()
        {
            using (MySqlCommand command = new MySqlCommand("DELETE FROM Mezok", connection))
            {
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Beszúrja az adatbázis mezők táblájába a pálya mezőket
        /// </summary>
        /// <param name="line">Hatékonyság miatt, a mezőket soronként töltjük fel</param>
        public void InsertTiles(string line)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO Mezok (sor) VALUES(@line)", connection))
            {
                command.Parameters.AddWithValue("@line", line);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Lekérdezi a lementett pályát
        /// </summary>
        /// <returns>Vissza adja a pálya mezőit</returns>
        public List<Tile> GetTiles()
        {
            List<Tile> temp = new List<Tile>();
            string[] lines = new string[60];
            using (MySqlCommand command = new MySqlCommand("SELECT sor FROM Mezok", connection))
            {
                using (MySqlDataReader MySqlDataReader = command.ExecuteReader())
                {
                    while (MySqlDataReader.Read())
                    {
                        lines = MySqlDataReader.GetString(MySqlDataReader.GetOrdinal("sor")).Split(';');
                        foreach (var item in lines)
                        {
                            string[] helper = new string[3];
                            helper = item.Split(',');
                            if (item != "")
                            {
                                temp.Add(new Tile(new Rectangle(int.Parse(helper[0]), int.Parse(helper[1]), Globals.TILESIZE, Globals.TILESIZE), (TileState)int.Parse(helper[2]),Color.White));

                            }

                        }
                    }
                }
            }
            return temp;
        }

        /// <summary>
        /// Lementi az alapanyagokat és az aktuális darabszámukat
        /// </summary>
        /// <param name="ID">Nyersanyag id-je</param>
        /// <param name="count">Darabszám</param>
        public void SaveMaterials(int bMaterialID, int count)
        {
            using (MySqlCommand command = new MySqlCommand("INSERT INTO Mentett_alapanyag (alapanyag_id,mennyiseg) VALUES(@bm, @count)", connection))
            {
                command.Parameters.AddWithValue("@bm", bMaterialID);
                command.Parameters.AddWithValue("@count", count);
                command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Kitörli a lementett alapanyagokat
        /// </summary>
        public void DeleteMaterials()
        {
            using (MySqlCommand command = new MySqlCommand("DELETE FROM Mentett_alapanyag", connection))
            {
                command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Lekérdezi a lementett alapanyagokat
        /// </summary>
        /// <returns></returns>
        public Dictionary<BaseMaterial, int> GetSavedMaterial()
        {
            Dictionary<BaseMaterial, int> temp = new Dictionary<BaseMaterial, int>();
            using (MySqlCommand command = new MySqlCommand("SELECT a.ID,a.Nev,m.Mennyiseg From Alapanyag a INNER JOIN mentett_alapanyag m ON a.id= m.alapanyag_ID", connection))
            {
                using (MySqlDataReader MySqlDataReader = command.ExecuteReader())
                {
                    while (MySqlDataReader.Read())
                    {
                        temp.Add(new Settlers.BaseMaterial(MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("ID")), MySqlDataReader.GetString(MySqlDataReader.GetOrdinal("Nev"))), MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("Mennyiseg")));
                    }
                }
            }
            return temp;
        }

        /// <summary>
        /// Lekérdezi az adott épületnek a termelési adatait
        /// </summary>
        /// <param name="b">Az adott épület</param>
        /// <returns></returns>
        public Production GetProductions(Building b)
        {
            Production temp = new Production();
            int bTID = (int)b.BuildingType;
            using (MySqlCommand command = new MySqlCommand("SELECT a.ID as ID, a.Nev as Nev From Alapanyag_Gyartas gy INNER JOIN Alapanyag a ON gy.KeszAlapanyagID = a.ID WHERE EpuletTipusID = @bTID", connection))
            {
                command.Parameters.AddWithValue("@bTID", bTID);
                using (MySqlDataReader MySqlDataReader = command.ExecuteReader())
                {
                    while (MySqlDataReader.Read())
                    {
                        temp.ReadyMaterial.ID = MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("ID"));
                        temp.ReadyMaterial.Name = MySqlDataReader.GetString(MySqlDataReader.GetOrdinal("Nev"));
                    }
                }
            }
            using (MySqlCommand command = new MySqlCommand("SELECT a.ID as ID, a.Nev as Nev,Mennyiseg From Alapanyag_Gyartas gy INNER JOIN Alapanyag a ON gy.AlapanyagID = a.ID WHERE EpuletTipusID = @bTID", connection))
            {
                command.Parameters.AddWithValue("@bTID", bTID);
                using (MySqlDataReader MySqlDataReader = command.ExecuteReader())
                {
                    while (MySqlDataReader.Read())
                    {
                            temp.BaseMaterials.Add(new BaseMaterial(MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("ID")), MySqlDataReader.GetString(MySqlDataReader.GetOrdinal("Nev"))), MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("Mennyiseg")));
                    }
                }
            }
            return temp;
        }
    }
}