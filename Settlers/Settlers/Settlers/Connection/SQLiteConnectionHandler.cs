using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Settlers
{
    public class SQLiteConnectionHandler : ISQLConnectionHandler
    {
        SQLiteConnection connection;

        public SQLiteConnectionHandler(string iPath)
        {
            string connectionString = $"Data Source={iPath};Version=3;";

            connection = new SQLiteConnection(connectionString);
        }

        public List<BuildingTypeCreate> GetBuildingTypeCreate()
        {
            List<BuildingTypeCreate> temp = new List<BuildingTypeCreate>();
            using (SQLiteCommand command = new SQLiteCommand("Select ID,EpuletTipusID,AlapanyagID,Mennyiseg From EpuletTipusElkeszites", connection)) 
            {
                using (SQLiteDataReader SQLiteDataReader = command.ExecuteReader())
                {
                    while (SQLiteDataReader.Read())
                    {
                        temp.Add(new Settlers.BuildingTypeCreate(SQLiteDataReader.GetInt32(SQLiteDataReader.GetOrdinal("ID")),SQLiteDataReader.GetInt32(SQLiteDataReader.GetOrdinal("EpuletTipusID")),SQLiteDataReader.GetInt32(SQLiteDataReader.GetOrdinal("AlapanyagID")),SQLiteDataReader.GetInt32(SQLiteDataReader.GetOrdinal("Mennyiseg"))));
                    }
                }
            }
            return temp;
        }

        public void DeleteBuilding()
        {
            throw new NotImplementedException();
        }

        public void InsertBuilding()
        {
            throw new NotImplementedException();
        }

        public List<BaseMaterial> GetBaseMaterial()
        {
            List<BaseMaterial> temp = new List<BaseMaterial>();
            using (SQLiteCommand command = new SQLiteCommand("Select ID,Nev From Alapanyag", connection))
            {
                using (SQLiteDataReader SQLiteDataReader = command.ExecuteReader())
                {
                    while (SQLiteDataReader.Read())
                    {
                        temp.Add(new Settlers.BaseMaterial(SQLiteDataReader.GetInt32(SQLiteDataReader.GetOrdinal("ID")), SQLiteDataReader.GetString(SQLiteDataReader.GetOrdinal("Nev"))));
                    }
                }
            }
            return temp;
        }

        public List<Building> GetBuilding()
        {
            List<Building> temp = new List<Building>();
            using (SQLiteCommand command = new SQLiteCommand("Select ID,EpuletTipusID,Koordinata From Epulet", connection))
            {
                using (SQLiteDataReader SQLiteDataReader = command.ExecuteReader())
                {
                    while (SQLiteDataReader.Read())
                    {
                        temp.Add(new Building(SQLiteDataReader.GetInt32(SQLiteDataReader.GetOrdinal("ID")), SQLiteDataReader.GetInt32(SQLiteDataReader.GetOrdinal("EpuletTipusID"))));
                    }
                }
            }
            return temp;
        }

        public List<BuildingType> GetBuildingType()
        {
            List<BuildingType> temp = new List<BuildingType>();
            using (SQLiteCommand command = new SQLiteCommand("Select ID,Nev,TipusID From Epulet", connection))
            {
                using (SQLiteDataReader SQLiteDataReader = command.ExecuteReader())
                {
                    while (SQLiteDataReader.Read())
                    {
                        temp.Add(new Settlers.BuildingType(SQLiteDataReader.GetInt32(SQLiteDataReader.GetOrdinal("ID")), SQLiteDataReader.GetString(SQLiteDataReader.GetOrdinal("Nev")), SQLiteDataReader.GetInt32(SQLiteDataReader.GetOrdinal("TipusID"))));
                    }
                }
            }
            return temp;
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
            using (SQLiteCommand command = new SQLiteCommand())
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
