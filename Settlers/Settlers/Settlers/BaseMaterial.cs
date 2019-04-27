using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    /// <summary>
    /// Nyersanyagok definiálása
    /// </summary>
   public class BaseMaterial
    {
        public int ID { get; set; } // Nyersanyag azonosítója
        public string Name { get; set; } // Nyersanyag neve
        public bool EndQuantity { get; set; } // Megtermelendő mennyiség

        public BaseMaterial()
        {
            EndQuantity = false;
        }
        public BaseMaterial(int id, string name)
        {
            this.ID = id;
            this.Name = name;
            EndQuantity = false;
        }
        public BaseMaterial(string name)
        {
            this.Name = name;
            EndQuantity = false;
        }
    }
}
