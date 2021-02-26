using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;
namespace Norm.BSON.DbTypes
{
	internal static class ObjectIdGenerator
	{
		private static readonly DateTime epoch;
		private static readonly object inclock;
		private static int inc;
		private static byte[] machineHash;
		private static byte[] procID;
		static ObjectIdGenerator()
		{
			ObjectIdGenerator.epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			ObjectIdGenerator.inclock = new object();
			ObjectIdGenerator.GenerateConstants();
		}
		public static byte[] Generate()
		{
			byte[] array = new byte[12];
			int num = 0;
			Array.Copy(BitConverter.GetBytes(ObjectIdGenerator.GenerateTime()), 0, array, num, 4);
			num += 4;
			Array.Copy(ObjectIdGenerator.machineHash, 0, array, num, 3);
			num += 3;
			Array.Copy(ObjectIdGenerator.procID, 0, array, num, 2);
			num += 2;
			Array.Copy(BitConverter.GetBytes(ObjectIdGenerator.GenerateInc()), 0, array, num, 3);
			return array;
		}
		private static int GenerateTime()
		{
			DateTime dateTime = DateTime.Now.ToUniversalTime();
			DateTime d = new DateTime(ObjectIdGenerator.epoch.Year, ObjectIdGenerator.epoch.Month, ObjectIdGenerator.epoch.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
			return Convert.ToInt32(Math.Floor((d - ObjectIdGenerator.epoch).TotalMilliseconds));
		}
		private static int GenerateInc()
		{
			int result;
			lock (ObjectIdGenerator.inclock)
			{
				result = ObjectIdGenerator.inc++;
			}
			return result;
		}
		private static void GenerateConstants()
		{
			ObjectIdGenerator.machineHash = ObjectIdGenerator.GenerateHostHash();
			ObjectIdGenerator.procID = BitConverter.GetBytes(ObjectIdGenerator.GenerateProcId());
		}
		private static byte[] GenerateHostHash()
		{
			byte[] result;
			using (MD5 mD = MD5.Create())
			{
				string hostName = Dns.GetHostName();
				result = mD.ComputeHash(Encoding.Default.GetBytes(hostName));
			}
			return result;
		}
		private static int GenerateProcId()
		{
			Process currentProcess = Process.GetCurrentProcess();
			return currentProcess.Id;
		}
	}
}
