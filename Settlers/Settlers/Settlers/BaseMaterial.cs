using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
   public class BaseMaterial
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public BaseMaterial(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
        public BaseMaterial(string name)
        {
            this.Name = name;
        }
    }
}
