namespace Settlers
{
    public class BuildingType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TypeID { get; set; }

        public BuildingType(int id, string name,int typeID)
        {
            this.ID = id;
            this.Name = name;
            this.TypeID = typeID;
        }
        public BuildingType(string name, int typeID)
        {
            this.Name = name;
            this.TypeID = typeID;
        }
    }
}