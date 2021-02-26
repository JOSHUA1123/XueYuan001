using Norm.BSON.DbTypes;
using System;
using System.ComponentModel;
namespace Norm
{
	[TypeConverter(typeof(ObjectIdTypeConverter))]
	public class ObjectId
	{
		private string _string;
		public static ObjectId Empty
		{
			get
			{
				return new ObjectId("000000000000000000000000");
			}
		}
		public byte[] Value
		{
			get;
			private set;
		}
		public ObjectId()
		{
		}
		public ObjectId(string value) : this(ObjectId.DecodeHex(value))
		{
		}
		internal ObjectId(byte[] value)
		{
			this.Value = value;
		}
		public static ObjectId NewObjectId()
		{
			return new ObjectId
			{
				Value = ObjectIdGenerator.Generate()
			};
		}
		public static bool TryParse(string value, out ObjectId id)
		{
			id = ObjectId.Empty;
			if (value == null || value.Length != 24)
			{
				return false;
			}
			bool result;
			try
			{
				id = new ObjectId(value);
				result = true;
			}
			catch (FormatException)
			{
				result = false;
			}
			return result;
		}
		public static bool operator ==(ObjectId a, ObjectId b)
		{
			return object.ReferenceEquals(a, b) || (a != null && b != null && a.Equals(b));
		}
		public static bool operator !=(ObjectId a, ObjectId b)
		{
			return !(a == b);
		}
		public override int GetHashCode()
		{
			if (this.Value == null)
			{
				return 0;
			}
			return this.ToString().GetHashCode();
		}
		public override string ToString()
		{
			if (this._string == null && this.Value != null)
			{
				this._string = BitConverter.ToString(this.Value).Replace("-", string.Empty).ToLower();
			}
			return this._string;
		}
		public override bool Equals(object o)
		{
			ObjectId other = o as ObjectId;
			return this.Equals(other);
		}
		public bool Equals(ObjectId other)
		{
			return other != null && this.ToString() == other.ToString();
		}
		protected static byte[] DecodeHex(string val)
		{
			char[] array = val.ToCharArray();
			int num = array.Length;
			byte[] array2 = new byte[num / 2];
			for (int i = 0; i < num; i += 2)
			{
				array2[i / 2] = Convert.ToByte(new string(array, i, 2), 16);
			}
			return array2;
		}
		public static implicit operator string(ObjectId oid)
		{
			if (!(oid == null))
			{
				return oid.ToString();
			}
			return null;
		}
		public static implicit operator ObjectId(string oidString)
		{
			ObjectId result = ObjectId.Empty;
			if (!string.IsNullOrEmpty(oidString))
			{
				result = new ObjectId(oidString);
			}
			return result;
		}
	}
}
