using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Common
{
	// Token: 0x02000024 RID: 36
	public class Data
	{
		/// <summary>
		/// 将实体的值绑定到控件
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="entity"></param>
		// Token: 0x060000CC RID: 204 RVA: 0x000095EC File Offset: 0x000077EC
		public static void EntityBind(Panel panel, object entity)
		{
			if (entity == null)
			{
				return;
			}
			Data.EntityBind(panel, entity);
		}

		/// <summary>
		/// 递归设置控件的值
		/// </summary>
		/// <param name="control"></param>
		/// <returns></returns>
		// Token: 0x060000CD RID: 205 RVA: 0x0000FD58 File Offset: 0x0000DF58
		public static void EntityBind(Control control, object entity)
		{
			foreach (object obj in control.Controls)
			{
				Control control2 = (Control)obj;
				if (!string.IsNullOrWhiteSpace(control2.ID))
				{
					Data._entityBindSingle(control2, entity);
				}
			}
			foreach (object obj2 in control.Controls)
			{
				Control control3 = (Control)obj2;
				Data.EntityBind(control3, entity);
			}
		}

		/// <summary>
		/// 向单个控件绑定
		/// </summary>
		/// <param name="control"></param>
		/// <param name="entity"></param>
		// Token: 0x060000CE RID: 206 RVA: 0x0000FE10 File Offset: 0x0000E010
		private static void _entityBindSingle(Control c, object entity)
		{
			Type type = entity.GetType();
			PropertyInfo[] properties = type.GetProperties();
			int i = 0;
			while (i < properties.Length)
			{
				PropertyInfo propertyInfo = properties[i];
				if (c.ID.Equals(propertyInfo.Name, StringComparison.CurrentCultureIgnoreCase))
				{
					object value = type.GetProperty(propertyInfo.Name).GetValue(entity, null);
					if (value != null)
					{
						Data._controlBindFunc(c, value);
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}

		/// <summary>
		/// 向控件赋值
		/// </summary>
		/// <param name="control"></param>
		/// <param name="value"></param>
		// Token: 0x060000CF RID: 207 RVA: 0x0000FE74 File Offset: 0x0000E074
		private static void _controlBindFunc(Control c, object value)
		{
			if (c is DropDownList || c is CheckBoxList || c is RadioButtonList)
			{
				ListControl listControl = c as ListControl;
				listControl.SelectedIndex = listControl.Items.IndexOf(listControl.Items.FindByValue(value.ToString()));
			}
			if (c is TextBox || c is Label || c is Literal)
			{
				ITextControl textControl = c as ITextControl;
				if (value == null)
				{
					return;
				}
				if (c is Literal)
				{
					textControl.Text = value.ToString();
					return;
				}
				WebControl webControl = c as WebControl;
				string text = (webControl.Attributes["Formate"] == null) ? null : webControl.Attributes["Formate"];
				if (text == null)
				{
					textControl.Text = value.ToString();
					return;
				}
				if (value is DateTime)
				{
					textControl.Text = Convert.ToDateTime(value).ToString(text);
				}
				if (value is int)
				{
					textControl.Text = Convert.ToInt32(value).ToString(text);
				}
				if (value is float)
				{
					textControl.Text = Convert.ToSingle(value).ToString(text);
				}
				if (value is double)
				{
					textControl.Text = Convert.ToDouble(value).ToString(text);
				}
			}
			if (c is CheckBox || c is RadioButton)
			{
				(c as CheckBox).Checked = Convert.ToBoolean(value);
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000FFD4 File Offset: 0x0000E1D4
		public static object EntityFill(Control control, object entity)
		{
			foreach (object obj in control.Controls)
			{
				Control control2 = (Control)obj;
				if (!string.IsNullOrWhiteSpace(control2.ID))
				{
					Type type = entity.GetType();
					foreach (PropertyInfo propertyInfo in type.GetProperties())
					{
						if (propertyInfo.Name == control2.ID)
						{
							entity = Data._entityFillSingle(control2, entity, propertyInfo.Name);
							break;
						}
					}
				}
			}
			foreach (object obj2 in control.Controls)
			{
				Control control3 = (Control)obj2;
				entity = Data.EntityFill(control3, entity);
			}
			return entity;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000100D8 File Offset: 0x0000E2D8
		private static object _entityFillSingle(Control c, object entity, string piName)
		{
			string value = "";
			if (c is DropDownList || c is CheckBoxList || c is RadioButtonList)
			{
				ListControl listControl = c as ListControl;
				value = listControl.SelectedValue;
			}
			if (c is TextBox)
			{
				TextBox textBox = c as TextBox;
				value = textBox.Text.Trim();
			}
			if (c is CheckBox || c is RadioButton)
			{
				CheckBox checkBox = c as CheckBox;
				value = checkBox.Checked.ToString();
			}
			PropertyInfo property = entity.GetType().GetProperty(piName);
			object value2 = string.IsNullOrEmpty(value) ? null : DataConvert.ChangeType(value, property.PropertyType);
			property.SetValue(entity, value2, null);
			return entity;
		}
	}
}
