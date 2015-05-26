using Hidistro.AccountCenter.Profile;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class UserShippingAddresses : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtShipTo;
		private System.Web.UI.WebControls.TextBox txtAddress;
		private System.Web.UI.WebControls.TextBox txtZipcode;
		private System.Web.UI.WebControls.TextBox txtTelPhone;
		private System.Web.UI.WebControls.TextBox txtCellPhone;
		private RegionSelector dropRegionsSelect;
		private IButton btnCancel;
		private IButton btnAddAddress;
		private IButton btnEditAddress;
		private System.Web.UI.WebControls.Literal lblAddressCount;
		private Common_Address_AddressList dtlstRegionsSelect;
		private static int shippingId = 0;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserShippingAddresses.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.txtShipTo = (System.Web.UI.WebControls.TextBox)this.FindControl("txtShipTo");
			this.txtAddress = (System.Web.UI.WebControls.TextBox)this.FindControl("txtAddress");
			this.txtZipcode = (System.Web.UI.WebControls.TextBox)this.FindControl("txtZipcode");
			this.txtTelPhone = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTelPhone");
			this.txtCellPhone = (System.Web.UI.WebControls.TextBox)this.FindControl("txtCellPhone");
			this.dropRegionsSelect = (RegionSelector)this.FindControl("dropRegions");
			this.btnAddAddress = ButtonManager.Create(this.FindControl("btnAddAddress"));
			this.btnCancel = ButtonManager.Create(this.FindControl("btnCancel"));
			this.btnEditAddress = ButtonManager.Create(this.FindControl("btnEditAddress"));
			this.dtlstRegionsSelect = (Common_Address_AddressList)this.FindControl("list_Common_Consignee_ConsigneeList");
			this.lblAddressCount = (System.Web.UI.WebControls.Literal)this.FindControl("lblAddressCount");
			this.btnAddAddress.Click += new System.EventHandler(this.btnAddAddress_Click);
			this.btnEditAddress.Click += new System.EventHandler(this.btnEditAddress_Click);
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.dtlstRegionsSelect.ItemCommand += new Common_Address_AddressList.CommandEventHandler(this.dtlstRegionsSelect_ItemCommand);
			PageTitle.AddSiteNameTitle("我的收货地址", HiContext.Current.Context);
			if (!this.Page.IsPostBack)
			{
				this.lblAddressCount.Text = HiContext.Current.Config.ShippingAddressQuantity.ToString();
				this.dropRegionsSelect.DataBind();
				this.Reset();
				this.btnEditAddress.Visible = false;
				this.BindList();
			}
		}
		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Reset();
		}
		protected void btnEditAddress_Click(object sender, System.EventArgs e)
		{
			if (this.ValShippingAddress())
			{
				ShippingAddressInfo shippingAddressInfo = this.GetShippingAddressInfo();
				shippingAddressInfo.ShippingId = System.Convert.ToInt32(this.ViewState["shippingId"]);
				if (PersonalHelper.UpdateShippingAddress(shippingAddressInfo))
				{
					this.ShowMessage("成功的修改了一个收货地址", true);
					this.Reset();
				}
				else
				{
					this.ShowMessage("地址已经在，请重新输入一次再试", false);
				}
				this.btnEditAddress.Visible = false;
				this.btnAddAddress.Visible = true;
				this.BindList();
			}
		}
		private bool ValShippingAddress()
		{
			Regex regex = new Regex("[\\u4e00-\\u9fa5a-zA-Z]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*");
			bool result;
			if (string.IsNullOrEmpty(this.txtShipTo.Text.Trim()) || !regex.IsMatch(this.txtShipTo.Text.Trim()))
			{
				this.ShowMessage("收货人名字不能为空，只能是汉字或字母开头，长度在2-20个字符之间", false);
				result = false;
			}
			else
			{
				if (string.IsNullOrEmpty(this.txtAddress.Text.Trim()))
				{
					this.ShowMessage("详细地址不能为空", false);
					result = false;
				}
				else
				{
					if (this.txtAddress.Text.Trim().Length < 3 || this.txtAddress.Text.Trim().Length > 60)
					{
						this.ShowMessage("详细地址长度在3-60个字符之间", false);
						result = false;
					}
					else
					{
						if (!this.dropRegionsSelect.GetSelectedRegionId().HasValue || this.dropRegionsSelect.GetSelectedRegionId().Value == 0)
						{
							this.ShowMessage("请选择收货地址", false);
							result = false;
						}
						else
						{
							if (string.IsNullOrEmpty(this.txtTelPhone.Text.Trim()) && string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()))
							{
								this.ShowMessage("电话号码和手机二者必填其一", false);
								this.Reset();
								result = false;
							}
							else
							{
								if (!string.IsNullOrEmpty(this.txtTelPhone.Text.Trim()) && (this.txtTelPhone.Text.Trim().Length < 3 || this.txtTelPhone.Text.Trim().Length > 20))
								{
									this.ShowMessage("电话号码长度限制在3-20个字符之间", false);
									result = false;
								}
								else
								{
									if (!string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()) && (this.txtCellPhone.Text.Trim().Length < 3 || this.txtCellPhone.Text.Trim().Length > 20))
									{
										this.ShowMessage("手机号码长度限制在3-20个字符之间", false);
										result = false;
									}
									else
									{
										result = true;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}
		protected void btnAddAddress_Click(object sender, System.EventArgs e)
		{
			if (this.ValShippingAddress())
			{
				if (PersonalHelper.GetShippingAddressCount(HiContext.Current.User.UserId) >= HiContext.Current.Config.ShippingAddressQuantity)
				{
					this.ShowMessage(string.Format("最多只能添加{0}个收货地址", HiContext.Current.Config.ShippingAddressQuantity), false);
					this.Reset();
				}
				else
				{
					ShippingAddressInfo shippingAddressInfo = this.GetShippingAddressInfo();
					if (PersonalHelper.CreateShippingAddress(shippingAddressInfo))
					{
						this.ShowMessage("成功的添加了一个收货地址", true);
						this.Reset();
					}
					else
					{
						this.ShowMessage("地址已经在，请重新输入一次再试", false);
					}
					this.BindList();
				}
			}
		}
		protected void dtlstRegionsSelect_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			UserShippingAddresses.shippingId = (int)this.dtlstRegionsSelect.DataKeys[e.Item.ItemIndex];
			this.ViewState["shippingId"] = UserShippingAddresses.shippingId;
			if (e.CommandName == "Edit")
			{
				ShippingAddressInfo userShippingAddress = PersonalHelper.GetUserShippingAddress(UserShippingAddresses.shippingId);
				if (userShippingAddress != null)
				{
					this.txtShipTo.Text = userShippingAddress.ShipTo;
					this.dropRegionsSelect.SetSelectedRegionId(new int?(userShippingAddress.RegionId));
					this.txtAddress.Text = userShippingAddress.Address;
					this.txtZipcode.Text = userShippingAddress.Zipcode;
					this.txtTelPhone.Text = userShippingAddress.TelPhone;
					this.txtCellPhone.Text = userShippingAddress.CellPhone;
					this.btnCancel.Visible = true;
					this.btnAddAddress.Visible = false;
					this.btnEditAddress.Visible = true;
				}
			}
			if (e.CommandName == "Delete")
			{
				if (PersonalHelper.DeleteShippingAddress(UserShippingAddresses.shippingId))
				{
					this.ShowMessage("成功的删除了你要删除的记录", true);
					this.BindList();
				}
				else
				{
					this.ShowMessage("删除失败", false);
				}
				UserShippingAddresses.shippingId = 0;
			}
		}
		private ShippingAddressInfo GetShippingAddressInfo()
		{
			return new ShippingAddressInfo
			{
				UserId = HiContext.Current.User.UserId,
				ShipTo = this.txtShipTo.Text,
				RegionId = this.dropRegionsSelect.GetSelectedRegionId().Value,
				Address = this.txtAddress.Text,
				Zipcode = this.txtZipcode.Text,
				CellPhone = this.txtCellPhone.Text,
				TelPhone = this.txtTelPhone.Text
			};
		}
		private void BindList()
		{
			System.Collections.Generic.IList<ShippingAddressInfo> shippingAddress = PersonalHelper.GetShippingAddress(HiContext.Current.User.UserId);
			this.dtlstRegionsSelect.DataSource = shippingAddress;
			this.dtlstRegionsSelect.DataBind();
		}
		private void Reset()
		{
			this.txtShipTo.Text = string.Empty;
			this.dropRegionsSelect.SetSelectedRegionId(null);
			this.txtAddress.Text = string.Empty;
			this.txtZipcode.Text = string.Empty;
			this.txtTelPhone.Text = string.Empty;
			this.txtCellPhone.Text = string.Empty;
			UserShippingAddresses.shippingId = 0;
			this.btnAddAddress.Visible = true;
		}
	}
}
