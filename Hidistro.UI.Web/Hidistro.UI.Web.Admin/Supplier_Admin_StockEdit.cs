using ASPNET.WebControls;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Web.CustomMade;
using System;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_Admin_StockEdit : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl h_h1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl h_h2;
		protected WebCalendar calendarStartDate;
		protected System.Web.UI.HtmlControls.HtmlGenericControl h_h4;
		protected System.Web.UI.WebControls.TextBox txtStockCode;
		protected System.Web.UI.WebControls.TextBox txt_Options;
		protected System.Web.UI.HtmlControls.HtmlGenericControl h_h3;
		protected System.Web.UI.WebControls.Repeater Rpbinditems;
		protected System.Web.UI.WebControls.Button btn_Submits_Add;
		protected System.Web.UI.WebControls.Button btn_Submits;
		private int bundlingid;
		private int status;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(base.Request.QueryString["StockId"], out this.bundlingid))
			{
				this.ShowMsg("参数提交错误", false);
				return;
			}
			if (!int.TryParse(base.Request.QueryString["Status"], out this.status))
			{
				this.ShowMsg("参数提交错误", false);
				return;
			}
			if (this.status == 2)
			{
				this.h_h1.InnerHtml = "查看出库单";
				this.h_h2.InnerHtml = "出库信息";
				this.h_h3.InnerHtml = "出库商品信息";
				this.h_h4.InnerHtml = "出库单号：<em>*</em>";
			}
			this.btn_Submits_Add.Click += new System.EventHandler(this.btn_Submits_Add_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindEditBindStock();
			}
		}
		protected void BindEditBindStock()
		{
			System.Data.DataTable dataTable = Methods.Supplier_StockInfoSelect(this.bundlingid);
			System.Data.DataTable dataSource = Methods.Supplier_StockItemSelect(this.bundlingid);
			this.calendarStartDate.Text = System.DateTime.Parse(dataTable.Rows[0]["AddDate"].ToString()).ToString("yyyy-MM-dd");
			this.txtStockCode.Text = dataTable.Rows[0]["Stock_Code"].ToString();
			this.txt_Options.Text = dataTable.Rows[0]["Options"].ToString();
			this.Rpbinditems.DataSource = dataSource;
			this.Rpbinditems.DataBind();
		}
		protected void btn_Submits_Add_Click(object sender, System.EventArgs e)
		{
			this.UpdateStock();
		}
		private void UpdateStock()
		{
			if (!string.IsNullOrEmpty(this.calendarStartDate.Text) && !string.IsNullOrEmpty(this.txtStockCode.Text))
			{
				System.DateTime.Parse(this.calendarStartDate.Text);
				string value = this.txtStockCode.Text;
				if (string.IsNullOrEmpty(value))
				{
					value = System.DateTime.Now.ToString("yyyyMMdd") + Hidistro.Membership.Context.HiContext.Current.User.UserId + Supplier_Admin_StockEdit.GetNumPwd(6);
				}
				this.txt_Options.Text.ToString();
				int num = 0;
				string text = base.Request.Form["selectProductsinfo"];
				string[] array = text.Split(new char[]
				{
					','
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					string[] array3 = text2.Split(new char[]
					{
						'|'
					});
					if (!string.IsNullOrEmpty(array3[4].ToString()))
					{
						if (array3[3] == "no")
						{
							int num2 = int.Parse(array3[4]);
							num += num2;
						}
						else
						{
							int num2 = int.Parse(array3[4]);
							num += num2;
							Methods.Supplier_StockAddfor_UpdateStockItem_Stock(int.Parse(array3[0]), num2);
							Methods.Supplier_StockAddfor_UpdateSkus_Stock(array3[1], num2, int.Parse(array3[5]));
						}
					}
					else
					{
						int num2 = int.Parse(array3[5]);
						num += num2;
					}
				}
				Methods.Supplier_StockAddfor_UpdateStockInfo_Stock(this.bundlingid, num);
			}
			else
			{
				this.ShowMsg("你的资料填写不完整", false);
			}
			this.ShowMsg("修改库存成功", true);
		}
		public static string GetNumPwd(int numCount)
		{
			string text = "0123456789";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 0; i < numCount; i++)
			{
				stringBuilder.Append(text[new System.Random(System.Guid.NewGuid().GetHashCode()).Next(0, text.Length - 1)]);
			}
			return stringBuilder.ToString();
		}
	}
}
