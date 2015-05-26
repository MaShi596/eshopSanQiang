using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.PictureMange)]
	public class ImageData : AdminPage
	{
		protected System.Web.UI.WebControls.Label lblImageData;
		protected System.Web.UI.WebControls.TextBox txtWordName;
		protected System.Web.UI.WebControls.Button btnImagetSearch;
		protected ImageLinkButton btnDelete1;
		protected ImageOrderDropDownList ImageOrder;
		protected ImageTypeLabel ImageTypeID;
		protected System.Web.UI.WebControls.DataList photoDataList;
		protected ImageLinkButton btnDelete2;
		protected Pager pager;
		protected System.Web.UI.WebControls.Button btnSaveImageDataName;
		protected System.Web.UI.WebControls.Button btnMoveImageData;
		protected System.Web.UI.WebControls.HiddenField ReImageDataNameId;
		protected System.Web.UI.WebControls.TextBox ReImageDataName;
		protected System.Web.UI.WebControls.HiddenField RePlaceImg;
		protected System.Web.UI.WebControls.HiddenField RePlaceId;
		protected System.Web.UI.WebControls.FileUpload FileUpload;
		protected ImageDataGradeDropDownList dropImageFtp;
		private string keyWordIName = string.Empty;
		private string keyOrder;
		private int? typeId = null;
		private int? enumOrder;
		private int pageIndex;
		public string GlobalsPath = Globals.ApplicationPath;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnDelete1.Click += new System.EventHandler(this.btnDelete_Click);
			this.btnDelete2.Click += new System.EventHandler(this.btnDelete_Click);
			this.btnSaveImageDataName.Click += new System.EventHandler(this.btnSaveImageDataName_Click);
			this.btnMoveImageData.Click += new System.EventHandler(this.btnMoveImageData_Click);
			this.ImageOrder.SelectedIndexChanged += new System.EventHandler(this.ImageOrder_SelectedIndexChanged);
			this.btnImagetSearch.Click += new System.EventHandler(this.btnImagetSearch_Click);
			this.photoDataList.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.photoDataList_ItemCommand);
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.ImageOrder.DataBind();
				this.dropImageFtp.DataBind();
				this.BindImageData();
			}
		}
		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyWordIName"]))
			{
				this.keyWordIName = Globals.UrlDecode(this.Page.Request.QueryString["keyWordIName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyWordSel"]))
			{
				this.keyOrder = Globals.UrlDecode(this.Page.Request.QueryString["keyWordSel"]);
			}
			int value = 0;
			if (int.TryParse(this.Page.Request.QueryString["imageTypeId"], out value))
			{
				this.typeId = new int?(value);
			}
			if (this.enumOrder.HasValue)
			{
				this.ImageOrder.SelectedValue = this.enumOrder;
			}
		}
		private void ImageOrder_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.enumOrder = this.ImageOrder.SelectedValue;
			this.BindImageData();
		}
		private void btnImagetSearch_Click(object sender, System.EventArgs e)
		{
			this.keyWordIName = this.txtWordName.Text;
			this.BindImageData();
		}
		private void BindImageData()
		{
			this.pageIndex = this.pager.PageIndex;
			PhotoListOrder order = PhotoListOrder.UploadTimeDesc;
			if (this.enumOrder.HasValue)
			{
				order = (PhotoListOrder)System.Enum.ToObject(typeof(PhotoListOrder), this.enumOrder.Value);
			}
			DbQueryResult photoList = GalleryHelper.GetPhotoList(this.keyWordIName, this.typeId, this.pageIndex, order);
			this.photoDataList.DataSource = photoList.Data;
			this.photoDataList.DataBind();
			this.pager.TotalRecords=photoList.TotalRecords;
			this.lblImageData.Text = "共" + this.pager.TotalRecords + "张";
		}
		private void photoDataList_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			int photoId = System.Convert.ToInt32(this.photoDataList.DataKeys[e.Item.ItemIndex]);
			string photoPath = GalleryHelper.GetPhotoPath(photoId);
			if (GalleryHelper.DeletePhoto(photoId))
			{
				StoreHelper.DeleteImage(photoPath);
				this.ShowMsg("删除图片成功", true);
			}
			this.BindImageData();
		}
		private void btnMoveImageData_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
			int pTypeId = System.Convert.ToInt32(this.dropImageFtp.SelectedItem.Value);
			foreach (System.Web.UI.WebControls.DataListItem dataListItem in this.photoDataList.Controls)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)dataListItem.FindControl("checkboxCol");
				if (checkBox != null && checkBox.Checked)
				{
					int item = (int)this.photoDataList.DataKeys[dataListItem.ItemIndex];
					list.Add(item);
				}
			}
			if (GalleryHelper.MovePhotoType(list, pTypeId) > 0)
			{
				this.ShowMsg("图片移动成功！", true);
			}
			this.BindImageData();
		}
		private void btnSaveImageDataName_Click(object sender, System.EventArgs e)
		{
			string text = this.ReImageDataName.Text;
			if (!string.IsNullOrEmpty(text) && text.Length <= 30)
			{
				int photoId = System.Convert.ToInt32(this.ReImageDataNameId.Value);
				GalleryHelper.RenamePhoto(photoId, text);
				this.BindImageData();
				return;
			}
			this.ShowMsg("图片名称不能为空长度限制在30个字符以内！", false);
		}
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			bool flag = true;
			bool flag2 = true;
			foreach (System.Web.UI.WebControls.DataListItem dataListItem in this.photoDataList.Controls)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)dataListItem.FindControl("checkboxCol");
				System.Web.UI.WebControls.HiddenField hiddenField = (System.Web.UI.WebControls.HiddenField)dataListItem.FindControl("HiddenFieldImag");
				if (checkBox != null && checkBox.Checked)
				{
					flag2 = false;
					try
					{
						int photoId = (int)this.photoDataList.DataKeys[dataListItem.ItemIndex];
						string value = hiddenField.Value;
						StoreHelper.DeleteImage(value);
						if (!GalleryHelper.DeletePhoto(photoId))
						{
							flag = false;
						}
					}
					catch
					{
						this.ShowMsg("删除文件错误", false);
						this.BindImageData();
					}
				}
			}
			if (flag2)
			{
				this.ShowMsg("未选择删除的图片", false);
				return;
			}
			if (flag)
			{
				this.ShowMsg("删除图片成功", true);
			}
			this.BindImageData();
		}
		public static string TruncStr(string str, int maxSize)
		{
			str = ImageData.Html_ToClient(str);
			if (str != string.Empty)
			{
				int num = 0;
				System.Text.ASCIIEncoding aSCIIEncoding = new System.Text.ASCIIEncoding();
				byte[] bytes = aSCIIEncoding.GetBytes(str);
				for (int i = 0; i <= bytes.Length - 1; i++)
				{
					if (bytes[i] == 63)
					{
						num += 2;
					}
					else
					{
						num++;
					}
					if (num > maxSize)
					{
						str = str.Substring(0, i);
						return str;
					}
				}
				return str;
			}
			return string.Empty;
		}
		public static string Html_ToClient(string Str)
		{
			if (Str == null)
			{
				return null;
			}
			if (Str != string.Empty)
			{
				return System.Web.HttpContext.Current.Server.HtmlDecode(Str.Trim());
			}
			return string.Empty;
		}
	}
}
