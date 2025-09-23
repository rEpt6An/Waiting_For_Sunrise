namespace Assets.C_.common.common
{
    public class Re
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public object Data { get; private set; }

        private Re(bool isSuccess, string message, object data) 
        {
            Success = isSuccess;
            Message = message;
            Data = data;
        }  

        public static Re Ok()
        {
            return new(true, "Ok", null);
        }

        public static Re Ok(object data)
        {
            return new(true, "Ok", data);
        }

        public static Re Fail(string message)
        {
            return new(false, message, null);
        }

        public bool IsSuccess()
        {
            return Success;
        }
    }
}