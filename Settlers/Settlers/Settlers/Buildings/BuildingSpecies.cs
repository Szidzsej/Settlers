namespace Settlers
{
    public class BuildingSpecies
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public BuildingSpecies(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
        public BuildingSpecies(string name)
        {
            this.Name = name;
        }
    }
}