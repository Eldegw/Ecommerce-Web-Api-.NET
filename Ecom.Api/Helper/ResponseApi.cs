namespace Ecom.Api.Helper
{
    public class ResponseApi
    {
        public ResponseApi(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageFromStatusCode(statusCode);
        }

        private string GetMessageFromStatusCode(int statuscode)
        {
            return statuscode switch
            {
                200=>"Done",
                400=>"Bad Request",
                401=>"Un Authorized",
                404=>"Not Found Resource",
                500=>"Server Error",
                _ =>null,
            };
        }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
