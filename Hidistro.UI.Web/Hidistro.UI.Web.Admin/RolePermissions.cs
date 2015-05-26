using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Membership.Core;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class RolePermissions : AdminPage
	{
		protected System.Web.UI.WebControls.Literal lblRoleName;
		protected System.Web.UI.WebControls.LinkButton btnSetTop;
		protected System.Web.UI.WebControls.CheckBox cbAll;
		protected System.Web.UI.WebControls.CheckBox cbSummary;
		protected System.Web.UI.WebControls.CheckBox cbShop;
		protected System.Web.UI.WebControls.CheckBox cbSiteContent;
		protected System.Web.UI.WebControls.CheckBox cbEmailSettings;
		protected System.Web.UI.WebControls.CheckBox cbSMSSettings;
		protected System.Web.UI.WebControls.CheckBox cbPaymentModes;
		protected System.Web.UI.WebControls.CheckBox cbShippingModes;
		protected System.Web.UI.WebControls.CheckBox cbShippingTemplets;
		protected System.Web.UI.WebControls.CheckBox cbExpressComputerpes;
		protected System.Web.UI.WebControls.CheckBox cbMessageTemplets;
		protected System.Web.UI.WebControls.CheckBox cbPictureMange;
		protected System.Web.UI.WebControls.CheckBox cbPageManger;
		protected System.Web.UI.WebControls.CheckBox cbManageThemes;
		protected System.Web.UI.WebControls.CheckBox cbAfficheList;
		protected System.Web.UI.WebControls.CheckBox cbHelpCategories;
		protected System.Web.UI.WebControls.CheckBox cbHelpList;
		protected System.Web.UI.WebControls.CheckBox cbArticleCategories;
		protected System.Web.UI.WebControls.CheckBox cbArticleList;
		protected System.Web.UI.WebControls.CheckBox cbFriendlyLinks;
		protected System.Web.UI.WebControls.CheckBox cbManageHotKeywords;
		protected System.Web.UI.WebControls.CheckBox cbVotes;
		protected System.Web.UI.WebControls.CheckBox cbProductCatalog;
		protected System.Web.UI.WebControls.CheckBox cbManageProducts;
		protected System.Web.UI.WebControls.CheckBox cbManageProductsView;
		protected System.Web.UI.WebControls.CheckBox cbManageProductsAdd;
		protected System.Web.UI.WebControls.CheckBox cbManageProductsEdit;
		protected System.Web.UI.WebControls.CheckBox cbManageProductsDelete;
		protected System.Web.UI.WebControls.CheckBox cbInStock;
		protected System.Web.UI.WebControls.CheckBox cbManageProductsUp;
		protected System.Web.UI.WebControls.CheckBox cbManageProductsDown;
		protected System.Web.UI.WebControls.CheckBox cbPackProduct;
		protected System.Web.UI.WebControls.CheckBox cbUpPackProduct;
		protected System.Web.UI.WebControls.CheckBox cbProductUnclassified;
		protected System.Web.UI.WebControls.CheckBox cbSubjectProducts;
		protected System.Web.UI.WebControls.CheckBox cbProductBatchUpload;
		protected System.Web.UI.WebControls.CheckBox cbProductBatchExport;
		protected System.Web.UI.WebControls.CheckBox cbProductLines;
		protected System.Web.UI.WebControls.CheckBox cbProductLinesView;
		protected System.Web.UI.WebControls.CheckBox cbAddProductLine;
		protected System.Web.UI.WebControls.CheckBox cbEditProductLine;
		protected System.Web.UI.WebControls.CheckBox cbDeleteProductLine;
		protected System.Web.UI.WebControls.CheckBox cbProductTypes;
		protected System.Web.UI.WebControls.CheckBox cbProductTypesView;
		protected System.Web.UI.WebControls.CheckBox cbProductTypesAdd;
		protected System.Web.UI.WebControls.CheckBox cbProductTypesEdit;
		protected System.Web.UI.WebControls.CheckBox cbProductTypesDelete;
		protected System.Web.UI.WebControls.CheckBox cbManageCategories;
		protected System.Web.UI.WebControls.CheckBox cbManageCategoriesView;
		protected System.Web.UI.WebControls.CheckBox cbManageCategoriesAdd;
		protected System.Web.UI.WebControls.CheckBox cbManageCategoriesEdit;
		protected System.Web.UI.WebControls.CheckBox cbManageCategoriesDelete;
		protected System.Web.UI.WebControls.CheckBox cbBrandCategories;
		protected System.Web.UI.WebControls.CheckBox cbSales;
		protected System.Web.UI.WebControls.CheckBox cbManageOrder;
		protected System.Web.UI.WebControls.CheckBox cbManageOrderView;
		protected System.Web.UI.WebControls.CheckBox cbManageOrderDelete;
		protected System.Web.UI.WebControls.CheckBox cbManageOrderEdit;
		protected System.Web.UI.WebControls.CheckBox cbManageOrderConfirm;
		protected System.Web.UI.WebControls.CheckBox cbManageOrderSendedGoods;
		protected System.Web.UI.WebControls.CheckBox cbExpressPrint;
		protected System.Web.UI.WebControls.CheckBox cbManageOrderRemark;
		protected System.Web.UI.WebControls.CheckBox cbExpressTemplates;
		protected System.Web.UI.WebControls.CheckBox cbShipper;
		protected System.Web.UI.WebControls.CheckBox cbOrderRefundApply;
		protected System.Web.UI.WebControls.CheckBox cbOrderReturnsApply;
		protected System.Web.UI.WebControls.CheckBox cbOrderReplaceApply;
		protected System.Web.UI.WebControls.CheckBox cbPurchaseOrder;
		protected System.Web.UI.WebControls.CheckBox cbManagePurchaseOrder;
		protected System.Web.UI.WebControls.CheckBox cbManagePurchaseOrderView;
		protected System.Web.UI.WebControls.CheckBox cbManagePurchaseOrderEdit;
		protected System.Web.UI.WebControls.CheckBox cbManagePurchaseOrderDelete;
		protected System.Web.UI.WebControls.CheckBox cbPurchaseOrderSendGoods;
		protected System.Web.UI.WebControls.CheckBox cbPurchaseOrderRemark;
		protected System.Web.UI.WebControls.CheckBox cbPurchaseOrderRefundApply;
		protected System.Web.UI.WebControls.CheckBox cbPurchaseOrderReturnsApply;
		protected System.Web.UI.WebControls.CheckBox cbPurchaseOrderReplaceApply;
		protected System.Web.UI.WebControls.CheckBox cbManageUsers;
		protected System.Web.UI.WebControls.CheckBox cbManageMembers;
		protected System.Web.UI.WebControls.CheckBox cbManageMembersView;
		protected System.Web.UI.WebControls.CheckBox cbManageMembersEdit;
		protected System.Web.UI.WebControls.CheckBox cbManageMembersDelete;
		protected System.Web.UI.WebControls.CheckBox cbMemberRanks;
		protected System.Web.UI.WebControls.CheckBox cbMemberRanksView;
		protected System.Web.UI.WebControls.CheckBox cbMemberRanksAdd;
		protected System.Web.UI.WebControls.CheckBox cbMemberRanksEdit;
		protected System.Web.UI.WebControls.CheckBox cbMemberRanksDelete;
		protected System.Web.UI.WebControls.CheckBox cbOpenIdServices;
		protected System.Web.UI.WebControls.CheckBox cbOpenIdSettings;
		protected System.Web.UI.WebControls.CheckBox cbDistribution;
		protected System.Web.UI.WebControls.CheckBox cbDistributorGrades;
		protected System.Web.UI.WebControls.CheckBox cbDistributorGradesView;
		protected System.Web.UI.WebControls.CheckBox cbDistributorGradesAdd;
		protected System.Web.UI.WebControls.CheckBox cbDistributorGradesEdit;
		protected System.Web.UI.WebControls.CheckBox cbDistributorGradesDelete;
		protected System.Web.UI.WebControls.CheckBox cbDistributors;
		protected System.Web.UI.WebControls.CheckBox cbDistributorsView;
		protected System.Web.UI.WebControls.CheckBox cbDistributorsEdit;
		protected System.Web.UI.WebControls.CheckBox cbDistributorsDelete;
		protected System.Web.UI.WebControls.CheckBox cbRequests;
		protected System.Web.UI.WebControls.CheckBox cbDistributorsRequests;
		protected System.Web.UI.WebControls.CheckBox cbDistributorsRequestInstruction;
		protected System.Web.UI.WebControls.CheckBox cbManageDistributorSites;
		protected System.Web.UI.WebControls.CheckBox cbDistributorSiteRequests;
		protected System.Web.UI.WebControls.CheckBox cbMakeProductsPack;
		protected System.Web.UI.WebControls.CheckBox ckTaobaoNote;
		protected System.Web.UI.WebControls.CheckBox cbDistributorSendedMsg;
		protected System.Web.UI.WebControls.CheckBox cbDistributorAcceptMsg;
		protected System.Web.UI.WebControls.CheckBox cbDistributorNewMsg;
		protected System.Web.UI.WebControls.CheckBox cbCRMmanager;
		protected System.Web.UI.WebControls.CheckBox cbMemberMarket;
		protected System.Web.UI.WebControls.CheckBox cbClientGroup;
		protected System.Web.UI.WebControls.CheckBox cbClientNew;
		protected System.Web.UI.WebControls.CheckBox cbClientActivy;
		protected System.Web.UI.WebControls.CheckBox cbClientSleep;
		protected System.Web.UI.WebControls.CheckBox cbProductConsultationsManage;
		protected System.Web.UI.WebControls.CheckBox cbProductReviewsManage;
		protected System.Web.UI.WebControls.CheckBox cbReceivedMessages;
		protected System.Web.UI.WebControls.CheckBox cbSendedMessages;
		protected System.Web.UI.WebControls.CheckBox cbSendMessage;
		protected System.Web.UI.WebControls.CheckBox cbManageLeaveComments;
		protected System.Web.UI.WebControls.CheckBox cbMarketing;
		protected System.Web.UI.WebControls.CheckBox cbGifts;
		protected System.Web.UI.WebControls.CheckBox cbProductPromotion;
		protected System.Web.UI.WebControls.CheckBox cbOrderPromotion;
		protected System.Web.UI.WebControls.CheckBox cbBundPromotion;
		protected System.Web.UI.WebControls.CheckBox cbGroupBuy;
		protected System.Web.UI.WebControls.CheckBox cbCountDown;
		protected System.Web.UI.WebControls.CheckBox cbCoupons;
		protected System.Web.UI.WebControls.CheckBox cbFinancial;
		protected System.Web.UI.WebControls.CheckBox cbAccountSummary;
		protected System.Web.UI.WebControls.CheckBox cbReCharge;
		protected System.Web.UI.WebControls.CheckBox cbBalanceDrawRequest;
		protected System.Web.UI.WebControls.CheckBox cbDistributorAccount;
		protected System.Web.UI.WebControls.CheckBox cbDistributorReCharge;
		protected System.Web.UI.WebControls.CheckBox cbDistributorBalanceDrawRequest;
		protected System.Web.UI.WebControls.CheckBox cbBalanceDetailsStatistics;
		protected System.Web.UI.WebControls.CheckBox cbBalanceDrawRequestStatistics;
		protected System.Web.UI.WebControls.CheckBox cbTotalReport;
		protected System.Web.UI.WebControls.CheckBox cbSaleTotalStatistics;
		protected System.Web.UI.WebControls.CheckBox cbUserOrderStatistics;
		protected System.Web.UI.WebControls.CheckBox cbSaleList;
		protected System.Web.UI.WebControls.CheckBox cbSaleTargetAnalyse;
		protected System.Web.UI.WebControls.CheckBox cbProductSaleRanking;
		protected System.Web.UI.WebControls.CheckBox cbProductSaleStatistics;
		protected System.Web.UI.WebControls.CheckBox cbMemberRanking;
		protected System.Web.UI.WebControls.CheckBox cbMemberArealDistributionStatistics;
		protected System.Web.UI.WebControls.CheckBox cbUserIncreaseStatistics;
		protected System.Web.UI.WebControls.CheckBox cbDistributionReport;
		protected System.Web.UI.WebControls.CheckBox cbPurchaseOrderStatistics;
		protected System.Web.UI.WebControls.CheckBox cbDistributionProductSaleRanking;
		protected System.Web.UI.WebControls.CheckBox cbDistributorAchievementsRanking;
		protected System.Web.UI.WebControls.Button btnSet1;
		private string RequestRoleId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["roleId"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.RequestRoleId = this.Page.Request.QueryString["roleId"];
			this.btnSet1.Click += new System.EventHandler(this.btnSet_Click);
			this.btnSetTop.Click += new System.EventHandler(this.btnSet_Click);
			if (!this.Page.IsPostBack)
			{
				System.Guid guid = new System.Guid(this.RequestRoleId);
				if (System.Text.RegularExpressions.Regex.IsMatch(this.RequestRoleId, "[A-Z0-9]{8}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{12}", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
				{
					Hidistro.Membership.Core.RoleInfo role = Hidistro.Membership.Core.RoleHelper.GetRole(guid);
					this.lblRoleName.Text = role.Name;
				}
				if (this.Page.Request.QueryString["Status"] == "1")
				{
					this.ShowMsg("设置部门权限成功", true);
				}
				this.LoadData(guid);
			}
		}
		private void btnSet_Click(object sender, System.EventArgs e)
		{
			System.Guid guid = new System.Guid(this.RequestRoleId);
			this.PermissionsSet(guid);
			this.Page.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/store/RolePermissions.aspx?roleId={0}&Status=1", guid)));
		}
		private void LoadData(System.Guid roleId)
		{
			System.Collections.Generic.IList<int> privilegeByRoles = Hidistro.Membership.Core.RoleHelper.GetPrivilegeByRoles(roleId);
			this.cbSummary.Checked = privilegeByRoles.Contains(1000);
			this.cbSiteContent.Checked = privilegeByRoles.Contains(1001);
			this.cbVotes.Checked = privilegeByRoles.Contains(2009);
			this.cbFriendlyLinks.Checked = privilegeByRoles.Contains(2007);
			this.cbManageThemes.Checked = privilegeByRoles.Contains(2001);
			this.cbManageHotKeywords.Checked = privilegeByRoles.Contains(2008);
			this.cbAfficheList.Checked = privilegeByRoles.Contains(2002);
			this.cbHelpCategories.Checked = privilegeByRoles.Contains(2003);
			this.cbHelpList.Checked = privilegeByRoles.Contains(2004);
			this.cbArticleCategories.Checked = privilegeByRoles.Contains(2005);
			this.cbArticleList.Checked = privilegeByRoles.Contains(2006);
			this.cbEmailSettings.Checked = privilegeByRoles.Contains(1002);
			this.cbSMSSettings.Checked = privilegeByRoles.Contains(1003);
			this.cbMessageTemplets.Checked = privilegeByRoles.Contains(1008);
			this.cbShippingTemplets.Checked = privilegeByRoles.Contains(1006);
			this.cbExpressComputerpes.Checked = privilegeByRoles.Contains(1007);
			this.cbPictureMange.Checked = privilegeByRoles.Contains(1009);
			this.cbDistributorGradesView.Checked = privilegeByRoles.Contains(6001);
			this.cbDistributorGradesAdd.Checked = privilegeByRoles.Contains(6002);
			this.cbDistributorGradesEdit.Checked = privilegeByRoles.Contains(6003);
			this.cbDistributorGradesDelete.Checked = privilegeByRoles.Contains(6004);
			this.cbDistributorsView.Checked = privilegeByRoles.Contains(6005);
			this.cbDistributorsEdit.Checked = privilegeByRoles.Contains(6006);
			this.cbDistributorsDelete.Checked = privilegeByRoles.Contains(6007);
			this.cbDistributorsRequests.Checked = privilegeByRoles.Contains(6008);
			this.cbDistributorsRequestInstruction.Checked = privilegeByRoles.Contains(6009);
			this.cbManagePurchaseOrderView.Checked = privilegeByRoles.Contains(11001);
			this.cbManagePurchaseOrderEdit.Checked = privilegeByRoles.Contains(11002);
			this.cbManagePurchaseOrderDelete.Checked = privilegeByRoles.Contains(11003);
			this.cbPurchaseOrderSendGoods.Checked = privilegeByRoles.Contains(11004);
			this.cbPurchaseOrderRemark.Checked = privilegeByRoles.Contains(11006);
			this.cbPurchaseOrderRefundApply.Checked = privilegeByRoles.Contains(11007);
			this.cbPurchaseOrderReturnsApply.Checked = privilegeByRoles.Contains(11009);
			this.cbPurchaseOrderReplaceApply.Checked = privilegeByRoles.Contains(11008);
			this.cbDistributorAccount.Checked = privilegeByRoles.Contains(9004);
			this.cbDistributorReCharge.Checked = privilegeByRoles.Contains(9005);
			this.cbDistributorBalanceDrawRequest.Checked = privilegeByRoles.Contains(9006);
			this.cbDistributionReport.Checked = privilegeByRoles.Contains(10010);
			this.cbPurchaseOrderStatistics.Checked = privilegeByRoles.Contains(10011);
			this.cbDistributionProductSaleRanking.Checked = privilegeByRoles.Contains(100112);
			this.cbDistributorAchievementsRanking.Checked = privilegeByRoles.Contains(100113);
			this.cbManageDistributorSites.Checked = privilegeByRoles.Contains(6010);
			this.cbDistributorSiteRequests.Checked = privilegeByRoles.Contains(6011);
			this.cbProductLinesView.Checked = privilegeByRoles.Contains(3013);
			this.cbAddProductLine.Checked = privilegeByRoles.Contains(3014);
			this.cbEditProductLine.Checked = privilegeByRoles.Contains(3015);
			this.cbDeleteProductLine.Checked = privilegeByRoles.Contains(3016);
			this.cbProductTypesView.Checked = privilegeByRoles.Contains(3017);
			this.cbProductTypesAdd.Checked = privilegeByRoles.Contains(3018);
			this.cbProductTypesEdit.Checked = privilegeByRoles.Contains(3019);
			this.cbProductTypesDelete.Checked = privilegeByRoles.Contains(3020);
			this.cbManageCategoriesView.Checked = privilegeByRoles.Contains(3021);
			this.cbManageCategoriesAdd.Checked = privilegeByRoles.Contains(3022);
			this.cbManageCategoriesEdit.Checked = privilegeByRoles.Contains(3023);
			this.cbManageCategoriesDelete.Checked = privilegeByRoles.Contains(3024);
			this.cbBrandCategories.Checked = privilegeByRoles.Contains(3025);
			this.cbManageProductsView.Checked = privilegeByRoles.Contains(3001);
			this.cbManageProductsAdd.Checked = privilegeByRoles.Contains(3002);
			this.cbManageProductsEdit.Checked = privilegeByRoles.Contains(3003);
			this.cbManageProductsDelete.Checked = privilegeByRoles.Contains(3004);
			this.cbInStock.Checked = privilegeByRoles.Contains(3005);
			this.cbManageProductsUp.Checked = privilegeByRoles.Contains(3006);
			this.cbManageProductsDown.Checked = privilegeByRoles.Contains(3007);
			this.cbPackProduct.Checked = privilegeByRoles.Contains(3008);
			this.cbUpPackProduct.Checked = privilegeByRoles.Contains(3009);
			this.cbProductUnclassified.Checked = privilegeByRoles.Contains(3010);
			this.cbProductBatchUpload.Checked = privilegeByRoles.Contains(3012);
			this.cbProductBatchExport.Checked = privilegeByRoles.Contains(3026);
			this.cbMakeProductsPack.Checked = privilegeByRoles.Contains(6012);
			this.ckTaobaoNote.Checked = privilegeByRoles.Contains(6013);
			this.cbDistributorSendedMsg.Checked = privilegeByRoles.Contains(6014);
			this.cbDistributorAcceptMsg.Checked = privilegeByRoles.Contains(6015);
			this.cbDistributorNewMsg.Checked = privilegeByRoles.Contains(6016);
			this.cbSubjectProducts.Checked = privilegeByRoles.Contains(3011);
			this.cbClientGroup.Checked = privilegeByRoles.Contains(7007);
			this.cbClientActivy.Checked = privilegeByRoles.Contains(7009);
			this.cbClientNew.Checked = privilegeByRoles.Contains(7008);
			this.cbClientSleep.Checked = privilegeByRoles.Contains(7010);
			this.cbMemberRanksView.Checked = privilegeByRoles.Contains(5004);
			this.cbMemberRanksAdd.Checked = privilegeByRoles.Contains(5005);
			this.cbMemberRanksEdit.Checked = privilegeByRoles.Contains(5006);
			this.cbMemberRanksDelete.Checked = privilegeByRoles.Contains(5007);
			this.cbManageMembersView.Checked = privilegeByRoles.Contains(5001);
			this.cbManageMembersEdit.Checked = privilegeByRoles.Contains(5002);
			this.cbManageMembersDelete.Checked = privilegeByRoles.Contains(5003);
			this.cbBalanceDrawRequest.Checked = privilegeByRoles.Contains(9003);
			this.cbAccountSummary.Checked = privilegeByRoles.Contains(9001);
			this.cbReCharge.Checked = privilegeByRoles.Contains(9002);
			this.cbBalanceDetailsStatistics.Checked = privilegeByRoles.Contains(5010);
			this.cbBalanceDrawRequestStatistics.Checked = privilegeByRoles.Contains(5011);
			this.cbMemberArealDistributionStatistics.Checked = privilegeByRoles.Contains(10008);
			this.cbUserIncreaseStatistics.Checked = privilegeByRoles.Contains(10009);
			this.cbMemberRanking.Checked = privilegeByRoles.Contains(10007);
			this.cbOpenIdServices.Checked = privilegeByRoles.Contains(5008);
			this.cbOpenIdSettings.Checked = privilegeByRoles.Contains(5009);
			this.cbManageOrderView.Checked = privilegeByRoles.Contains(4001);
			this.cbManageOrderDelete.Checked = privilegeByRoles.Contains(4002);
			this.cbManageOrderEdit.Checked = privilegeByRoles.Contains(4003);
			this.cbManageOrderConfirm.Checked = privilegeByRoles.Contains(4004);
			this.cbManageOrderSendedGoods.Checked = privilegeByRoles.Contains(4005);
			this.cbExpressPrint.Checked = privilegeByRoles.Contains(4006);
			this.cbManageOrderRemark.Checked = privilegeByRoles.Contains(4008);
			this.cbExpressTemplates.Checked = privilegeByRoles.Contains(4009);
			this.cbShipper.Checked = privilegeByRoles.Contains(4010);
			this.cbPaymentModes.Checked = privilegeByRoles.Contains(1004);
			this.cbShippingModes.Checked = privilegeByRoles.Contains(1005);
			this.cbOrderRefundApply.Checked = privilegeByRoles.Contains(4012);
			this.cbOrderReturnsApply.Checked = privilegeByRoles.Contains(4014);
			this.cbOrderReplaceApply.Checked = privilegeByRoles.Contains(4013);
			this.cbSaleTotalStatistics.Checked = privilegeByRoles.Contains(10001);
			this.cbUserOrderStatistics.Checked = privilegeByRoles.Contains(10002);
			this.cbSaleList.Checked = privilegeByRoles.Contains(10003);
			this.cbSaleTargetAnalyse.Checked = privilegeByRoles.Contains(10004);
			this.cbProductSaleRanking.Checked = privilegeByRoles.Contains(10005);
			this.cbProductSaleStatistics.Checked = privilegeByRoles.Contains(10006);
			this.cbGifts.Checked = privilegeByRoles.Contains(8001);
			this.cbGroupBuy.Checked = privilegeByRoles.Contains(8005);
			this.cbCountDown.Checked = privilegeByRoles.Contains(8006);
			this.cbCoupons.Checked = privilegeByRoles.Contains(8007);
			this.cbProductPromotion.Checked = privilegeByRoles.Contains(8002);
			this.cbOrderPromotion.Checked = privilegeByRoles.Contains(8003);
			this.cbBundPromotion.Checked = privilegeByRoles.Contains(8004);
			this.cbProductConsultationsManage.Checked = privilegeByRoles.Contains(7001);
			this.cbProductReviewsManage.Checked = privilegeByRoles.Contains(7002);
			this.cbReceivedMessages.Checked = privilegeByRoles.Contains(7003);
			this.cbSendedMessages.Checked = privilegeByRoles.Contains(7004);
			this.cbSendMessage.Checked = privilegeByRoles.Contains(7005);
			this.cbManageLeaveComments.Checked = privilegeByRoles.Contains(7006);
		}
		private void PermissionsSet(System.Guid roleId)
		{
			string text = string.Empty;
			if (this.cbSummary.Checked)
			{
				text = text + 1000 + ",";
			}
			if (this.cbSiteContent.Checked)
			{
				text = text + 1001 + ",";
			}
			if (this.cbVotes.Checked)
			{
				text = text + 2009 + ",";
			}
			if (this.cbFriendlyLinks.Checked)
			{
				text = text + 2007 + ",";
			}
			if (this.cbManageThemes.Checked)
			{
				text = text + 2001 + ",";
			}
			if (this.cbManageHotKeywords.Checked)
			{
				text = text + 2008 + ",";
			}
			if (this.cbAfficheList.Checked)
			{
				text = text + 2002 + ",";
			}
			if (this.cbHelpCategories.Checked)
			{
				text = text + 2003 + ",";
			}
			if (this.cbHelpList.Checked)
			{
				text = text + 2004 + ",";
			}
			if (this.cbArticleCategories.Checked)
			{
				text = text + 2005 + ",";
			}
			if (this.cbArticleList.Checked)
			{
				text = text + 2006 + ",";
			}
			if (this.cbEmailSettings.Checked)
			{
				text = text + 1002 + ",";
			}
			if (this.cbSMSSettings.Checked)
			{
				text = text + 1003 + ",";
			}
			if (this.cbMessageTemplets.Checked)
			{
				text = text + 1008 + ",";
			}
			if (this.cbShippingTemplets.Checked)
			{
				text = text + 1006 + ",";
			}
			if (this.cbExpressComputerpes.Checked)
			{
				text = text + 1007 + ",";
			}
			if (this.cbPictureMange.Checked)
			{
				text = text + 1009 + ",";
			}
			if (this.cbDistributorGradesView.Checked)
			{
				text = text + 6001 + ",";
			}
			if (this.cbDistributorGradesAdd.Checked)
			{
				text = text + 6002 + ",";
			}
			if (this.cbDistributorGradesEdit.Checked)
			{
				text = text + 6003 + ",";
			}
			if (this.cbDistributorGradesDelete.Checked)
			{
				text = text + 6004 + ",";
			}
			if (this.cbDistributorsView.Checked)
			{
				text = text + 6005 + ",";
			}
			if (this.cbDistributorsEdit.Checked)
			{
				text = text + 6006 + ",";
			}
			if (this.cbDistributorsDelete.Checked)
			{
				text = text + 6007 + ",";
			}
			if (this.cbDistributorsRequests.Checked)
			{
				text = text + 6008 + ",";
			}
			if (this.cbDistributorsRequestInstruction.Checked)
			{
				text = text + 6009 + ",";
			}
			if (this.cbDistributorAccount.Checked)
			{
				text = text + 9004 + ",";
			}
			if (this.cbDistributorReCharge.Checked)
			{
				text = text + 9005 + ",";
			}
			if (this.cbDistributorBalanceDrawRequest.Checked)
			{
				text = text + 9006 + ",";
			}
			if (this.cbDistributionReport.Checked)
			{
				text = text + 10010 + ",";
			}
			if (this.cbPurchaseOrderStatistics.Checked)
			{
				text = text + 10011 + ",";
			}
			if (this.cbDistributionProductSaleRanking.Checked)
			{
				text = text + 100112 + ",";
			}
			if (this.cbDistributorAchievementsRanking.Checked)
			{
				text = text + 100113 + ",";
			}
			if (this.cbManageDistributorSites.Checked)
			{
				text = text + 6010 + ",";
			}
			if (this.cbDistributorSiteRequests.Checked)
			{
				text = text + 6011 + ",";
			}
			if (this.cbProductLinesView.Checked)
			{
				text = text + 3013 + ",";
			}
			if (this.cbAddProductLine.Checked)
			{
				text = text + 3014 + ",";
			}
			if (this.cbEditProductLine.Checked)
			{
				text = text + 3015 + ",";
			}
			if (this.cbDeleteProductLine.Checked)
			{
				text = text + 3016 + ",";
			}
			if (this.cbProductTypesView.Checked)
			{
				text = text + 3017 + ",";
			}
			if (this.cbProductTypesAdd.Checked)
			{
				text = text + 3018 + ",";
			}
			if (this.cbProductTypesEdit.Checked)
			{
				text = text + 3019 + ",";
			}
			if (this.cbProductTypesDelete.Checked)
			{
				text = text + 3020 + ",";
			}
			if (this.cbManageCategoriesView.Checked)
			{
				text = text + 3021 + ",";
			}
			if (this.cbManageCategoriesAdd.Checked)
			{
				text = text + 3022 + ",";
			}
			if (this.cbManageCategoriesEdit.Checked)
			{
				text = text + 3023 + ",";
			}
			if (this.cbManageCategoriesDelete.Checked)
			{
				text = text + 3024 + ",";
			}
			if (this.cbBrandCategories.Checked)
			{
				text = text + 3025 + ",";
			}
			if (this.cbManageProductsView.Checked)
			{
				text = text + 3001 + ",";
			}
			if (this.cbManageProductsAdd.Checked)
			{
				text = text + 3002 + ",";
			}
			if (this.cbManageProductsEdit.Checked)
			{
				text = text + 3003 + ",";
			}
			if (this.cbManageProductsDelete.Checked)
			{
				text = text + 3004 + ",";
			}
			if (this.cbInStock.Checked)
			{
				text = text + 3005 + ",";
			}
			if (this.cbManageProductsUp.Checked)
			{
				text = text + 3006 + ",";
			}
			if (this.cbManageProductsDown.Checked)
			{
				text = text + 3007 + ",";
			}
			if (this.cbPackProduct.Checked)
			{
				text = text + 3008 + ",";
			}
			if (this.cbUpPackProduct.Checked)
			{
				text = text + 3009 + ",";
			}
			if (this.cbProductUnclassified.Checked)
			{
				text = text + 3010 + ",";
			}
			if (this.cbProductBatchUpload.Checked)
			{
				text = text + 3012 + ",";
			}
			if (this.cbProductBatchExport.Checked)
			{
				text = text + 3026 + ",";
			}
			if (this.cbSubjectProducts.Checked)
			{
				text = text + 3011 + ",";
			}
			if (this.cbMakeProductsPack.Checked)
			{
				text = text + 6012 + ",";
			}
			if (this.ckTaobaoNote.Checked)
			{
				text = text + 6013 + ",";
			}
			if (this.cbDistributorSendedMsg.Checked)
			{
				text = text + 6014 + ",";
			}
			if (this.cbDistributorAcceptMsg.Checked)
			{
				text = text + 6015 + ",";
			}
			if (this.cbDistributorNewMsg.Checked)
			{
				text = text + 6016 + ",";
			}
			if (this.cbClientGroup.Checked)
			{
				text = text + 7007 + ",";
			}
			if (this.cbClientNew.Checked)
			{
				text = text + 7008 + ",";
			}
			if (this.cbClientSleep.Checked)
			{
				text = text + 7010 + ",";
			}
			if (this.cbClientActivy.Checked)
			{
				text = text + 7009 + ",";
			}
			if (this.cbMemberRanksView.Checked)
			{
				text = text + 5004 + ",";
			}
			if (this.cbMemberRanksAdd.Checked)
			{
				text = text + 5005 + ",";
			}
			if (this.cbMemberRanksEdit.Checked)
			{
				text = text + 5006 + ",";
			}
			if (this.cbMemberRanksDelete.Checked)
			{
				text = text + 5007 + ",";
			}
			if (this.cbManageMembersView.Checked)
			{
				text = text + 5001 + ",";
			}
			if (this.cbManageMembersEdit.Checked)
			{
				text = text + 5002 + ",";
			}
			if (this.cbManageMembersDelete.Checked)
			{
				text = text + 5003 + ",";
			}
			if (this.cbBalanceDrawRequest.Checked)
			{
				text = text + 9003 + ",";
			}
			if (this.cbAccountSummary.Checked)
			{
				text = text + 9001 + ",";
			}
			if (this.cbReCharge.Checked)
			{
				text = text + 9002 + ",";
			}
			if (this.cbBalanceDetailsStatistics.Checked)
			{
				text = text + 5010 + ",";
			}
			if (this.cbBalanceDrawRequestStatistics.Checked)
			{
				text = text + 5011 + ",";
			}
			if (this.cbMemberArealDistributionStatistics.Checked)
			{
				text = text + 10008 + ",";
			}
			if (this.cbUserIncreaseStatistics.Checked)
			{
				text = text + 10009 + ",";
			}
			if (this.cbMemberRanking.Checked)
			{
				text = text + 10007 + ",";
			}
			if (this.cbOpenIdServices.Checked)
			{
				text = text + 5008 + ",";
			}
			if (this.cbOpenIdSettings.Checked)
			{
				text = text + 5009 + ",";
			}
			if (this.cbManageOrderView.Checked)
			{
				text = text + 4001 + ",";
			}
			if (this.cbManageOrderDelete.Checked)
			{
				text = text + 4002 + ",";
			}
			if (this.cbManageOrderEdit.Checked)
			{
				text = text + 4003 + ",";
			}
			if (this.cbManageOrderConfirm.Checked)
			{
				text = text + 4004 + ",";
			}
			if (this.cbManageOrderSendedGoods.Checked)
			{
				text = text + 4005 + ",";
			}
			if (this.cbExpressPrint.Checked)
			{
				text = text + 4006 + ",";
			}
			if (this.cbExpressTemplates.Checked)
			{
				text = text + 4009 + ",";
			}
			if (this.cbShipper.Checked)
			{
				text = text + 4010 + ",";
			}
			if (this.cbPaymentModes.Checked)
			{
				text = text + 1004 + ",";
			}
			if (this.cbShippingModes.Checked)
			{
				text = text + 1005 + ",";
			}
			if (this.cbSaleTotalStatistics.Checked)
			{
				text = text + 10001 + ",";
			}
			if (this.cbUserOrderStatistics.Checked)
			{
				text = text + 10002 + ",";
			}
			if (this.cbSaleList.Checked)
			{
				text = text + 10003 + ",";
			}
			if (this.cbSaleTargetAnalyse.Checked)
			{
				text = text + 10004 + ",";
			}
			if (this.cbProductSaleRanking.Checked)
			{
				text = text + 10005 + ",";
			}
			if (this.cbProductSaleStatistics.Checked)
			{
				text = text + 10006 + ",";
			}
			if (this.cbOrderRefundApply.Checked)
			{
				text = text + 4012 + ",";
			}
			if (this.cbOrderReplaceApply.Checked)
			{
				text = text + 4013 + ",";
			}
			if (this.cbOrderReturnsApply.Checked)
			{
				text = text + 4014 + ",";
			}
			if (this.cbManagePurchaseOrderView.Checked)
			{
				text = text + 11001 + ",";
			}
			if (this.cbManagePurchaseOrderEdit.Checked)
			{
				text = text + 11002 + ",";
			}
			if (this.cbManagePurchaseOrderDelete.Checked)
			{
				text = text + 11003 + ",";
			}
			if (this.cbPurchaseOrderSendGoods.Checked)
			{
				text = text + 11004 + ",";
			}
			if (this.cbPurchaseOrderRemark.Checked)
			{
				text = text + 11006 + ",";
			}
			if (this.cbPurchaseOrderRefundApply.Checked)
			{
				text = text + 11007 + ",";
			}
			if (this.cbPurchaseOrderReturnsApply.Checked)
			{
				text = text + 11009 + ",";
			}
			if (this.cbPurchaseOrderReplaceApply.Checked)
			{
				text = text + 11008 + ",";
			}
			if (this.cbGifts.Checked)
			{
				text = text + 8001 + ",";
			}
			if (this.cbGroupBuy.Checked)
			{
				text = text + 8005 + ",";
			}
			if (this.cbCountDown.Checked)
			{
				text = text + 8006 + ",";
			}
			if (this.cbCoupons.Checked)
			{
				text = text + 8007 + ",";
			}
			if (this.cbProductPromotion.Checked)
			{
				text = text + 8002 + ",";
			}
			if (this.cbOrderPromotion.Checked)
			{
				text = text + 8003 + ",";
			}
			if (this.cbBundPromotion.Checked)
			{
				text = text + 8004 + ",";
			}
			if (this.cbProductConsultationsManage.Checked)
			{
				text = text + 7001 + ",";
			}
			if (this.cbProductReviewsManage.Checked)
			{
				text = text + 7002 + ",";
			}
			if (this.cbReceivedMessages.Checked)
			{
				text = text + 7003 + ",";
			}
			if (this.cbSendedMessages.Checked)
			{
				text = text + 7004 + ",";
			}
			if (this.cbSendMessage.Checked)
			{
				text = text + 7005 + ",";
			}
			if (this.cbManageLeaveComments.Checked)
			{
				text = text + 7006 + ",";
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Substring(0, text.LastIndexOf(","));
			}
			Hidistro.Membership.Core.RoleHelper.AddPrivilegeInRoles(roleId, text);
			ManagerHelper.ClearRolePrivilege(roleId);
		}
	}
}
