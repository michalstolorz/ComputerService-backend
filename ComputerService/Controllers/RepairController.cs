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
    public class RepairController : ControllerBase
    {
        private readonly IRepairService _repairService;
        private readonly IRequiredRepairTypeService _requiredRepairTypeService;
        private readonly IEmployeeRepairService _employeeRepairService;

        public RepairController(IRepairService repairService, IRequiredRepairTypeService requiredRepairTypeService, IEmployeeRepairService employeeRepairService)
        {
            _repairService = repairService;
            _requiredRepairTypeService = requiredRepairTypeService;
            _employeeRepairService = employeeRepairService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("getRepair/{id}")]
        [Authorize(Roles = "Admin, Employee, Customer")]
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
        [Authorize(Roles = "Admin, Employee, Customer, Boss")]
        public async Task<IActionResult> GetRepairs([FromQuery] GetRepairsRequest request, CancellationToken cancellationToken)
        {
            var result = await _repairService.GetRepairsAsync(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("getRepairsForAssign")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> GetRepairsForAssign(CancellationToken cancellationToken)
        {
            List<List<GetRepairsResponse>> list = new List<List<GetRepairsResponse>>
            {
                await _repairService.GetRepairsByStatusAsync((int)EnumStatus.New, cancellationToken),
                await _repairService.GetRepairsByStatusAsync((int)EnumStatus.InProgress, cancellationToken),
                await _repairService.GetRepairsByStatusAsync((int)EnumStatus.ForCustomerApproval, cancellationToken)
            };

            return Ok(list);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="customerId"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //[HttpGet("getCustomerRepairs/{customerId}")]
        //[Authorize(Roles = "Admin, Customer, Employee, Boss")]
        //public async Task<IActionResult> GetCustomerRepairs(int customerId, CancellationToken cancellationToken)
        //{
        //    var result = await _repairService.GetCustomerRepairsAsync(customerId, cancellationToken);

        //    return Ok(result);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("addRepair")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> CreateRepair(AddRepairRequest request, CancellationToken cancellationToken)
        {
            var resultId = await _repairService.AddRepairAsync(request, cancellationToken);

            await _requiredRepairTypeService.AssignRepairTypeToRepairAsync(new AssignRepairTypeToRepairRequest { RepairTypeIds = request.RepairTypeIds }, resultId, cancellationToken);

            await _employeeRepairService.AddEmployeeRepair(resultId, cancellationToken);

            return Ok(resultId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("updateRepairDescription")]
        [Authorize(Roles = "Admin, Employee")]
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
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> EvaluateRepairCost([FromBody] EvaluateRepairCostRequest request, CancellationToken cancellationToken)
        {
            await _repairService.EvaluateRepairCostAsync(request, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("updateRepairStatus")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> UpdateRepairStatus([FromBody] UpdateRepairStatusRequest request, CancellationToken cancellationToken)
        {
            var result = await _repairService.UpdateRepairStatusAsync(request, cancellationToken);

            return Ok(result);
        }

    }
}
