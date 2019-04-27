using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    /// <summary>
    /// Munkások definiálása
    /// </summary>
    public class Worker
    {
        public int ID { get; set; } // Munkás azonosítója
        public string Name { get; set; } // Munkás neve

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
