using Microsoft.Xna.Framework;
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

        public List<BuildingTypeCreate> GetBuildingTypeCreate()
        {
            List<BuildingTypeCreate> temp = new List<BuildingTypeCreate>();
            using (MySqlCommand command = new MySqlCommand("Select ID,EpuletTipusID,AlapanyagID,Mennyiseg From EpuletTipusElkeszites", connection)) 
            {
                using (MySqlDataReader MySqlDataReader = command.ExecuteReader())
                {
                    while (MySqlDataReader.Read())
                    {
                        temp.Add(new Settlers.BuildingTypeCreate(MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("ID")),MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("EpuletTipusID")),MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("AlapanyagID")),MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("Mennyiseg"))));
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

        public void InsertBuilding(List<Building> buildings)
        {
            string helper = null;
            DeleteBuilding();
            using (MySqlCommand command = new MySqlCommand("Insert Into epulet (ID,EpuletTipusID,Koordinata, Statusz) Values(@id,@bTID,@coor,@stat)",connection))
            {
                if ((buildings.Count() != 0) && buildings != null)
                {
                    foreach (var item in buildings)
                    {
                        
                        command.Parameters.AddWithValue("@id", item.ID);
                        command.Parameters.AddWithValue("@bTID", (int)item.BuildingType);
                        helper = item.Bounds.X + "," + item.Bounds.Y;
                        command.Parameters.AddWithValue("@coor", helper);
                        
                        command.Parameters.AddWithValue("@stat", item.Status.ToString());
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<BaseMaterial> GetBaseMaterial()
        {
            List<BaseMaterial> temp = new List<BaseMaterial>();
            using (MySqlCommand command = new MySqlCommand("Select ID,Nev From Alapanyag", connection))
            {
                using (MySqlDataReader MySqlDataReader = command.ExecuteReader())
                {
                    while (MySqlDataReader.Read())
                    {
                        temp.Add(new Settlers.BaseMaterial(MySqlDataReader.GetInt32(MySqlDataReader.GetOrdinal("ID")), MySqlDataReader.GetString(MySqlDataReader.GetOrdinal("Nev"))));
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
    }
}
