using Client.Models.Installed;
using Client.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GamesRepository _games;
        private readonly PluginsRepository _plugins;
        private readonly ILogger<GamesController> _logger;

        public GamesController(GamesRepository games, PluginsRepository plugins, ILogger<GamesController> logger) =>
            (_games, _plugins, _logger) = (games, plugins, logger);

        [HttpGet]
        public IEnumerable<Guid> Get() => _games.Keys;

        [HttpPost("{guid}")]
        public void Post(Guid guid, [FromForm] string path)
        {
            _games.TryAdd(guid, new() { Guid = guid, Path = path });
            _plugins.TryAdd(guid, new Dictionary<Guid, PluginInstalledModel>());
        }

        [HttpPut("{guid}")]
        public void Put(string guid, [FromForm] string path)
        {
            if (_games.TryGetValue(new(guid), out var game))
            {
                game.Path = path;
            }
        }

        [HttpDelete("{guid}")]
        public void Delete(string guid) => _games.Remove(new(guid));
    }
}