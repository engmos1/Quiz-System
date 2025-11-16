using DataAccess.Models.Enums;

namespace ExaminationSystem.ViewModels
{
    public class ResponseViewModel<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } 
        public ErrorCode ErrorCode { get; set; }

        public static ResponseViewModel<T> Success(T data)
        {
           return new ResponseViewModel<T> 
           { 
               Data = data,
               IsSuccess = true,    
               Message = "Success" ,  
               ErrorCode = ErrorCode.NoError,
           };
        }

        public static ResponseViewModel<T> Failure(string message, ErrorCode errorCode)
        {
            return new ResponseViewModel<T>
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = errorCode,
            };
        }

    }
}
