using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAuction.Application.DTOs 
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string? ErrorMessage { get; set; }

        /// <summary>Alias for API responses (controllers use <c>result.Error</c>).</summary>
        public string Error => ErrorMessage ?? string.Empty;

        public static Result<T> Success(T data) => new Result<T> { IsSuccess = true, Data = data };
        public static Result<T> Failure(string error) => new Result<T> { IsSuccess = false, ErrorMessage = error };
    }
}
