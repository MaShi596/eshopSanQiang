using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.sales
{
	public class ManageDebitNote : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected System.Web.UI.WebControls.Label lblStatus;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;
		protected System.Web.UI.WebControls.DataList dlstDebitNote;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			if (!base.IsPostBack)
			{
				this.BindDebitNote();
			}
		}
		private void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要删除的收款单", false);
				return;
			}
			int num;
			OrderHelper.DelDebitNote(text.Split(new char[]
			{
				','
			}), out num);
			this.BindDebitNote();
			this.ShowMsg(string.Format("成功删除了{0}个收款单", num), true);
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadDebitNotes(true);
		}
		private void BindDebitNote()
		{
			DebitNoteQuery debitNoteQuery = this.GetDebitNoteQuery();
			DbQueryResult allDebitNote = OrderHelper.GetAllDebitNote(debitNoteQuery);
			this.dlstDebitNote.DataSource = allDebitNote.Data;
			this.dlstDebitNote.DataBind();
            this.pager.TotalRecords = allDebitNote.TotalRecords;
            this.pager1.TotalRecords = allDebitNote.TotalRecords;
			this.txtOrderId.Text = debitNoteQuery.OrderId;
		}
		private DebitNoteQuery GetDebitNoteQuery()
		{
			DebitNoteQuery debitNoteQuery = new DebitNoteQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				debitNoteQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			debitNoteQuery.PageIndex = this.pager.PageIndex;
			debitNoteQuery.PageSize = this.pager.PageSize;
			debitNoteQuery.SortBy = "PayDate";
			debitNoteQuery.SortOrder = SortAction.Desc;
			return debitNoteQuery;
		}
		private void ReloadDebitNotes(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("OrderId", this.txtOrderId.Text);
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				nameValueCollection.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
