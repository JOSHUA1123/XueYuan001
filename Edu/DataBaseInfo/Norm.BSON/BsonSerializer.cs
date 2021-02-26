using Norm.BSON.DbTypes;
using Norm.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
namespace Norm.BSON
{
	internal class BsonSerializer : BsonSerializerBase
	{
		private static readonly IDictionary<Type, BSONTypes> _typeMap = new Dictionary<Type, BSONTypes>
		{

			{
				typeof(int),
				BSONTypes.Int32
			},

			{
				typeof(long),
				BSONTypes.Int64
			},

			{
				typeof(bool),
				BSONTypes.Boolean
			},

			{
				typeof(string),
				BSONTypes.String
			},

			{
				typeof(double),
				BSONTypes.Double
			},

			{
				typeof(Guid),
				BSONTypes.Binary
			},

			{
				typeof(Regex),
				BSONTypes.Regex
			},

			{
				typeof(DateTime),
				BSONTypes.DateTime
			},

			{
				typeof(float),
				BSONTypes.Double
			},

			{
				typeof(byte[]),
				BSONTypes.Binary
			},

			{
				typeof(ObjectId),
				BSONTypes.MongoOID
			},

			{
				typeof(ScopedCode),
				BSONTypes.ScopedCode
			}
		};
		private readonly BinaryWriter _writer;
		private Document _current;
		private BsonSerializer(BinaryWriter writer)
		{
			this._writer = writer;
		}
		public static byte[] Serialize<T>(T document)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(250))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					new BsonSerializer(binaryWriter).WriteDocument(document);
					result = memoryStream.ToArray();
				}
			}
			return result;
		}
		private void NewDocument()
		{
			Document current = this._current;
			this._current = new Document
			{
				Parent = current,
				Length = (int)this._writer.BaseStream.Position,
				Digested = 4
			};
			this._writer.Write(0);
		}
		private void EndDocument(bool includeEeo)
		{
			Document current = this._current;
			if (includeEeo)
			{
				this.Written(1);
				this._writer.Write(0);
			}
			this._writer.Seek(this._current.Length, SeekOrigin.Begin);
			this._writer.Write(this._current.Digested);
			this._writer.Seek(0, SeekOrigin.End);
			this._current = this._current.Parent;
			if (this._current != null)
			{
				this.Written(current.Digested);
			}
		}
		private void Written(int length)
		{
			this._current.Digested += length;
		}
		private void WriteDocument(object document)
		{
			this.NewDocument();
			if (document is Expando)
			{
				this.WriteFlyweight((Expando)document);
			}
			else
			{
				this.WriteObject(document);
			}
			this.EndDocument(true);
		}
		private void WriteFlyweight(Expando document)
		{
			foreach (ExpandoProperty current in document.AllProperties())
			{
				this.SerializeMember(current.PropertyName, current.Value);
			}
		}
		private static bool IsDbReference(Type type)
		{
			return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(DbReference<>) || type.GetGenericTypeDefinition() == typeof(DbReference<, >));
		}
		private void WriteObject(object document)
		{
			ReflectionHelper helperForType = ReflectionHelper.GetHelperForType(document.GetType());
			MagicProperty magicProperty = helperForType.FindIdProperty();
			Type type = document.GetType();
			string typeDiscriminator = helperForType.GetTypeDiscriminator();
			if (!string.IsNullOrEmpty(typeDiscriminator))
			{
				this.SerializeMember("__type", typeDiscriminator);
			}
			List<string> list = new List<string>();
			foreach (MagicProperty current in helperForType.GetProperties())
			{
				string text = (current == magicProperty && !BsonSerializer.IsDbReference(current.DeclaringType)) ? "_id" : MongoConfiguration.GetPropertyAlias(type, current.Name);
				object value;
				if (!current.IgnoreProperty(document, out value))
				{
					list.Add(text);
					this.SerializeMember(text, value);
				}
			}
			IExpando expando = document as IExpando;
			if (expando != null)
			{
				foreach (ExpandoProperty current2 in expando.AllProperties())
				{
					if (!list.Contains(current2.PropertyName))
					{
						this.SerializeMember(current2.PropertyName, current2.Value);
					}
				}
			}
		}
		private void SerializeMember(string name, object value)
		{
			if (value == null)
			{
				this.Write(BSONTypes.Null);
				this.WriteName(name);
				return;
			}
			Type type = value.GetType();
			IBsonTypeConverter typeConverterFor = BsonSerializerBase.Configuration.GetTypeConverterFor(type);
			if (typeConverterFor != null)
			{
				value = typeConverterFor.ConvertToBson(value);
			}
			type = value.GetType();
			if (type.IsEnum)
			{
				type = Enum.GetUnderlyingType(type);
			}
			BSONTypes type2;
			if (!BsonSerializer._typeMap.TryGetValue(type, out type2))
			{
				this.Write(name, value);
				return;
			}
			this.Write(type2);
			this.WriteName(name);
			switch (type2)
			{
			case BSONTypes.Double:
				this.Written(8);
				if (value is float)
				{
					this._writer.Write(Convert.ToDouble((float)value));
					return;
				}
				this._writer.Write((double)value);
				return;
			case BSONTypes.String:
				this.Write((string)value);
				return;
			case BSONTypes.Object:
			case BSONTypes.Array:
			case BSONTypes.Undefined:
			case BSONTypes.Null:
			case BSONTypes.Reference:
			case BSONTypes.Code:
			case BSONTypes.Symbol:
			case BSONTypes.Timestamp:
				break;
			case BSONTypes.Binary:
				this.WriteBinary(value);
				return;
			case BSONTypes.MongoOID:
				this.Written(((ObjectId)value).Value.Length);
				this._writer.Write(((ObjectId)value).Value);
				return;
			case BSONTypes.Boolean:
				this.Written(1);
				this._writer.Write(((bool)value) ? 1 : 0);
				return;
			case BSONTypes.DateTime:
				this.Written(8);
				this._writer.Write((long)((DateTime)value).ToUniversalTime().Subtract(BsonHelper.EPOCH).TotalMilliseconds);
				return;
			case BSONTypes.Regex:
				this.Write((Regex)value);
				break;
			case BSONTypes.ScopedCode:
				this.Write((ScopedCode)value);
				return;
			case BSONTypes.Int32:
				this.Written(4);
				this._writer.Write((int)value);
				return;
			case BSONTypes.Int64:
				this.Written(8);
				this._writer.Write((long)value);
				return;
			default:
				return;
			}
		}
		private void Write(string name, object value)
		{
			if (value is IDictionary)
			{
				this.Write(BSONTypes.Object);
				this.WriteName(name);
				this.NewDocument();
				this.Write((IDictionary)value);
				this.EndDocument(true);
				return;
			}
			if (value is IEnumerable)
			{
				this.Write(BSONTypes.Array);
				this.WriteName(name);
				this.NewDocument();
				this.Write((IEnumerable)value);
				this.EndDocument(true);
				return;
			}
			if (value is ModifierCommand)
			{
				ModifierCommand modifierCommand = (ModifierCommand)value;
				this.Write(BSONTypes.Object);
				this.WriteName(modifierCommand.CommandName);
				this.NewDocument();
				this.SerializeMember(name, modifierCommand.ValueForCommand);
				this.EndDocument(true);
				return;
			}
			if (value is QualifierCommand)
			{
				QualifierCommand qualifierCommand = (QualifierCommand)value;
				this.Write(BSONTypes.Object);
				this.WriteName(name);
				this.NewDocument();
				this.SerializeMember(qualifierCommand.CommandName, qualifierCommand.ValueForCommand);
				this.EndDocument(true);
				return;
			}
			this.Write(BSONTypes.Object);
			this.WriteName(name);
			this.WriteDocument(value);
		}
		private void Write(IEnumerable enumerable)
		{
			int num = 0;
			foreach (object current in enumerable)
			{
				this.SerializeMember(num++.ToString(), current);
			}
		}
		private void Write(IDictionary dictionary)
		{
			foreach (object current in dictionary.Keys)
			{
				this.SerializeMember((string)current, dictionary[current]);
			}
		}
		private void WriteBinary(object value)
		{
			if (value is byte[])
			{
				byte[] array = (byte[])value;
				int num = array.Length;
				this._writer.Write(num);
				this._writer.Write(0);
				this._writer.Write(array);
				this.Written(5 + num);
				return;
			}
			if (value is Guid)
			{
				byte[] array2 = ((Guid)value).ToByteArray();
				this._writer.Write(array2.Length);
				this._writer.Write(3);
				this._writer.Write(array2);
				this.Written(5 + array2.Length);
			}
		}
		private void Write(BSONTypes type)
		{
			this._writer.Write((byte)type);
			this.Written(1);
		}
		private void WriteName(string name)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(name);
			this._writer.Write(bytes);
			this._writer.Write(0);
			this.Written(bytes.Length + 1);
		}
		private void Write(string name)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(name);
			this._writer.Write(bytes.Length + 1);
			this._writer.Write(bytes);
			this._writer.Write(0);
			this.Written(bytes.Length + 5);
		}
		private void Write(Regex regex)
		{
			this.WriteName(regex.ToString());
			string text = string.Empty;
			if ((regex.Options & RegexOptions.ECMAScript) == RegexOptions.ECMAScript)
			{
				text += 'e';
			}
			if ((regex.Options & RegexOptions.IgnoreCase) == RegexOptions.IgnoreCase)
			{
				text += 'i';
			}
			if ((regex.Options & RegexOptions.CultureInvariant) == RegexOptions.CultureInvariant)
			{
				text += 'l';
			}
			if ((regex.Options & RegexOptions.Multiline) == RegexOptions.Multiline)
			{
				text += 'm';
			}
			if ((regex.Options & RegexOptions.Singleline) == RegexOptions.Singleline)
			{
				text += 's';
			}
			if ((regex.Options & RegexOptions.IgnorePatternWhitespace) == RegexOptions.IgnorePatternWhitespace)
			{
				text += 'w';
			}
			if ((regex.Options & RegexOptions.ExplicitCapture) == RegexOptions.ExplicitCapture)
			{
				text += 'x';
			}
			this.WriteName(text);
		}
		private void Write(ScopedCode value)
		{
			this.NewDocument();
			this.Write(value.CodeString);
			this.WriteDocument(value.Scope);
			this.EndDocument(false);
		}
	}
}
