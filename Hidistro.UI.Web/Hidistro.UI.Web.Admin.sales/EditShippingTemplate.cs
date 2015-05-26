using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.sales
{
	public class EditShippingTemplate : AdminPage
	{
		[System.Serializable]
		public class Region
		{
			private string regionsId;
			private string regions;
			private decimal regionPrice;
			private decimal regionAddPrice;
			public string Regions
			{
				get
				{
					return this.regions;
				}
				set
				{
					this.regions = value;
				}
			}
			public string RegionsId
			{
				get
				{
					return this.regionsId;
				}
				set
				{
					this.regionsId = value;
				}
			}
			public decimal RegionPrice
			{
				get
				{
					return this.regionPrice;
				}
				set
				{
					this.regionPrice = value;
				}
			}
			public decimal RegionAddPrice
			{
				get
				{
					return this.regionAddPrice;
				}
				set
				{
					this.regionAddPrice = value;
				}
			}
		}
		protected Hidistro.UI.Common.Controls.Style Style1;
		protected Script Script1;
		protected System.Web.UI.WebControls.TextBox txtModeName;
		protected System.Web.UI.WebControls.TextBox txtWeight;
		protected System.Web.UI.WebControls.TextBox txtAddWeight;
		protected System.Web.UI.WebControls.TextBox txtPrice;
		protected System.Web.UI.WebControls.TextBox txtAddPrice;
		protected Grid grdRegion;
		protected System.Web.UI.WebControls.Button btnUpdate;
		protected System.Web.UI.WebControls.TextBox txtRegion_Id;
		protected System.Web.UI.HtmlControls.HtmlInputText txtRegion;
		protected System.Web.UI.WebControls.TextBox txtRegionPrice;
		protected System.Web.UI.WebControls.TextBox txtAddRegionPrice;
		protected System.Web.UI.WebControls.Button btnAdd;
		protected RegionArea regionArea;
		private int templateId;
		private System.Collections.Generic.IList<EditShippingTemplate.Region> RegionList
		{
			get
			{
				if (this.ViewState["Region"] == null)
				{
					this.ViewState["Region"] = new System.Collections.Generic.List<EditShippingTemplate.Region>();
				}
				return (System.Collections.Generic.IList<EditShippingTemplate.Region>)this.ViewState["Region"];
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["TemplateId"], out this.templateId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			this.grdRegion.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdRegion_RowDeleting);
			if (!this.Page.IsPostBack)
			{
				if (this.Page.Request.QueryString["isUpdate"] != null && this.Page.Request.QueryString["isUpdate"] == "true")
				{
					this.ShowMsg("成功修改了一个配送方式", true);
				}
				ShippingModeInfo shippingTemplate = SalesHelper.GetShippingTemplate(this.templateId, true);
				if (shippingTemplate == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.BindControl(shippingTemplate);
				this.BindRegion();
			}
		}
		private bool ValidateValues(out decimal regionPrice, out decimal addRegionPrice)
		{
			string text = string.Empty;
			if (string.IsNullOrEmpty(this.txtRegion_Id.Text))
			{
				text += Formatter.FormatErrorMessage("到达地不能为空");
			}
			if (string.IsNullOrEmpty(this.txtRegionPrice.Text))
			{
				text += Formatter.FormatErrorMessage("起步价不能为空");
				regionPrice = 0m;
			}
			else
			{
				if (!decimal.TryParse(this.txtRegionPrice.Text.Trim(), out regionPrice))
				{
					text += Formatter.FormatErrorMessage("起步价只能为非负数字");
				}
				else
				{
					if (decimal.Parse(this.txtRegionPrice.Text.Trim()) > 10000000m)
					{
						text += Formatter.FormatErrorMessage("起步价限制在1000万以内");
					}
				}
			}
			if (string.IsNullOrEmpty(this.txtAddRegionPrice.Text))
			{
				text += Formatter.FormatErrorMessage("加价不能为空");
				addRegionPrice = 0m;
			}
			else
			{
				if (!decimal.TryParse(this.txtAddRegionPrice.Text.Trim(), out addRegionPrice))
				{
					text += Formatter.FormatErrorMessage("加价只能为非负数字");
				}
				else
				{
					if (decimal.Parse(this.txtAddRegionPrice.Text.Trim()) > 10000000m)
					{
						text += Formatter.FormatErrorMessage("加价限制在1000万以内");
					}
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			decimal regionPrice;
			decimal regionAddPrice;
			if (!this.ValidateValues(out regionPrice, out regionAddPrice))
			{
				return;
			}
			EditShippingTemplate.Region region = new EditShippingTemplate.Region();
			region.RegionsId = this.txtRegion_Id.Text;
			region.Regions = this.txtRegion.Value;
			region.RegionPrice = regionPrice;
			region.RegionAddPrice = regionAddPrice;
			this.RegionList.Add(region);
			this.BindRegion();
			this.txtRegion_Id.Text = string.Empty;
			this.txtRegion.Value = string.Empty;
			this.txtRegionPrice.Text = "0";
			this.txtAddRegionPrice.Text = "0";
		}
		private void BindRegion()
		{
			this.grdRegion.DataSource = this.RegionList;
			this.grdRegion.DataBind();
		}
		private void grdRegion_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			this.RegionList.RemoveAt(e.RowIndex);
			this.BindRegion();
		}
		private void BindControl(ShippingModeInfo modeItem)
		{
			this.txtModeName.Text = Globals.HtmlDecode(modeItem.Name);
			this.txtWeight.Text = modeItem.Weight.ToString("F2");
			this.txtAddWeight.Text = modeItem.AddWeight.Value.ToString("F2");
			if (modeItem.AddPrice.HasValue)
			{
				this.txtAddPrice.Text = modeItem.AddPrice.Value.ToString("F2");
			}
			this.txtPrice.Text = modeItem.Price.ToString("F2");
			this.RegionList.Clear();
			if (modeItem.ModeGroup != null && modeItem.ModeGroup.Count > 0)
			{
				foreach (ShippingModeGroupInfo current in modeItem.ModeGroup)
				{
					EditShippingTemplate.Region region = new EditShippingTemplate.Region();
					region.RegionPrice = decimal.Parse(current.Price.ToString("F2"));
					region.RegionAddPrice = decimal.Parse(current.AddPrice.ToString("F2"));
					System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
					System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
					foreach (ShippingRegionInfo current2 in current.ModeRegions)
					{
						stringBuilder.Append(current2.RegionId + ",");
						stringBuilder2.Append(RegionHelper.GetFullRegion(current2.RegionId, ",") + ",");
					}
					if (!string.IsNullOrEmpty(stringBuilder.ToString()))
					{
						region.RegionsId = stringBuilder.ToString().Substring(0, stringBuilder.ToString().Length - 1);
					}
					if (!string.IsNullOrEmpty(stringBuilder2.ToString()))
					{
						region.Regions = stringBuilder2.ToString().Substring(0, stringBuilder2.ToString().Length - 1);
					}
					this.RegionList.Add(region);
				}
			}
		}
		private bool ValidateRegionValues(out decimal weight, out decimal? addWeight, out decimal price, out decimal? addPrice)
		{
			string text = string.Empty;
			addWeight = null;
			addPrice = null;
			if (!decimal.TryParse(this.txtWeight.Text.Trim(), out weight))
			{
				text += Formatter.FormatErrorMessage("起步重量不能为空,必须为正整数,限制在100千克以内");
			}
			if (!string.IsNullOrEmpty(this.txtAddWeight.Text.Trim()))
			{
				decimal value;
				if (decimal.TryParse(this.txtAddWeight.Text.Trim(), out value))
				{
					addWeight = new decimal?(value);
				}
				else
				{
					text += Formatter.FormatErrorMessage("加价重量不能为空,必须为正整数,限制在100千克以内");
				}
			}
			if (!decimal.TryParse(this.txtPrice.Text.Trim(), out price))
			{
				text += Formatter.FormatErrorMessage("默认起步价不能为空,必须为非负数字,限制在1000万以内");
			}
			if (!string.IsNullOrEmpty(this.txtAddPrice.Text.Trim()))
			{
				decimal value2;
				if (decimal.TryParse(this.txtAddPrice.Text.Trim(), out value2))
				{
					addPrice = new decimal?(value2);
				}
				else
				{
					text += Formatter.FormatErrorMessage("默认加价必须为非负数字,限制在1000万以内");
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			decimal weight;
			decimal? addWeight;
			decimal price;
			decimal? addPrice;
			if (!this.ValidateRegionValues(out weight, out addWeight, out price, out addPrice))
			{
				return;
			}
			new System.Collections.Generic.List<ShippingModeGroupInfo>();
			ShippingModeInfo shippingModeInfo = new ShippingModeInfo();
			shippingModeInfo.Name = Globals.HtmlEncode(this.txtModeName.Text.Trim());
			shippingModeInfo.Weight = weight;
			shippingModeInfo.AddWeight = addWeight;
			shippingModeInfo.Price = price;
			shippingModeInfo.AddPrice = addPrice;
			shippingModeInfo.TemplateId = this.templateId;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdRegion.Rows)
			{
				decimal price2 = 0m;
				decimal addPrice2 = 0m;
				decimal.TryParse(((System.Web.UI.WebControls.TextBox)gridViewRow.FindControl("txtModeRegionPrice")).Text, out price2);
				decimal.TryParse(((System.Web.UI.WebControls.TextBox)gridViewRow.FindControl("txtModeRegionAddPrice")).Text, out addPrice2);
				ShippingModeGroupInfo shippingModeGroupInfo = new ShippingModeGroupInfo();
				shippingModeGroupInfo.Price = price2;
				shippingModeGroupInfo.AddPrice = addPrice2;
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)this.grdRegion.Rows[gridViewRow.RowIndex].FindControl("txtRegionvalue_Id");
				if (!string.IsNullOrEmpty(textBox.Text))
				{
					string[] array = textBox.Text.Split(new char[]
					{
						','
					});
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string text = array2[i];
						ShippingRegionInfo shippingRegionInfo = new ShippingRegionInfo();
						shippingRegionInfo.RegionId = System.Convert.ToInt32(text.Trim());
						shippingModeGroupInfo.ModeRegions.Add(shippingRegionInfo);
					}
				}
				shippingModeInfo.ModeGroup.Add(shippingModeGroupInfo);
			}
			if (SalesHelper.UpdateShippingTemplate(shippingModeInfo))
			{
				this.Page.Response.Redirect("EditShippingTemplate.aspx?TemplateId=" + shippingModeInfo.TemplateId + "&isUpdate=true");
				return;
			}
			this.ShowMsg("您添加的地区有重复", false);
		}
	}
}
