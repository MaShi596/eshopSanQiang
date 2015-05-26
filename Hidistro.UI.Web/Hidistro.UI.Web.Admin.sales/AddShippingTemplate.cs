using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.sales
{
	public class AddShippingTemplate : AdminPage
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
		protected System.Web.UI.WebControls.Button btnCreate;
		protected System.Web.UI.WebControls.TextBox txtRegion_Id;
		protected System.Web.UI.HtmlControls.HtmlInputText txtRegion;
		protected System.Web.UI.WebControls.TextBox txtRegionPrice;
		protected System.Web.UI.WebControls.TextBox txtAddRegionPrice;
		protected System.Web.UI.WebControls.Button btnAdd;
		protected RegionArea regionArea;
		private System.Collections.Generic.IList<AddShippingTemplate.Region> RegionList
		{
			get
			{
				if (this.ViewState["Region"] == null)
				{
					this.ViewState["Region"] = new System.Collections.Generic.List<AddShippingTemplate.Region>();
				}
				return (System.Collections.Generic.IList<AddShippingTemplate.Region>)this.ViewState["Region"];
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			this.grdRegion.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdRegion_RowDeleting);
		}
		private void btnCreate_Click(object sender, System.EventArgs e)
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
			if (!SalesHelper.CreateShippingTemplate(shippingModeInfo))
			{
				this.ShowMsg("您添加的地区有重复", false);
				return;
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["source"]) && this.Page.Request.QueryString["source"] == "add")
			{
				this.CloseWindow();
				return;
			}
			this.ClearControlValue();
			this.ShowMsg("成功添加了一个配送方式模板", true);
		}
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			decimal regionPrice;
			decimal regionAddPrice;
			if (!this.ValidateValues(out regionPrice, out regionAddPrice))
			{
				return;
			}
			AddShippingTemplate.Region region = new AddShippingTemplate.Region();
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
		private void grdRegion_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			this.RegionList.RemoveAt(e.RowIndex);
			this.BindRegion();
		}
		private void BindRegion()
		{
			this.grdRegion.DataSource = this.RegionList;
			this.grdRegion.DataBind();
		}
		private void ClearControlValue()
		{
			this.txtAddPrice.Text = string.Empty;
			this.txtAddRegionPrice.Text = string.Empty;
			this.txtAddWeight.Text = string.Empty;
			this.txtModeName.Text = string.Empty;
			this.txtPrice.Text = string.Empty;
			this.txtRegion.Value = string.Empty;
			this.txtRegion_Id.Text = string.Empty;
			this.txtRegionPrice.Text = string.Empty;
			this.txtWeight.Text = string.Empty;
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
				text += Formatter.FormatErrorMessage("默认起步价必须为非负数字,限制在1000万以内");
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
			string.IsNullOrEmpty(text);
			return true;
		}
	}
}
