using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplicationforTest.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {

            // 加這行：輸出例外資訊到 Console
            Console.WriteLine($"❗例外錯誤：{context.Exception.Message}");
            Console.WriteLine($"🔍 追蹤：{context.Exception.StackTrace}");

            var error = new
            {
                Message = " 發生未預期的錯誤",
                Detail = context.Exception.Message
            };

            context.Result = new JsonResult(error)
            {
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }
    }
}
