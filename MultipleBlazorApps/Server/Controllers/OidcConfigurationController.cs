using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;

namespace MultipleBlazorApps.Server.Controllers
{
    public class OidcConfigurationController : Controller
    {
        #region Fields

        private readonly ILogger<OidcConfigurationController> _logger;

        #endregion Fields

        #region Constructors

        public OidcConfigurationController(IClientRequestParametersProvider clientRequestParametersProvider, ILogger<OidcConfigurationController> logger)
        {
            ClientRequestParametersProvider = clientRequestParametersProvider;
            _logger = logger;
        }

        #endregion Constructors

        #region Properties

        public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

        #endregion Properties

        #region Methods

        [HttpGet("_configuration/{clientId}")]
        [HttpGet("FirstApp/_configuration/{clientId}")]
        [HttpGet("SecondApp/_configuration/{clientId}")]
        public IActionResult GetClientRequestParameters([FromRoute] string clientId)
        {
            var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
            return Ok(parameters);
        }

        #endregion Methods
    }
}