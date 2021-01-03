using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerService.Core.Dto.Request;
using ComputerService.Core.Interfaces.Services;
using ComputerService.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComputerService.Core.Dto.Response;

namespace ComputerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsedPartController : ControllerBase
    {
        private readonly IUsedPartService _usedPartService;

        public UsedPartController(IUsedPartService usedPartService)
        {
            _usedPartService = usedPartService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("updateRepairUsedParts")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> UpdateRepairUsedParts([FromBody] UpdateRepairUsedPartsRequest request, CancellationToken cancellationToken)
        {
            await _usedPartService.UpdateRepairUsedPartsAsync(request, cancellationToken);

            return Ok();
        }
    }
}
