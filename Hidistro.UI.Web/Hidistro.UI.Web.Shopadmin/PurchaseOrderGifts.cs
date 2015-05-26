using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class PurchaseOrderGifts : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal litPageTitle;
		protected System.Web.UI.WebControls.Literal litPageNote;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected System.Web.UI.WebControls.DataList dlstGifts;
		protected Pager pager;
		protected System.Web.UI.WebControls.Button btnClear;
		protected System.Web.UI.WebControls.DataList dlstOrderGifts;
		protected Pager pagerOrderGifts;
		private string purchaseOrderId;
		private PurchaseOrderInfo purchaseOrder;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (this.Page.Request.QueryString["PurchaseOrderId"] == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			this.purchaseOrderId = this.Page.Request.QueryString["PurchaseOrderId"];
			this.purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.purchaseOrderId);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			this.dlstGifts.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstGifts_ItemCommand);
			this.dlstOrderGifts.DeleteCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstOrderGifts_DeleteCommand);
			if (!base.IsPostBack)
			{
				if (this.purchaseOrder == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				if (this.purchaseOrder.PurchaseStatus != OrderStatus.WaitBuyerPay)
				{
					base.GotoResourceNotFound();
					return;
				}
				if (this.purchaseOrder.PurchaseOrderGifts.Count > 0)
				{
					this.litPageTitle.Text = "编辑采购单礼品";
				}
				this.BindGifts();
				this.BindOrderGifts();
			}
		}
		private void BindGifts()
		{
			DbQueryResult gifts = SubsiteSalesHelper.GetGifts(new GiftQuery
			{
				Page = 
				{
					PageSize = 10,
					PageIndex = this.pager.PageIndex
				},
				Name = this.txtSearchText.Text.Trim()
			});
			this.dlstGifts.DataSource = gifts.Data;
			this.dlstGifts.DataBind();
            this.pager.TotalRecords = gifts.TotalRecords;
		}
		private void BindOrderGifts()
		{
			DbQueryResult purchaseOrderGifts = SubsiteSalesHelper.GetPurchaseOrderGifts(new PurchaseOrderGiftQuery
			{
				PageSize = 10,
				PageIndex = this.pagerOrderGifts.PageIndex,
				PurchaseOrderId = this.purchaseOrderId
			});
			this.dlstOrderGifts.DataSource = purchaseOrderGifts.Data;
			this.dlstOrderGifts.DataBind();
			this.pagerOrderGifts.TotalRecords=purchaseOrderGifts.TotalRecords;
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.BindGifts();
		}
		private void btnClear_Click(object sender, System.EventArgs e)
		{
			if (!this.purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_MODIFY_GIFTS))
			{
				this.ShowMsg("当前采购单状态没有订单礼品操作", false);
				return;
			}
			if (!SubsiteSalesHelper.ClearPurchaseOrderGifts(this.purchaseOrder))
			{
				this.ShowMsg("清空礼品列表失败", false);
				return;
			}
			this.BindOrderGifts();
		}
		private void dlstGifts_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			if (e.CommandName == "check")
			{
				if (!this.purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_MODIFY_GIFTS))
				{
					this.ShowMsg("当前采购单状态没有订单礼品操作", false);
					return;
				}
				int itemIndex = e.Item.ItemIndex;
				int giftId = int.Parse(this.dlstGifts.DataKeys[itemIndex].ToString());
				System.Web.UI.WebControls.TextBox textBox = this.dlstGifts.Items[itemIndex].FindControl("txtQuantity") as System.Web.UI.WebControls.TextBox;
				int num;
				if (!int.TryParse(textBox.Text.Trim(), out num))
				{
					this.ShowMsg("礼品数量填写错误", false);
					return;
				}
				if (num <= 0)
				{
					this.ShowMsg("礼品赠送数量不能为0", false);
					return;
				}
				GiftInfo giftDetails = SubsiteSalesHelper.GetGiftDetails(giftId);
				if (giftDetails == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				if (!SubsiteSalesHelper.AddPurchaseOrderGift(this.purchaseOrder, giftDetails, num))
				{
					this.ShowMsg("添加采购单礼品失败", false);
					return;
				}
				this.BindOrderGifts();
			}
		}
		private void dlstOrderGifts_DeleteCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			if (!this.purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_MODIFY_GIFTS))
			{
				this.ShowMsg("当前采购单状态没有订单礼品操作", false);
				return;
			}
			int itemIndex = e.Item.ItemIndex;
			int giftId = int.Parse(this.dlstOrderGifts.DataKeys[itemIndex].ToString());
			if (!SubsiteSalesHelper.DeletePurchaseOrderGift(this.purchaseOrder, giftId))
			{
				this.ShowMsg("删除采购单礼品失败", false);
			}
			this.BindOrderGifts();
		}
	}
}
