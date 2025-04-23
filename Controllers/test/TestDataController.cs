using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Models;
using MyFirstApi.Services;

namespace MyFirstApi.Controllers
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
        public async Task<ApiResponse<List<T_sys_test>>> GetTestData()
        {
            try
            {
                var testData = await _testService.GetTestData();
                return new ApiResponse<List<T_sys_test>>(testData);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<T_sys_test>>(500, ex.Message);
            }
        }
    }
}    