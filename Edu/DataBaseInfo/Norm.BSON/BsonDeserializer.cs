using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
namespace Norm.BSON
{
	public class BsonDeserializer : BsonSerializerBase
	{
		private static readonly Type _IEnumerableType = typeof(IEnumerable);
		private static readonly Type _IDictionaryType = typeof(IDictionary<, >);
		private static readonly IDictionary<BSONTypes, Type> _typeMap = new Dictionary<BSONTypes, Type>
		{

			{
				BSONTypes.Int32,
				typeof(int)
			},

			{
				BSONTypes.Int64,
				typeof(long)
			},

			{
				BSONTypes.Boolean,
				typeof(bool)
			},

			{
				BSONTypes.String,
				typeof(string)
			},

			{
				BSONTypes.Double,
				typeof(double)
			},

			{
				BSONTypes.Binary,
				typeof(byte[])
			},

			{
				BSONTypes.Regex,
				typeof(Regex)
			},

			{
				BSONTypes.DateTime,
				typeof(DateTime)
			},

			{
				BSONTypes.MongoOID,
				typeof(ObjectId)
			}
		};
		private readonly BinaryReader _reader;
		private Document _current;
		private BsonDeserializer(BinaryReader reader)
		{
			this._reader = reader;
		}
		public static T Deserialize<T>(byte[] objectData) where T : class
		{
			IDictionary<WeakReference, Expando> dictionary = new Dictionary<WeakReference, Expando>();
			return BsonDeserializer.Deserialize<T>(objectData, ref dictionary);
		}
		public static T Deserialize<T>(byte[] objectData, ref IDictionary<WeakReference, Expando> outProps)
		{
			T result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.Write(objectData, 0, objectData.Length);
				memoryStream.Position = 0L;
				result = BsonDeserializer.Deserialize<T>(new BinaryReader(memoryStream));
			}
			return result;
		}
		public static T Deserialize<T>(int length, BinaryReader reader, ref IDictionary<WeakReference, Expando> outProps)
		{
			return BsonDeserializer.Deserialize<T>(reader, length);
		}
		private static T Deserialize<T>(BinaryReader stream)
		{
			return BsonDeserializer.Deserialize<T>(stream, stream.ReadInt32());
		}
		private static T Deserialize<T>(BinaryReader stream, int length)
		{
			BsonDeserializer bsonDeserializer = new BsonDeserializer(stream);
			T result = default(T);
			try
			{
				result = bsonDeserializer.Read<T>(length);
			}
			catch (Exception ex)
			{
				int count = bsonDeserializer._current.Length - bsonDeserializer._current.Digested;
				bsonDeserializer._reader.ReadBytes(count);
				throw ex;
			}
			return result;
		}
		private T Read<T>(int length)
		{
			this.NewDocument(length);
			return (T)((object)this.DeserializeValue(typeof(T), BSONTypes.Object));
		}
		private void Read(int read)
		{
			this._current.Digested += read;
		}
		private bool IsDone()
		{
			bool flag = this._current.Digested + 1 == this._current.Length;
			if (flag)
			{
				this._reader.ReadByte();
				Document current = this._current;
				this._current = current.Parent;
				if (this._current != null)
				{
					this.Read(current.Length);
				}
			}
			return flag;
		}
		private void NewDocument(int length)
		{
			Document current = this._current;
			this._current = new Document
			{
				Length = length,
				Parent = current,
				Digested = 4
			};
		}
		private object DeserializeValue(Type type, BSONTypes storedType)
		{
			return this.DeserializeValue(type, storedType, null);
		}
		private object DeserializeValue(Type type, BSONTypes storedType, object container)
		{
			IBsonTypeConverter typeConverterFor = BsonSerializerBase.Configuration.GetTypeConverterFor(type);
			if (typeConverterFor != null)
			{
				Type serializedType = typeConverterFor.SerializedType;
				object data = this.DeserializeValueAfterConversion(serializedType, storedType, container);
				return typeConverterFor.ConvertFromBson(data);
			}
			return this.DeserializeValueAfterConversion(type, storedType, container);
		}
		private object DeserializeValueAfterConversion(Type type, BSONTypes storedType, object container)
		{
			if (storedType == BSONTypes.Null)
			{
				return null;
			}
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = Nullable.GetUnderlyingType(type);
			}
			if (type == typeof(string))
			{
				return this.ReadString();
			}
			if (type == typeof(int))
			{
				return this.ReadInt(storedType);
			}
			if (type.IsEnum)
			{
				return this.ReadEnum(type, storedType);
			}
			if (type == typeof(float))
			{
				this.Read(8);
				return (float)this._reader.ReadDouble();
			}
			if (storedType == BSONTypes.Binary)
			{
				return this.ReadBinary();
			}
			if (BsonDeserializer._IEnumerableType.IsAssignableFrom(type) || storedType == BSONTypes.Array)
			{
				return this.ReadList(type, container);
			}
			if (type == typeof(bool))
			{
				this.Read(1);
				return this._reader.ReadBoolean();
			}
			if (type == typeof(DateTime))
			{
				return BsonHelper.EPOCH.AddMilliseconds((double)this.ReadLong(BSONTypes.Int64));
			}
			if (type == typeof(ObjectId))
			{
				this.Read(12);
				return new ObjectId(this._reader.ReadBytes(12));
			}
			if (type == typeof(long))
			{
				return this.ReadLong(storedType);
			}
			if (type == typeof(double))
			{
				this.Read(8);
				return this._reader.ReadDouble();
			}
			if (type == typeof(Regex))
			{
				return this.ReadRegularExpression();
			}
			if (type == typeof(ScopedCode))
			{
				return this.ReadScopedCode();
			}
			if (type == typeof(Expando))
			{
				return this.ReadFlyweight();
			}
			return this.ReadObject(type);
		}
		private object ReadObject(Type type)
		{
			bool flag = false;
			object obj = null;
			ReflectionHelper reflectionHelper = null;
			if (type == typeof(object))
			{
				type = typeof(Expando);
			}
			if (!type.IsInterface && !type.IsAbstract)
			{
				obj = Activator.CreateInstance(type, true);
				reflectionHelper = ReflectionHelper.GetHelperForType(type);
				reflectionHelper.ApplyDefaultValues(obj);
			}
			string text;
			while (true)
			{
				BSONTypes bSONTypes = this.ReadType();
				text = this.ReadName();
				if (text == "$err" || text == "errmsg")
				{
					BsonDeserializer.HandleError((string)this.DeserializeValue(typeof(string), BSONTypes.String));
				}
				if (text == "__type")
				{
					if (flag)
					{
						break;
					}
					string typeName = this.ReadString();
					type = Type.GetType(typeName, true);
					reflectionHelper = ReflectionHelper.GetHelperForType(type);
					obj = Activator.CreateInstance(type, true);
					reflectionHelper.ApplyDefaultValues(obj);
				}
				else
				{
					if (obj == null)
					{
						goto Block_7;
					}
					flag = true;
					MagicProperty magicProperty = (text == "_id" || text == "$id") ? reflectionHelper.FindIdProperty() : reflectionHelper.FindProperty(text);
					if (magicProperty == null && !reflectionHelper.IsExpando)
					{
						goto Block_11;
					}
					bool flag2 = false;
					if (bSONTypes == BSONTypes.Object)
					{
						int num = this._reader.ReadInt32();
						if (num == 5)
						{
							this._reader.ReadByte();
							this.Read(5);
							flag2 = true;
						}
						else
						{
							this.NewDocument(num);
						}
					}
					object obj2 = null;
					if (magicProperty != null && magicProperty.Setter == null)
					{
						obj2 = magicProperty.Getter(obj);
					}
					Type type2 = (magicProperty != null) ? magicProperty.Type : (BsonDeserializer._typeMap.ContainsKey(bSONTypes) ? BsonDeserializer._typeMap[bSONTypes] : typeof(object));
					object obj3 = flag2 ? null : this.DeserializeValue(type2, bSONTypes, obj2);
					if (magicProperty == null)
					{
						((IExpando)obj)[text] = obj3;
					}
					else
					{
						if (obj2 == null && obj3 != null)
						{
							magicProperty.Setter(obj, obj3);
						}
					}
					if (this.IsDone())
					{
						return obj;
					}
				}
			}
			throw new MongoException("Found type declaration after processing properties - data loss would occur - the object has been incorrectly serialized");
			Block_7:
			throw new MongoException("Could not find the type to instantiate in the document, and " + type.Name + " is an interface or abstract type. Add a MongoDiscriminatedAttribute to the type or base type, or try to work with a concrete type next time.");
			Block_11:
			throw new MongoException(string.Format("Deserialization failed: type {0} does not have a property named {1}", type.FullName, text));
		}
		private object ReadList(Type listType, object existingContainer)
		{
			if (BsonDeserializer.IsDictionary(listType))
			{
				return this.ReadDictionary(listType, existingContainer);
			}
			if (listType == typeof(object))
			{
				listType = typeof(List<Expando>);
			}
			this.NewDocument(this._reader.ReadInt32());
			Type listItemType = ListHelper.GetListItemType(listType);
			bool flag = typeof(object) == listItemType;
			BaseWrapper baseWrapper = BaseWrapper.Create(listType, listItemType, existingContainer);
			while (!this.IsDone())
			{
				BSONTypes bSONTypes = this.ReadType();
				this.ReadName();
				if (bSONTypes == BSONTypes.Object)
				{
					this.NewDocument(this._reader.ReadInt32());
				}
				Type type = flag ? BsonDeserializer._typeMap[bSONTypes] : listItemType;
				object value = this.DeserializeValue(type, bSONTypes);
				baseWrapper.Add(value);
			}
			return baseWrapper.Collection;
		}
		private static bool IsDictionary(Type type)
		{
			List<Type> list = new List<Type>(type.GetInterfaces());
			list.Insert(0, type);
			foreach (Type current in list)
			{
				if (current.IsGenericType && current.GetGenericTypeDefinition() == typeof(IDictionary<, >))
				{
					return true;
				}
			}
			return false;
		}
		private object ReadDictionary(Type listType, object existingContainer)
		{
			Type dictionarValueType = ListHelper.GetDictionarValueType(listType);
			IDictionary dictionary = (existingContainer == null) ? ListHelper.CreateDictionary(listType, ListHelper.GetDictionarKeyType(listType), dictionarValueType) : ((IDictionary)existingContainer);
			while (!this.IsDone())
			{
				BSONTypes bSONTypes = this.ReadType();
				string key = this.ReadName();
				if (bSONTypes == BSONTypes.Object)
				{
					this.NewDocument(this._reader.ReadInt32());
				}
				object value = this.DeserializeValue(dictionarValueType, bSONTypes);
				dictionary.Add(key, value);
			}
			return dictionary;
		}
		private object ReadBinary()
		{
			int num = this._reader.ReadInt32();
			byte b = this._reader.ReadByte();
			this.Read(5 + num);
			if (b == 0)
			{
				return this._reader.ReadBytes(num);
			}
			if (b == 2)
			{
				return this._reader.ReadBytes(this._reader.ReadInt32());
			}
			if (b == 3)
			{
				return new Guid(this._reader.ReadBytes(num));
			}
			throw new MongoException("No support for binary type: " + b);
		}
		private string ReadName()
		{
			List<byte> list = new List<byte>(128);
			byte item;
			while ((item = this._reader.ReadByte()) > 0)
			{
				list.Add(item);
			}
			this.Read(list.Count + 1);
			return Encoding.UTF8.GetString(list.ToArray());
		}
		private string ReadString()
		{
			int num = this._reader.ReadInt32();
			byte[] bytes = this._reader.ReadBytes(num - 1);
			this._reader.ReadByte();
			this.Read(4 + num);
			return Encoding.UTF8.GetString(bytes);
		}
		private int ReadInt(BSONTypes storedType)
		{
			if (storedType != BSONTypes.Double)
			{
				switch (storedType)
				{
				case BSONTypes.Int32:
					this.Read(4);
					return this._reader.ReadInt32();
				case BSONTypes.Int64:
					this.Read(8);
					return (int)this._reader.ReadInt64();
				}
				throw new MongoException("Could not create an int from " + storedType);
			}
			this.Read(8);
			return (int)this._reader.ReadDouble();
		}
		private long ReadLong(BSONTypes storedType)
		{
			if (storedType != BSONTypes.Double)
			{
				switch (storedType)
				{
				case BSONTypes.Int32:
					this.Read(4);
					return (long)this._reader.ReadInt32();
				case BSONTypes.Int64:
					this.Read(8);
					return this._reader.ReadInt64();
				}
				throw new MongoException("Could not create an int64 from " + storedType);
			}
			this.Read(8);
			return (long)this._reader.ReadDouble();
		}
		private object ReadEnum(Type type, BSONTypes storedType)
		{
			if (storedType == BSONTypes.Int64)
			{
				return Enum.Parse(type, this.ReadLong(storedType).ToString(), false);
			}
			return Enum.Parse(type, this.ReadInt(storedType).ToString(), false);
		}
		private object ReadRegularExpression()
		{
			string pattern = this.ReadName();
			string text = this.ReadName();
			RegexOptions regexOptions = RegexOptions.None;
			if (text.Contains("e"))
			{
				regexOptions |= RegexOptions.ECMAScript;
			}
			if (text.Contains("i"))
			{
				regexOptions |= RegexOptions.IgnoreCase;
			}
			if (text.Contains("l"))
			{
				regexOptions |= RegexOptions.CultureInvariant;
			}
			if (text.Contains("m"))
			{
				regexOptions |= RegexOptions.Multiline;
			}
			if (text.Contains("s"))
			{
				regexOptions |= RegexOptions.Singleline;
			}
			if (text.Contains("w"))
			{
				regexOptions |= RegexOptions.IgnorePatternWhitespace;
			}
			if (text.Contains("x"))
			{
				regexOptions |= RegexOptions.ExplicitCapture;
			}
			return new Regex(pattern, regexOptions);
		}
		private BSONTypes ReadType()
		{
			this.Read(1);
			return (BSONTypes)this._reader.ReadByte();
		}
		private ScopedCode ReadScopedCode()
		{
			this._reader.ReadInt32();
			this.Read(4);
			string codeString = this.ReadString();
			this.NewDocument(this._reader.ReadInt32());
			return new ScopedCode
			{
				CodeString = codeString,
				Scope = this.DeserializeValue(typeof(Expando), BSONTypes.Object)
			};
		}
		private Expando ReadFlyweight()
		{
			Expando expando = new Expando();
			do
			{
				BSONTypes bSONTypes = this.ReadType();
				string text = this.ReadName();
				if (text == "$err" || text == "errmsg")
				{
					BsonDeserializer.HandleError((string)this.DeserializeValue(typeof(string), BSONTypes.String));
				}
				if (bSONTypes == BSONTypes.Object)
				{
					this.NewDocument(this._reader.ReadInt32());
				}
				Type type = BsonDeserializer._typeMap.ContainsKey(bSONTypes) ? BsonDeserializer._typeMap[bSONTypes] : typeof(object);
				object value = this.DeserializeValue(type, bSONTypes);
				expando.Set<object>(text, value);
			}
			while (!this.IsDone());
			return expando;
		}
		private static void HandleError(string message)
		{
			throw new MongoException(message);
		}
	}
}
