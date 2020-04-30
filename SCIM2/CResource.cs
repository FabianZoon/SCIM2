using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligenteSCIM2
{
    public class CResource
    {
        public string[] Schemas { get; set; }
        public CResource() { }
        public void SetSchema(SchemasHelper.Schema schema)
        {
            Schemas = SchemasHelper.Get(schema);
        }
    }
}
