using System;
using System.Collections;
using System.Text;

/// <summary>
/// Contains conversion support elements such as classes, interfaces and static methods.
/// </summary>
// Token: 0x0200009E RID: 158
public class SupportClass
{
	/// <summary>
	/// Converts an array of sbytes to an array of bytes
	/// </summary>
	/// <param name="sbyteArray">The array of sbytes to be converted</param>
	/// <returns>The new array of bytes</returns>
	// Token: 0x0600042C RID: 1068 RVA: 0x0002158C File Offset: 0x0001F78C
	public static byte[] ToByteArray(sbyte[] sbyteArray)
	{
		byte[] array = null;
		if (sbyteArray != null)
		{
			array = new byte[sbyteArray.Length];
			for (int i = 0; i < sbyteArray.Length; i++)
			{
				array[i] = (byte)sbyteArray[i];
			}
		}
		return array;
	}

	/// <summary>
	/// Converts a string to an array of bytes
	/// </summary>
	/// <param name="sourceString">The string to be converted</param>
	/// <returns>The new array of bytes</returns>
	// Token: 0x0600042D RID: 1069 RVA: 0x0000ACB5 File Offset: 0x00008EB5
	public static byte[] ToByteArray(string sourceString)
	{
		return Encoding.UTF8.GetBytes(sourceString);
	}

	/// <summary>
	/// Converts a array of object-type instances to a byte-type array.
	/// </summary>
	/// <param name="tempObjectArray">Array to convert.</param>
	/// <returns>An array of byte type elements.</returns>
	// Token: 0x0600042E RID: 1070 RVA: 0x000215C0 File Offset: 0x0001F7C0
	public static byte[] ToByteArray(object[] tempObjectArray)
	{
		byte[] array = null;
		if (tempObjectArray != null)
		{
			array = new byte[tempObjectArray.Length];
			for (int i = 0; i < tempObjectArray.Length; i++)
			{
				array[i] = (byte)tempObjectArray[i];
			}
		}
		return array;
	}

	/// <summary>
	/// Performs an unsigned bitwise right shift with the specified number
	/// </summary>
	/// <param name="number">Number to operate on</param>
	/// <param name="bits">Ammount of bits to shift</param>
	/// <returns>The resulting number from the shift operation</returns>
	// Token: 0x0600042F RID: 1071 RVA: 0x0000ACC2 File Offset: 0x00008EC2
	public static int URShift(int number, int bits)
	{
		if (number >= 0)
		{
			return number >> bits;
		}
		return (number >> bits) + (2 << ~bits);
	}

	/// <summary>
	/// Performs an unsigned bitwise right shift with the specified number
	/// </summary>
	/// <param name="number">Number to operate on</param>
	/// <param name="bits">Ammount of bits to shift</param>
	/// <returns>The resulting number from the shift operation</returns>
	// Token: 0x06000430 RID: 1072 RVA: 0x0000ACDD File Offset: 0x00008EDD
	public static int URShift(int number, long bits)
	{
		return SupportClass.URShift(number, (int)bits);
	}

	/// <summary>
	/// Performs an unsigned bitwise right shift with the specified number
	/// </summary>
	/// <param name="number">Number to operate on</param>
	/// <param name="bits">Ammount of bits to shift</param>
	/// <returns>The resulting number from the shift operation</returns>
	// Token: 0x06000431 RID: 1073 RVA: 0x0000ACE7 File Offset: 0x00008EE7
	public static long URShift(long number, int bits)
	{
		if (number >= 0L)
		{
			return number >> bits;
		}
		return (number >> bits) + (2L << ~bits);
	}

	/// <summary>
	/// Performs an unsigned bitwise right shift with the specified number
	/// </summary>
	/// <param name="number">Number to operate on</param>
	/// <param name="bits">Ammount of bits to shift</param>
	/// <returns>The resulting number from the shift operation</returns>
	// Token: 0x06000432 RID: 1074 RVA: 0x0000AD04 File Offset: 0x00008F04
	public static long URShift(long number, long bits)
	{
		return SupportClass.URShift(number, (int)bits);
	}

