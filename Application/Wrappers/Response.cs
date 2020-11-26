using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data, string message = null)
        {
            succeeded = true;
            this.message = message;
            this.data = data;
        }
        public Response(string _message)
        {
            succeeded = false;
            message = _message;
        }
        public bool succeeded { get; set; }
        public string message { get; set; }
        public List<string> errors { get; set; }
        public T data { get; set; }
    }
}
