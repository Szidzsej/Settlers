using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Settlers
{
    static class Globals
    {
        /// <summary>
        /// Mezők mérete
        /// </summary>
        public static int TILESIZE = 15;
        /// <summary>
        /// Épület mérete
        /// </summary>
        public static int BUILDINGSIZE = 30;
        /// <summary>
        /// Menű ikon mérete
        /// </summary>
        public static int MENUICON = 60;
        /// <summary>
        /// Sorok száma
        /// </summary>
        public static int TILEROWCOUNT=40;
        /// <summary>
        /// Oszlopok száma
        /// </summary>
        public static int TILECOLUMNCOUNT = 60;
        /// <summary>
        /// Játék kezdetekor lévő munkások száma
        /// </summary>
        public static int STARTWORKERS = 5;
        /// <summary>
        /// Termékek elkészítési ideje
        /// </summary>
        public static int CREATEPRODUCTTIME = 2000;
        /// <summary>
        /// Fa és kő kitermelési ideje, 1 kő/fa esetén 
        /// </summary>
        public static int CREATEWOODSTONEPRODUCTTIME = 10000;
        /// <summary>
        /// Épület felépítéséhez szükséges idő
        /// </summary>
        public static int CREATEBUILDINGTIME = 1500;
        /// <summary>
        /// Lakóházak hány munkás adnak
        /// </summary>
        public static int WORKERSPERHOUSE = 5;
    }
}
