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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyGiftList : DistributorPage
	{
		private string giftName;
		private bool isPromotion;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chkPromotion;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected System.Web.UI.WebControls.LinkButton lkbtnDelete;
		protected Grid grdGift;
		protected Pager pager;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.lkbtnDelete.Click += new System.EventHandler(this.lkbtnDelete_Click);
            this.grdGift.ReBindData += new Grid.ReBindDataEventHandler(this.grdGift_ReBindData);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindData();
				this.DeleteGriftById();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void DeleteGriftById()
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["oper"]) && base.Request.QueryString["oper"].Trim() == "delete" && !string.IsNullOrEmpty(base.Request.QueryString["GiftId"]) && int.Parse(base.Request.QueryString["GiftId"].Trim()) > 0)
			{
				int giftId = int.Parse(base.Request.QueryString["GiftId"].Trim());
				GiftInfo giftDetails = GiftHelper.GetGiftDetails(giftId);
				if (SubsiteGiftHelper.DeleteGiftById(giftId))
				{
					this.ReloadGiftsList(true);
					this.ShowMsg("删除礼品" + giftDetails.Name + "成功！", true);
					return;
				}
				this.ShowMsg("删除礼品" + giftDetails.Name + "失败！", false);
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
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isPromotion"]))
				{
					bool.TryParse(this.Page.Request.QueryString["isPromotion"], out this.isPromotion);
				}
				this.chkPromotion.Checked = this.isPromotion;
				this.txtSearchText.Text = Globals.HtmlDecode(this.giftName);
				return;
			}
			this.giftName = Globals.HtmlEncode(this.txtSearchText.Text.Trim());
			this.isPromotion = this.chkPromotion.Checked;
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadGiftsList(true);
		}
		private void lkbtnDelete_Click(object sender, System.EventArgs e)
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
				this.ShowMsg("请先选择要删除的礼品", false);
				return;
			}
			bool success = true;
			string string_ = "删除礼品成功！";
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow2 in this.grdGift.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox2 = (System.Web.UI.WebControls.CheckBox)gridViewRow2.FindControl("checkboxCol");
				if (checkBox2.Checked)
				{
					int giftId = (int)this.grdGift.DataKeys[gridViewRow2.RowIndex].Value;
					if (!SubsiteGiftHelper.DeleteGiftById(giftId))
					{
						success = false;
						string_ = "删除礼品失败！";
						break;
					}
				}
			}
			this.ShowMsg(string_, success);
			this.ReloadGiftsList(true);
		}
		private void ReloadGiftsList(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("GiftName", Globals.HtmlEncode(this.txtSearchText.Text.Trim()));
			nameValueCollection.Add("isPromotion", this.chkPromotion.Checked.ToString());
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
			giftQuery.IsPromotion = this.isPromotion;
			giftQuery.Page.PageSize = this.pager.PageSize;
			giftQuery.Page.PageIndex = this.pager.PageIndex;
			giftQuery.Page.SortBy = this.grdGift.SortOrderBy;
			if (this.grdGift.SortOrder.ToLower() == "desc")
			{
				giftQuery.Page.SortOrder = SortAction.Desc;
			}
			DbQueryResult abstroGiftsById = SubsiteGiftHelper.GetAbstroGiftsById(giftQuery);
			this.grdGift.DataSource = abstroGiftsById.Data;
			this.grdGift.DataBind();
            this.pager.TotalRecords = abstroGiftsById.TotalRecords;
            this.pager1.TotalRecords = abstroGiftsById.TotalRecords;
		}
	}
}
