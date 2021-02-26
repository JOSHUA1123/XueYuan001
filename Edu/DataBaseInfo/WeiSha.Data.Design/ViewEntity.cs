using System;
namespace DataBaseInfo.Design
{
	[Serializable]
	public sealed class ViewEntity : Entity
	{
		protected internal override Field[] GetFields()
		{
			return new Field[0];
		}
		protected override object[] GetValues()
		{
			return new object[0];
		}
		protected override void SetValues(IRowReader reader)
		{
		}
	}
}
