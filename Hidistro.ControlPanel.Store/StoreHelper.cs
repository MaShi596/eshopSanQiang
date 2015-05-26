using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web;
using System.Xml;
namespace Hidistro.ControlPanel.Store
{
	public static class StoreHelper
	{
		public static IList<FriendlyLinksInfo> GetFriendlyLinks()
		{
			return StoreProvider.Instance().GetFriendlyLinks();
		}
		public static FriendlyLinksInfo GetFriendlyLink(int linkId)
		{
			return StoreProvider.Instance().GetFriendlyLink(linkId);
		}
		public static bool UpdateFriendlyLink(FriendlyLinksInfo friendlyLink)
		{
			return null != friendlyLink && StoreProvider.Instance().CreateUpdateDeleteFriendlyLink(friendlyLink, DataProviderAction.Update);
		}
		public static int FriendlyLinkDelete(int linkId)
		{
			return StoreProvider.Instance().FriendlyLinkDelete(linkId);
		}
		public static bool CreateFriendlyLink(FriendlyLinksInfo friendlyLink)
		{
			return null != friendlyLink && StoreProvider.Instance().CreateUpdateDeleteFriendlyLink(friendlyLink, DataProviderAction.Create);
		}
		public static void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence)
		{
			StoreProvider.Instance().SwapFriendlyLinkSequence(linkId, replaceLinkId, displaySequence, replaceDisplaySequence);
		}
		public static void DeleteHotKeywords(int int_0)
		{
			StoreProvider.Instance().DeleteHotKeywords(int_0);
		}
		public static void SwapHotWordsSequence(int int_0, int replaceHid, int displaySequence, int replaceDisplaySequence)
		{
			StoreProvider.Instance().SwapHotWordsSequence(int_0, replaceHid, displaySequence, replaceDisplaySequence);
		}
		public static void UpdateHotWords(int int_0, int categoryId, string hotKeyWords)
		{
			StoreProvider.Instance().UpdateHotWords(int_0, categoryId, hotKeyWords);
		}
		public static void AddHotkeywords(int categoryId, string keywords)
		{
			StoreProvider.Instance().AddHotkeywords(categoryId, keywords);
		}
		public static string GetHotkeyword(int int_0)
		{
			return StoreProvider.Instance().GetHotkeyword(int_0);
		}
		public static System.Data.DataTable GetHotKeywords()
		{
			return StoreProvider.Instance().GetHotKeywords();
		}
		public static System.Data.DataSet GetVotes()
		{
			return StoreProvider.Instance().GetVotes();
		}
		public static int SetVoteIsBackup(long voteId)
		{
			return StoreProvider.Instance().SetVoteIsBackup(voteId);
		}
		public static int CreateVote(VoteInfo vote)
		{
			int num = 0;
			long num2 = StoreProvider.Instance().CreateVote(vote);
			if (num2 > 0L)
			{
				num = 1;
				if (vote.VoteItems != null)
				{
					foreach (VoteItemInfo current in vote.VoteItems)
					{
						current.VoteId = num2;
						current.ItemCount = 0;
						num += StoreProvider.Instance().CreateVoteItem(current, null);
					}
				}
			}
			return num;
		}
		public static bool UpdateVote(VoteInfo vote)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!StoreProvider.Instance().UpdateVote(vote, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!StoreProvider.Instance().DeleteVoteItem(vote.VoteId, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
						}
						else
						{
							int num = 0;
							if (vote.VoteItems != null)
							{
								foreach (VoteItemInfo current in vote.VoteItems)
								{
									current.VoteId = vote.VoteId;
									current.ItemCount = 0;
									num += StoreProvider.Instance().CreateVoteItem(current, dbTransaction);
								}
								if (num < vote.VoteItems.Count)
								{
									dbTransaction.Rollback();
									result = false;
									return result;
								}
							}
							dbTransaction.Commit();
							result = true;
						}
					}
				}
				catch
				{
					dbTransaction.Rollback();
					result = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public static int DeleteVote(long voteId)
		{
			return StoreProvider.Instance().DeleteVote(voteId);
		}
		public static VoteInfo GetVoteById(long voteId)
		{
			return StoreProvider.Instance().GetVoteById(voteId);
		}
		public static IList<VoteItemInfo> GetVoteItems(long voteId)
		{
			return StoreProvider.Instance().GetVoteItems(voteId);
		}
		public static int GetVoteCounts(long voteId)
		{
			return StoreProvider.Instance().GetVoteCounts(voteId);
		}
		public static string BackupData()
		{
			return StoreProvider.Instance().BackupData(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Storage/data/Backup/"));
		}
		public static bool InserBackInfo(string fileName, string version, long fileSize)
		{
			string filename = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
			bool result;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("root");
				XmlElement xmlElement = xmlDocument.CreateElement("backupfile");
				xmlElement.SetAttribute("BackupName", fileName);
				xmlElement.SetAttribute("Version", version.ToString());
				xmlElement.SetAttribute("FileSize", fileSize.ToString());
				xmlElement.SetAttribute("BackupTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				xmlNode.AppendChild(xmlElement);
				xmlDocument.Save(filename);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static System.Data.DataTable GetBackupFiles()
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			dataTable.Columns.Add("BackupName", typeof(string));
			dataTable.Columns.Add("Version", typeof(string));
			dataTable.Columns.Add("FileSize", typeof(string));
			dataTable.Columns.Add("BackupTime", typeof(string));
			string filename = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
			foreach (XmlNode xmlNode in childNodes)
			{
				XmlElement xmlElement = (XmlElement)xmlNode;
				System.Data.DataRow dataRow = dataTable.NewRow();
				dataRow["BackupName"] = xmlElement.GetAttribute("BackupName");
				dataRow["Version"] = xmlElement.GetAttribute("Version");
				dataRow["FileSize"] = xmlElement.GetAttribute("FileSize");
				dataRow["BackupTime"] = xmlElement.GetAttribute("BackupTime");
				dataTable.Rows.Add(dataRow);
			}
			return dataTable;
		}
		public static bool DeleteBackupFile(string backupName)
		{
			string filename = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
			bool result;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
				foreach (XmlNode xmlNode in childNodes)
				{
					XmlElement xmlElement = (XmlElement)xmlNode;
					if (xmlElement.GetAttribute("BackupName") == backupName)
					{
						xmlDocument.SelectSingleNode("root").RemoveChild(xmlNode);
					}
				}
				xmlDocument.Save(filename);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static bool RestoreData(string bakFullName)
		{
			bool result = StoreProvider.Instance().RestoreData(bakFullName);
			StoreProvider.Instance().Restor();
			return result;
		}
		public static string UploadLinkImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile))
			{
				result = string.Empty;
			}
			else
			{
				string text = HiContext.Current.GetStoragePath() + "/link/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}
		public static string UploadLogo(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile))
			{
				result = string.Empty;
			}
			else
			{
				string text = HiContext.Current.GetStoragePath() + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}
		public static void DeleteImage(string imageUrl)
		{
			if (!string.IsNullOrEmpty(imageUrl))
			{
				try
				{
					string path = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + imageUrl);
					if (File.Exists(path))
					{
						File.Delete(path);
					}
				}
				catch
				{
				}
			}
		}
	}
}
