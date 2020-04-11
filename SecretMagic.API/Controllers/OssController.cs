using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretMagic.API.Commom;
using SecretMagic.API.Services;
using SecretMagic.Model;

namespace SecretMagic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OssController : ControllerBase
    {
        private readonly ILogger<OssController> logger;
        private readonly IOssService ossService;

        public OssController(ILogger<OssController> logger, IOssService ossService)
        {
            this.logger = logger;
            this.ossService = ossService;
        }

        [HttpGet("GetOssStsToken")]
        public IActionResult GetOssStsToken()
        {
            try
            {
                var result = ossService.GetOssStsToken();
                return Ok(result);
            }
            catch (InternalException ie)
            {
                logger.LogError(500, ie, ie.Message, null);
                return NotFound(ie.Message);
            }
            catch (BadRequestException be)
            {
                logger.LogError(404, be, be.Message, null);
                return BadRequest(be.Message);
            }
        }

        [HttpGet("RedirectOssResource/{key}")]
        public IActionResult RedirectOssResource(string key)
        {
            var uri = ossService.GetAssignedUri(key);
            return new RedirectResult(uri.AbsoluteUri);
        }
    }
}