using GtcRest.Interfaces.Service;
using GtcRest.Models.Domain;
using GtcRest.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RestApiSample.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class GtcController : ControllerBase
    {
        private readonly ILogger<GtcController> _logger;
        private readonly IGtcService _gtcServivce;

        public GtcController(ILogger<GtcController> logger, IGtcService gtcService)
        {
            _logger = logger;
            _gtcServivce = gtcService;
        }


        [HttpPost]
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int>> CreateGtcAsync([FromBody] GtcModel GtcModel)
        {
            try
            {
                var result = await _gtcServivce.CreateGtcAsync(GtcModel);
                // to return the full object, change the null to result
                return Created($"api/gtc/{result.Id}", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateGtcAsync failed. {postBody}", GtcModel);
            }
            return BadRequest("CreateGtcAsync failed.");
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GtcModel>), 200)]
        public async Task<ActionResult<List<GtcModel>>> GetGtcAsync()
        {
            try
            {
                var result = await _gtcServivce.GetGtcAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetGtcAsync() failed.");
                throw;
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GtcModel), 200)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<ActionResult<GtcModel>> GetGtcAsync(int id)
        {
            try
            {
                var result = await _gtcServivce.GetGtcAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetGtcAsync({id}) failed.", id);
                throw;
            }
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateGtcAsync([FromBody] GtcModel GtcModel)
        {
            try
            {
                var result = await _gtcServivce.UpdateGtcAsync(GtcModel);
                if(result != null)
                {
                    return Ok();
                }
                else
                {
                    return NotFound(GtcModel.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateGtcAsync {@putBody} failed.", GtcModel);
            }
            return BadRequest("UpdateGtcAsync failed.");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteGtcAsync(int id)
        {
            // move this logic to the service. Return custom enum that describes (at a domain level) the possible outcomes
            try
            {
                var result = await _gtcServivce.DeleteGtcAsync(id);
                if(result == OperationResult.NotFound)
                {
                    return NotFound();
                }
                else if (result == OperationResult.Deleted)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest($"The Gtc with id={id} was not deleted.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteGtcAsync({Id}) failed.", id);
                throw;
            }
        }
    }
}
