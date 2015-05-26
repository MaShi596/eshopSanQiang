using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Validator
{
	public class ValidateTarget : WebControl, INamingContainer
	{
		private ClientValidatorCollection validatorCollection;
		private ArrayList validators;
		private string controlToValidate;
		private bool nullable;
		private string validateGroup = "default";
		private string containerId;
		private string targetClientId;
		[Browsable(false)]
		public string TargetClientId
		{
			get
			{
				return this.targetClientId;
			}
		}
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public ClientValidatorCollection Validators
		{
			get
			{
				if (this.validatorCollection == null)
				{
					this.validators = new ArrayList();
					this.validatorCollection = new ClientValidatorCollection(this, this.validators);
				}
				return this.validatorCollection;
			}
		}
		public bool Nullable
		{
			get
			{
				return this.nullable;
			}
			set
			{
				this.nullable = value;
			}
		}
		public string ControlToValidate
		{
			get
			{
				return this.controlToValidate;
			}
			set
			{
				this.controlToValidate = value;
			}
		}
		public string ValidateGroup
		{
			get
			{
				return this.validateGroup;
			}
			set
			{
				this.validateGroup = value;
			}
		}
		public string ContainerId
		{
			get
			{
				return this.containerId;
			}
			set
			{
				this.containerId = value;
			}
		}
		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (string.IsNullOrEmpty(this.ControlToValidate))
			{
				throw new ArgumentNullException("ControlToValidate");
			}
			WebControl webControl = (WebControl)this.NamingContainer.FindControl(this.ControlToValidate);
			if (webControl != null && this.Validators.Count > 0)
			{
				this.targetClientId = webControl.ClientID;
				if (!(webControl is RadioButtonList))
				{
					webControl.Attributes.Add("ValidateGroup", this.ValidateGroup);
				}
				if (!string.IsNullOrEmpty(this.ContainerId))
				{
					ValidatorContainer validatorContainer = (ValidatorContainer)this.Page.FindControl(this.ContainerId);
					if (validatorContainer == null)
					{
						validatorContainer = this.FindFromMasterPage();
					}
					if (validatorContainer == null)
					{
						throw new Exception(string.Format(CultureInfo.InvariantCulture, "The validator container: '{0}' was not found", new object[]
						{
							this.ContainerId
						}));
					}
					this.CreateToContainer(validatorContainer);
					return;
				}
				else
				{
					this.CreateToChilds();
				}
			}
		}
		private ValidatorContainer FindFromMasterPage()
		{
			Control control = this.NamingContainer;
			ValidatorContainer validatorContainer = (ValidatorContainer)control.FindControl(this.ContainerId);
			while (validatorContainer == null && control.Parent != null)
			{
				control = control.Parent;
				validatorContainer = (ValidatorContainer)control.FindControl(this.ContainerId);
			}
			return validatorContainer;
		}
		private void CreateToChilds()
		{
			this.Validators[0].SetOwner(this);
			this.Controls.Add(this.Validators[0].GenerateInitScript());
			for (int i = 1; i < this.Validators.Count; i++)
			{
				this.Validators[i].SetOwner(this);
				this.Controls.Add(this.Validators[i].GenerateAppendScript());
			}
		}
		private void CreateToContainer(ValidatorContainer container)
		{
			if (container != null)
			{
				this.Validators[0].SetOwner(this);
				container.AddValidatorControl(this.Validators[0].GenerateInitScript());
				for (int i = 1; i < this.Validators.Count; i++)
				{
					this.Validators[i].SetOwner(this);
					container.AddValidatorControl(this.Validators[i].GenerateAppendScript());
				}
			}
		}
		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			writer.WriteLine("<script type=\"text/javascript\" language=\"javascript\">");
		}
		public override void RenderEndTag(HtmlTextWriter writer)
		{
			writer.WriteLine("</script>");
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (this.HasControls())
			{
				this.RenderBeginTag(writer);
				for (int i = 0; i < this.Controls.Count; i++)
				{
					this.Controls[i].RenderControl(writer);
					writer.WriteLine();
				}
				this.RenderEndTag(writer);
			}
		}
	}
}
