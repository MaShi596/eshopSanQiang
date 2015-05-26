using Hidistro.AccountCenter.Business;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class OrderReviews : MemberTemplatedWebControl
	{
		private string orderId;
		private Common_OrderManage_ReviewsOrderItems orderItems;
		private System.Web.UI.WebControls.Literal litWeight;
		private System.Web.UI.WebControls.Literal litOrderId;
		private FormatedTimeLabel litAddDate;
		private FormatedMoneyLabel lbltotalPrice;
		private OrderStatusLabel lblOrderStatus;
		private System.Web.UI.WebControls.Literal litCloseReason;
		private IButton btnRefer;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-OrderReviews.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
			{
				base.GotoResourceNotFound();
			}
			this.orderId = this.Page.Request.QueryString["orderId"];
			this.orderItems = (Common_OrderManage_ReviewsOrderItems)this.FindControl("Common_OrderManage_ReviewsOrderItems");
			this.litWeight = (System.Web.UI.WebControls.Literal)this.FindControl("litWeight");
			this.litOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderId");
			this.lbltotalPrice = (FormatedMoneyLabel)this.FindControl("lbltotalPrice");
			this.litAddDate = (FormatedTimeLabel)this.FindControl("litAddDate");
			this.lblOrderStatus = (OrderStatusLabel)this.FindControl("lblOrderStatus");
			this.litCloseReason = (System.Web.UI.WebControls.Literal)this.FindControl("litCloseReason");
			this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
			this.btnRefer.Click += new System.EventHandler(this.btnRefer_Click);
			if (!this.Page.IsPostBack && (HiContext.Current.User.UserRole == Hidistro.Membership.Core.Enums.UserRole.Member || HiContext.Current.User.UserRole == Hidistro.Membership.Core.Enums.UserRole.Underling))
			{
				this.btnRefer.Text = "提交评论";
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				this.BindOrderItems(orderInfo);
				this.BindOrderBase(orderInfo);
			}
		}
		private void BindOrderBase(OrderInfo order)
		{
			this.litOrderId.Text = order.OrderId;
			this.lbltotalPrice.Money = order.GetTotal();
			this.litAddDate.Time = order.OrderDate;
			this.lblOrderStatus.OrderStatusCode = order.OrderStatus;
			if (order.OrderStatus == OrderStatus.Closed)
			{
				this.litCloseReason.Text = order.CloseReason;
			}
		}
		private void BindOrderItems(OrderInfo order)
		{
			this.orderItems.DataSource = order.LineItems.Values;
			this.orderItems.DataBind();
			this.litWeight.Text = order.Weight.ToString();
		}
		private bool ValidateConvert()
		{
			string text = string.Empty;
			if (HiContext.Current.User.UserRole != Hidistro.Membership.Core.Enums.UserRole.Member && HiContext.Current.User.UserRole != Hidistro.Membership.Core.Enums.UserRole.Underling)
			{
				text += Formatter.FormatErrorMessage("请填写用户名和密码");
			}
			bool result;
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMessage(text, false);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
		public void btnRefer_Click(object sender, System.EventArgs e)
		{
			if (this.ValidateConvert())
			{
				System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
				foreach (System.Web.UI.WebControls.RepeaterItem repeaterItem in this.orderItems.Items)
				{
					System.Web.UI.HtmlControls.HtmlTextArea htmlTextArea = repeaterItem.FindControl("txtcontent") as System.Web.UI.HtmlControls.HtmlTextArea;
					System.Web.UI.HtmlControls.HtmlInputHidden htmlInputHidden = repeaterItem.FindControl("hdproductId") as System.Web.UI.HtmlControls.HtmlInputHidden;
					if (!string.IsNullOrEmpty(htmlTextArea.Value.Trim()) && !string.IsNullOrEmpty(htmlInputHidden.Value.Trim()))
					{
						dictionary.Add(htmlInputHidden.Value, htmlTextArea.Value);
					}
				}
				if (dictionary.Count <= 0)
				{
					this.ShowMessage("请输入评价内容呀！", false);
				}
				else
				{
					string text = "";
					foreach (System.Collections.Generic.KeyValuePair<string, string> current in dictionary)
					{
						int productId = System.Convert.ToInt32(current.Key.Split(new char[]
						{
							'&'
						})[0].ToString());
						string value = current.Value;
						ProductReviewInfo productReviewInfo = new ProductReviewInfo();
						productReviewInfo.ReviewDate = System.DateTime.Now;
						productReviewInfo.ProductId = productId;
						productReviewInfo.UserId = HiContext.Current.User.UserId;
						productReviewInfo.UserName = HiContext.Current.User.Username;
						productReviewInfo.UserEmail = HiContext.Current.User.Email;
						productReviewInfo.ReviewText = value;
						ValidationResults validationResults = Validation.Validate<ProductReviewInfo>(productReviewInfo, new string[]
						{
							"Refer"
						});
						text = string.Empty;
						if (validationResults.IsValid)
						{
							if (ProductProcessor.ProductExists(productId))
							{
								int num = 0;
								int num2 = 0;
								ProductBrowser.LoadProductReview(productId, out num, out num2);
								if (num == 0)
								{
									text = "您没有购买此商品，因此不能进行评论";
									break;
								}
								if (num2 >= num)
								{
									text = "您已经对此商品进行了评论，请再次购买后方能再进行评论";
									break;
								}
								if (ProductProcessor.InsertProductReview(productReviewInfo))
								{
									continue;
								}
								text = "评论失败，请重试";
								break;
							}
						}
						else
						{
							using (System.Collections.Generic.IEnumerator<ValidationResult> enumerator3 = ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults).GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									ValidationResult current2 = enumerator3.Current;
									text += Formatter.FormatErrorMessage(current2.Message);
								}
								break;
							}
						}
						text = "您要评论的商品已经不存在";
						break;
					}
					if (text != "")
					{
						this.ShowMessage(text, false);
					}
					else
					{
						this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", string.Format("<script>alert(\"{0}\");window.location.href=\"{1}\"</script>", "评论成功", Globals.GetSiteUrls().UrlData.FormatUrl("user_UserProductReviews")));
					}
				}
			}
		}
	}
}
