using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using System;
using System.Collections;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.ControlPanel.Utility
{
	public class RoleDropDownList : DropDownList
	{
		private bool allowNull = true;
		private string nullToDisplay = "";
		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				this.allowNull = value;
			}
		}
		public string NullToDisplay
		{
			get
			{
				return this.nullToDisplay;
			}
			set
			{
				this.nullToDisplay = value;
			}
		}
		public new Guid SelectedValue
		{
			get
			{
				Guid empty = Guid.Empty;
				if (base.SelectedValue.Length == 36)
				{
					empty = new Guid(base.SelectedValue);
				}
				return empty;
			}
			set
			{
				base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString()));
			}
		}
		public override void DataBind()
		{
			this.Items.Clear();
			ArrayList arrayList = new ArrayList();
			arrayList = RoleHelper.GetRoles();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			foreach (RoleInfo roleInfo in arrayList)
			{
				if (!this.IsDefaultRole(roleInfo.Name))
				{
					if (string.Compare(roleInfo.Name, RoleHelper.SystemAdministrator, true, CultureInfo.InvariantCulture) == 0)
					{
						base.Items.Add(new ListItem("超级管理员", roleInfo.RoleID.ToString()));
					}
					else
					{
						base.Items.Add(new ListItem(roleInfo.Name, roleInfo.RoleID.ToString()));
					}
				}
			}
		}
		private bool IsDefaultRole(string roleName)
		{
			return string.Compare(roleName, Hidistro.Membership.Context.HiContext.Current.Config.RolesConfiguration.Manager, true, CultureInfo.InvariantCulture) == 0 || string.Compare(roleName, Hidistro.Membership.Context.HiContext.Current.Config.RolesConfiguration.Member, true, CultureInfo.InvariantCulture) == 0 || string.Compare(roleName, Hidistro.Membership.Context.HiContext.Current.Config.RolesConfiguration.Distributor, true, CultureInfo.InvariantCulture) == 0 || string.Compare(roleName, Hidistro.Membership.Context.HiContext.Current.Config.RolesConfiguration.Underling, true, CultureInfo.InvariantCulture) == 0;
		}
	}
}
