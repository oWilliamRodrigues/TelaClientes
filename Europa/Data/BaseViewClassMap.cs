using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Data
{
    public class BaseViewClassMap<T> : BaseClassMap<T> where T : BaseEntity
    {
        public BaseViewClassMap()
        {
            SchemaAction.None();
            ReadOnly();
        }
    }
}
