using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Coupons)]
	public class UsedCoupons : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtCouponName;
		protected System.Web.UI.WebControls.TextBox txtOrderID;
		protected System.Web.UI.WebControls.DropDownList Dpstatus;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected Grid grdCoupons;
		protected Pager pager;
		private string couponName = string.Empty;
		private string couponOrder = string.Empty;
		private int? couponstatus;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindCouponList();
			}
		}
		private void ReloadHelpList(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("couponName", Globals.UrlEncode(this.txtCouponName.Text.Trim()));
			nameValueCollection.Add("OrderID", Globals.UrlEncode(this.txtOrderID.Text.Trim()));
			nameValueCollection.Add("CouponStatus", this.Dpstatus.SelectedValue);
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
			base.ReloadPage(nameValueCollection);
		}
		protected string IsCouponEnd(object endtime)
		{
			System.DateTime dateTime = System.Convert.ToDateTime(endtime);
			if (dateTime.CompareTo(System.DateTime.Now) > 0)
			{
				return dateTime.ToString();
			}
			return "已过期";
		}
		protected void BindCouponList()
		{
			DbQueryResult couponsList = PromoteHelper.GetCouponsList(new CouponItemInfoQuery
			{
				CounponName = this.couponName,
				OrderId = this.couponOrder,
				CouponStatus = this.couponstatus,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = "GenerateTime",
				SortOrder = SortAction.Desc
			});
            this.pager.TotalRecords = couponsList.TotalRecords;
			this.grdCoupons.DataSource = couponsList.Data;
			this.grdCoupons.DataBind();
		}
		private void LoadParameters()
		{
			if (!base.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["couponName"]))
				{
					this.couponName = Globals.UrlDecode(this.Page.Request.QueryString["couponName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderID"]))
				{
					this.couponOrder = Globals.UrlDecode(this.Page.Request.QueryString["OrderID"]);
				}
				if (!string.IsNullOrEmpty(base.Request.QueryString["CouponStatus"]))
				{
					this.couponstatus = new int?(System.Convert.ToInt32(base.Request.QueryString["CouponStatus"]));
				}
				this.txtOrderID.Text = this.couponOrder;
				this.txtCouponName.Text = this.couponName;
				this.Dpstatus.SelectedValue = System.Convert.ToString(this.couponstatus);
				return;
			}
			this.couponName = this.txtCouponName.Text;
			this.couponOrder = this.txtOrderID.Text;
		}
		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadHelpList(true);
		}
	}
}
