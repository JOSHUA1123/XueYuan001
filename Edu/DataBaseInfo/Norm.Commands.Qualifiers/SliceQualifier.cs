using Norm.BSON;
using System;
namespace Norm.Commands.Qualifiers
{
	public class SliceQualifier : QualifierCommand
	{
		public SliceQualifier(int index) : this(index, index)
		{
		}
		public SliceQualifier(int left, int right) : base("$slice", new int[]
		{
			left,
			right
		})
		{
		}
	}
}
