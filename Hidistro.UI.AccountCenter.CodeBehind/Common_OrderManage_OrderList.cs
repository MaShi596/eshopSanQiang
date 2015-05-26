using ASPNET.WebControls;
using Hidistro.Core.Enums;
using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_OrderManage_OrderList : AscxTemplatedWebControl
	{
		public delegate void DataBindEventHandler(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e);
		public delegate void CommandEventHandler(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e);
		public delegate void ReBindDataEventHandler(object sender);
		public const string TagID = "Common_OrderManage_OrderList";
		private Grid listOrders;
		public event Common_OrderManage_OrderList.DataBindEventHandler ItemDataBound;
		public event Common_OrderManage_OrderList.CommandEventHandler ItemCommand;
		public event Common_OrderManage_OrderList.ReBindDataEventHandler ReBindData;
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
				return this.listOrders.DataKeys;
			}
		}
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.listOrders.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.listOrders.DataSource = value;
			}
		}
		public string SortOrderBy
		{
			get
			{
				string result;
				if (this.listOrders != null)
				{
					result = this.listOrders.SortOrderBy;
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
		public Common_OrderManage_OrderList()
		{
			base.ID = "Common_OrderManage_OrderList";
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_OrderList.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
            this.listOrders = (Grid)this.FindControl("listOrders");
            this.listOrders.RowDataBound += new GridViewRowEventHandler(this.listOrders_ItemDataBound);
            this.listOrders.RowCommand += new GridViewCommandEventHandler(this.listOrders_RowCommand);
            this.listOrders.ReBindData += new Grid.ReBindDataEventHandler(this.listOrders_ReBindData);
		}
		private void listOrders_ReBindData(object sender)
		{
			this.ReBindData(sender);
		}
		private void listOrders_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			this.ItemCommand(sender, e);
		}
		private void listOrders_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			this.ItemDataBound(sender, e);
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			this.listOrders.DataSource = this.DataSource;
			this.listOrders.DataBind();
		}
	}
}
