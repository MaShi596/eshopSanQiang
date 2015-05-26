using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_Address_AddressList : AscxTemplatedWebControl
	{
		public delegate void CommandEventHandler(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e);
		public const string TagID = "list_Common_Consignee_ConsigneeList";
		private System.Web.UI.WebControls.DataList dtlstRegionsSelect;
		public event Common_Address_AddressList.CommandEventHandler ItemCommand;
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
		public System.Web.UI.WebControls.DataListItemCollection Items
		{
			get
			{
				return this.dtlstRegionsSelect.Items;
			}
		}
		public System.Web.UI.WebControls.DataKeyCollection DataKeys
		{
			get
			{
				return this.dtlstRegionsSelect.DataKeys;
			}
		}
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.dtlstRegionsSelect.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.dtlstRegionsSelect.DataSource = value;
			}
		}
		public Common_Address_AddressList()
		{
			base.ID = "list_Common_Consignee_ConsigneeList";
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Address_AddressList.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.dtlstRegionsSelect = (System.Web.UI.WebControls.DataList)this.FindControl("dtlstRegionsSelect");
			this.dtlstRegionsSelect.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dtlstRegionsSelect_ItemCommand);
		}
		private void dtlstRegionsSelect_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			this.ItemCommand(sender, e);
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			this.dtlstRegionsSelect.DataSource = this.DataSource;
			this.dtlstRegionsSelect.DataBind();
		}
	}
}
