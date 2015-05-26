using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyGifts : DistributorPage
	{
		private string giftName;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected System.Web.UI.WebControls.LinkButton lkbtnDownloadCheck1;
		protected Grid grdGift;
		protected Pager pager;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.lkbtnDownloadCheck1.Click += new System.EventHandler(this.lkbtnDownloadCheck_Click);
            this.grdGift.ReBindData += new Grid.ReBindDataEventHandler(this.grdGift_ReBindData);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindData();
				this.DownLoad();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void DownLoad()
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["oper"]) && base.Request.QueryString["oper"].Trim() == "download" && !string.IsNullOrEmpty(base.Request.QueryString["GiftId"]) && int.Parse(base.Request.QueryString["GiftId"].Trim()) > 0)
			{
				int giftId = int.Parse(base.Request.QueryString["GiftId"].Trim());
				GiftInfo giftDetails = GiftHelper.GetGiftDetails(giftId);
				if (giftDetails.IsDownLoad && giftDetails.PurchasePrice > 0m)
				{
					if (SubsiteGiftHelper.DownLoadGift(giftDetails))
					{
						this.ReloadGiftsList(true);
						this.ShowMsg("下载礼品" + giftDetails.Name + "成功！", true);
						return;
					}
					this.ShowMsg("下载礼品" + giftDetails.Name + "失败！", false);
				}
			}
		}
		private void LoadParameters()
		{
			if (!base.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GiftName"]))
				{
					this.giftName = this.Page.Request.QueryString["GiftName"];
				}
				this.txtSearchText.Text = Globals.HtmlDecode(this.giftName);
				return;
			}
			this.giftName = Globals.HtmlEncode(this.txtSearchText.Text.Trim());
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadGiftsList(true);
		}
		private void ReloadGiftsList(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("GiftName", Globals.HtmlEncode(this.txtSearchText.Text.Trim()));
			nameValueCollection.Add("pageSize", this.hrefPageSize.SelectedSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
		private void grdGift_ReBindData(object sender)
		{
			this.ReloadGiftsList(false);
		}
		private void BindData()
		{
			GiftQuery giftQuery = new GiftQuery();
			giftQuery.Name = Globals.HtmlEncode(this.giftName);
			giftQuery.Page.PageSize = this.pager.PageSize;
			giftQuery.Page.PageIndex = this.pager.PageIndex;
			giftQuery.Page.SortBy = this.grdGift.SortOrderBy;
			if (this.grdGift.SortOrder.ToLower() == "desc")
			{
				giftQuery.Page.SortOrder = SortAction.Desc;
			}
			DbQueryResult gifts = SubsiteGiftHelper.GetGifts(giftQuery);
			this.grdGift.DataSource = gifts.Data;
			this.grdGift.DataBind();
			this.pager.TotalRecords=gifts.TotalRecords;
			this.pager1.TotalRecords=gifts.TotalRecords;
		}
		private void lkbtnDownloadCheck_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdGift.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox != null && checkBox.Checked)
				{
					num++;
				}
			}
			if (num == 0)
			{
				this.ShowMsg("请先选择要下载的礼品", false);
				return;
			}
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow2 in this.grdGift.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox2 = (System.Web.UI.WebControls.CheckBox)gridViewRow2.FindControl("checkboxCol");
				if (checkBox2.Checked)
				{
					int giftId = (int)this.grdGift.DataKeys[gridViewRow2.RowIndex].Value;
					GiftInfo giftDetails = GiftHelper.GetGiftDetails(giftId);
					SubsiteGiftHelper.DownLoadGift(giftDetails);
				}
			}
			this.ShowMsg("下载的礼品成功", true);
			this.ReloadGiftsList(true);
		}
	}
}
