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
        MySqlConnection connection;

        public MySqlConnectionHandler()
        {
            string connectionString = $"Datasource=127.0.0.1;Port=3306;Database=settlers;Username=root;Password=;CharSet=utf8;";

            connection = new MySqlConnection(connectionString);
        }

        public int[] GetBuildingTypeCreate(BuildingTypeEnum bEnum)
        {
            int[] temp = new int[2];

            using (MySqlCommand command = new MySqlCommand("Select EpuletTipusID,AlapanyagID,Mennyiseg From EpuletTipusElkeszites", connection)) 
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

        public void DeleteBuilding()
        {
            using (MySqlCommand command = new MySqlCommand("DELETE FROM epulet WHere 1",connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public void InsertBuilding(Building building)
        {
            string helper = null;
            using (MySqlCommand command = new MySqlCommand("Insert Into epulet (ID,EpuletTipusID,Koordinata, Statusz) Values(@id,@bTID,@coor,@stat)",connection))
            {
                command.Parameters.AddWithValue("@id", building.ID);
                command.Parameters.AddWithValue("@bTID", (int)building.BuildingType);
                helper = building.Bounds.X + "," + building.Bounds.Y;
                command.Parameters.AddWithValue("@coor", helper);
                        
                command.Parameters.AddWithValue("@stat", (int)building.Status);
                command.ExecuteNonQuery();
            }
        }

        public Dictionary<BaseMaterial,int> GetBaseMaterial()
        {
            Dictionary<BaseMaterial, int> temp = new Dictionary<BaseMaterial, int>();
            using (MySqlCommand command = new MySqlCommand("Select ID,Nev,KezdoMennyiseg From Alapanyag", connection))
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

        public List<Building> GetBuilding()
        {
            List<Building> temp = new List<Building>();
            string[] helper = new string[2];
            BuildingStatus status;
            using (MySqlCommand command = new MySqlCommand("Select ID,EpuletTipusID,Koordinata,Statusz From Epulet", connection))
            {
                using (MySqlDataReader MySqlDataReader = command.ExecuteReader())
                {
                    while (MySqlDataReader.Read())
                    {
                        helper = MySqlDataReader.GetString(MySqlDataReader.GetOrdinal("Koordinata")).Split(',');
                        status = GetStatus(MySqlDataReader.GetString(MySqlDataReader.GetOrdinal("Statusz")));
                        temp.Add(new Building(MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("ID")),(BuildingTypeEnum)MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("EpuletTipusID")), new Rectangle(int.Parse(helper[0]),int.Parse(helper[1]),Globals.BUILDINGSIZE, Globals.BUILDINGSIZE),status));
                    }
                }
            }
            return temp;
        }

        public BuildingType GetBuildingType()
        {
            BuildingType temp = null;
            using (MySqlCommand command = new MySqlCommand("Select ID,Nev,TipusID From EpuletTipus", connection))
            {
                using (MySqlDataReader MySqlDataReader = command.ExecuteReader())
                {
                    while (MySqlDataReader.Read())
                    {
                        temp = (new Settlers.BuildingType(MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("ID")), MySqlDataReader.GetString(MySqlDataReader.GetOrdinal("Nev")), MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("TipusID"))));
                    }
                }
            }
            return temp;
        }
        public BuildingStatus GetStatus(string status)
        {
            BuildingStatus bs;
            switch (status)
            {
                case "Placing":
                    bs = BuildingStatus.Placing;
                    break;
                case "Construction":
                    bs = BuildingStatus.Construction;
                    break;
                case "Ready":
                    bs = BuildingStatus.Ready;
                    break;
                default:
                    bs = BuildingStatus.Construction;
                    break;
            }
            return bs;
        }

        /// <summary>
        /// Kapcsolat tesztelése
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
                throw ex;
            }
        }
        public void Update(int iID, Dictionary<string, string> paramList, string table)
        {
            using (MySqlCommand command = new MySqlCommand())
            {
                string commandText = $"UPDATE {table} SET ";
                foreach (var x in paramList)
                {
                    commandText += $"{x.Key} = @{x.Key}, ";
                    command.Parameters.AddWithValue(x.Key, x.Value);
                }
                commandText = commandText.Substring(0, commandText.Length - 2);

                commandText += " WHERE ID = @iID";
                command.Parameters.AddWithValue("iID", iID);

                command.CommandText = commandText;
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
        }

        public void UpdateBuilding()
        {
            throw new NotImplementedException();
        }

        public void DeleteTiles()
        {
            using (MySqlCommand command = new MySqlCommand("DELETE FROM Mezok WHere 1", connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public void InsertTiles(string line)
        {
            using (MySqlCommand command = new MySqlCommand("Insert Into Mezok (sor) Values(@line)", connection))
            {
                command.Parameters.AddWithValue("@line", line);
                command.ExecuteNonQuery();
            }
        }

        public List<Tile> GetTiles()
        {
            List<Tile> temp = new List<Tile>();
            string[] lines = new string[60];
            using (MySqlCommand command = new MySqlCommand("Select sor From Mezok", connection))
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
                                temp.Add(new Tile(new Rectangle(int.Parse(helper[0]), int.Parse(helper[1]), Globals.TILESIZE, Globals.TILESIZE), (TileState)int.Parse(helper[2])));

                            }

                        }
                    }
                }
            }
            return temp;
        }

        public void SaveMaterials(int bMaterialID, int count)
        {
            using (MySqlCommand command = new MySqlCommand("Insert Into Mentett_alapanyag (alapanyag_id,mennyiseg) Values(@bm, @count)", connection))
            {
                command.Parameters.AddWithValue("@bm", bMaterialID);
                command.Parameters.AddWithValue("@count", count);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteMaterials()
        {
            using (MySqlCommand command = new MySqlCommand("DELETE FROM Mentett_alapanyag WHere 1", connection))
            {
                command.ExecuteNonQuery();
            }
        }
        public Dictionary<BaseMaterial, int> GetSavedMaterial()
        {
            Dictionary<BaseMaterial, int> temp = new Dictionary<BaseMaterial, int>();
            using (MySqlCommand command = new MySqlCommand("Select a.ID,a.Nev,m.Mennyiseg From Alapanyag a INNER JOIN mentett_alapanyag m ON a.id= m.alapanyag_ID", connection))
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
    }
}
