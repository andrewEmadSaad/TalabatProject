using System;

namespace Talabat.Apis.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string  Massege { get; set; }

        public ApiResponse(int statusCode, string massege=null)
        {
            StatusCode = statusCode;
            Massege = massege ?? GetDefaultMessgeForStatusCode(statusCode);
        }

        private string GetDefaultMessgeForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401=>"Authrized ,you are not",
                404 => "Resource was not Found",
                500=> "Errors are the path to the dark side , Errors lead to anger ,Anger Leads to hate , Hate leads to career change",
                _=>null

            };
        }
    }
}
