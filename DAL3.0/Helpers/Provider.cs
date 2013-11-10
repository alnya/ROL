using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DAL
{
    class Provider
    {
        public static string GetProvider(ConnectionSettings conn)
        {
            switch(conn.ConnectionType.ToLower())
            {
                case "oledb":
                    return "System.Data.OleDb";
                case "oracle":
                    return "Oracle.DataAccess.Client";
                case "odbc":
                    return "System.Data.Odbc";
            }

            return string.Empty;
        }

        public static DbType GetType(Type type)
        {
                if (type== typeof(string))
                   return DbType.String;

                if (type== typeof(UInt64))
                    return DbType.UInt64;

                if (type== typeof(Int64))
                    return DbType.Int64;

                if (type== typeof(Int32))
                   return DbType.Int32;

                if (type== typeof(UInt32))
                    return DbType.UInt32;

                if (type== typeof(float))
                    return DbType.Single;

                if (type== typeof(DateTime))
                    return DbType.Date;

                if (type== typeof(DateTime))
                    return DbType.DateTime;

                if (type== typeof(DateTime))
                    return DbType.Time;

                if (type== typeof(UInt16))
                    return DbType.UInt16;

                if (type== typeof(Int16))
                    return DbType.Int16;

                if (type== typeof(byte))
                    return DbType.SByte;

                if (type== typeof(object))
                    return DbType.Object;

                if (type==  typeof(decimal))
                    return DbType.VarNumeric;

                if (type== typeof(double))
                   return DbType.Currency;

                if (type== typeof(byte[]))
                    return DbType.Binary;

                if (type== typeof(decimal))
                    return DbType.Decimal;

                if (type== typeof(Double))
                    return DbType.Double;

                if (type== typeof(Guid))
                    return DbType.Guid;

                if (type== typeof(bool))
                    return DbType.Boolean;

            return DbType.Object; 
        }
    }
}
