using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Sun.SunControl;
using Sun.Core;
using Sun.Entity.Pagelet;

namespace Sun.VCC
{
    public class FriendlinkHandler : Base
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


            if (data.Count > 0)
            {
                entity = data[0];

                base.SetControl<Entity.Pagelet.FriendLink>(entity);

                Sun.SunControl.SunDropDownList groups = this.FindControl("groupID") as Sun.SunControl.SunDropDownList;

                var groupList = Sun.Data.PageletDataHelper.GetFillList<Sun.Entity.Pagelet.FriendLinkGroup>(
                    "FriendLinkGroup",
                    typeof(Sun.Entity.Pagelet.FriendLinkGroup),
                    null);

                groups.DataSource = groupList;
                groups.DataTextField = "Subject";
                groups.DataValueField = "ID";
                groups.DataBind();
                groups.SelectedValue = entity.GroupID.ToString();

                var rbtnList = this.FindControl("isEnable") as Sun.SunControl.SunRadioButtonList;//System.Web.UI.WebControls.RadioButtonList;
                rbtnList.SelectedIndex = entity.IsEnable == true ? 0 : 1;
            }





        }



    }
}
