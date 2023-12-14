using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moonpig.PostOffice.Infrastructure.BusinessLogic;
using Moonpig.PostOffice.Model.Dto;

namespace Moonpig.PostOffice.Api.Controllers
{
    [ApiController]
    [Route("post-office-api/[controller]/[action]")]
    public class DespatchDateController : Controller
    {
        private readonly IDespatchLogic _despatchLogic;

        public DespatchDateController(IDespatchLogic despatchLogic)
        {
            _despatchLogic = despatchLogic;
        }

        [HttpPost]
        public async Task<OrderResponseDto> Get([FromBody] OrderRequestDto request)
        {
            return await _despatchLogic.GetDespatchDateAsync(request);
        }
    }
}
