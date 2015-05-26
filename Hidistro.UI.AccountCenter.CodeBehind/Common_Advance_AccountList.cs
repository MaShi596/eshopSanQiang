using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_Advance_AccountList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_Advance_AccountList";
		private System.Web.UI.WebControls.DataList dataListAccountDetails;
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.dataListAccountDetails.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.dataListAccountDetails.DataSource = value;
			}
		}
		public Common_Advance_AccountList()
		{
			base.ID = "Common_Advance_AccountList";
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_Advance_AccountList.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.dataListAccountDetails = (System.Web.UI.WebControls.DataList)this.FindControl("dataListAccountDetails");
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.dataListAccountDetails.DataSource != null)
			{
				this.dataListAccountDetails.DataBind();
			}
		}
	}
}
