using ASPNET.WebControls;
using Hidistro.Entities.Sales;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Web.CustomMade;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_Admin_ShipOrdersPriceTjForShipPoint : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected RoleDropDownList dropRolesList;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected Grid grdManager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.grdManager.ReBindData += new Grid.ReBindDataEventHandler(this.grdManager_ReBindData);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
		}
		private void grdManager_ReBindData(object sender)
		{
			this.ReloadManagerLogs(false);
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadManagerLogs(true);
		}
		private void BindData()
		{
			OrderQuery query = this.GetQuery();
			System.Data.DataTable dataSource = Methods.Supplier_ShipPointShipOrderTjGet(query);
			this.grdManager.DataSource = dataSource;
			this.grdManager.DataBind();
			this.txtSearchText.Text = query.UserName;
            this.calendarStartDate.SelectedDate = query.StartDate;
            this.calendarEndDate.SelectedDate = query.EndDate;
		}
		private void ReloadManagerLogs(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("Username", this.txtSearchText.Text);
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("StartDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("EndDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
		private OrderQuery GetQuery()
		{
			OrderQuery orderQuery = new OrderQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Username"]))
			{
				orderQuery.UserName = base.Server.UrlDecode(this.Page.Request.QueryString["Username"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
			{
				orderQuery.StartDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["StartDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
			{
				orderQuery.EndDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["EndDate"]));
			}
			return orderQuery;
		}
	}
}
