using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace NHibernateIUserType
{
    class CustomDateTime : IUserType
    {
        public bool Equals(object x, object y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            object r = rs[names[0]];
            if (r == DBNull.Value)
            {
                return null;
            }

            DateTime storedTime = (DateTime)r;
            return new DateTimeOffset(storedTime, this.databaseTimeZone.BaseUtcOffset);
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                NHibernateUtil.DateTime.NullSafeSet(cmd, null, index);
            }
            else
            {
                DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
                DateTime paramVal = dateTimeOffset.ToOffset(this.databaseTimeZone.BaseUtcOffset).DateTime;

                IDataParameter parameter = (IDataParameter)cmd.Parameters[index];
                parameter.Value = paramVal;
            }
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public Type ReturnedType
        {
            get { return typeof(DateTimeOffset); }
        }

        public SqlType[] SqlTypes
        {
            get { return new[] { new SqlType(DbType.DateTime) }; }
        }

        public bool IsMutable
        {
            get { return false; }
        }
    }
}
