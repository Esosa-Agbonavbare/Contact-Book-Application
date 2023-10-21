using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Model.DTOs
{
    public class Response<T>
    {
        public string Message { get; set; }
        public int ResponseCode { get; set; }
        public T Data { get; set; }


        public Response<T> Success(string message, int statusCode, T data)
        {
            return new Response<T>
            {
                Message = message,
                ResponseCode = statusCode,
                Data = data
            };
        }

        public Response<T> Failed(string message, int statusCode)
        {
            return new Response<T>
            {
                Message = message,
                ResponseCode = statusCode,
            };
        }

        public Response<T> Failed(string message)
        {
            return new Response<T>
            {
                Message = message,
            };
        }

        public Response<T> Success(string message, int statusCode)
        {
            return new Response<T>
            {
                Message = message,
                ResponseCode = statusCode,
            };
        }
    }
}
