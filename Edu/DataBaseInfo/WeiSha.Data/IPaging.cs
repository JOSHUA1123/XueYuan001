using System;
namespace DataBaseInfo
{
	public interface IPaging
	{
		void Prefix(string prefix);
		void Suffix(string suffix);
		void End(string end);
	}
}
