// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Microsoft.Extensions.DependencyInjection;
using osu.Server.Spectator.Database;
using osu.Server.Spectator.Entities;
using osu.Server.Spectator.Hubs;
using osu.Server.Spectator.Hubs.Metadata;
using osu.Server.Spectator.Hubs.Multiplayer;
using osu.Server.Spectator.Hubs.Spectator;
using osu.Server.Spectator.Storage;
using StackExchange.Redis;

namespace osu.Server.Spectator.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHubEntities(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<EntityStore<SpectatorClientState>>()
                                    .AddSingleton<EntityStore<MultiplayerClientState>>()
                                    .AddSingleton<EntityStore<ServerMultiplayerRoom>>()
                                    .AddSingleton<EntityStore<ConnectionState>>()
                                    .AddSingleton<EntityStore<MetadataClientState>>()
                                    .AddSingleton<GracefulShutdownManager>()
                                    .AddSingleton<MetadataBroadcaster>()
                                    .AddSingleton<IScoreStorage, FileScoreStorage>()
                                    .AddSingleton<ScoreUploader>()
                                    .AddSingleton<IScoreProcessedSubscriber, ScoreProcessedSubscriber>()
                                    .AddSingleton<BuildUserCountUpdater>();
        }

        /// <summary>
        /// Adds MySQL (<see cref="IDatabaseFactory"/>) and Redis (<see cref="IConnectionMultiplexer"/>) services.
        /// </summary>
        public static IServiceCollection AddDatabaseServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<IDatabaseFactory, DatabaseFactory>()
                                    .AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(AppSettings.RedisHost));
        }
    }
}
