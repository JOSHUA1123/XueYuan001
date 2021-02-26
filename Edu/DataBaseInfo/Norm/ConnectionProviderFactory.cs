using System;
using System.Collections.Generic;
namespace Norm
{
	internal static class ConnectionProviderFactory
	{
		private static readonly object _lock = new object();
		private static volatile IDictionary<string, ConnectionStringBuilder> _cachedBuilders = new Dictionary<string, ConnectionStringBuilder>();
		private static volatile IDictionary<string, IConnectionProvider> _providers = new Dictionary<string, IConnectionProvider>();
		public static IConnectionProvider Create(string connectionString)
		{
			ConnectionStringBuilder connectionStringBuilder;
			if (!ConnectionProviderFactory._cachedBuilders.TryGetValue(connectionString, out connectionStringBuilder))
			{
				lock (ConnectionProviderFactory._lock)
				{
					if (!ConnectionProviderFactory._cachedBuilders.TryGetValue(connectionString, out connectionStringBuilder))
					{
						connectionStringBuilder = ConnectionStringBuilder.Create(connectionString);
						ConnectionProviderFactory._cachedBuilders.Add(connectionString, connectionStringBuilder);
					}
				}
			}
			IConnectionProvider connectionProvider;
			if (!ConnectionProviderFactory._providers.TryGetValue(connectionString, out connectionProvider))
			{
				lock (ConnectionProviderFactory._lock)
				{
					if (!ConnectionProviderFactory._providers.TryGetValue(connectionString, out connectionProvider))
					{
						connectionProvider = ConnectionProviderFactory.CreateNewProvider(connectionStringBuilder);
						ConnectionProviderFactory._providers[connectionString] = connectionProvider;
					}
				}
			}
			return connectionProvider;
		}
		private static IConnectionProvider CreateNewProvider(ConnectionStringBuilder builder)
		{
			if (builder.Pooled)
			{
				return new PooledConnectionProvider(builder);
			}
			return new NormalConnectionProvider(builder);
		}
	}
}
