using NHibernate.Engine;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Europa.Data.Dialect
{
    /// <summary>
    /// Maps a <see cref="System.TimeSpan" /> Property to an <see cref="DbType.TimeSpan" /> column 
    /// </summary>
    [Serializable]
    public partial class TimeSpanType : TimeAsTimeSpanType
    {
        public override void NullSafeSet(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
        {
            var obj = (TimeSpan)value;
            ((IDbDataParameter)st.Parameters[index]).Value = new TimeSpan(0, obj.Hours, obj.Minutes, obj.Seconds);
        }

        public override void Set(IDbCommand st, object value, int index)
        {
            ((IDbDataParameter)st.Parameters[index]).Value = (TimeSpan)value;
        }
    }
}
