using System;
using System.Threading;
namespace Norm
{
	public struct TimedLock : IDisposable
	{
		private readonly object _target;
		private TimedLock(object o)
		{
			this._target = o;
		}
		public static TimedLock Lock(object o)
		{
			return TimedLock.Lock(o, TimeSpan.FromSeconds(10.0));
		}
		public static TimedLock Lock(object o, TimeSpan timeout)
		{
			TimedLock result = new TimedLock(o);
			if (!Monitor.TryEnter(o, timeout))
			{
				throw new LockTimeoutException();
			}
			return result;
		}
		public void Dispose()
		{
			Monitor.Exit(this._target);
		}
	}
}
