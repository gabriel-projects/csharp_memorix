using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.Models
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public string? TraceId { get; set; }

        public static Result<T> Ok(T data, string? message = null, string? traceId = null)
            => new() { Success = true, Data = data, Message = message, TraceId = traceId };

        public static Result<T> Fail(string message, string? traceId = null)
            => new() { Success = false, Message = message, TraceId = traceId };
    }
}
