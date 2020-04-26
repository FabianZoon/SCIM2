using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligenteSCIM2
{
    public class Result
    {
        public string[] schemas { get; set; }
        public int totalResults { get; set; }
        public int itemsPerPage { get; set; }
        public int startIndex { get; set; }
        public Resources[] resources { get; set; }


    }

    public class Resources
    {

    }

}
