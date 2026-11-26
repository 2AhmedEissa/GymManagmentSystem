namespace GymManagmentSystem_BLL.Common.Results
{
    public class ServiceResult
    {
        public bool IsSuccess { get; protected set; }
        public string? ErrorMessage { get; protected set; }
        public string? PropertyName { get; protected set; }

        public static ServiceResult Success() => new() { IsSuccess = true };

        public static ServiceResult Failure(string errorMessage, string? propertyName = null) =>
            new() { IsSuccess = false, ErrorMessage = errorMessage, PropertyName = propertyName };
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; private set; }

        public static ServiceResult<T> Success(T data) =>
            new() { IsSuccess = true, Data = data };

        public new static ServiceResult<T> Failure(string errorMessage, string? propertyName = null) =>
            new() { IsSuccess = false, ErrorMessage = errorMessage, PropertyName = propertyName };
    }
}

