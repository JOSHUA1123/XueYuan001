using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace Norm.BSON
{
	public class ObjectParser
	{
		private static readonly Regex _rxObject = new Regex("\\s*{\\s*(?<obj>.*)\\s*}\\s*(,|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static readonly Regex _rxArray = new Regex("\\s*\\[\\s*(?<arrayValues>.*)\\s*]\\s*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static readonly Regex _rxPair = new Regex("\\s*\"(?<key>.*?)((?<!\\\\)(?<!\\\\)\")\\s*:\\s*(?<value>(((?'Open'\\[)[^[]*)(?'Close-Open']))|((.*?)|(\"(.*?)((?<!\\\\)(?<!\\\\)\"))))\\s*(,|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static readonly Regex _rxArrayMember = new Regex("\\s*(?<value>(((?'Open'\\[)[^[]*)(?'Close-Open']))|((.*?)|(\"(.*?)((?<!\\\\)(?<!\\\\)\"))))\\s*(,|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static readonly Regex _rxBool = new Regex("^\\s*(true)|(false)\\s*(,|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static readonly Regex _rxNull = new Regex("^\\s*null\\s*(,|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static readonly Regex _rxNumber = new Regex("^\\s*-?\\s*(([0-9]*[.]?[0-9]*)|([0-9]+))\\s*(e(\\+|-)?[0-9]+)?\\s*(,|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		public Expando ParseJSON(string jsonToParse)
		{
			Expando expando = new Expando();
			string text = ObjectParser._rxObject.Match(jsonToParse).Groups["obj"].Value;
			Match match;
			do
			{
				match = ObjectParser._rxPair.Match(text);
				if (match.Success)
				{
					expando[match.Groups["key"].Value] = this.ParseMember(match.Groups["value"].Value);
					text = text.Remove(0, match.Length);
				}
			}
			while (match.Success && text.Length > 0);
			return expando;
		}
		private object[] ParseJSONArray(string jsonToParse)
		{
			List<object> list = new List<object>();
			string text = ObjectParser._rxArray.Match(jsonToParse).Groups["arrayValues"].Value;
			Match match;
			do
			{
				match = ObjectParser._rxArrayMember.Match(text);
				if (match.Success)
				{
					list.Add(this.ParseMember(match.Groups["value"].Value));
					text = text.Remove(0, match.Length);
				}
			}
			while (match.Success && text.Length > 0);
			return list.ToArray();
		}
		private object ParseMember(string member)
		{
			member = (member ?? "");
			object result;
			if (ObjectParser._rxObject.IsMatch(member))
			{
				result = this.ParseJSON(member);
			}
			else
			{
				if (ObjectParser._rxArray.IsMatch(member))
				{
					result = this.ParseJSONArray(member);
				}
				else
				{
					if (ObjectParser._rxNull.IsMatch(member))
					{
						result = null;
					}
					else
					{
						if (ObjectParser._rxBool.IsMatch(member))
						{
							result = bool.Parse(member);
						}
						else
						{
							if (ObjectParser._rxNumber.IsMatch(member))
							{
								result = double.Parse(member);
							}
							else
							{
								member = member.Trim();
								if (member.StartsWith("\"") && member.EndsWith("\""))
								{
									member = member.Remove(0, 1);
									member = member.Substring(0, member.Length - 1);
								}
								result = member;
							}
						}
					}
				}
			}
			return result;
		}
	}
}
