using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    public interface IMySqlConnectionHandler
    {
        /// <summary>
        /// Kapcsolat tesztelése és megnyitása
        /// </summary>
        /// <returns></returns>
        bool TryOpen();
        /// <summary>
        /// Épület tipusának megadásával lekérdezzük az adott épülethez szükséges nyersanyagokat
        /// </summary>
        /// <param name="bEnum">Enumban tárolt épület tipus azonositója</param>
        /// <returns></returns>
        int[] GetBuildingTypeCreate(BuildingTypeEnum bEnum);
        /// <summary>
        /// Az adatbázisba lementett épületeket kérdezi le
        /// </summary>
        /// <returns>Vissza adja az épületeket egy listában</returns>
        List<Building> GetBuilding();
        /// <summary>
        /// Lekérdezi az adatbázisban definiált nyersanyagokat, és a játék kezdetekor megkapott alap mennyiségét
        /// </summary>
        /// <returns>Dictionaryben elemenkét kapunk egy nyersanyagot és egy mennyiséget</returns>
        Dictionary<BaseMaterial,int> GetBaseMaterial();
        /// <summary>
        /// Mentéskor beszúrunk egy adott épületet az épület táblába
        /// </summary>
        /// <param name="building">Egy Building tipusú épület</param>
        void InsertBuilding(Building building);
        /// <summary>
        /// Kitörli az adatbázis épület táblájában lévő összes elemet
        /// </summary>
        void DeleteBuilding();
        /// <summary>
        /// Lekérdezi a lementett pályát
        /// </summary>
        /// <returns>Vissza adja a pálya mezőit</returns>
        List<Tile> GetTiles();
        /// <summary>
        /// Beszúrja az adatbázis mezők táblájába a pálya mezőket
        /// </summary>
        /// <param name="line">Hatékonyság miatt, a mezőket soronként töltjük fel</param>
        void InsertTiles(string line);
        /// <summary>
        /// Kitörli a mezők tábla elemeit
        /// </summary>
        void DeleteTiles();
        /// <summary>
        /// Lementi az alapanyagokat és az aktuális darabszámukat
        /// </summary>
        /// <param name="ID">Nyersanyag id-je</param>
        /// <param name="count">Darabszám</param>
        void SaveMaterials(int ID, int count);
        /// <summary>
        /// Lekérdezi a lementett alapanyagokat
        /// </summary>
        /// <returns></returns>
        Dictionary<BaseMaterial, int> GetSavedMaterial();
        /// <summary>
        /// Kitörli a lementett alapanyagokat
        /// </summary>
        void DeleteMaterials();
        /// <summary>
        /// Lekérdezi az adott épületnek a termelési adatait
        /// </summary>
        /// <param name="b">Az adott épület</param>
        /// <returns></returns>
        Production GetProductions(Building b);
        /// <summary>
        /// Lekérdezi a mezők tábla id-jait, és vissza adja hány darabot talált
        /// </summary>
        /// <returns>Vissza adja a mezők számát</returns>
        int SelectTiles();
    }
}
