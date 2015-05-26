using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Distribution;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.DistributorRequests)]
	public class DistributorRequests : AdminPage
	{
		private string Keywords;
		private string RealName;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtRealName;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdDistributorRequests;
		protected Pager pager1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl litUserName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl litName;
		protected System.Web.UI.HtmlControls.HtmlInputHidden txtUserId;
		protected System.Web.UI.WebControls.Button btnRefuseDistrbutor;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				int userId = 0;
				if (string.IsNullOrEmpty(base.Request["id"]) || !int.TryParse(base.Request["id"], out userId))
				{
					base.Response.Write("{\"Status\":\"0\"}");
					base.Response.End();
					return;
				}
				Hidistro.Membership.Context.Distributor distributor = DistributorHelper.GetDistributor(userId);
				if (distributor == null)
				{
					base.Response.Write("{\"Status\":\"0\"}");
					base.Response.End();
					return;
				}
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				stringBuilder.AppendFormat(",\"UserName\":\"{0}\"", distributor.Username);
				stringBuilder.AppendFormat(",\"RealName\":\"{0}\"", distributor.RealName);
				stringBuilder.AppendFormat(",\"CompanyName\":\"{0}\"", distributor.CompanyName);
				stringBuilder.AppendFormat(",\"Email\":\"{0}\"", distributor.Email);
				stringBuilder.AppendFormat(",\"Area\":\"{0}\"", RegionHelper.GetFullRegion(distributor.RegionId, string.Empty));
				stringBuilder.AppendFormat(",\"Address\":\"{0}\"", distributor.Address);
				stringBuilder.AppendFormat(",\"QQ\":\"{0}\"", distributor.QQ);
				stringBuilder.AppendFormat(",\"MSN\":\"{0}\"", distributor.MSN);
				stringBuilder.AppendFormat(",\"PostCode\":\"{0}\"", distributor.Zipcode);
				stringBuilder.AppendFormat(",\"Wangwang\":\"{0}\"", distributor.Wangwang);
				stringBuilder.AppendFormat(",\"CellPhone\":\"{0}\"", distributor.CellPhone);
				stringBuilder.AppendFormat(",\"Telephone\":\"{0}\"", distributor.TelPhone);
				stringBuilder.AppendFormat(",\"RegisterDate\":\"{0}\"", distributor.CreateDate);
				stringBuilder.AppendFormat(",\"LastLoginDate\":\"{0}\"", distributor.LastLoginDate);
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				base.Response.Write("{\"Status\":\"1\"" + stringBuilder.ToString() + "}");
				base.Response.End();
			}
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnRefuseDistrbutor.Click += new System.EventHandler(this.btnRefuseDistrbutor_Click);
			if (!base.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Keywords"]))
				{
					this.Keywords = this.Page.Request.QueryString["Keywords"];
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RealName"]))
				{
					this.RealName = this.Page.Request.QueryString["RealName"];
				}
				this.txtUserName.Text = this.Keywords;
				this.txtRealName.Text = this.RealName;
			}
			else
			{
				this.Keywords = this.txtUserName.Text.Trim();
				this.RealName = this.txtRealName.Text.Trim();
			}
			if (!this.Page.IsPostBack)
			{
				this.BindDistributorRequest();
			}
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			base.ReloadPage(new System.Collections.Specialized.NameValueCollection
			{

				{
					"Keywords",
					this.txtUserName.Text.Trim()
				},

				{
					"RealName",
					this.txtRealName.Text.Trim()
				},

				{
					"PageSize",
					this.pager.PageSize.ToString()
				},

				{
					"SortBy",
					this.grdDistributorRequests.SortOrderBy
				},

				{
					"SortOrder",
					SortAction.Desc.ToString()
				}
			});
		}
		private void btnRefuseDistrbutor_Click(object sender, System.EventArgs e)
		{
			if (DistributorHelper.RefuseDistributorRequest(int.Parse(this.txtUserId.Value)))
			{
				this.ShowMsg("成功的删除了申请信息", true);
				this.BindDistributorRequest();
				return;
			}
			this.ShowMsg("拒绝失败", false);
		}
		private void BindDistributorRequest()
		{
			DbQueryResult distributors = DistributorHelper.GetDistributors(new DistributorQuery
			{
				IsApproved = false,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				Username = this.Keywords,
				RealName = this.RealName,
				SortBy = "CreateDate",
				SortOrder = SortAction.Desc
			});
			this.grdDistributorRequests.DataSource = distributors.Data;
			this.grdDistributorRequests.DataBind();
            this.pager.TotalRecords = distributors.TotalRecords;
            this.pager1.TotalRecords = distributors.TotalRecords;
		}
	}
}
