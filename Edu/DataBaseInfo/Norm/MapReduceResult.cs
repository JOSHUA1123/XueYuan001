using System;
namespace Norm
{
	public class MapReduceResult<K, V>
	{
		public K _id
		{
			get;
			set;
		}
		public K Key
		{
			get
			{
				return this._id;
			}
		}
		public V Value
		{
			get;
			set;
		}
	}
}
