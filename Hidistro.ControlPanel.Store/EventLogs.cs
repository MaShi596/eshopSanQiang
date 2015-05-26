using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
namespace Hidistro.ControlPanel.Store
{
	public static class EventLogs
	{
		public static void WriteOperationLog(Privilege privilege, string description)
		{
			OperationLogEntry entry = new OperationLogEntry
			{
				AddedTime = DateTime.Now,
				Privilege = privilege,
				Description = description,
				IpAddress = Globals.IPAddress,
				PageUrl = HiContext.Current.Context.Request.RawUrl,
				UserName = HiContext.Current.Context.User.Identity.Name
			};
			StoreProvider.Instance().WriteOperationLogEntry(entry);
		}
		public static int DeleteLogs(string strIds)
		{
			return StoreProvider.Instance().DeleteLogs(strIds);
		}
		public static bool DeleteLog(long logId)
		{
			return StoreProvider.Instance().DeleteLog(logId);
		}
		public static bool DeleteAllLogs()
		{
			return StoreProvider.Instance().DeleteAllLogs();
		}
		public static DbQueryResult GetLogs(OperationLogQuery query)
		{
			return StoreProvider.Instance().GetLogs(query);
		}
		public static IList<string> GetOperationUseNames()
		{
			return StoreProvider.Instance().GetOperationUserNames();
		}
	}
}
