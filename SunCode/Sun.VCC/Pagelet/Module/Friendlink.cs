using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sun.SunControl;
using Sun.Core;
using Sun.Entity.Pagelet;
using System.Data;

namespace Sun.VCC
{
    public class Friendlink : Base
    {
        protected override void OnInit(EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Sun.Core.Wisp wisp = new Wisp();

            var entity = new Entity.Pagelet.FriendLink();

            Entity.Pagelet.FriendLink fillEntity = wisp.GetFillEntity<Entity.Pagelet.FriendLink>(entity.GetType());

            var data = Sun.Data.PageletDataHelper.GetFillList<FriendLink>(
                wisp.GetEntityInfo().MappingName,
                wisp.GetEntityInfo().EntityType,
                fillEntity);

            Sun.SunControl.SunRepeater re = this.FindControl("aggregation") as Sun.SunControl.SunRepeater;

            re.DataSource = data;
            re.DataBind();


        }



    }
}
