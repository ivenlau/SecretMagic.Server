using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretMagic.API.Authorization;
using SecretMagic.API.Commom;
using SecretMagic.API.Services;
using SecretMagic.Model;

namespace SecretMagic.API.Controllers
{
    /// <summary>
    /// API for SuperAdmin data testing
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SiteSettingController : ControllerBase
    {
        private readonly ILogger<SiteSettingController> logger;
        private readonly ISiteSettingService service;

        public SiteSettingController(ILogger<SiteSettingController> logger, ISiteSettingService service)
        {
            this.logger = logger;
            this.service = service;
        }

        [HttpGet("GetSetting")]
        public ActionResult<IEnumerable<SiteSetting>> GetSetting()
        {
            try
            {
                return Ok(this.service.GetSiteSetting());
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

        [HttpPost("UpdateBasicSetting")]
        [Protected("System Management")]
        public async Task<ActionResult> UpdateBasicSetting(SiteSetting setting)
        {
            try
            {
                await service.UpdateBasicSetting(setting);
                return Ok();
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

        [HttpPost("UpdateUiSetting")]
        [Protected("System Management")]
        public async Task<ActionResult> UpdateUiSetting(SiteSetting setting)
        {
            try
            {
                await service.UpdateUiSetting(setting.UiSetting);
                return Ok();
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
    }
}