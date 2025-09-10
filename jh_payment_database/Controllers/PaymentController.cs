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

        [HttpPut("debit/{userId}")]
        public async Task<ResponseModel> DebitFund(PaymentRequest paymentRequest)
        {
            return await _transactionService.DebitFund(paymentRequest);
        }

        [HttpGet("refund")]
        public async Task<ResponseModel> TransferFund(long senderId, long receiverId, decimal amount)
        {
            return await _transactionService.TransferAsync(senderId, receiverId, amount);
        }

        [HttpGet("checkbalance/{userId}")]
        public async Task<ResponseModel> CheckBalance([FromRoute] long userId)
        {
            return await _transactionService.CheckBalance(userId);
        }
    }
}
