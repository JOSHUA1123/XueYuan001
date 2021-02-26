using System;
using System.Collections.Generic;
namespace DataBaseInfo
{
	[Serializable]
	public class WhereClip
	{
		public static readonly WhereClip None = new WhereClip(null, new SQLParameter[0]);
		private List<SQLParameter> plist = new List<SQLParameter>();
		private string where;
		public string Value
		{
			get
			{
				return this.where;
			}
			set
			{
				this.where = value;
			}
		}
		public SQLParameter[] Parameters
		{
			get
			{
				return this.plist.ToArray();
			}
			set
			{
				this.plist = new List<SQLParameter>(value);
			}
		}
		public WhereClip()
		{
		}
		public WhereClip(string where, params SQLParameter[] parameters)
		{
			this.where = where;
			if (parameters != null && parameters.Length > 0)
			{
				this.plist.AddRange(parameters);
			}
		}
		public WhereClip(string where, IDictionary<string, object> parameters) : this(where, new SQLParameter[0])
		{
			if (parameters != null && parameters.Count > 0)
			{
				foreach (KeyValuePair<string, object> current in parameters)
				{
					this.plist.Add(new SQLParameter(current.Key, current.Value));
				}
			}
		}
		public static bool operator true(WhereClip right)
		{
			return false;
		}
		public static bool operator false(WhereClip right)
		{
			return false;
		}
		public static WhereClip operator !(WhereClip where)
		{
			if (DataHelper.IsNullOrEmpty(where))
			{
				return null;
			}
			return new WhereClip(" not (" + where.ToString() + ")", where.Parameters);
		}
		public static WhereClip operator &(WhereClip leftWhere, WhereClip rightWhere)
		{
			if (DataHelper.IsNullOrEmpty(leftWhere) && DataHelper.IsNullOrEmpty(rightWhere))
			{
				return WhereClip.None;
			}
			if (DataHelper.IsNullOrEmpty(leftWhere))
			{
				return rightWhere;
			}
			if (DataHelper.IsNullOrEmpty(rightWhere))
			{
				return leftWhere;
			}
			List<SQLParameter> list = new List<SQLParameter>();
			list.AddRange(leftWhere.Parameters);
			list.AddRange(rightWhere.Parameters);
			return new WhereClip(string.Concat(new string[]
			{
				"(",
				leftWhere.ToString(),
				" and ",
				rightWhere.ToString(),
				")"
			}), list.ToArray());
		}
		public static WhereClip operator |(WhereClip leftWhere, WhereClip rightWhere)
		{
			if (DataHelper.IsNullOrEmpty(leftWhere) && DataHelper.IsNullOrEmpty(rightWhere))
			{
				return WhereClip.None;
			}
			if (DataHelper.IsNullOrEmpty(leftWhere))
			{
				return rightWhere;
			}
			if (DataHelper.IsNullOrEmpty(rightWhere))
			{
				return leftWhere;
			}
			List<SQLParameter> list = new List<SQLParameter>();
			list.AddRange(leftWhere.Parameters);
			list.AddRange(rightWhere.Parameters);
			return new WhereClip(string.Concat(new string[]
			{
				"(",
				leftWhere.ToString(),
				" or ",
				rightWhere.ToString(),
				")"
			}), list.ToArray());
		}
		public WhereClip And(WhereClip rightWhere)
		{
			if (DataHelper.IsNullOrEmpty(this) && DataHelper.IsNullOrEmpty(rightWhere))
			{
				return this;
			}
			if (DataHelper.IsNullOrEmpty(rightWhere))
			{
				return this;
			}
			if (DataHelper.IsNullOrEmpty(this))
			{
				this.plist.AddRange(rightWhere.Parameters);
				this.where = rightWhere.ToString();
				return this;
			}
			this.plist.AddRange(rightWhere.Parameters);
			this.where = string.Concat(new string[]
			{
				"(",
				this.ToString(),
				" and ",
				rightWhere.ToString(),
				")"
			});
			return this;
		}
		public WhereClip Or(WhereClip rightWhere)
		{
			if (DataHelper.IsNullOrEmpty(this) && DataHelper.IsNullOrEmpty(rightWhere))
			{
				return this;
			}
			if (DataHelper.IsNullOrEmpty(rightWhere))
			{
				return this;
			}
			if (DataHelper.IsNullOrEmpty(this))
			{
				this.plist.AddRange(rightWhere.Parameters);
				this.where = rightWhere.ToString();
				return this;
			}
			this.plist.AddRange(rightWhere.Parameters);
			this.where = string.Concat(new string[]
			{
				"(",
				this.ToString(),
				" or ",
				rightWhere.ToString(),
				")"
			});
			return this;
		}
		public WhereClip Not()
		{
			if (DataHelper.IsNullOrEmpty(this))
			{
				return this;
			}
			this.where = " not (" + this.ToString() + ")";
			return this;
		}
		public override string ToString()
		{
			return this.where;
		}
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
