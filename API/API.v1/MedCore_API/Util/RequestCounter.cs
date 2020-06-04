using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace MedCore_API.Util
{
    public class RequestCounter
    {
        private readonly RequestDelegate _next;
        static object lockObj = new object();

        public RequestCounter(RequestDelegate next)
        {
            _next = next;
            Data = new Dictionary<int, int>();
        }
    
        static int lastMinute = -1;
        static Dictionary<int, int> Data;

        public static void Register()
        {
            try
            {
                var minute = DateTime.Now.Minute;

                lock(lockObj)
                {
                    if(Data.ContainsKey(minute))
                    {
                        if(lastMinute == minute)
                        {
                            var value = Data[minute];
                            Data[minute] = value + 1;
                        }
                        else
                            Data[minute] = 1;
                    }
                    else
                        Data.Add(minute, 1);
                }

                lastMinute = minute;
            }
            catch
            {}
        }

        public static double GetCounter()
        {
            if(Data.Keys.Count() == 0)
                return 0;

            var minute = DateTime.Now.Minute;

            if(Data.Keys.Count > 1) 
            {
                if(minute == 0)
                    minute = Data.Keys.Max();
                else
                    minute = Data.Keys.Where(k => k < minute).Max();
            }

            return Data[minute];
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Register();
            await _next(context);
        }
    }

    public static class RequestCounterExtensions 
    {
         public static IApplicationBuilder UseRequestCounter(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCounter>();
        }
    }
}