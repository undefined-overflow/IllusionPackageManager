using Client.Models.Installed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using static System.IO.File;
using static System.Text.Json.JsonSerializer;

namespace Client.Repositories
{
    public sealed class PluginsRepository : Dictionary<Guid, Dictionary<Guid, PluginInstalledModel>>, IDisposable
    {
        public static readonly string FILE_NAME = "plugins.json";

        private readonly GamesRepository _games;

        public void Dispose()
        {
            foreach (var game in _games.Values)
            {
                if (TryGetValue(game.Guid, out var plugins))
                {
                    WriteAllText(Path.Combine(game.Path, FILE_NAME), Serialize(plugins));
                }
            }

            GC.SuppressFinalize(this);
        }

        public PluginsRepository(GamesRepository games) : base(GetPlugins(games)) => _games = games;

        private static Dictionary<Guid, Dictionary<Guid, PluginInstalledModel>> GetPlugins(GamesRepository games) =>
            games.Values
                .Select(game => new
                {
                    Game = game,
                    Plugins = Deserialize<Dictionary<Guid, PluginInstalledModel>>(GetFileContent(Path.Combine(game.Path, FILE_NAME)))
                })
                .ToDictionary(key => key.Game.Guid, value => value.Plugins);

        private static string GetFileContent(string path)
        {
            try
            {
                return ReadAllText(path);
            }
            catch
            {
                WriteAllText(path, "{}");
                return "{}";
            }
        }
    }
}