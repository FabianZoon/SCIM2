using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligenteSCIM2
{
    public class Filter
    {
        public string Value1;
        public string Operator;
        public string Value2;
        public Filter() { }
        public Filter(string filterString)
        {
            Value1 = filterString.Substring(0, filterString.IndexOf(" ")).Trim();
            filterString = filterString.Substring( filterString.IndexOf(" ")+1);
            Operator = filterString.Substring(0, filterString.IndexOf(" ")).Trim();
            filterString = filterString.Substring(filterString.IndexOf(" ") + 1).Trim();
            Value2 = filterString;
            if (Value1.StartsWith("\"") && Value1.EndsWith("\"")) Value1 = Value1.Substring(1, Value1.Length - 2);
            if (Value2.StartsWith("\"") && Value2.EndsWith("\"")) Value2 = Value2.Substring(1, Value2.Length - 2);
            //userName%20eq%20%22isaac.zoon%40diligente.xyz%22
        }

    }
}
