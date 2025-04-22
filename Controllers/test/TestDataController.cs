using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models;
using MyWebApi.Services;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestDataController : ControllerBase
    {
        private readonly T_sys_testService _testService;

        public TestDataController(T_sys_testService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<ActionResult<List<T_sys_test>>> GetTestData()
        {
            var testData = await _testService.GetTestData();
            return Ok(testData);
        }
    }
}    