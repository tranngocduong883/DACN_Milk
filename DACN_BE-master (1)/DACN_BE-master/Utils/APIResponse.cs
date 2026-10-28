namespace DACN_H_P.Utils
{
    public class APIResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data {  get; set; }
        public static APIResponse<T> OK(string message, T data)
        {
            return new APIResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static APIResponse<T> Fail(string message)
        {
            return new APIResponse<T>
            {
                Success = false,
                Message = message,
            };
        }
    }
}
