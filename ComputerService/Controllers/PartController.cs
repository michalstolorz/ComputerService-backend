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
    public class PartController : ControllerBase
    {
        private readonly IPartService _partService;

        public PartController(IPartService partService)
        {
            _partService = partService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("addPart")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        public async Task<IActionResult> AddPart([FromBody] AddPartRequest request, CancellationToken cancellationToken)
        {
            var result = await _partService.AddPartAsync(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("getPart/{id}")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        public async Task<IActionResult> GetPart(int id, CancellationToken cancellationToken)
        {
            var result = await _partService.GetPartAsync(id, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("getParts")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        public async Task<IActionResult> GetParts(string partName, CancellationToken cancellationToken)
        {
            var result = await _partService.GetPartsAsync(partName, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("supplyPart")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        public async Task<IActionResult> SupplyPart(SupplyPartRequest request, CancellationToken cancellationToken)
        {
            await _partService.SupplyPartAsync(request, cancellationToken);

            return Ok();
        }
    }
}
