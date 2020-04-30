using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligenteSCIM2
{
    public class CResources
    {
        public string[] Schemas { get; set; }
        public CResource[] Resources { get; set; }
        public CResources() { }
        public CResources(SchemasHelper.Schema schema)
        {
            Schemas = SchemasHelper.Get(schema);
        }
    }
}
