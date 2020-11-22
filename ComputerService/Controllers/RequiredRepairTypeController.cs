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
    public class RequiredRepairTypeController : ControllerBase
    {
        private readonly IRequiredRepairTypeService _requiredRepairTypeService;

        public RequiredRepairTypeController(IRequiredRepairTypeService requiredRepairTypeService)
        {
            _requiredRepairTypeService = requiredRepairTypeService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="repairId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("assignRepairTypeToRepair/{repairId}")]
        public async Task<IActionResult> AssignRepairTypeToRepair([FromBody] AssignRepairTypeToRepairRequest request, int repairId, CancellationToken cancellationToken)
        {
            await _requiredRepairTypeService.AssignRepairTypeToRepairAsync(request, repairId, cancellationToken);

            return Ok();
        }
    }
}
