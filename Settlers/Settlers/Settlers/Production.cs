using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    public class Production
    {
        public BaseMaterial ReadyMaterial { get; set; }
        public Dictionary<BaseMaterial, int> BaseMaterials { get; set; }

        public int CreateTime { get; set; }
        public Production()
        {
            CreateTime = 0;
            this.ReadyMaterial = new BaseMaterial();
            this.BaseMaterials = new Dictionary<BaseMaterial, int>();
        }
        public void Update(Building building, Dictionary<BaseMaterial, int> baseMaterials, int ms)
        {
            CreateTime += ms;
            if (CreateTime >= Globals.CREATEPRODUCTTIME && building.HasWorker)
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
        public int HouseUpdate(Building b, int allWorkers)
        {
            b.IsItEmpty = true;
            return allWorkers + Globals.WORKERSPERHOUSE;

        }
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
