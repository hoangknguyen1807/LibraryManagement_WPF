using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApp
{
    public class Book
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int YearPublished { get; set; }
        public int Price { get; set; }
        public string ISBN { get; set; }
    }
}
