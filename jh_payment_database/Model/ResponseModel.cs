using System.Net;

namespace jh_payment_database.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public dynamic ResponseBody { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode StatusCode { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { set; get; }

        public static ResponseModel Ok(dynamic reponse, string message = "No content")
        {
            return new ResponseModel
            {
                ResponseBody = reponse,
                StatusCode = HttpStatusCode.OK,
                Message = message ?? "Successfully completed"
            };
        }

        public static ResponseModel BadRequest(string message)
        {
            return new ResponseModel
            {
                ResponseBody = "No content",
                StatusCode = HttpStatusCode.BadRequest,
                Message = message ?? "Successfully completed"
            };
        }
    }
}
