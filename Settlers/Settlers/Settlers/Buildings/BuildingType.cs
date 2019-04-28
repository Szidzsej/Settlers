namespace Settlers
{
    public class BuildingType
    {
        public int ID { get; set; } // Épület tipusának azonositója
        public string Name { get; set; } // épület tipusának a neve
        public int TypeID { get; set; } // Tipus azonositója az adatbázisban
        public string Image { get; set; } // Az épület tipusnak a textúrájának a neve

        public BuildingType(int id, string name,int typeID)
        {
            this.ID = id;
            this.Name = name;
            this.TypeID = typeID;
        }
        public BuildingType(string name, int typeID, string image)
        {
            this.Name = name;
            this.TypeID = typeID;
            this.Image = image;
        }
    }
}