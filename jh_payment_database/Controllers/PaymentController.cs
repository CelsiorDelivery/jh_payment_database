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

        [HttpPost("debit/{userEmail}")]
        public async Task<ResponseModel> DebitFund(DebitPaymentRequest paymentRequest)
        {
            return await _transactionService.DebitFund(paymentRequest);
        }

        [HttpPost("transfer")]
        public async Task<ResponseModel> TransferFund([FromBody] PaymentRequest paymentRequest)
        {
            return await _transactionService.TransferAsync(paymentRequest);
        }

        [HttpPost("transfer/card")]
        public async Task<ResponseModel> TransferCardFund([FromBody] CardPaymentRequest paymentRequest)
        {
            return await _transactionService.TransferCardAsync(paymentRequest);
        }

        [HttpPut("refund/{userEmail}/{transactionId}")]
        public async Task<ResponseModel> ReFund([FromRoute] string userEmail, [FromRoute] string transactionId)
        {
            return await _transactionService.ReFund(userEmail, transactionId);
        }

        [HttpPut("partial-refund/{userEmail}/{transactionId}")]
        public async Task<ResponseModel> PartialRefund([FromRoute] string userEmail, [FromRoute] string transactionId)
        {
            return await _transactionService.PartialRefund(userEmail, transactionId);
        }

        [HttpGet("checkbalance/{userEmail}")]
        public async Task<ResponseModel> CheckBalance([FromRoute] string userEmail)
        {
            return await _transactionService.CheckBalance(userEmail);
        }

        [HttpGet("transaction/{userEmail}")]
        public async Task<ResponseModel> GetTransactionDetail([FromRoute] string userEmail, [FromQuery] PageRequestModel pageRequestModel)
        {
            return await _transactionService.GetTransactionDetails(userEmail, pageRequestModel);
        }
    }
}
