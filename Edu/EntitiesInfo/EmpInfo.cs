namespace EntitiesInfo {
    	using System;
    	
    	
    	/// <summary>
    	/// 表名：EmpInfo 主键列：Emp_Id
    	/// </summary>
    	[SerializableAttribute()]
    	public partial class EmpInfo : DataBaseInfo.Entity {
    		
    		protected Int32 _Emp_Id;
    		
    		protected Int32? _Acc_Id;
    		
    		protected Int32? _Org_Id;
    		
    		protected String _Org_Name;
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32 Emp_Id {
    			get {
    				return this._Emp_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Emp_Id, _Emp_Id, value);
    				this._Emp_Id = value;
    			}
    		}
    		
    		/// <summary>
    		/// False
    		/// </summary>
    		public Int32? Acc_Id {
    			get {
    				return this._Acc_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Acc_Id, _Acc_Id, value);
    				this._Acc_Id = value;
    			}
    		}
    		
    		public Int32? Org_Id {
    			get {
    				return this._Org_Id;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Id, _Org_Id, value);
    				this._Org_Id = value;
    			}
    		}
    		
    		public String Org_Name {
    			get {
    				return this._Org_Name;
    			}
    			set {
    				this.OnPropertyValueChange(_.Org_Name, _Org_Name, value);
    				this._Org_Name = value;
    			}
    		}
    		
    		/// <summary>
    		/// 获取实体对应的表名
    		/// </summary>
    		protected override DataBaseInfo.Table GetTable() {
    			return new DataBaseInfo.Table<EmpInfo>("EmpInfo");
    		}
    		
    		/// <summary>
    		/// 获取实体中的标识列
    		/// </summary>
    		protected override DataBaseInfo.Field GetIdentityField() {
    			return _.Emp_Id;
    		}
    		
    		/// <summary>
    		/// 获取实体中的主键列
    		/// </summary>
    		protected override DataBaseInfo.Field[] GetPrimaryKeyFields() {
    			return new DataBaseInfo.Field[] {
    					_.Emp_Id};
    		}
    		
    		/// <summary>
    		/// 获取列信息
    		/// </summary>
    		protected override DataBaseInfo.Field[] GetFields() {
    			return new DataBaseInfo.Field[] {
    					_.Emp_Id,
    					_.Acc_Id,
    					_.Org_Id,
    					_.Org_Name};
    		}
    		
    		/// <summary>
    		/// 获取列数据
    		/// </summary>
    		protected override object[] GetValues() {
    			return new object[] {
    					this._Emp_Id,
    					this._Acc_Id,
    					this._Org_Id,
    					this._Org_Name};
    		}
    		
    		/// <summary>
    		/// 给当前实体赋值
    		/// </summary>
    		protected override void SetValues(DataBaseInfo.IRowReader reader) {
    			if ((false == reader.IsDBNull(_.Emp_Id))) {
    				this._Emp_Id = reader.GetInt32(_.Emp_Id);
    			}
    			if ((false == reader.IsDBNull(_.Acc_Id))) {
    				this._Acc_Id = reader.GetInt32(_.Acc_Id);
    			}
    			if ((false == reader.IsDBNull(_.Org_Id))) {
    				this._Org_Id = reader.GetInt32(_.Org_Id);
    			}
    			if ((false == reader.IsDBNull(_.Org_Name))) {
    				this._Org_Name = reader.GetString(_.Org_Name);
    			}
    		}
    		
    		public override int GetHashCode() {
    			return base.GetHashCode();
    		}
    		
    		public override bool Equals(object obj) {
    			if ((obj == null)) {
    				return false;
    			}
    			if ((false == typeof(EmpInfo).IsAssignableFrom(obj.GetType()))) {
    				return false;
    			}
    			if ((((object)(this)) == ((object)(obj)))) {
    				return true;
    			}
    			return false;
    		}
    		
    		public class _ {
    			
    			/// <summary>
    			/// 表示选择所有列，与*等同
    			/// </summary>
    			public static DataBaseInfo.AllField All = new DataBaseInfo.AllField<EmpInfo>();
    			
    			/// <summary>
    			/// False - 字段名：Emp_Id - 数据类型：Int32
    			/// </summary>
    			public static DataBaseInfo.Field Emp_Id = new DataBaseInfo.Field<EmpInfo>("Emp_Id");
    			
    			/// <summary>
    			/// False - 字段名：Acc_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static DataBaseInfo.Field Acc_Id = new DataBaseInfo.Field<EmpInfo>("Acc_Id");
    			
    			/// <summary>
    			/// 字段名：Org_Id - 数据类型：Int32(可空)
    			/// </summary>
    			public static DataBaseInfo.Field Org_Id = new DataBaseInfo.Field<EmpInfo>("Org_Id");
    			
    			/// <summary>
    			/// 字段名：Org_Name - 数据类型：String
    			/// </summary>
    			public static DataBaseInfo.Field Org_Name = new DataBaseInfo.Field<EmpInfo>("Org_Name");
    		}
    	}
    }
    