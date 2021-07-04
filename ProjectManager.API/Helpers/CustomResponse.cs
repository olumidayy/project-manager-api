using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProjectManager.API.Response
{
    public class CustomResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }

        public CustomResponse(string status, string message, dynamic data = null)
        {
            Message = message;
            Status = status;
            Data = data;
        }

        public CustomResponse(ModelStateDictionary modelState)
        {
            var message = string.Join(" | ", modelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            Message = message;
            Status = "error";
        }
    }
}