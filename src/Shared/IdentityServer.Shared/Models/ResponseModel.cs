namespace IdentityServer.Shared.Models;
public class ResponseModel<T>
{
    public T Data { get; set; }
    public int StatusCode { get; set; }
    public bool IsSuccessful { get; set; }
    public List<ErrorModel> Errors { get; set; }

    #region Successes

    #region Sync
    public static ResponseModel<T> Success()
    {
        return new ResponseModel<T> { StatusCode = 200, IsSuccessful = true };
    }

    public static ResponseModel<T> Success(int statusCode)
    {
        return new ResponseModel<T> { StatusCode = statusCode, IsSuccessful = true };
    }

    public static ResponseModel<T> Success(T data)
    {
        return new ResponseModel<T> { Data = data, StatusCode = 200, IsSuccessful = true };
    }

    public static ResponseModel<T> Success(T data, int statusCode)
    {
        return new ResponseModel<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
    }
    #endregion

    #region Async
    public static Task<ResponseModel<T>> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static Task<ResponseModel<T>> SuccessAsync(T data)
    {
        return Task.FromResult(Success(data));
    }

    public static Task<ResponseModel<T>> SuccessAsync(int statusCode)
    {
        return Task.FromResult(Success(statusCode));
    }

    public static Task<ResponseModel<T>> SuccessAsync(T data, int statusCode)
    {
        return Task.FromResult(Success(data, statusCode));
    }
    #endregion

    #endregion

    #region Failures

    #region Sync
    public static ResponseModel<T> Failure(List<ErrorModel> errors, int statusCode)
    {
        return new ResponseModel<T> { Errors = errors, StatusCode = statusCode, IsSuccessful = false };
    }

    public static ResponseModel<T> Failure(ErrorModel error, int statusCode)
    {
        return new ResponseModel<T> { Errors = new List<ErrorModel>() { error }, StatusCode = statusCode, IsSuccessful = false };
    }

    public static ResponseModel<T> Failure(int code, string message, string description, int statusCode)
    {
        List<ErrorModel> errors = new()
            {
                new ErrorModel(code, message, description)
            };
        return new ResponseModel<T> { Errors = errors, StatusCode = statusCode, IsSuccessful = false };
    }
    //TODO: Wrong error code
    public static ResponseModel<T> UserNotFound()
    {
        List<ErrorModel> errors = new()
            {
                new ErrorModel(1,"Invalid user","User not found")
            };
        return new ResponseModel<T> { Errors = errors, StatusCode = 404, IsSuccessful = false };
    }
    #endregion

    #region Async
    public static Task<ResponseModel<T>> FailureAsync(List<ErrorModel> errors, int statusCode)
    {
        return Task.FromResult(Failure(errors, statusCode));
    }

    public static Task<ResponseModel<T>> FailureAsync(ErrorModel error, int statusCode)
    {
        return Task.FromResult(Failure(error, statusCode));
    }

    public static Task<ResponseModel<T>> FailureAsync(int code, string message, string description, int statusCode)
    {
        return Task.FromResult(Failure(code, message, description, statusCode));
    }
    #endregion

    #endregion
}