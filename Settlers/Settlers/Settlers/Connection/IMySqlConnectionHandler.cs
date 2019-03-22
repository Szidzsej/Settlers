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
        void InsertBuilding(List<Building> buildings);
        void UpdateBuilding();
        void DeleteBuilding();
    }
}
