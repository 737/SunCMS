using System;
using Sun.Data;
using Sun.Entity;
using Sun.Entity.Pagelet;
using Sun.SunControl;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web;
using System.Reflection;
using System.Collections.Specialized;

namespace Sun.Core
{
    public class HelperPagelet
    {
        private Wisp __wisp;

        public HelperPagelet()
        {
            this.__wisp = new Wisp();
            this.__query = null; //new PageletQuery(queryInfo);

        }

        public void Update(PageletModel model)
        {
            if (model == null)
            {
                return;
            }

            //将旧的model一并更新
            this.__model = model;
            switch (model.EntityInfo.DataStyle)
            {
                case EDataStyle.CONFIG:
                    this.SetConfigData();
                    break;
                case EDataStyle.DATABANK:
                    this.SetDateBankData();
                    break;
                default:
                    break;
            }
        }


        private void SetConfigData()
        {
            if (this.GetModel() != null)
            {
                Sun.ConfigHelper.SaveConfig(this.GetModel().GetData(), this.GetQuery().GetEntityInfo().MappingName);
            }
        }

        private void SetDateBankData()
        {

        }


        private PageletModel __model;
        public PageletModel GetModel()
        {
            if (this.__model == null)
            {
                this.__model = new PageletModel(this.GetQuery());
            }

            return this.__model;
        }
        public void SetModel(PageletModel model)
        {
            if (this.__model != model)
            {
                this.__model = model;
            }
        }

        public PageletQuery __query;
        public PageletQuery GetQuery()
        {
            return this.__query;
        }

        private PageletView __pageletView;
        public PageletView GetPageletView()
        {
            if (this.__pageletView == null)
            {
                PageletView pageletView = new PageletView(
                    this.__wisp.GetPageletModelName(),
                    this.__wisp.GetPageletName(),
                    this.__wisp.GetPageletAction()
                    );
                this.__pageletView = pageletView;
            }
            return this.__pageletView;
        }

    }
}
