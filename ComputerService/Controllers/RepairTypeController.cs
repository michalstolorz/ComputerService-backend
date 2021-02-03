using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ComputerService.Core.Dto.Request;
using ComputerService.Core.Interfaces.Services;
using ComputerService.Core.Models;
using ComputerService.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairTypeController : ControllerBase
    {
        private readonly IRepairTypeService _repairTypeService;

        public RepairTypeController(IRepairTypeService repairTypeService)
        {
            _repairTypeService = repairTypeService;
        }

        /// <summary>
        /// Add multiple repair type models
        /// </summary>
        /// <param name="request">Request with collection of repair type names</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Collection of added repair types</returns>
        [HttpPost("addRepairType")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType(typeof(IEnumerable<RepairType>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddRepairType([FromBody] AddRepairTypeRequest request, CancellationToken cancellationToken)
        {
            var result = await _repairTypeService.AddRepairTypeAsync(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get repair type with given id
        /// </summary>
        /// <param name="id">Repair type id</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Repair type model with given id</returns>
        [HttpGet("getRepairType/{id}")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType(typeof(RepairTypeModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRepairType(int id, CancellationToken cancellationToken)
        {
            var result = await _repairTypeService.GetRepairTypeAsync(id, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get list of all repair types or list of repair types with given repair type name
        /// </summary>
        /// <param name="repairTypeName">String for make a predicate</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>List of all repair type models matching predicate</returns>
        [HttpGet("getRepairTypes")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType(typeof(List<RepairTypeModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRepairTypes([FromQuery] string repairTypeName, CancellationToken cancellationToken)
        {
            var result = await _repairTypeService.GetRepairTypesAsync(repairTypeName, cancellationToken);

            return Ok(result);
        }
    }
}
