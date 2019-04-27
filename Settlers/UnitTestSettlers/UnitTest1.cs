using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Settlers;

namespace UnitTestSettlers
{
    [TestClass]
    public class UnitTest1 
    {
        [TestMethod]
        public void TestMethod1()
        {
            Building wood = new Building(1,  new Rectangle(0, 0, Globals.BUILDINGSIZE, Globals.BUILDINGSIZE),null, (BuildingStatus)1, (BuildingTypeEnum) 1,false);
            wood.UpdateBuidlingStatus(Globals.CREATEBUILDINGTIME);
            Assert.AreEqual(BuildingStatus.Ready, wood.Status);
            wood.UpdateWorkerStatus(1);
            Assert.AreEqual(true, wood.HasWorker);
            Assert.AreEqual("woodcutter", wood.GetTextureName(wood.BuildingType));
            Building house = new Building(1, new Rectangle(0, 0, Globals.BUILDINGSIZE, Globals.BUILDINGSIZE), null, (BuildingStatus)2, (BuildingTypeEnum)7, false);
            Production p = new Production();
            Assert.AreEqual(10, p.HouseUpdate(house, 5));
            MySqlConnectionHandler connection = new MySqlConnectionHandler();
            //Ha meg van nyitva
            Assert.AreEqual(true, connection.TryOpen());
            //Ha nincs meg nyitva
            /*Assert.AreEqual(false, connection.TryOpen());*/
            //Ha van lementve
            Assert.AreEqual(40, connection.SelectTiles());
            //Ha nincs lementve
            /*Assert.AreEqual(0, connection.SelectTiles());*/
            p = connection.GetProductions(wood);
            Assert.AreEqual(0 , p.BaseMaterials.Count);
            Assert.AreEqual("Wood", p.ReadyMaterial.Name);
            Map map = new Map();
            Assert.AreEqual(BuildingTypeEnum.House, map.GetGameMenuBuildingType("house"));
            Assert.AreEqual(BuildingTypeEnum.Woodcutter, map.GetGameMenuBuildingType("woodcutter"));
            Assert.AreEqual(BuildingTypeEnum.Stonequarry, map.GetGameMenuBuildingType("stonequarry"));
            Assert.AreEqual(BuildingTypeEnum.Bakery, map.GetGameMenuBuildingType("bakery"));
            Assert.AreEqual(BuildingTypeEnum.Well, map.GetGameMenuBuildingType("well"));
            Assert.AreEqual(BuildingTypeEnum.Wheatfarm, map.GetGameMenuBuildingType("wheatfarm"));
            Assert.AreEqual(BuildingTypeEnum.Windmill, map.GetGameMenuBuildingType("windmill"));
            Assert.AreEqual(BuildingTypeEnum.Hunter, map.GetGameMenuBuildingType("hunter"));

        }
    }
}
