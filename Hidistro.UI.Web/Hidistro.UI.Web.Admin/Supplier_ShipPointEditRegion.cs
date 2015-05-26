using Hidistro.ControlPanel.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Web.CustomMade;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_ShipPointEditRegion : AdminPage
	{
		protected System.Web.UI.WebControls.Literal lblLoginNameValue;
		protected RegionSelector rsddlRegion;
		protected System.Web.UI.WebControls.Button btn_addRegion;
		protected System.Web.UI.WebControls.Repeater dlstRegion;
		private int userId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btn_addRegion.Click += new System.EventHandler(this.btn_addRegion_Click);
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.SiteManager manager = ManagerHelper.GetManager(this.userId);
				if (manager == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				System.Data.DataTable dataSource = Methods.Supplier_aspnet_UserRegionSelect(this.userId);
				this.dlstRegion.DataSource = dataSource;
				this.dlstRegion.DataBind();
				this.lblLoginNameValue.Text = manager.Username;
			}
		}
		private void GetRegionData(int useid)
		{
			System.Data.DataTable dataSource = Methods.Supplier_aspnet_UserRegionSelect(this.userId);
			this.dlstRegion.DataSource = dataSource;
			this.dlstRegion.DataBind();
		}
		protected void BtnDel_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
		{
			int num = System.Convert.ToInt32(e.CommandName);
			Methods.Supplier_aspnet_UserRegionDelete(num);
			this.GetRegionData(this.userId);
		}
		private void btn_addRegion_Click(object sender, System.EventArgs e)
		{
			int useid = this.userId;
			string province = "";
			string city = "";
			string area = "";
			if (!this.rsddlRegion.GetSelectedRegionId().HasValue)
			{
				this.ShowMsg("请选择发货点区域", true);
				return;
			}
			int value = this.rsddlRegion.GetSelectedRegionId().Value;
			System.Data.DataTable dataTable = Methods.Supplier_aspnet_UserRegionForRegionId(useid, value);
			if (dataTable.Rows.Count > 0)
			{
				this.ShowMsg("请勿插入重复记录", true);
				return;
			}
			string[] array = this.rsddlRegion.SelectedRegions.ToString().Split(new char[]
			{
				'，'
			});
			if (array.Length == 3)
			{
				province = array[0];
				city = array[1];
				area = array[2];
			}
			if (array.Length == 2)
			{
				province = array[0];
				city = array[1];
			}
			if (array.Length == 1)
			{
				province = array[0];
			}
			Methods.Supplier_aspnet_UserRegionInsert(useid, province, city, area, value);
			this.GetRegionData(useid);
			this.ShowMsg("成功添加一条发货点区域", true);
		}
	}
}
