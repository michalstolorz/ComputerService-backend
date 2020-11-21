using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerService.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairController : ControllerBase
    {
        private readonly IRepairService _repairService;

        public RepairController(IRepairService repairService)
        {
            _repairService = repairService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("getRepair/{id}")]
        public async Task<IActionResult> GetRepair(int id, CancellationToken cancellationToken)
        {
            var result = await _repairService.GetRepairAsync(id, cancellationToken);

            return Ok(result);
        }

        //[HttpGet("getAllRepairs")]
        //public async Task<IActionResult> GetAllRepairs()
        //{

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("addRepair")]
        public async Task<IActionResult> CreateRepair(CancellationToken cancellationToken)
        {
            var resultId = await _repairService.CreateRepairAsync(cancellationToken);

            return Ok(resultId);
        }

        //[HttpPut("updateRepair")]
        //public async Task<IActionResult> UpdateRepair()
        //{

        //}
    }
}
