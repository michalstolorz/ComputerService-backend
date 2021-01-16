using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerService.Core.Dto.Request;
using ComputerService.Core.Interfaces.Services;
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
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("addRepairType")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        public async Task<IActionResult> AddRepairType([FromBody] AddRepairTypeRequest request, CancellationToken cancellationToken)
        {
            var result = await _repairTypeService.AddRepairTypeAsync(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("getRepairType/{id}")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        public async Task<IActionResult> GetRepairType(int id, CancellationToken cancellationToken)
        {
            var result = await _repairTypeService.GetRepairTypeAsync(id, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repairTypeName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("getRepairTypes")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        public async Task<IActionResult> GetRepairTypes([FromQuery] string repairTypeName, CancellationToken cancellationToken)
        {
            var result = await _repairTypeService.GetRepairTypesAsync(repairTypeName, cancellationToken);

            return Ok(result);
        }
    }
}
