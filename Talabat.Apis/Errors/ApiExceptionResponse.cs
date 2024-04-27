namespace Talabat.Apis.Errors
{
    public class ApiExceptionResponse:ApiResponse
    {
        public string Details { get; set; }
        public ApiExceptionResponse(int statusCode, string massege = null,string details=null):base( statusCode,  massege )
        {
            Details = details;
        }
    }
}
