using Hidistro.Core;
using Hidistro.Membership.Context;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.DialogTemplates
{
	public class SelectImage : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected System.Web.UI.HtmlControls.HtmlSelect slsbannerposition;
		protected System.Web.UI.HtmlControls.HtmlGenericControl imagesize;
		protected System.Web.UI.WebControls.Repeater rp_img;
		private string type = "";
		private int pagesize = 30;
		public int pagetotal;
		public int pageindex = 1;
		public int sum;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.loadParamQuery();
			if (!string.IsNullOrEmpty(base.Request.QueryString["iscallback"]) && System.Convert.ToBoolean(base.Request.QueryString["iscallback"]))
			{
				this.UploadImage();
			}
			if (!string.IsNullOrEmpty(base.Request.QueryString["del"]))
			{
				string path = base.Request.QueryString["del"];
				string path2 = Globals.PhysicalPath(path);
				if (System.IO.File.Exists(path2))
				{
					System.IO.File.Delete(path2);
				}
			}
			if (!base.IsPostBack)
			{
				this.DataBindImages();
			}
		}
		private void loadParamQuery()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["size"]))
			{
				this.imagesize.InnerText = Globals.HtmlEncode(this.Page.Request.QueryString["size"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["type"]))
			{
				this.slsbannerposition.Value = this.Page.Request.QueryString["type"];
				this.slsbannerposition.Disabled = true;
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["pageindex"]) && System.Convert.ToInt32(this.Page.Request.QueryString["pageindex"]) > 0)
			{
				this.pageindex = System.Convert.ToInt32(this.Page.Request.QueryString["pageindex"]);
			}
		}
		private void UploadImage()
		{
			System.Drawing.Image image = null;
			System.Drawing.Image image2 = null;
			Bitmap bitmap = null;
			Graphics graphics = null;
			System.IO.MemoryStream memoryStream = null;
			try
			{
				System.Web.HttpPostedFile httpPostedFile = base.Request.Files["Filedata"];
				string str = System.DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
				string str2 = Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/UploadImage/" + this.slsbannerposition.Value + "/";
				string text = str + System.IO.Path.GetExtension(httpPostedFile.FileName);
				httpPostedFile.SaveAs(Globals.MapPath(str2 + text));
				base.Response.StatusCode = 200;
				base.Response.Write(string.Concat(new string[]
				{
					Globals.ApplicationPath,
					Hidistro.Membership.Context.HiContext.Current.GetSkinPath(),
					"/UploadImage/",
					this.slsbannerposition.Value,
					"/",
					text
				}));
			}
			catch (System.Exception)
			{
				base.Response.StatusCode = 500;
				base.Response.Write("服务器错误");
				base.Response.End();
			}
			finally
			{
				if (bitmap != null)
				{
					bitmap.Dispose();
				}
				if (graphics != null)
				{
					graphics.Dispose();
				}
				if (image2 != null)
				{
					image2.Dispose();
				}
				if (image != null)
				{
					image.Dispose();
				}
				if (memoryStream != null)
				{
					memoryStream.Close();
				}
				base.Response.End();
			}
		}
		private void DataBindImages()
		{
			string path = Globals.MapPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/UploadImage/" + this.slsbannerposition.Value);
			System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(path);
			IOrderedEnumerable<System.IO.FileInfo> source = 
				from file in directoryInfo.GetFiles()
				orderby file.CreationTime descending
				select file;
			this.sum = source.Count<System.IO.FileInfo>();
			this.pagetotal = this.sum / this.pagesize;
			if (this.sum % this.pagesize != 0)
			{
				this.pagetotal++;
			}
			if (this.pageindex < 1 || this.pageindex > this.pagetotal)
			{
				this.pageindex = 1;
			}
			this.rp_img.DataSource = source.Skip((this.pageindex - 1) * this.pagesize).Take(this.pagesize);
			this.rp_img.DataBind();
		}
		protected string ShowImage(string filename)
		{
			filename = string.Concat(new string[]
			{
				Globals.ApplicationPath,
				Hidistro.Membership.Context.HiContext.Current.GetSkinPath(),
				"/UploadImage/",
				this.slsbannerposition.Value,
				"/",
				filename
			});
			return filename;
		}
	}
}
