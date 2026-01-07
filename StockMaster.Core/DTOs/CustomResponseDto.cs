using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StockMaster.Core.DTOs
{
    public class CustomResponseDto<T>
    {
        public T Data { get; set; }

        [JsonIgnore] // Bu sadece kod içinde lazım, istemciye gitmesin
        public int StatusCode { get; set; }

        public List<string> Errors { get; set; }

        // Başarılı (Data var)
        public static CustomResponseDto<T> Success(int statusCode, T data)
        {
            return new CustomResponseDto<T> { Data = data, StatusCode = statusCode };
        }

        // Başarılı (Data yok - Örn: Update/Delete)
        public static CustomResponseDto<T> Success(int statusCode)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode };
        }

        // Hatalı
        public static CustomResponseDto<T> Fail(int statusCode, List<string> errors)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = errors };
        }

        public static CustomResponseDto<T> Fail(int statusCode, string error)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = new List<string> { error } };
        }
    }
}
