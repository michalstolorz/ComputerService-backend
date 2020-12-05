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
    public class RepairController : ControllerBase
    {
        private readonly IRepairService _repairService;
        private readonly IRequiredRepairTypeService _requiredRepairTypeService;

        public RepairController(IRepairService repairService, IRequiredRepairTypeService requiredRepairTypeService)
        {
            _repairService = repairService;
            _requiredRepairTypeService = requiredRepairTypeService;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("getRepairs")]
        public async Task<IActionResult> GetRepairs([FromQuery] GetRepairsRequest request, CancellationToken cancellationToken)
        {
            var result = await _repairService.GetRepairsAsync(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("addRepair")]
        public async Task<IActionResult> CreateRepair(AddRepairRequest request, CancellationToken cancellationToken)
        {
            var resultId = await _repairService.AddRepairAsync(request, cancellationToken);

            await _requiredRepairTypeService.AssignRepairTypeToRepairAsync(new AssignRepairTypeToRepairRequest { RepairTypeIds = request.RepairTypeIds }, resultId, cancellationToken);

            return Ok(resultId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("updateRepairDescription")]
        public async Task<IActionResult> UpdateRepairDescription([FromBody] UpdateRepairDescriptionRequest request, CancellationToken cancellationToken)
        {
            await _repairService.UpdateRepairDescriptionAsync(request, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("evaluateRepairCost")]
        public async Task<IActionResult> EvaluateRepairCost([FromBody] EvaluateRepairCostRequest request, CancellationToken cancellationToken)
        {
            await _repairService.EvaluateRepairCostAsync(request, cancellationToken);

            return Ok();
        }

        //[HttpPut("updateRepair")]
        //public async Task<IActionResult> UpdateRepair()
        //{

        //}
    }
}
