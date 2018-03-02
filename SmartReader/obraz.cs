using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartReader
{
    class obraz
    {
        //Количес}тво образов
        public int count { get; set; }

        //Буква
        public string name { get; set; }

        //Карта образа
        public int[] map { get; set; }


        public obraz(int count, string name, int[] map)
        {
            this.count = count;
            this.name = name;
            this.map = map;
        }

    }
}
