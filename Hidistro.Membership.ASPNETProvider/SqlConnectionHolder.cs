using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Hosting;
namespace Hidistro.Membership.ASPNETProvider
{
	internal sealed class SqlConnectionHolder
	{
		internal SqlConnection _Connection;
		private bool _Opened;
		internal SqlConnection Connection
		{
			get
			{
				return this._Connection;
			}
		}
		internal SqlConnectionHolder(string connectionString)
		{
			try
			{
				this._Connection = new SqlConnection(connectionString);
			}
			catch (ArgumentException innerException)
			{
				throw new ArgumentException(SR.GetString("An error occurred while attempting to initialize a System.Data.SqlClient.SqlConnection object. The value that was provided for the connection string may be wrong, or it may contain an invalid syntax."), "connectionString", innerException);
			}
		}
		internal void Open(HttpContext context, bool revertImpersonate)
		{
			if (this._Opened)
			{
				return;
			}
			if (revertImpersonate)
			{
				using (HostingEnvironment.Impersonate())
				{
					this.Connection.Open();
					goto IL_34;
				}
			}
			this.Connection.Open();
			IL_34:
			this._Opened = true;
		}
		internal void Close()
		{
			if (!this._Opened)
			{
				return;
			}
			this.Connection.Close();
			this._Opened = false;
		}
	}
}
