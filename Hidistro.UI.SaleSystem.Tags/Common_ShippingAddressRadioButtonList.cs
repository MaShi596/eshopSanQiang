using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Member;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ShippingAddressRadioButtonList : RadioButtonList
	{
		public const string TagID = "Common_ShippingAddressesRadioButtonList";
		public new int SelectedValue
		{
			get
			{
				return int.Parse(base.SelectedValue);
			}
			set
			{
				base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString()));
			}
		}
		public Common_ShippingAddressRadioButtonList()
		{
			base.ID = "Common_ShippingAddressesRadioButtonList";
		}
		public override void DataBind()
		{
			IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(HiContext.Current.User.UserId);
			foreach (ShippingAddressInfo current in shippingAddresses)
			{
				this.Items.Add(new ListItem(current.Address + "(收货人：" + current.ShipTo + ")", current.ShippingId.ToString()));
			}
		}
	}
}
