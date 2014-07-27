using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Sun.Data.SqlServer
{
    /// <summary>
    /// //某个表的 "字段" 集合
    /// </summary>
    [Serializable]
    public class SqlColumnCollection : CollectionBase
    {
        public void Add(SqlColumn field)
        {
            base.List.Add(field);
        }

        /// <summary>
        /// //将一个sqlcolumninfo对象插入到 此集合中
        /// </summary>
        public void Insert(SqlColumn field)
        {
            int count = base.Count;         //base.count是从0开始的,,所以不需要加1
            base.List.Insert(count, field);
        }
        /// <summary>
        /// //将一个sqlcolumninfo对象插入到指定的位置,,但是如果base.count > 传入的index的话,,则将index=base.count 此集合中
        /// </summary>
        public void Insert(int index, SqlColumn field)
        {
            int count = index;
            if ((count > base.Count) || (count == -1))
            {
                count = base.Count;
            }
            base.List.Insert(count, field);
        }        

        public void Remove(SqlColumn field)
        {
            base.List.Remove(field);
        }

        public SqlColumn this[string name]
        {
            get
            {
                foreach (SqlColumn column in base.List)
                {
                    if (column.Name.ToLower() == name.ToLower())
                    {
                        return column;
                    }
                }
                return null;
            }
        }

        public SqlColumn this[int index]
        {
            get
            {
                return (SqlColumn)base.List[index];
            }
        }

        /// <summary>
        /// //返回主键
        /// </summary>
        public SqlColumn[] GetPK()
        {
            List<SqlColumn> list = new List<SqlColumn>();
            foreach (SqlColumn info in base.List)
            {
                if (info.IsKey)
                {
                    list.Add(info);
                }
            }
            return list.ToArray();
        }

        public int IndexOf(SqlColumn field)
        {
            return base.List.IndexOf(field);
        }

        /// <summary>
        /// //将字段和表名组合    //返回这种形式  sun_Users.Id,sun_Users.Name,sun_Users.Type  等
        /// </summary>
        internal string ToSqlText()
        {
            string str = string.Empty;
            foreach (SqlColumn cloum in this)
            {
                if (str.Length > 0)
                {
                    str = str + ",";
                }
                if (cloum.Function != null)
                {
                    string str2 = str;
                    str = str2 + cloum.Function + cloum.TableName + "." + cloum.Name;
                }
                else
                {
                    str = str + cloum.TableName + "." + cloum.Name;
                }
            }
            return str;
        }

        
    }
}
