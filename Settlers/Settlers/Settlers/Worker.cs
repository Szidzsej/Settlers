using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    public class Worker
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Worker(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
        public Worker(string name)
        {
            this.Name = name;
        }
    }
}
