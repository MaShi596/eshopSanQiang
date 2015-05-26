using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class EditMyGroupBuy : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected DistributorProductCategoriesDropDownList dropCategories;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected DistributorGroupBuyProductDropDownList dropGroupBuyProduct;
		protected System.Web.UI.WebControls.Label lblPrice;
		protected WebCalendar calendarStartDate;
		protected HourDropDownList drophours;
		protected WebCalendar calendarEndDate;
		protected HourDropDownList HourDropDownList1;
		protected System.Web.UI.WebControls.TextBox txtNeedPrice;
		protected System.Web.UI.WebControls.TextBox txtMaxCount;
		protected System.Web.UI.WebControls.TextBox txtCount;
		protected System.Web.UI.WebControls.TextBox txtPrice;
		protected System.Web.UI.WebControls.TextBox txtContent;
		protected System.Web.UI.WebControls.Button btnUpdateGroupBuy;
		protected System.Web.UI.WebControls.Button btnFinish;
		protected System.Web.UI.WebControls.Button btnSuccess;
		protected System.Web.UI.WebControls.Button btnFail;
		private int groupBuyId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				int productId;
				if (int.TryParse(base.Request["productId"], out productId))
				{
					string priceByProductId = SubsitePromoteHelper.GetPriceByProductId(productId);
					if (priceByProductId.Length > 0)
					{
						base.Response.Clear();
						base.Response.ContentType = "application/json";
						base.Response.Write("{ ");
						base.Response.Write("\"Status\":\"OK\",");
						base.Response.Write(string.Format("\"Price\":\"{0}\"", decimal.Parse(priceByProductId).ToString("F2")));
						base.Response.Write("}");
						base.Response.End();
						return;
					}
				}
			}
			else
			{
				if (!int.TryParse(base.Request.QueryString["groupBuyId"], out this.groupBuyId))
				{
					base.GotoResourceNotFound();
					return;
				}
				this.btnUpdateGroupBuy.Click += new System.EventHandler(this.btnUpdateGroupBuy_Click);
				this.btnFail.Click += new System.EventHandler(this.btnFail_Click);
				this.btnSuccess.Click += new System.EventHandler(this.btnSuccess_Click);
				this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
				if (!base.IsPostBack)
				{
					this.dropGroupBuyProduct.DataBind();
					this.dropCategories.DataBind();
					this.HourDropDownList1.DataBind();
					this.drophours.DataBind();
					GroupBuyInfo groupBuy = SubsitePromoteHelper.GetGroupBuy(this.groupBuyId);
					if (SubsitePromoteHelper.GetOrderCount(this.groupBuyId) > 0)
					{
						this.dropGroupBuyProduct.Enabled = false;
					}
					if (groupBuy == null)
					{
						base.GotoResourceNotFound();
						return;
					}
					if (groupBuy.Status == GroupBuyStatus.EndUntreated)
					{
						this.btnFail.Visible = true;
						this.btnSuccess.Visible = true;
					}
					if (groupBuy.Status == GroupBuyStatus.UnderWay)
					{
						this.btnFinish.Visible = true;
					}
					this.LoadGroupBuy(groupBuy);
				}
			}
		}
		private void btnFinish_Click(object sender, System.EventArgs e)
		{
			if (SubsitePromoteHelper.SetGroupBuyEndUntreated(this.groupBuyId))
			{
				this.btnFail.Visible = true;
				this.btnSuccess.Visible = true;
				this.btnFinish.Visible = false;
				this.ShowMsg("成功设置团购活动为结束状态", true);
				return;
			}
			this.ShowMsg("设置团购活动状态失败", true);
		}
		private void btnFail_Click(object sender, System.EventArgs e)
		{
			if (SubsitePromoteHelper.SetGroupBuyStatus(this.groupBuyId, GroupBuyStatus.Failed))
			{
				this.btnFail.Visible = false;
				this.btnSuccess.Visible = false;
				this.ShowMsg("成功设置团购活动为失败状态", true);
				return;
			}
			this.ShowMsg("设置团购活动状态失败", true);
		}
		private void btnSuccess_Click(object sender, System.EventArgs e)
		{
			if (SubsitePromoteHelper.SetGroupBuyStatus(this.groupBuyId, GroupBuyStatus.Success))
			{
				this.btnFail.Visible = false;
				this.btnSuccess.Visible = false;
				this.ShowMsg("成功设置团购活动为成功状态", true);
				return;
			}
			this.ShowMsg("设置团购活动状态失败", true);
		}
		private void LoadGroupBuy(GroupBuyInfo groupBuy)
		{
			this.txtCount.Text = groupBuy.GroupBuyConditions[0].Count.ToString();
			this.txtPrice.Text = groupBuy.GroupBuyConditions[0].Price.ToString("F");
			this.txtContent.Text = Globals.HtmlDecode(groupBuy.Content);
			this.txtMaxCount.Text = groupBuy.MaxCount.ToString();
			this.txtNeedPrice.Text = groupBuy.NeedPrice.ToString("F");
            this.calendarEndDate.SelectedDate = new System.DateTime?(groupBuy.EndDate.Date);
			this.HourDropDownList1.SelectedValue = new int?(groupBuy.EndDate.Hour);
            this.calendarStartDate.SelectedDate = new System.DateTime?(groupBuy.StartDate);
			this.drophours.SelectedValue = new int?(groupBuy.StartDate.Hour);
			this.dropGroupBuyProduct.SelectedValue = new int?(groupBuy.ProductId);
		}
		private void btnUpdateGroupBuy_Click(object sender, System.EventArgs e)
		{
			GroupBuyInfo groupBuyInfo = new GroupBuyInfo();
			groupBuyInfo.GroupBuyId = this.groupBuyId;
			string text = string.Empty;
			if (this.dropGroupBuyProduct.SelectedValue > 0)
			{
				GroupBuyInfo groupBuy = SubsitePromoteHelper.GetGroupBuy(this.groupBuyId);
				if (groupBuy.ProductId != this.dropGroupBuyProduct.SelectedValue.Value && SubsitePromoteHelper.ProductGroupBuyExist(this.dropGroupBuyProduct.SelectedValue.Value))
				{
					this.ShowMsg("已经存在此商品的团购活动，并且活动正在进行中", false);
					return;
				}
				groupBuyInfo.ProductId = this.dropGroupBuyProduct.SelectedValue.Value;
			}
			else
			{
				text += Formatter.FormatErrorMessage("请选择团购商品");
			}
			if (!this.calendarEndDate.SelectedDate.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择结束日期");
			}
			else
			{
				groupBuyInfo.EndDate = this.calendarEndDate.SelectedDate.Value.AddHours((double)this.HourDropDownList1.SelectedValue.Value);
				if (System.DateTime.Compare(groupBuyInfo.EndDate, System.DateTime.Now) <= 0 && groupBuyInfo.Status == GroupBuyStatus.UnderWay)
				{
					text += Formatter.FormatErrorMessage("结束日期必须要晚于今天日期");
				}
				else
				{
					if (System.DateTime.Compare(this.calendarStartDate.SelectedDate.Value.AddHours((double)this.drophours.SelectedValue.Value), groupBuyInfo.EndDate) >= 0)
					{
						text += Formatter.FormatErrorMessage("开始日期必须要早于结束日期");
					}
					else
					{
						groupBuyInfo.StartDate = this.calendarStartDate.SelectedDate.Value.AddHours((double)this.drophours.SelectedValue.Value);
					}
				}
			}
			if (!string.IsNullOrEmpty(this.txtNeedPrice.Text))
			{
				decimal needPrice;
				if (decimal.TryParse(this.txtNeedPrice.Text.Trim(), out needPrice))
				{
					groupBuyInfo.NeedPrice = needPrice;
				}
				else
				{
					text += Formatter.FormatErrorMessage("违约金填写格式不正确");
				}
			}
			int maxCount;
			if (int.TryParse(this.txtMaxCount.Text.Trim(), out maxCount))
			{
				groupBuyInfo.MaxCount = maxCount;
			}
			else
			{
				text += Formatter.FormatErrorMessage("限购数量不能为空，只能为整数");
			}
			groupBuyInfo.Content = Globals.HtmlEncode(this.txtContent.Text);
			GropBuyConditionInfo gropBuyConditionInfo = new GropBuyConditionInfo();
			int count;
			if (int.TryParse(this.txtCount.Text.Trim(), out count))
			{
				gropBuyConditionInfo.Count = count;
			}
			else
			{
				text += Formatter.FormatErrorMessage("团购满足数量不能为空，只能为整数");
			}
			decimal price;
			if (decimal.TryParse(this.txtPrice.Text.Trim(), out price))
			{
				gropBuyConditionInfo.Price = price;
			}
			else
			{
				text += Formatter.FormatErrorMessage("团购价格不能为空，只能为数值类型");
			}
			groupBuyInfo.GroupBuyConditions.Add(gropBuyConditionInfo);
			if (groupBuyInfo.MaxCount < groupBuyInfo.GroupBuyConditions[0].Count)
			{
				text += Formatter.FormatErrorMessage("限购数量必须大于等于满足数量 ");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return;
			}
			if (SubsitePromoteHelper.UpdateGroupBuy(groupBuyInfo))
			{
				this.ShowMsg("编辑团购活动成功", true);
				return;
			}
			this.ShowMsg("编辑团购活动失败", true);
		}
	}
}
