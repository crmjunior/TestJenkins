using AutoMapper;
using MedCore_API.Util;
using MedCoreAPI.ViewModel.Base;
using Microsoft.AspNetCore.Mvc;

namespace MedCore_API.Controllers
{
    [ApiVersionNeutral]
    [ApiController]
    public class CounterController : BaseService
    {
        public CounterController(IMapper mapper) : base(mapper)
        { }

        [HttpGet("counter")]
        public double Counter()
        {
            return RequestCounter.GetCounter();
        }
    }
}