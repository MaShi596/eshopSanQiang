using ASPNET.WebControls;
using Hidistro.Core.Enums;
using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_OrderManage_ReplaceApply : AscxTemplatedWebControl
	{
		public const string TagID = "Common_OrderManage_ReplaceApply";
		private Grid listReplace;
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
				return this.listReplace.DataKeys;
			}
		}
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.listReplace.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.listReplace.DataSource = value;
			}
		}
		public string SortOrderBy
		{
			get
			{
				string result;
				if (this.listReplace != null)
				{
					result = this.listReplace.SortOrderBy;
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
		public Common_OrderManage_ReplaceApply()
		{
			base.ID = "Common_OrderManage_ReplaceApply";
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_ReplaceApply.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.listReplace = (Grid)this.FindControl("listReplace");
			this.listReplace.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.listReplace_RowDataBound);
		}
		private void listReplace_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
			this.listReplace.DataSource = this.DataSource;
			this.listReplace.DataBind();
		}
	}
}
