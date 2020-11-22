using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerService.Core.Dto.Request;
using ComputerService.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairTypeController : ControllerBase
    {
        private readonly IRepairTypeService _repairType;

        public RepairTypeController(IRepairTypeService repairType)
        {
            _repairType = repairType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("addRepairType")]
        public async Task<IActionResult> AddRepairType([FromBody] AddRepairTypeRequest request, CancellationToken cancellationToken)
        {
            var result = await _repairType.AddRepairTypeAsync(request, cancellationToken);

            return Ok(result);
        }
    }
}
