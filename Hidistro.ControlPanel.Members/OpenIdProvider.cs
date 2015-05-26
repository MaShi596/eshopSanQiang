using Hidistro.Core;
using Hidistro.Entities.Members;
using System;
using System.Collections.Generic;
namespace Hidistro.ControlPanel.Members
{
	public abstract class OpenIdProvider
	{
		private static readonly OpenIdProvider _defaultInstance;
		static OpenIdProvider()
		{
			OpenIdProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.ControlPanel.Data.OpenIdData,Hidistro.ControlPanel.Data") as OpenIdProvider);
		}
		public static OpenIdProvider Instance()
		{
			return OpenIdProvider._defaultInstance;
		}
		public abstract void SaveSettings(OpenIdSettingsInfo settings);
		public abstract void DeleteSettings(string openIdType);
		public abstract OpenIdSettingsInfo GetOpenIdSettings(string openIdType);
		public abstract IList<string> GetConfigedTypes();
	}
}
