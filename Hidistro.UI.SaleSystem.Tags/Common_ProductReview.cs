using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Data;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ProductReview : ThemedTemplatedRepeater
	{
		public const string TagID = "list_Common_ProductReview";
		private int maxNum = 6;
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
		public int MaxNum
		{
			get
			{
				return this.maxNum;
			}
			set
			{
				this.maxNum = value;
			}
		}
		[Browsable(false)]
		public override object DataSource
		{
			get
			{
				return base.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				base.DataSource = value;
			}
		}
		public Common_ProductReview()
		{
			base.ID = "list_Common_ProductReview";
		}
		public override void DataBind()
		{
			this.EnsureChildControls();
			DataTable dataTable = (DataTable)this.DataSource;
			if (dataTable != null && dataTable.Rows.Count > this.maxNum)
			{
				int num = dataTable.Rows.Count - 1;
				for (int i = num; i >= this.maxNum; i--)
				{
					dataTable.Rows.RemoveAt(i);
				}
			}
			base.DataSource = dataTable;
			base.DataBind();
		}
	}
}
