using System.Collections;
using System.Collections.Generic;

namespace Talabat.Apis.Errors
{
    public class ApiValidationErorrResponse:ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationErorrResponse():base(400)
        {

        }
    }
}
