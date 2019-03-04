using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    public class BuildingTypeCreate
    {
        public int ID { get; set; }
        public int BuildingTypeID { get; set; }
        public Dictionary<int, int> Materials;

        public BuildingTypeCreate(int iD, int bTypeID, int bMaterialID,int quantity)
        {
            this.ID = iD;
            this.BuildingTypeID = bTypeID;
            this.Materials.Add(bMaterialID, quantity);
        }
        public BuildingTypeCreate(int bTypeID, int bMaterialID, int quantity)
        {
            this.BuildingTypeID = bTypeID;
            this.Materials.Add(bMaterialID, quantity);
        }
    }
}
