using Europa.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Domain.Core.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoContato
    {
        Email = 1,
        Celular = 2,
        Telefone = 3
    }
}
