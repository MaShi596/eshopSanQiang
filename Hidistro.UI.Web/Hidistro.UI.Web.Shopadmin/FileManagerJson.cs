using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.Subsites.Utility;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace Hidistro.UI.Web.Shopadmin
{
	public class FileManagerJson : DistributorPage
	{
		public class NameSorter : System.Collections.IComparer
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
		public class SizeSorter : System.Collections.IComparer
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
		public class DateTimeSorter : System.Collections.IComparer
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
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.Users.GetUser(0, Hidistro.Membership.Context.Users.GetLoggedOnUsername(), true, true);
			System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
			if (user.UserRole != Hidistro.Membership.Core.Enums.UserRole.Distributor)
			{
				base.Response.Write("没有权限！");
				base.Response.End();
				return;
			}
			string a = "false";
			if (base.Request.QueryString["isAdvPositions"] != null)
			{
				a = base.Request.QueryString["isAdvPositions"].ToString().ToLower().Trim();
			}
			string path;
			string url;
			if (a == "false")
			{
				path = string.Format("~/Storage/sites/{0}/fckfiles/", user.UserId);
				url = string.Format("/Storage/sites/{0}/fckfiles/", user.UserId);
			}
			else
			{
				Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(user.UserId);
				path = string.Format("~/Templates/sites/{0}/{1}/fckfiles/Files/Image/", user.UserId, siteSettings.Theme);
				url = string.Format("/Templates/sites/{0}/{1}/fckfiles/Files/Image/", user.UserId, siteSettings.Theme);
			}
			string text = base.Request.QueryString["order"];
			text = (string.IsNullOrEmpty(text) ? "uploadtime" : text.ToLower());
			if (base.Request.QueryString["cid"] == null)
			{
			}
			this.FillTableForPath(path, url, text, hashtable);
			string text2 = base.Request.Url.ToString();
			text2 = text2.Substring(0, text2.IndexOf("/", 7));
			text2 += base.Request.ApplicationPath;
			if (text2.EndsWith("/"))
			{
				text2 = text2.Substring(0, text2.Length - 1);
			}
			hashtable["domain"] = text2;
			base.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
			base.Response.Write(JsonMapper.ToJson(hashtable));
			base.Response.End();
		}
		public void FillTableForPath(string path, string url, string order, System.Collections.Hashtable table)
		{
			string path2 = base.Server.MapPath(path);
			if (!System.IO.Directory.Exists(path2))
			{
				base.Response.Write("此目录不存在！");
				base.Response.End();
			}
			string[] files = System.IO.Directory.GetFiles(path2);
			switch (order)
			{
			case "uploadtime":
				System.Array.Sort(files, new FileManagerJson.DateTimeSorter(0, true));
				goto IL_17A;
			case "uploadtime desc":
				System.Array.Sort(files, new FileManagerJson.DateTimeSorter(0, false));
				goto IL_17A;
			case "lastupdatetime":
				System.Array.Sort(files, new FileManagerJson.DateTimeSorter(1, true));
				goto IL_17A;
			case "lastupdatetime desc":
				System.Array.Sort(files, new FileManagerJson.DateTimeSorter(1, false));
				goto IL_17A;
			case "photoname":
				System.Array.Sort(files, new FileManagerJson.NameSorter(true));
				goto IL_17A;
			case "photoname desc":
				System.Array.Sort(files, new FileManagerJson.NameSorter(false));
				goto IL_17A;
			case "filesize":
				System.Array.Sort(files, new FileManagerJson.SizeSorter(true));
				goto IL_17A;
			case "filesize desc":
				System.Array.Sort(files, new FileManagerJson.SizeSorter(false));
				goto IL_17A;
			}
			System.Array.Sort(files, new FileManagerJson.NameSorter(true));
			IL_17A:
			table["total_count"] = files.Length;
			System.Collections.Generic.List<System.Collections.Hashtable> list = new System.Collections.Generic.List<System.Collections.Hashtable>();
			table["file_list"] = list;
			for (int i = 0; i < files.Length; i++)
			{
				System.IO.FileInfo fileInfo = new System.IO.FileInfo(files[i]);
				System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
				hashtable["cid"] = "-1";
				hashtable["name"] = fileInfo.Name;
				hashtable["path"] = url + fileInfo.Name;
				hashtable["filesize"] = fileInfo.Length;
				hashtable["addedtime"] = fileInfo.CreationTime;
				hashtable["updatetime"] = fileInfo.LastWriteTime;
				hashtable["filetype"] = fileInfo.Extension.Substring(1);
				list.Add(hashtable);
			}
		}
	}
}
