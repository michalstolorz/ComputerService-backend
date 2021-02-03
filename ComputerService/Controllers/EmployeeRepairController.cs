using AutoMapper;
using ComputerService.Common.Enums;
using ComputerService.Core.Dto.Request;
using ComputerService.Core.Dto.Response;
using ComputerService.Core.Interfaces.Services;
using ComputerService.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeRepairController : ControllerBase
    {
        private readonly IEmployeeRepairService _employeeRepairService;
        private readonly IRepairService _repairService;

        public EmployeeRepairController(IEmployeeRepairService employeeRepairService, IRepairService repairService)
        {
            _employeeRepairService = employeeRepairService;
            _repairService = repairService;
        }

        /// <summary>
        /// Adds EmployeeRepair with given repairId and current logged employee. Changes repair status to InProgress
        /// </summary>
        /// <param name="repairId">Repair id to which assign employee</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Returns new added EmployeeRepair id</returns>
        [HttpPost("assignEmployeeToRepair/{repairId}")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AssignEmployeeToRepair(int repairId, CancellationToken cancellationToken)
        {
            var result = await _employeeRepairService.AddEmployeeRepair(repairId, cancellationToken);

            var repair = await _repairService.GetRepairAsync(repairId, cancellationToken);

            if (repair.Status == EnumStatus.New)
                await _repairService.UpdateRepairStatusAsync(new UpdateRepairStatusRequest { StatusId = (int)EnumStatus.InProgress, RepairId = repairId }, cancellationToken);

            return Ok(result);
        }
    }
}
