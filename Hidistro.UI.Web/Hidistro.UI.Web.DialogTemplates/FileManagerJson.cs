using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
namespace Hidistro.UI.Web.DialogTemplates
{
	public class FileManagerJson : System.Web.IHttpHandler
	{
		private class NameSorter : System.Collections.IComparer
		{
			private bool ascend;
			public NameSorter(bool isAscend)
			{
				this.ascend = isAscend;
			}
			public int Compare(object x, object y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					if (!this.ascend)
					{
						return 1;
					}
					return -1;
				}
				else
				{
					if (y == null)
					{
						if (!this.ascend)
						{
							return -1;
						}
						return 1;
					}
					else
					{
						System.IO.FileInfo fileInfo = new System.IO.FileInfo(x.ToString());
						System.IO.FileInfo fileInfo2 = new System.IO.FileInfo(y.ToString());
						if (!this.ascend)
						{
							return fileInfo2.FullName.CompareTo(fileInfo.FullName);
						}
						return fileInfo.FullName.CompareTo(fileInfo2.FullName);
					}
				}
			}
		}
		private class SizeSorter : System.Collections.IComparer
		{
			private bool ascend;
			public SizeSorter(bool isAscend)
			{
				this.ascend = isAscend;
			}
			public int Compare(object x, object y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					if (!this.ascend)
					{
						return 1;
					}
					return -1;
				}
				else
				{
					if (y == null)
					{
						if (!this.ascend)
						{
							return -1;
						}
						return 1;
					}
					else
					{
						System.IO.FileInfo fileInfo = new System.IO.FileInfo(x.ToString());
						System.IO.FileInfo fileInfo2 = new System.IO.FileInfo(y.ToString());
						if (!this.ascend)
						{
							return fileInfo2.Length.CompareTo(fileInfo.Length);
						}
						return fileInfo.Length.CompareTo(fileInfo2.Length);
					}
				}
			}
		}
		private class DateTimeSorter : System.Collections.IComparer
		{
			private bool ascend;
			private int type;
			public DateTimeSorter(int sortType, bool isAscend)
			{
				this.ascend = isAscend;
				this.type = sortType;
			}
			public int Compare(object x, object y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					if (!this.ascend)
					{
						return 1;
					}
					return -1;
				}
				else
				{
					if (y == null)
					{
						if (!this.ascend)
						{
							return -1;
						}
						return 1;
					}
					else
					{
						System.IO.FileInfo fileInfo = new System.IO.FileInfo(x.ToString());
						System.IO.FileInfo fileInfo2 = new System.IO.FileInfo(y.ToString());
						if (this.type == 0)
						{
							if (!this.ascend)
							{
								return fileInfo2.CreationTime.CompareTo(fileInfo.CreationTime);
							}
							return fileInfo.CreationTime.CompareTo(fileInfo2.CreationTime);
						}
						else
						{
							if (!this.ascend)
							{
								return fileInfo2.LastWriteTime.CompareTo(fileInfo.LastWriteTime);
							}
							return fileInfo.LastWriteTime.CompareTo(fileInfo2.LastWriteTime);
						}
					}
				}
			}
		}
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
			System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
			if (user.UserRole != Hidistro.Membership.Core.Enums.UserRole.SiteManager && user.UserRole != Hidistro.Membership.Core.Enums.UserRole.Distributor)
			{
				this.message = "没有权限";
			}
			else
			{
				string text = context.Request.QueryString["cid"];
				if (text == null || text == "-1")
				{
					text = "AdvertImg";
				}
				string text2 = string.Format("{0}/UploadImage/" + text + "/", Hidistro.Membership.Context.HiContext.Current.GetSkinPath());
				string url;
				if (context.Request.ApplicationPath != "/")
				{
					url = text2.Substring(context.Request.ApplicationPath.Length);
				}
				else
				{
					url = text2;
				}
				string text3 = context.Request.QueryString["order"];
				text3 = (string.IsNullOrEmpty(text3) ? "uploadtime" : text3.ToLower());
				this.message = "未知错误";
				if (this.FillTableForPath(text2, url, text3, hashtable, text))
				{
					string text4 = context.Request.Url.ToString();
					text4 = text4.Substring(0, text4.IndexOf("/", 7));
					text4 += context.Request.ApplicationPath;
					if (text4.EndsWith("/"))
					{
						text4 = text4.Substring(0, text4.Length - 1);
					}
					hashtable["domain"] = text4;
					this.message = JsonMapper.ToJson(hashtable);
				}
			}
			context.Response.ContentType = "text/json";
			context.Response.Write(this.message);
		}
		private bool FillTableForPath(string path, string url, string order, System.Collections.Hashtable table, string cid)
		{
			string path2 = Globals.MapPath(path);
			if (!System.IO.Directory.Exists(path2))
			{
				this.message = "此目录不存在";
				return false;
			}
			string[] files = System.IO.Directory.GetFiles(path2);
			switch (order)
			{
			case "uploadtime":
				System.Array.Sort(files, new FileManagerJson.DateTimeSorter(0, true));
				goto IL_166;
			case "uploadtime desc":
				System.Array.Sort(files, new FileManagerJson.DateTimeSorter(0, false));
				goto IL_166;
			case "lastupdatetime":
				System.Array.Sort(files, new FileManagerJson.DateTimeSorter(1, true));
				goto IL_166;
			case "lastupdatetime desc":
				System.Array.Sort(files, new FileManagerJson.DateTimeSorter(1, false));
				goto IL_166;
			case "photoname":
				System.Array.Sort(files, new FileManagerJson.NameSorter(true));
				goto IL_166;
			case "photoname desc":
				System.Array.Sort(files, new FileManagerJson.NameSorter(false));
				goto IL_166;
			case "filesize":
				System.Array.Sort(files, new FileManagerJson.SizeSorter(true));
				goto IL_166;
			case "filesize desc":
				System.Array.Sort(files, new FileManagerJson.SizeSorter(false));
				goto IL_166;
			}
			System.Array.Sort(files, new FileManagerJson.NameSorter(true));
			IL_166:
			table["category_list"] = this.BindFileCategory();
			table["total_count"] = files.Length;
			System.Collections.Generic.List<System.Collections.Hashtable> list = new System.Collections.Generic.List<System.Collections.Hashtable>();
			table["file_list"] = list;
			table["current_cateogry"] = cid;
			for (int i = 0; i < files.Length; i++)
			{
				System.IO.FileInfo fileInfo = new System.IO.FileInfo(files[i]);
				System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
				hashtable["cid"] = cid;
				hashtable["name"] = fileInfo.Name;
				hashtable["path"] = url + fileInfo.Name;
				hashtable["filesize"] = fileInfo.Length;
				hashtable["addedtime"] = fileInfo.CreationTime;
				hashtable["updatetime"] = fileInfo.LastWriteTime;
				hashtable["filetype"] = fileInfo.Extension.Substring(1);
				list.Add(hashtable);
			}
			return true;
		}
		private System.Collections.Generic.IList<System.Collections.Hashtable> BindFileCategory()
		{
			System.Collections.Generic.List<System.Collections.Hashtable> list = new System.Collections.Generic.List<System.Collections.Hashtable>();
			System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
			hashtable["cId"] = "AdvertImg";
			hashtable["cName"] = "广告位图片";
			list.Add(hashtable);
			hashtable = new System.Collections.Hashtable();
			hashtable["cId"] = "TitleImg";
			hashtable["cName"] = "标题图片";
			list.Add(hashtable);
			return list;
		}
	}
}
