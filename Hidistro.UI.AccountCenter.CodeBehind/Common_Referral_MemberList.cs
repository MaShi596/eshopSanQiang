using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_Referral_MemberList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_Referral_MemberList";
		private System.Web.UI.WebControls.DataList dataListPointDetails;
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
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.dataListPointDetails.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.dataListPointDetails.DataSource = value;
			}
		}
		public Common_Referral_MemberList()
		{
			base.ID = "Common_Referral_MemberList";
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_Referral_MemberList.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.dataListPointDetails = (System.Web.UI.WebControls.DataList)this.FindControl("dataListPointDetails");
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.dataListPointDetails.DataSource != null)
			{
				this.dataListPointDetails.DataBind();
			}
		}
	}
}
