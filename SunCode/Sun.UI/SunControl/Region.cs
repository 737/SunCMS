using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using Sun.SunControl;
using Sun.Core;
using Sun.Data;
using Sun.Entity.Pagelet;

namespace Sun.UI.SunControl
{
    public class Region : PlaceHolder
    {
        private static readonly string TagPrefixKey = "Sun.MotherPage.Region";

        public static Region GetRegion(string regionID)
        {
            if (HttpContext.Current == null)
            {
                return null;
            }
            return (HttpContext.Current.Items[TagPrefixKey + regionID] as Region);
        }

        private void RegisterRegion()
        {
            string key = TagPrefixKey + this.ID;

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items.Contains(key))
                {
                    throw new InvalidOperationException("Region IDs must be unique. '" + this.ID + "' is already in use.");
                }
                HttpContext.Current.Items[key] = this;
            }
        }

        public override Control FindControl(string id)
        {
            Control control = base.FindControl(id);
            if ((control == null) && (this.Controls.Count > 0))
            {
                control = this.Controls[0].FindControl(id);
            }
            return control;
        }
        public bool isControlExist(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            Control control = this.FindControl(id);
            if (control != null)
            {
                return true;
            }
            return false;
        }

        public void SetRegion(HelperPagelet childlet)
        {   
            PageletView view = childlet.GetPageletView();

            if (view != null)
            {
                this.Controls.Clear();
                this.Controls.Add(view.GetView());
            }
        }

        //public void SetRegion(HelperPagelet childlet)
        //{
        //    PageletModel model = childlet.GetModel();
        //    PageletView view = childlet.GetView();
        //    PageletQuery query = childlet.GetQuery();

        //    if (view != null)
        //    {
        //        this.Controls.Clear();
        //        this.Controls.Add(view.GetView());
        //    }
        //    if ((model != null) && (model.EntityInfo != null))
        //    {
        //        if ((query.GetPageletAction() == PageletAction.Select) && (this.isControlExist("aggregation")))
        //        {
        //            Sun.SunControl.SunRepeater repeater = this.FindControl("aggregation") as Sun.SunControl.SunRepeater;
        //            repeater.DataSource = model.GetData();
        //            repeater.DataBind();
        //        }
        //        else
        //        {
        //            PropertyInfo[] InfoList = model.EntityInfo.EntityType.GetProperties();

        //            // 取list<t>中的第一个对象
        //            object t_model = null;
        //            IList ilist = model.GetData();
        //            if (ilist.Count > 0)
        //            {
        //                t_model = ilist[0];
        //            }

        //            foreach (PropertyInfo info in InfoList)
        //            {
        //                ISunControl control = this.FindControl(info.Name) as ISunControl;
        //                if (control != null)
        //                {
        //                    object t_value = info.GetValue(t_model, null);
        //                    if (t_value != null)
        //                    {
        //                        control.Value = t_value;
        //                    }
        //                }
        //            }
        //        }

        //        //switch (model.EntityInfo.BindTStyle)
        //        //{
        //        //    case Entity.Pagelet.EBindStyle.LIST:
        //        //        Sun.SunControl.SunRepeater repeater = this.FindControl("aggregation") as Sun.SunControl.SunRepeater;
        //        //        if (repeater != null)
        //        //        {
        //        //            repeater.DataSource = model.GetData();
        //        //            repeater.DataBind();
        //        //        }
        //        //        break;
        //        //    case Entity.Pagelet.EBindStyle.AROW:
        //        //        PropertyInfo[] InfoList = model.EntityInfo.EntityType.GetProperties();

        //        //        foreach (PropertyInfo info in InfoList)
        //        //        {
        //        //            ISunControl control = this.FindControl(info.Name) as ISunControl;

        //        //            //-------- repeater -------



        //        //            //Repeater peater = this.FindControl("repetaer") as Repeater;
        //        //            //peater.DataSource = oModel;
        //        //            //peater.DataBind();

        //        //            //return;

        //        //            //-------- /repeater -------


        //        //            if (control != null)
        //        //            {
        //        //                control.Value = info.GetValue(model.GetData(), null);
        //        //            }
        //        //        }
        //        //        break;

        //        //}
        //    }
        //}

        public PageletModel UpdateRegionModel(PageletModel model)
        {
            object entity = null;
            Type modelType = null;

            if (model != null)
            {
                modelType = model.EntityInfo.EntityType;
                entity = Activator.CreateInstance(modelType);

                foreach (PropertyInfo info in modelType.GetProperties())
                {
                    if (info.CanWrite)
                    {
                        ISunControl control = this.FindControl(info.Name) as ISunControl;
                        if (control != null)
                        {
                            try
                            {
                                info.SetValue(entity, control.Value, null);
                            }
                            catch
                            {
                            }
                        }
                    }
                }

                model.SetEntity(entity);
            }

            return model;
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
                this.RegisterRegion();
            }
        }
    }
}
