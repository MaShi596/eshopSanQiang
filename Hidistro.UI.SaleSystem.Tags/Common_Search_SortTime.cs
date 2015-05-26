using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Search_SortTime : LinkButton
	{
		public delegate void SortingHandler(string sortOrder, string sortOrderBy);
		public const string TagID = "btn_Common_Search_SortTime";
		private string imageFormat = "<img border=\"0\" src=\"{0}\" alt=\"{1}\" />";
		private bool showText = true;
		private ImagePosition position;
		public event Common_Search_SortTime.SortingHandler Sorting;
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
		public string DefaultImageUrl
		{
			get;
			set;
		}
		public string AscImageUrl
		{
			get;
			set;
		}
		public string DescImageUrl
		{
			get;
			set;
		}
		public string Alt
		{
			get;
			set;
		}
		public bool ShowText
		{
			get
			{
				return this.showText;
			}
			set
			{
				this.showText = value;
			}
		}
		public ImagePosition ImagePosition
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}
		public string ImageUrl
		{
			get
			{
				if (this.ViewState["Src"] != null)
				{
					return (string)this.ViewState["Src"];
				}
				return null;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					this.ViewState["Src"] = value;
					return;
				}
				this.ViewState["Src"] = null;
			}
		}
		private void OnSorting(string sortOrder, string sortOrderBy)
		{
			if (this.Sorting != null)
			{
				this.Sorting(sortOrder, sortOrderBy);
			}
		}
		public Common_Search_SortTime()
		{
			base.ID = "btn_Common_Search_SortTime";
			this.ShowText = false;
		}
		protected override void OnLoad(EventArgs eventArgs_0)
		{
			base.OnLoad(eventArgs_0);
			base.Click += new EventHandler(this.Common_Search_SortTime_Click);
		}
		private void Common_Search_SortTime_Click(object sender, EventArgs e)
		{
			string sortOrder = string.Empty;
			if (this.Page.Request.QueryString["sortOrder"] == "Desc")
			{
				sortOrder = "Asc";
			}
			else
			{
				sortOrder = "Desc";
			}
			this.OnSorting(sortOrder, "AddedDate");
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]) && this.Page.Request.QueryString["sortOrderBy"] == "AddedDate")
			{
				if (this.Page.Request.QueryString["sortOrder"] == "Desc")
				{
					this.ImageUrl = this.DescImageUrl;
					this.Alt = "按时间升序排序";
				}
				else
				{
					this.ImageUrl = this.AscImageUrl;
					this.Alt = "按时间降序排序";
				}
				this.ToolTip = this.Alt;
			}
			else
			{
				this.ImageUrl = this.DefaultImageUrl;
				this.ToolTip = "按时间排序";
			}
			base.Attributes.Add("name", this.NamingContainer.UniqueID + "$" + this.ID);
			string imageTag = this.GetImageTag();
			if (!this.ShowText)
			{
				base.Text = "";
			}
			if (this.ImagePosition == ImagePosition.Right)
			{
				base.Text += imageTag;
			}
			else
			{
				base.Text = imageTag + base.Text;
			}
			base.Render(writer);
		}
		private string GetImageTag()
		{
			if (string.IsNullOrEmpty(this.ImageUrl))
			{
				return string.Empty;
			}
			return string.Format(CultureInfo.InvariantCulture, this.imageFormat, new object[]
			{
				this.ImageUrl,
				this.Alt
			});
		}
	}
}
