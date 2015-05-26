using ASPNET.WebControls;
using Hidistro.Core.Enums;
using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_OrderManage_ReturnsApply : AscxTemplatedWebControl
	{
		public const string TagID = "Common_OrderManage_ReturnsApply";
		private Grid listReturns;
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		public System.Web.UI.WebControls.DataKeyArray DataKeys
		{
			get
			{
				return this.listReturns.DataKeys;
			}
		}
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.listReturns.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.listReturns.DataSource = value;
			}
		}
		public string SortOrderBy
		{
			get
			{
				string result;
				if (this.listReturns != null)
				{
					result = this.listReturns.SortOrderBy;
				}
				else
				{
					result = string.Empty;
				}
				return result;
			}
		}
		public SortAction SortOrder
		{
			get
			{
				return SortAction.Desc;
			}
		}
		public Common_OrderManage_ReturnsApply()
		{
			base.ID = "Common_OrderManage_ReturnsApply";
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_ReturnsApply.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.listReturns = (Grid)this.FindControl("listReturns");
			this.listReturns.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.listReturns_RowDataBound);
		}
		private void listReturns_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblHandleStatus");
				if (label.Text == "0")
				{
					label.Text = "待处理";
				}
				else
				{
					if (label.Text == "1")
					{
						label.Text = "已处理";
					}
					else
					{
						label.Text = "已拒绝";
					}
				}
			}
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			this.listReturns.DataSource = this.DataSource;
			this.listReturns.DataBind();
		}
	}
}
