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
using System.Net;
using ComputerService.Core.Models;

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
        /// Get repair by given id
        /// </summary>
        /// <param name="id">Repair id</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>GetRepairDetailsResponse</returns>
        [HttpGet("getRepair/{id}")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType(typeof(GetRepairDetailsResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRepair(int id, CancellationToken cancellationToken)
        {
            var result = await _repairService.GetRepairAsync(id, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get all repairs which match the predicate
        /// </summary>
        /// <param name="request">Request with params for create a predicate</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Collection of GetRepairsResponse</returns>
        [HttpGet("getRepairs")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType(typeof(IEnumerable<GetRepairsResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRepairs([FromQuery] GetRepairsRequest request, CancellationToken cancellationToken)
        {
            var result = await _repairService.GetRepairsAsync(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get repair by given id for currently logged customer
        /// </summary>
        /// <param name="id">Repair id</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>GetRepairDetailsResponse</returns>
        [HttpGet("getCustomerRepair/{id}")]
        [Authorize(Roles = "Admin, Customer")]
        [ProducesResponseType(typeof(GetRepairDetailsResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomerRepair(int id, CancellationToken cancellationToken)
        {
            var result = await _repairService.GetCustomerRepairAsync(id, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get all repairs for currently logged customer
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Collection of GetRepairsResponse</returns>
        [HttpGet("getCustomerRepairs")]
        [Authorize(Roles = "Admin, Customer")]
        [ProducesResponseType(typeof(IEnumerable<GetRepairsResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomerRepairs(CancellationToken cancellationToken)
        {
            var result = await _repairService.GetCustomerRepairsAsync(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get collection of repairs with statuses: New, InProgress, ForCustomerApproval
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Concated list of GetRepairsResponse of all repairs with all 3 repair statuses</returns>
        [HttpGet("getRepairsForAssign")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType(typeof(IEnumerable<GetRepairsResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRepairsForAssign(CancellationToken cancellationToken)
        {
            var listNewRepairs = await _repairService.GetRepairsByStatusAsync((int)EnumStatus.New, cancellationToken);
            var listInProgressRepairs = await _repairService.GetRepairsByStatusAsync((int)EnumStatus.InProgress, cancellationToken);
            var listForCustomerApprovalRepairs = await _repairService.GetRepairsByStatusAsync((int)EnumStatus.ForCustomerApproval, cancellationToken);

            var finalList = listNewRepairs.Concat(listInProgressRepairs).Concat(listForCustomerApprovalRepairs).ToList();

            return Ok(finalList);
        }

        /// <summary>
        /// Get collection of repairs with statuses: New, InProgress, ForCustomerApproval and non-zero repair cost
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Concated list of GetRepairsResponse of all repairs with all 3 repair statuses non-zero repair cost</returns>
        [HttpGet("getRepairsForInvoices")]
        [Authorize(Roles = "Admin, Boss")]
        [ProducesResponseType(typeof(IEnumerable<GetRepairsResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRepairsForInvoices(CancellationToken cancellationToken)
        {
            var result = await _repairService.GetRepairsForInvoicesAsync(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Add repair with params given in request
        /// </summary>
        /// <param name="request">Request with customer id, repair description and repair type ids</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Id of newly added reapair</returns>
        [HttpPost("addRepair")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddRepair(AddRepairRequest request, CancellationToken cancellationToken)
        {
            var resultId = await _repairService.AddRepairAsync(request, cancellationToken);

            await _requiredRepairTypeService.AssignRepairTypeToRepairAsync(new AssignRepairTypeToRepairRequest { RepairTypeIds = request.RepairTypeIds, RepairId = resultId }, cancellationToken);

            await _employeeRepairService.AddEmployeeRepair(resultId, cancellationToken);

            return Ok(resultId);
        }

        /// <summary>
        /// Update repair description by new description in request
        /// </summary>
        /// <param name="request">Request with repair id and new description</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns></returns>
        [HttpPut("updateRepairDescription")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateRepairDescription([FromBody] UpdateRepairDescriptionRequest request, CancellationToken cancellationToken)
        {
            await _repairService.UpdateRepairDescriptionAsync(request, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Update repair cost in repair model
        /// </summary>
        /// <param name="request">Request with repair id and repair cost</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns></returns>
        [HttpPut("evaluateRepairCost")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> EvaluateRepairCost([FromBody] EvaluateRepairCostRequest request, CancellationToken cancellationToken)
        {
            await _repairService.EvaluateRepairCostAsync(request, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Update repair status in repair model
        /// </summary>
        /// <param name="request">Request with repair id and repair status</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Updated repair model</returns>
        [HttpPut("updateRepairStatus")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType(typeof(RepairModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateRepairStatus([FromBody] UpdateRepairStatusRequest request, CancellationToken cancellationToken)
        {
            var result = await _repairService.UpdateRepairStatusAsync(request, cancellationToken);

            return Ok(result);
        }
    }
}
