using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Sun.Data;
using Sun.Entity.Pagelet;

namespace Sun.Data.SqlServer
{
    public class CommonDataProvider
    {


        public ArrayList GetList(EntityInfo info)
        {
            SqlHelper helper = new SqlHelper(info.MappingName);

            SqlDataReader reader = helper.GetDataReader(null, null);

            //DataTable dt = new DataTable();

            //dt.Load(reader);

            return FillModel.FillCollection(reader, info.EntityType);
        }

    }
}
