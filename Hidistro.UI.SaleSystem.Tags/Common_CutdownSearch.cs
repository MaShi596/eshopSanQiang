using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_CutdownSearch : AscxTemplatedWebControl
	{
		public delegate void ReSearchEventHandler(object sender, EventArgs e);
		public const string TagID = "search_Common_CutdownSearch";
		private IButton btnSearch;
		private TextBox txtKeywords;
		private TextBox txtStartPrice;
		private TextBox txtEndPrice;
		private ProductTagsCheckBoxList ckbListproductSearchType;
		public event Common_CutdownSearch.ReSearchEventHandler ReSearch;
		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}
		public ProductBrowseQuery Item
		{
			get
			{
				ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
				if (this.txtKeywords != null)
				{
					productBrowseQuery.Keywords = this.txtKeywords.Text.Trim();
				}
				if (this.txtStartPrice != null)
				{
					decimal value = 0m;
					if (!string.IsNullOrEmpty(this.txtStartPrice.Text.Trim()) && decimal.TryParse(this.txtStartPrice.Text.Trim(), out value))
					{
						productBrowseQuery.MinSalePrice = new decimal?(value);
					}
					else
					{
						productBrowseQuery.MinSalePrice = null;
					}
				}
				if (this.txtEndPrice != null)
				{
					decimal value2 = 0m;
					if (!string.IsNullOrEmpty(this.txtEndPrice.Text.Trim()) && decimal.TryParse(this.txtEndPrice.Text.Trim(), out value2))
					{
						productBrowseQuery.MaxSalePrice = new decimal?(value2);
					}
					else
					{
						productBrowseQuery.MaxSalePrice = null;
					}
				}
				productBrowseQuery.ProductCode = "";
				string text = string.Empty;
				IList<int> selectedValue = this.ckbListproductSearchType.SelectedValue;
				if (selectedValue != null && selectedValue.Count > 0)
				{
					foreach (int current in selectedValue)
					{
						text = text + current.ToString() + "_";
					}
					text = text.Substring(0, text.Length - 1);
				}
				productBrowseQuery.TagIds = text;
				Globals.EntityCoding(productBrowseQuery, true);
				return productBrowseQuery;
			}
		}
		public Common_CutdownSearch()
		{
			base.ID = "search_Common_CutdownSearch";
		}
		protected override void OnInit(EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_Search/Skin-Common_CutdownSearch.ascx";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
			this.txtKeywords = (TextBox)this.FindControl("txtKeywords");
			this.txtStartPrice = (TextBox)this.FindControl("txtStartPrice");
			this.txtEndPrice = (TextBox)this.FindControl("txtEndPrice");
			this.ckbListproductSearchType = (ProductTagsCheckBoxList)this.FindControl("ckbListproductSearchType");
			this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
				{
					this.txtKeywords.Text = DataHelper.CleanSearchString(Globals.UrlDecode(this.Page.Request.QueryString["keywords"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
				{
					this.txtStartPrice.Text = this.Page.Request.QueryString["minSalePrice"].ToString();
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
				{
					this.txtEndPrice.Text = this.Page.Request.QueryString["maxSalePrice"].ToString();
				}
			}
			this.ckbListproductSearchType.DataBind();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TagIds"]))
			{
				IList<int> list = new List<int>();
				string[] array = this.Page.Request.QueryString["TagIds"].Split(new char[]
				{
					'_'
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string value = array2[i];
					list.Add(Convert.ToInt32(value));
				}
				this.ckbListproductSearchType.SelectedValue = list;
			}
		}
		public void OnReSearch(object sender, EventArgs e)
		{
			if (this.ReSearch != null)
			{
				this.ReSearch(sender, e);
			}
		}
		private void btnSearch_Click(object sender, EventArgs e)
		{
			this.OnReSearch(sender, e);
		}
	}
}
