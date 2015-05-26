using System;
using System.Runtime.CompilerServices;
namespace Hidistro.Membership.Context
{
	public class UserEventArgs : System.EventArgs
	{
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_0;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_1;
		[System.Runtime.CompilerServices.CompilerGenerated]
		private string string_2;
		public string DealPassword
		{
			get;
			set;
		}
		public string Password
		{
			get;
			set;
		}
		public string Username
		{
			get;
			set;
		}
		public UserEventArgs(string username, string password, string dealPassword)
		{
			this.Username = username;
			this.Password = password;
			this.DealPassword = dealPassword;
		}
	}
}
