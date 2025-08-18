using System;
using System.Data;
using Europa.Extensions;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Europa.Data
{
    public class BitBooleanType : IUserType
    {
        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public new bool Equals(object x, object y)
        {
            return object.Equals(x, y);
        }

        public int GetHashCode(object x)
        {
            if (x.IsNull())
            {
                return 0;
            }

            return x.GetHashCode();
        }

        public bool IsMutable
        {
            get { return false; }
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            object obj = NHibernateUtil.Int32.NullSafeGet(rs, names[0]);

            if (obj.IsNull())
            {
                return null;
            }

            return (int)obj == 1;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value.IsNull())
            {
                ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                ((IDataParameter)cmd.Parameters[index]).Value = (bool)value ? 1 : 0;
            }
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public Type ReturnedType
        {
            get { return typeof(bool); }
        }

        public SqlType[] SqlTypes
        {
            get { return new[] { SqlTypeFactory.Int32 }; }
        }
    }
}