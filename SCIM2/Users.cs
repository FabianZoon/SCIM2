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
            Result result = new Result();
            result.schemas = new string[1]{ "urn:ietf:params:scim:api:messages:2.0:ListResponse" };
            result.itemsPerPage = 100;
            result.startIndex = 1;
            result.totalResults = 0;
            result.resources = new Resources[0];
            return result;
        }

    }
}
