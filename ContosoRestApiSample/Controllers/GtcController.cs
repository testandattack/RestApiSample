using ContosoRest.Interfaces.Service;
using ContosoRest.Models.Domain;
using ContosoRest.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RestApiSample.Controllers
{
    /// <summary>
    /// This controller is the primary entry point for all Contoso object calls
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ContosoController : ControllerBase
    {
        private readonly ILogger<ContosoController> _logger;
        private readonly IContosoService _contosoServivce;

        public ContosoController(ILogger<ContosoController> logger, IContosoService contosoService)
        {
            _logger = logger;
            _contosoServivce = contosoService;
        }


        /// <summary>
        /// Creates a new ContosoModel.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="ContosoModel"></param>
        /// <returns>the Id of the created <see cref="ContosoModel"/> or 'Bad Request' if the call fails.</returns>
        [HttpPost]
        [SwaggerOperation(OperationId = "contoso_POST")]
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int>> CreateContosoAsync([FromBody] ContosoModel ContosoModel)
        {
            try
            {
                var result = await _contosoServivce.CreateContosoAsync(ContosoModel);
                // to return the full object, change the null to result
                return Created($"api/contoso/{result.Id}", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateContosoAsync failed. {postBody}", ContosoModel);
            }
            return BadRequest("CreateContosoAsync failed.");
        }

        /// <summary>
        /// Gets all ContosoModel items
        /// </summary>
        /// <remarks></remarks>
        /// <returns>a List of <see cref="ContosoModel"/> items</returns>
        [HttpGet]
        [SwaggerOperation(OperationId = "contoso_GET")]
        [ProducesResponseType(typeof(List<ContosoModel>), 200)]
        public async Task<ActionResult<List<ContosoModel>>> GetContosoAsync()
        {
            try
            {
                var result = await _contosoServivce.GetContosoAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetContosoAsync() failed.");
                throw;
            }
        }


        /// <summary>
        /// Gets a ContosoModel item
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id" example="1">The Id of the item to retrieve.</param>
        /// <returns>a <see cref="ContosoModel"/> item</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "contoso_GET/id")]
        [ProducesResponseType(typeof(ContosoModel), 200)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<ActionResult<ContosoModel>> GetContosoAsync(int id)
        {
            try
            {
                var result = await _contosoServivce.GetContosoAsync(id);
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
                _logger.LogError(ex, "GetContosoAsync({id}) failed.", id);
                throw;
            }
        }

        /// <summary>
        /// Updates a ContosoModel item.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="ContosoModel">The item, containing the changes, to update.</param>
        /// <returns>OK if successful, 'Not Found' if the item's id was not found, or 'Bad Request' if the call fails.</returns>
        [HttpPut]
        [SwaggerOperation(OperationId = "contoso_PUT")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateContosoAsync([FromBody] ContosoModel ContosoModel)
        {
            try
            {
                var result = await _contosoServivce.UpdateContosoAsync(ContosoModel);
                if(result != null)
                {
                    return Ok();
                }
                else
                {
                    return NotFound(ContosoModel.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateContosoAsync {@putBody} failed.", ContosoModel);
            }
            return BadRequest("UpdateContosoAsync failed.");
        }

        /// <summary>
        /// Deletes a ContosoModel item.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id" example="1">The Id of the item to delete.</param>
        /// <returns>'No Content'(success) if successful, 'Not Found' if the item's id was not found, or Bad Request if the call fails.</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "contoso_DELETE")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteContosoAsync(int id)
        {
            // move this logic to the service. Return custom enum that describes (at a domain level) the possible outcomes
            try
            {
                var result = await _contosoServivce.DeleteContosoAsync(id);
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
                    return BadRequest($"The Contoso with id={id} was not deleted.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteContosoAsync({Id}) failed.", id);
                throw;
            }
        }
    }
}
