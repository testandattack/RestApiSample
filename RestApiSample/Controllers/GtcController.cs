using GtcRest.Interfaces.Service;
using GtcRest.Models.Domain;
using GtcRest.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RestApiSample.Controllers
{
    /// <summary>
    /// This controller is the primary entry point for all Gtc object calls
    /// </summary>
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


        /// <summary>
        /// Creates a new GtcModel.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="GtcModel"></param>
        /// <returns>the Id of the created <see cref="GtcModel"/> or 'Bad Request' if the call fails.</returns>
        [HttpPost]
        [SwaggerOperation(OperationId = "gtc_POST")]
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

        /// <summary>
        /// Gets all GtcModel items
        /// </summary>
        /// <remarks></remarks>
        /// <returns>a List of <see cref="GtcModel"/> items</returns>
        [HttpGet]
        [SwaggerOperation(OperationId = "gtc_GET")]
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


        /// <summary>
        /// Gets a GtcModel item
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id" example="1">The Id of the item to retrieve.</param>
        /// <returns>a <see cref="GtcModel"/> item</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "gtc_GET/id")]
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

        /// <summary>
        /// Updates a GtcModel item.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="GtcModel">The item, containing the changes, to update.</param>
        /// <returns>OK if successful, 'Not Found' if the item's id was not found, or 'Bad Request' if the call fails.</returns>
        [HttpPut]
        [SwaggerOperation(OperationId = "gtc_PUT")]
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

        /// <summary>
        /// Deletes a GtcModel item.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id" example="1">The Id of the item to delete.</param>
        /// <returns>'No Content'(success) if successful, 'Not Found' if the item's id was not found, or Bad Request if the call fails.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "gtc_DELETE")]
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
