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

        public BaseMaterial()
        {
        }
        public BaseMaterial(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
        public BaseMaterial(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            BaseMaterial other = obj as BaseMaterial;
            if (other == null)
            {
                return false;
            }
            return Name.Equals(other.Name);
        }
    }
}
