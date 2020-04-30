using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DiligenteSCIM2.Result;

namespace DiligenteSCIM2
{
    public class Result
    {
        private int totalResults;
        private int itemsPerPage;

        public string[] Schemas { get; set; }
        public int TotalResults
        {
            get
            {
                if (Resources != null && totalResults < Resources.Length) totalResults = Resources.Length;
                return totalResults;
            }
            set => totalResults = value;
        }
        public int ItemsPerPage
        {
            get
            {
                if (Resources != null && itemsPerPage < Resources.Length) itemsPerPage = Resources.Length;
                return itemsPerPage;
            }
            set => itemsPerPage = value;
        }
        public int StartIndex { get; set; }
        public Object[] Resources { get; set; }


        public Result(SchemasHelper.Schema schema)
        {
            Schemas = SchemasHelper.Get(schema);
            StartIndex = 1;
        }
    }

   

}
