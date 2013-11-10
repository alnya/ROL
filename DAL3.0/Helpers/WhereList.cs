using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Reflection;
using System.Text;

namespace DAL
{
    public class WhereList
    {
        private Type _obj;
        private WhereValueModifier _mod;
        private List<WhereItem> _items = new List<WhereItem>();

        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Create a list of parameters for the where statement
        /// </summary>
        /////////////////////////////////////////////////////////////
        public WhereList(Type obj, WhereValueModifier mod)
        {
            _mod = mod;
            _obj = obj;
        }

        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Add a column to the search
        /// </summary>
        /////////////////////////////////////////////////////////////
        public void Add(string col, string val)
        {
            Add(col, val, WhereItemModifier.Equal);
        }

        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Add a column to the search
        /// </summary>
        /////////////////////////////////////////////////////////////
        public void Add(string col, string val, WhereItemModifier mod)
        {
            // get all attributes with the "_" prefix (these should map to database columns)
            MemberInfo[] datamembers = _obj.FindMembers(MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic, Type.FilterName, "_*");

            if (col.ToLower() == "id")
            {
                _items.Add(new WhereItem(typeof(int), col, val, mod));
                return;
            }

			// get name / value
            foreach (FieldInfo field in datamembers)
            {
                string mappedDataColumnName;

                object[] attribs = field.GetCustomAttributes(typeof(DataColumnAttribute), true);
                if (attribs.GetLength(0) == 0)
                    mappedDataColumnName = field.Name.Substring(1).ToLower();
                else
                {
                    DataColumnAttribute changeAttrib = (DataColumnAttribute)attribs[0];
                    mappedDataColumnName = changeAttrib.name;
                }
                if (col.ToLower() == mappedDataColumnName.ToLower())
                {
                    // found it!
                    _items.Add(new WhereItem(field.FieldType, col, val, mod));   
                    break;
                }
            }
        }

        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Return the SQL string
        /// </summary>
        /////////////////////////////////////////////////////////////
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (WhereItem item in _items)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" ");
                    sb.Append(_mod.ToString(string.Empty));
                    sb.Append(" ");
                }
                sb.Append(item.ToString());
            }

            return sb.ToString();
        }

        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Return the SQL string (typed)
        /// </summary>
        /////////////////////////////////////////////////////////////
        public string ToString(string connectionType)
        {
            StringBuilder sb = new StringBuilder();
            foreach (WhereItem item in _items)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" ");
                    sb.Append(_mod.ToString(connectionType));
                    sb.Append(" ");
                }
                sb.Append(item.ToString());
            }

            return sb.ToString();
        }
    }

    class WhereItem
    {
        string _col;
        string _val;
        Type _fieldInfo;
        WhereItemModifier _modifier;

        public WhereItem(Type fieldinfo, string col, string val, WhereItemModifier mod)
        {
            _fieldInfo = fieldinfo;
            _col = col;
            _val = val;
            _modifier = mod;
        }

        public WhereItem(Type fieldinfo, string col, string val)
        {
            _fieldInfo = fieldinfo;
            _col = col;
            _val = val;
            _modifier = WhereItemModifier.Equal;
        }

        public string ToString(string connectionType)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_col);
            sb.Append(GetMod(_modifier));

            if (_modifier == WhereItemModifier.In || _modifier == WhereItemModifier.NotIn)
                sb.Append(_val);
            else
            {
                switch (_fieldInfo.ToString().ToLower())
                {
                    case "system.string":
                        sb.Append("'");
                        if (_modifier == WhereItemModifier.Like)
                            sb.Append("%");
                        sb.Append(Format.DBString(_val));
                        if (_modifier == WhereItemModifier.Like || _modifier == WhereItemModifier.StartsWith)
                            sb.Append("%");
                        sb.Append("'");
                        break;
                    case "system.datetime":
                        if (connectionType.ToLower() == "oracle")
                        {
                            sb.Append(Format.DBDateOracle(_val));
                        }
                        else
                        {
                            sb.Append("'");
                            sb.Append(Format.DBDate(_val));
                            sb.Append("'");
                        }
                        break;
                    default:
                        sb.Append(Format.DBInt(_val));
                        break;
                }
            }

            return sb.ToString();
        }

        private string GetMod(WhereItemModifier mod)
        {
            switch (mod)
            {
                case WhereItemModifier.Equal:
                    return " = ";
                case WhereItemModifier.GreaterThan:
                    return " > ";
                case WhereItemModifier.LessThan:
                    return " < ";
                case WhereItemModifier.StartsWith:
                case WhereItemModifier.Like:
                    return " LIKE ";
                case WhereItemModifier.NotEqual:
                    return " <> ";
                case WhereItemModifier.In:
                    return " IN ";
                case WhereItemModifier.NotIn:
                    return " NOT IN ";
            }

            return " = ";
        }
    }

    public enum WhereItemModifier { Equal, NotEqual, GreaterThan, LessThan, Like, In, StartsWith, NotIn }
    public enum WhereValueModifier { And, Or }
}
