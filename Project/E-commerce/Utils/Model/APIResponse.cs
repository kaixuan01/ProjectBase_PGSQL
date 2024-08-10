using Microsoft.AspNetCore.Mvc;
using System;

namespace E_commerce.Model
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public string? ErrorCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public ApiResponse(bool success, string message, T? data, string? errorCode = null)
        {
            Success = success;
            Message = message;
            Data = data;
            ErrorCode = errorCode;
        }

        public static ApiResponse<T> CreateSuccessResponse(T data, string message = "Request was successful")
        {
            return new ApiResponse<T>(true, message, data);
        }

        public static ApiResponse<T> CreateErrorResponse(string message, string? errorCode = null)
        {
            return new ApiResponse<T>(false, message, default(T), errorCode);
        }
    }
}
