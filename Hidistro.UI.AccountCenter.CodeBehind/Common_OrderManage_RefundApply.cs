using ASPNET.WebControls;
using Hidistro.Core.Enums;
using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_OrderManage_RefundApply : AscxTemplatedWebControl
	{
		public const string TagID = "Common_OrderManage_RefundApply";
		private Grid listRefunds;
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
				return this.listRefunds.DataKeys;
			}
		}
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.listRefunds.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.listRefunds.DataSource = value;
			}
		}
		public string SortOrderBy
		{
			get
			{
				string result;
				if (this.listRefunds != null)
				{
					result = this.listRefunds.SortOrderBy;
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
		public Common_OrderManage_RefundApply()
		{
			base.ID = "Common_OrderManage_RefundApply";
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_RefundApply.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.listRefunds = (Grid)this.FindControl("listRefunds");
			this.listRefunds.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.listRefunds_RowDataBound);
		}
		private void listRefunds_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
			this.listRefunds.DataSource = this.DataSource;
			this.listRefunds.DataBind();
		}
	}
}
