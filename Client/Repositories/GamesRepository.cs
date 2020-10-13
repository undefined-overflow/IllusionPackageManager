using Client.Models.Installed;
using System;
using System.Collections.Generic;

using static System.IO.File;
using static System.Text.Json.JsonSerializer;

namespace Client.Repositories
{
    public sealed class GamesRepository : Dictionary<Guid, GameInstalledModel>, IDisposable
    {
        public static readonly string FILE_NAME = "games.json";

        public GamesRepository() : base(GetGames())
        {
        }

        private static Dictionary<Guid, GameInstalledModel> GetGames() =>
            Deserialize<Dictionary<Guid, GameInstalledModel>>(GetFileContent());

        private static string GetFileContent()
        {
            try
            {
                return ReadAllText(FILE_NAME);
            }
            catch
            {
                WriteAllText(FILE_NAME, "{}");
                return "{}";
            }
        }

        public void Dispose()
        {
            WriteAllText(FILE_NAME, Serialize(this));
            GC.SuppressFinalize(this);
        }
    }
}