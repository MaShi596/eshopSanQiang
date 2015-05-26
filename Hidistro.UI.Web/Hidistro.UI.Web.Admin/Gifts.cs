using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Gifts)]
	public class Gifts : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox chkPromotion;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton lkbDelectCheck;
		protected Grid grdGift;
		protected Pager pager;
		private string giftName;
		private bool isPromotion;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.lkbDelectCheck.Click += new System.EventHandler(this.lkbDelectCheck_Click);
			this.grdGift.RowEditing += new System.Web.UI.WebControls.GridViewEditEventHandler(this.grdGift_RowEditing);
			this.grdGift.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdGift_RowDeleting);
            this.grdGift.ReBindData += new Grid.ReBindDataEventHandler(this.grdGift_ReBindData);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindData();
				this.UpdateIsDownLoad();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void UpdateIsDownLoad()
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["oper"]) && base.Request.QueryString["oper"].Trim() == "update" && !string.IsNullOrEmpty(base.Request.QueryString["GiftId"]) && !string.IsNullOrEmpty(base.Request.QueryString["Status"]) && int.Parse(base.Request.QueryString["GiftId"].Trim()) > 0 && int.Parse(base.Request.QueryString["Status"].Trim()) >= 0)
			{
				int giftId = int.Parse(base.Request.QueryString["GiftId"]);
				bool isdownload = false;
				string str = "取消";
				if (int.Parse(base.Request.QueryString["Status"].Trim()) == 1)
				{
					isdownload = true;
					str = "允许";
				}
				if (GiftHelper.UpdateIsDownLoad(giftId, isdownload))
				{
					this.BindData();
					this.ShowMsg(str + "当前礼品下载成功！", true);
					return;
				}
				this.ShowMsg(str + "当前礼品下载失败！", false);
			}
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
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
				this.txtSearchText.Text = Globals.HtmlDecode(this.giftName);
				this.chkPromotion.Checked = this.isPromotion;
				return;
			}
			this.giftName = Globals.HtmlEncode(this.txtSearchText.Text.Trim());
			this.isPromotion = this.chkPromotion.Checked;
		}
		private void grdGift_ReBindData(object sender)
		{
			this.BindData();
		}
		private void grdGift_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
		{
			int num = (int)this.grdGift.DataKeys[e.NewEditIndex].Value;
			System.Web.HttpContext.Current.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/promotion/EditGift.aspx?giftId={0}", num)));
		}
		private void grdGift_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int giftId = (int)this.grdGift.DataKeys[e.RowIndex].Value;
			if (GiftHelper.DeleteGift(giftId))
			{
				this.ShowMsg("成功的删除了一件礼品信息", true);
				this.BindData();
				return;
			}
			this.ShowMsg("未知错误", false);
		}
		private void lkbDelectCheck_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdGift.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked && GiftHelper.DeleteGift(System.Convert.ToInt32(this.grdGift.DataKeys[gridViewRow.RowIndex].Value, System.Globalization.CultureInfo.InvariantCulture)))
				{
					num++;
				}
			}
			if (num > 0)
			{
				this.ShowMsg(string.Format("成功的删除了{0}件礼品", num), true);
				this.BindData();
				return;
			}
			this.ShowMsg("请选择您要删除的礼品", false);
		}
		private void BindData()
		{
			GiftQuery giftQuery = new GiftQuery();
			giftQuery.Name = this.giftName;
			giftQuery.IsPromotion = this.isPromotion;
			giftQuery.Page.PageSize = this.pager.PageSize;
			giftQuery.Page.PageIndex = this.pager.PageIndex;
			giftQuery.Page.SortBy = this.grdGift.SortOrderBy;
			if (this.grdGift.SortOrder.ToLower() == "desc")
			{
				giftQuery.Page.SortOrder = SortAction.Desc;
			}
			DbQueryResult gifts = GiftHelper.GetGifts(giftQuery);
			this.grdGift.DataSource = gifts.Data;
			this.grdGift.DataBind();
			this.pager.TotalRecords=gifts.TotalRecords;
			this.pager1.TotalRecords=gifts.TotalRecords;
		}
	}
}
