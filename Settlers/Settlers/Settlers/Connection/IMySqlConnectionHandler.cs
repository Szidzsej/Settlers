using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    public interface IMySqlConnectionHandler
    {
        /// <summary>
        /// Bármely tábla módosítása a megadott paraméterlista alapján
        /// </summary>
        /// <param name="iID"></param>
        /// <param name="paramList">paraméterek: key = oszlop, value = érték</param>
        /// <param name="table">mely táblát</param>
        void Update(int iID, Dictionary<string, string> paramList, string table);
        /// <summary>
        /// Kapcsolat tesztelése
        /// </summary>
        /// <returns></returns>
        bool TryOpen();
        BuildingType GetBuildingType();
        int[] GetBuildingTypeCreate(BuildingTypeEnum bEnum);
        List<Building> GetBuilding();
        Dictionary<BaseMaterial,int> GetBaseMaterial();
        void InsertBuilding(Building building);
        void UpdateBuilding();
        void DeleteBuilding();
        List<Tile> GetTiles();
        void InsertTiles(string line);
        void DeleteTiles();
        void SaveMaterials(int ID, int count);
        Dictionary<BaseMaterial, int> GetSavedMaterial();
        void DeleteMaterials();
        Production GetProductions(Building b);
        int SelectTiles();
    }
}
