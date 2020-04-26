using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligenteSCIM2
{
    public class Users
    {
        public static Result Get(int? startIndex, int? count)
        {
            Result result = new Result
            {
                schemas = new string[1] { "urn:ietf:params:scim:api:messages:2.0:ListResponse" },
                itemsPerPage = 100,
                startIndex = 1,
                totalResults = 0,
                resources = new Resources[0]
            };
            return result;
        }

    }
}
