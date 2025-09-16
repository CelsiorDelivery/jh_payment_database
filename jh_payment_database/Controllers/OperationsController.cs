using jh_payment_auth.Models;
using jh_payment_database.Entity;
using jh_payment_database.Model;
using jh_payment_database.Service;
using Microsoft.AspNetCore.Mvc;

namespace jh_payment_database.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/perops/[Controller]")]
    public class OperationsController : Controller
    {
        private readonly OperationService _operationService;

        public OperationsController(OperationService operationService)
        {
            _operationService = operationService;
        }

        [HttpGet("create-sample-records")]
        public async Task<ResponseModel> CreateSampleRecords()
        {
            return await _operationService.CreateSample();
        }
    }
}
