using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxonomy.Schedule.Wpf.Data
{
    public static class db_connection
    {
        public static taxonamyEntities connection = new taxonamyEntities();
        public static Employee Employee { get; set; }
    }
}
