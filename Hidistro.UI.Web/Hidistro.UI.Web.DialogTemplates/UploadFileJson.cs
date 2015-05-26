using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web;
namespace Hidistro.UI.Web.DialogTemplates
{
	public class UploadFileJson : System.Web.IHttpHandler
	{
		private string savePath;
		private string saveUrl;
		private string message = "";
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.Users.GetUser(0, Hidistro.Membership.Context.Users.GetLoggedOnUsername(), true, true);
			string str = "AdvertImg";
			if (!Hidistro.Membership.Context.HiContext.Current.Context.User.IsInRole("manager") && !Hidistro.Membership.Context.HiContext.Current.Context.User.IsInRole("systemadministrator") && user.UserRole != Hidistro.Membership.Core.Enums.UserRole.Distributor && user.UserRole != Hidistro.Membership.Core.Enums.UserRole.SiteManager)
			{
				this.showError("您没有权限执行此操作");
			}
			else
			{
				if (context.Request.Form["fileCategory"] != null)
				{
					str = context.Request.Form["fileCategory"];
				}
				string text = string.Empty;
				if (context.Request.Form["imgTitle"] != null)
				{
					text = context.Request.Form["imgTitle"];
				}
				this.savePath = string.Format("{0}/UploadImage/" + str + "/", Hidistro.Membership.Context.HiContext.Current.GetSkinPath());
				if (context.Request.ApplicationPath != "/")
				{
					this.saveUrl = this.savePath.Substring(context.Request.ApplicationPath.Length);
				}
				else
				{
					this.saveUrl = this.savePath;
				}
				System.Web.HttpPostedFile httpPostedFile = context.Request.Files["imgFile"];
				string text2 = "";
				if (this.CheckUploadFile(httpPostedFile, ref text2))
				{
					if (!System.IO.Directory.Exists(text2))
					{
						System.IO.Directory.CreateDirectory(text2);
					}
					string fileName = httpPostedFile.FileName;
					if (text.Length == 0)
					{
					}
					string str2 = System.IO.Path.GetExtension(fileName).ToLower();
					string str3 = System.DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo) + str2;
					string filename = text2 + str3;
					string str4 = this.saveUrl + str3;
					try
					{
						httpPostedFile.SaveAs(filename);
						System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
						hashtable["error"] = 0;
						hashtable["url"] = Globals.ApplicationPath + str4;
						this.message = JsonMapper.ToJson(hashtable);
					}
					catch
					{
						this.showError("保存文件出错");
					}
				}
			}
			context.Response.ContentType = "text/html";
			context.Response.Write(this.message);
		}
		private bool CheckUploadFile(System.Web.HttpPostedFile imgFile, ref string dirPath)
		{
			if (imgFile == null)
			{
				this.showError("请选择上传文件");
				return false;
			}
			if (!ResourcesHelper.CheckPostedFile(imgFile))
			{
				this.showError("不能上传空文件，且必须是有效的图片文件！");
				return false;
			}
			dirPath = Globals.MapPath(this.savePath);
			if (!System.IO.Directory.Exists(dirPath))
			{
				this.showError("上传目录不存在。");
				return false;
			}
			return true;
		}
		private void showError(string message)
		{
			System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
			hashtable["error"] = 1;
			hashtable["message"] = message;
			message = JavaScriptConvert.SerializeObject(hashtable);
		}
	}
}
