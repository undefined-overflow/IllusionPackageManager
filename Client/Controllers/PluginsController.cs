using Client.Managers;
using Client.Models.Controllers.Request;
using Client.Models.Controllers.Response;
using Client.Models.Installed;
using Client.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PluginsController : ControllerBase
    {
        private readonly GamesRepository _games;
        private readonly PluginsRepository _plugins;
        private readonly PluginManager _pluginsManager;
        private readonly ILogger<PluginsController> _logger;

        public PluginsController(GamesRepository games, PluginsRepository plugins, PluginManager pluginsManager, ILogger<PluginsController> logger) =>
            (_games, _plugins, _pluginsManager, _logger) = (games, plugins, pluginsManager, logger);

        [HttpGet("{guid}")]
        public IEnumerable<PluginsModel> Get(Guid guid)
        {
            if (_plugins.TryGetValue(guid, out var plugins))
            {
                return plugins.Select(plugin => new PluginsModel(plugin.Key, plugin.Value));
            }

            return Array.Empty<PluginsModel>();
        }

        [HttpPost("{guid}")]
        public async Task Post(Guid guid, [FromBody] PluginAddModel model)
        {
            if (_games.TryGetValue(model.Game, out GameInstalledModel installed))
            {
                await _pluginsManager.Install(guid, installed);
            }
        }

        [HttpDelete("{guid}")]
        public void Delete(string guid)
        {
        }
    }
}