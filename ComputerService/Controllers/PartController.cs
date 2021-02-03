using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ComputerService.Core.Dto.Request;
using ComputerService.Core.Interfaces.Services;
using ComputerService.Core.Models;
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
        /// Add new part with given params
        /// </summary>
        /// <param name="request">Request with part name, quantity and part bought price</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>New added part model</returns>
        [HttpPost("addPart")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType(typeof(PartModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddPart([FromBody] AddPartRequest request, CancellationToken cancellationToken)
        {
            var result = await _partService.AddPartAsync(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get part with given id
        /// </summary>
        /// <param name="id">Part id</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Part model with given id</returns>
        [HttpGet("getPart/{id}")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType(typeof(PartModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPart(int id, CancellationToken cancellationToken)
        {
            var result = await _partService.GetPartAsync(id, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get list of all parts or list of parts with given part name
        /// </summary>
        /// <param name="partName">String for make a predicate</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>List of all part models matching predicate</returns>
        [HttpGet("getParts")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType(typeof(List<PartModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetParts(string partName, CancellationToken cancellationToken)
        {
            var result = await _partService.GetPartsAsync(partName, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Add given quantity to given part
        /// </summary>
        /// <param name="request">Request with part id and quantity</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns></returns>
        [HttpPut("supplyPart")]
        [Authorize(Roles = "Admin, Employee, Boss")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SupplyPart(SupplyPartRequest request, CancellationToken cancellationToken)
        {
            await _partService.SupplyPartAsync(request, cancellationToken);

            return Ok();
        }
    }
}
