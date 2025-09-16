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
    public class PaymentController : Controller
    {
        private readonly TransactionService _transactionService;

        public PaymentController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("credit")]
        public async Task<ResponseModel> CreditFund(PaymentRequest paymentRequest)
        {
            return await _transactionService.CreditFund(paymentRequest);
        }

        [HttpPost("debit/{userId}")]
        public async Task<ResponseModel> DebitFund(DebitPaymentRequest paymentRequest)
        {
            return await _transactionService.DebitFund(paymentRequest);
        }

        [HttpPut("transfer")]
        public async Task<ResponseModel> TransferFund([FromBody] PaymentRequest paymentRequest)
        {
            return await _transactionService.TransferAsync(paymentRequest);
        }

        [HttpPut("refund/{userId}/{transactionId}")]
        public async Task<ResponseModel> ReFund([FromRoute] long userId, [FromRoute] string transactionId)
        {
            return await _transactionService.ReFund(userId, transactionId);
        }

        [HttpPut("partial-refund/{userId}/{transactionId}")]
        public async Task<ResponseModel> PartialRefund([FromRoute] long userId, [FromRoute] string transactionId)
        {
            return await _transactionService.PartialRefund(userId, transactionId);
        }

        [HttpGet("checkbalance/{userId}")]
        public async Task<ResponseModel> CheckBalance([FromRoute] long userId)
        {
            return await _transactionService.CheckBalance(userId);
        }

        [HttpPut("transaction/{userId}")]
        public async Task<ResponseModel> GetTransactionDetail([FromRoute] long userId, [FromBody] PageRequestModel pageRequestModel)
        {
            return await _transactionService.GetTransactionDetails(userId, pageRequestModel);
        }
    }
}
