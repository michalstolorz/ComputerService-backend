using ComputerService.Data.Models;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Dto.Request
{
    public class CreateRepairRequest
    {
        public ICollection<RequiredRepairType> RequiredRepairTypes { get; set; }
    }
}
