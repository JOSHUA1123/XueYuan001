using System;
namespace DataBaseInfo
{
	[Serializable]
	public class FieldValue
	{
		private Field field;
		private object fvalue;
		private bool isIdentity;
		private bool isPrimaryKey;
		private bool isChanged;
		public Field Field
		{
			get
			{
				return this.field;
			}
		}
		public object Value
		{
			get
			{
				return this.fvalue;
			}
			internal set
			{
				this.fvalue = value;
			}
		}
		public bool IsIdentity
		{
			get
			{
				return this.isIdentity;
			}
			set
			{
				this.isIdentity = value;
			}
		}
		public bool IsPrimaryKey
		{
			get
			{
				return this.isPrimaryKey;
			}
			set
			{
				this.isPrimaryKey = value;
			}
		}
		internal bool IsChanged
		{
			get
			{
				return this.isChanged;
			}
			set
			{
				this.isChanged = value;
			}
		}
		public FieldValue(Field field, object value)
		{
			this.field = field;
			this.fvalue = value;
		}
	}
}
