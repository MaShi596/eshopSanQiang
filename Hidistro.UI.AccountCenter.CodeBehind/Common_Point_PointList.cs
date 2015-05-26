using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Common_Point_PointList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_Point_PointList";
		private System.Web.UI.WebControls.DataList dataListPointDetails;
		public event System.Web.UI.WebControls.DataListItemEventHandler ItemDataBound;
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
		public Common_Point_PointList()
		{
			base.ID = "Common_Point_PointList";
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_UserPointList.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.dataListPointDetails = (System.Web.UI.WebControls.DataList)this.FindControl("dataListPointDetails");
			this.dataListPointDetails.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dataListPointDetails_ItemDataBound);
		}
		private void dataListPointDetails_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			this.ItemDataBound(sender, e);
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
