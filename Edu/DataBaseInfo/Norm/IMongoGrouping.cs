using System;
namespace Norm
{
	public interface IMongoGrouping<K, V>
	{
		K Key
		{
			get;
			set;
		}
		V Value
		{
			get;
			set;
		}
	}
}
