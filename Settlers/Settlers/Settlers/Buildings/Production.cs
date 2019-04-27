using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    public class Production
    {
        public BaseMaterial ReadyMaterial { get; set; } // Az épület által termelt nyersanyag
        public Dictionary<BaseMaterial, int> BaseMaterials { get; set; } // A termeléshez szükséges nyersanyagokat tárolja

        public int CreateTime { get; set; } // Termelések között eltelt idő
        public Production()
        {
            CreateTime = 0;
            this.ReadyMaterial = new BaseMaterial();
            this.BaseMaterials = new Dictionary<BaseMaterial, int>();
        }

        /// <summary>
        /// A termelést végző metódus
        /// </summary>
        /// <param name="building">Az adott épület, amelyikena termelés folyik </param>
        /// <param name="baseMaterials">Az összes nyersanyagot tároló változó</param>
        /// <param name="ms">2 update között eltelt idő</param>
        public void Update(Building building, Dictionary<BaseMaterial, int> baseMaterials, int ms)
        {
            CreateTime += ms;
            if ((building.BuildingType == BuildingTypeEnum.Woodcutter) || (building.BuildingType == BuildingTypeEnum.Stonequarry))
            {
                if (building.WoodStoneCount != 0)
                {
                    if ((CreateTime >= (Globals.CREATEWOODSTONEPRODUCTTIME / building.WoodStoneCount)) && building.HasWorker)
                    {
                        foreach (var item in baseMaterials)
                        {
                            if (item.Key.Name == building.Production.ReadyMaterial.Name)
                            {
                                baseMaterials[item.Key]++;
                                CreateTime = 0;
                                break;
                            }
                        }
                    }
                }
            }
            else if (CreateTime >= Globals.CREATEPRODUCTTIME && building.HasWorker)
            {

                if ((building.Production.BaseMaterials == null || building.Production.BaseMaterials.Count() == 0))
                {
                    foreach (var item in baseMaterials)
                    {
                        if (item.Key.Name == building.Production.ReadyMaterial.Name)
                        {
                            baseMaterials[item.Key]++;
                            CreateTime = 0;
                            break;
                        }
                    }
                }
                else
                {
                    bool canCreate = false;
                    foreach (var item in baseMaterials)
                    {
                        foreach (var item2 in building.Production.BaseMaterials)
                        {
                            if (item.Key.Name == item2.Key.Name)
                            {
                                if ((item.Value - item2.Value) > 0)
                                {
                                    canCreate = true;
                                    break;
                                }
                                else
                                {
                                    canCreate = false;
                                }
                            }
                        }
                    }
                    if (canCreate)
                    {
                        foreach (var item2 in building.Production.BaseMaterials)
                        {
                            Loop(baseMaterials, item2.Key, item2.Value);
                        }
                        foreach (var item in baseMaterials)
                        {
                            if (building.Production.ReadyMaterial.Name == item.Key.Name)
                            {
                                baseMaterials[item.Key]++;
                                break;
                            }
                        }
                    }
                    CreateTime = 0;
                }
            }
        }

        /// <summary>
        /// A lakóház felépülésekor, kapunk plusz 5 munkás, ekkor a ház üressé válik
        /// </summary>
        /// <param name="b">Az adott lakóház, amely felépült</param>
        /// <param name="allWorkers">Az eddigi összes munkás száma</param>
        /// <returns>Vissza adja az összes munkás számát az új "lakókkal" együtt</returns>
        public int HouseUpdate(Building b, int allWorkers)
        {
            b.IsItEmpty = true;
            return allWorkers + Globals.WORKERSPERHOUSE;

        }
        /// <summary>
        /// Metódus egy ciklushoz, hogy az átláthatóbb legyen
        /// </summary>
        /// <param name="baseMaterials"></param>
        /// <param name="bm"></param>
        /// <param name="quantity"></param>
        public void Loop(Dictionary<BaseMaterial,int> baseMaterials, BaseMaterial bm, int quantity)
        {
            foreach (var item in baseMaterials)
            {
                if (item.Key.Name == bm.Name)
                {
                    baseMaterials[item.Key] -= quantity;
                    return;
                }
            }
        }
    }
}