	/// <summary>
	/// This method returns the literal value received
	/// </summary>
	/// <param name="literal">The literal to return</param>
	/// <returns>The received value</returns>
	// Token: 0x06000433 RID: 1075 RVA: 0x0000AD0E File Offset: 0x00008F0E
	public static long Identity(long literal)
	{
		return literal;
	}

	/// <summary>
	/// This method returns the literal value received
	/// </summary>
	/// <param name="literal">The literal to return</param>
	/// <returns>The received value</returns>
	// Token: 0x06000434 RID: 1076 RVA: 0x0000AD0E File Offset: 0x00008F0E
	public static ulong Identity(ulong literal)
	{
		return literal;
	}

	/// <summary>
	/// This method returns the literal value received
	/// </summary>
	/// <param name="literal">The literal to return</param>
	/// <returns>The received value</returns>
	// Token: 0x06000435 RID: 1077 RVA: 0x0000AD0E File Offset: 0x00008F0E
	public static float Identity(float literal)
	{
		return literal;
	}

	/// <summary>
	/// This method returns the literal value received
	/// </summary>
	/// <param name="literal">The literal to return</param>
	/// <returns>The received value</returns>
	// Token: 0x06000436 RID: 1078 RVA: 0x0000AD0E File Offset: 0x00008F0E
	public static double Identity(double literal)
	{
		return literal;
	}

	/// <summary>
	/// Copies an array of chars obtained from a String into a specified array of chars
	/// </summary>
	/// <param name="sourceString">The String to get the chars from</param>
	/// <param name="sourceStart">Position of the String to start getting the chars</param>
	/// <param name="sourceEnd">Position of the String to end getting the chars</param>
	/// <param name="destinationArray">Array to return the chars</param>
	/// <param name="destinationStart">Position of the destination array of chars to start storing the chars</param>
	/// <returns>An array of chars</returns>
	// Token: 0x06000437 RID: 1079 RVA: 0x000215F8 File Offset: 0x0001F7F8
	public static void GetCharsFromString(string sourceString, int sourceStart, int sourceEnd, char[] destinationArray, int destinationStart)
	{
		int i = sourceStart;
		int num = destinationStart;
		while (i < sourceEnd)
		{
			destinationArray[num] = sourceString[i];
			i++;
			num++;
		}
	}

	/// <summary>
	/// Sets the capacity for the specified ArrayList
	/// </summary>
	/// <param name="vector">The ArrayList which capacity will be set</param>
	/// <param name="newCapacity">The new capacity value</param>
	// Token: 0x06000438 RID: 1080 RVA: 0x0000AD11 File Offset: 0x00008F11
	public static void SetCapacity(ArrayList vector, int newCapacity)
	{
		if (newCapacity > vector.Count)
		{
			vector.AddRange(new Array[newCapacity - vector.Count]);
		}
		else if (newCapacity < vector.Count)
		{
			vector.RemoveRange(newCapacity, vector.Count - newCapacity);
		}
		vector.Capacity = newCapacity;
	}

	/// <summary>
	/// Receives a byte array and returns it transformed in an sbyte array
	/// </summary>
	/// <param name="byteArray">Byte array to process</param>
	/// <returns>The transformed array</returns>
	// Token: 0x06000439 RID: 1081 RVA: 0x00021624 File Offset: 0x0001F824
	public static sbyte[] ToSByteArray(byte[] byteArray)
	{
		sbyte[] array = null;
		if (byteArray != null)
		{
			array = new sbyte[byteArray.Length];
			for (int i = 0; i < byteArray.Length; i++)
			{
				array[i] = (sbyte)byteArray[i];
			}
		}
		return array;
	}
}
