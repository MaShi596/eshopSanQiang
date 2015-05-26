using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.ControlPanel.Utility;
using LitJson;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
namespace Hidistro.UI.Web.Admin
{
	public class FileManagerJson : AdminPage
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
			if (user.UserRole != Hidistro.Membership.Core.Enums.UserRole.SiteManager)
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
			string text;
			string url;
			if (a == "false")
			{
				text = "~/Storage/master/gallery/";
				url = "/Storage/master/gallery/";
			}
			else
			{
				text = string.Format("{0}/fckfiles/Files/Image/", Hidistro.Membership.Context.HiContext.Current.GetSkinPath());
				if (base.Request.ApplicationPath != "/")
				{
					url = text.Substring(base.Request.ApplicationPath.Length);
				}
				else
				{
					url = text;
				}
			}
			string text2 = base.Request.QueryString["order"];
			text2 = (string.IsNullOrEmpty(text2) ? "uploadtime" : text2.ToLower());
			string text3 = base.Request.QueryString["cid"];
			if (text3 == null)
			{
				text3 = "-1";
			}
			if (a == "false")
			{
				this.FillTableForDb(text3, text2, hashtable);
			}
			else
			{
				this.FillTableForPath(text, url, text2, hashtable);
			}
			string text4 = base.Request.Url.ToString();
			text4 = text4.Substring(0, text4.IndexOf("/", 7));
			text4 += base.Request.ApplicationPath;
			if (text4.EndsWith("/"))
			{
				text4 = text4.Substring(0, text4.Length - 1);
			}
			hashtable["domain"] = text4;
			base.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
			base.Response.Write(JsonMapper.ToJson(hashtable));
			base.Response.End();
		}
		public void FillTableForDb(string cid, string order, System.Collections.Hashtable table)
		{
			Database database = DatabaseFactory.CreateDatabase();
			System.Collections.Generic.List<System.Collections.Hashtable> list = new System.Collections.Generic.List<System.Collections.Hashtable>();
			table["category_list"] = list;
			System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("select CategoryId,CategoryName from Hishop_PhotoCategories order by DisplaySequence");
			using (System.Data.IDataReader dataReader = database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
					hashtable["cId"] = dataReader["CategoryId"];
					hashtable["cName"] = dataReader["CategoryName"];
					list.Add(hashtable);
				}
			}
			System.Collections.Generic.List<System.Collections.Hashtable> list2 = new System.Collections.Generic.List<System.Collections.Hashtable>();
			table["file_list"] = list2;
			if (cid.Trim() == "-1")
			{
				sqlStringCommand.CommandText = string.Format("select * from Hishop_PhotoGallery order by {1}", cid, order);
			}
			else
			{
				sqlStringCommand.CommandText = string.Format("select * from Hishop_PhotoGallery where CategoryId={0} order by {1}", cid, order);
			}
			using (System.Data.IDataReader dataReader2 = database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader2.Read())
				{
					System.Collections.Hashtable hashtable2 = new System.Collections.Hashtable();
					hashtable2["cid"] = dataReader2["CategoryId"];
					hashtable2["name"] = dataReader2["PhotoName"];
					hashtable2["path"] = dataReader2["PhotoPath"];
					hashtable2["filesize"] = dataReader2["FileSize"];
					hashtable2["addedtime"] = dataReader2["UploadTime"];
					hashtable2["updatetime"] = dataReader2["LastUpdateTime"];
					string text = dataReader2["PhotoPath"].ToString().Trim();
					hashtable2["filetype"] = text.Substring(text.LastIndexOf('.'));
					list2.Add(hashtable2);
				}
			}
			table["total_count"] = list2.Count;
			table["current_cateogry"] = int.Parse(cid);
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
