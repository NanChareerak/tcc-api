namespace Tcc.Application.Dtos.Common;

public static class ResultCode
{
    public const string Success = "SUCCESS";
    public const string Created = "CREATED";
    public const string Updated = "UPDATED";
    public const string Deleted = "DELETED";

    public const string ValidationError = "VALIDATION_ERROR";
    public const string InvalidCredentials = "INVALID_CREDENTIALS";
    public const string DataNotFound = "DATA_NOT_FOUND";
    public const string DuplicateData = "DUPLICATE_DATA";
    public const string BusinessError = "BUSINESS_ERROR";
    public const string Unauthorized = "UNAUTHORIZED";
    public const string Forbidden = "FORBIDDEN";
    public const string InternalServerError = "INTERNAL_SERVER_ERROR";
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(
        T data,
        string message = "Success",
        string code = ResultCode.Success)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Code = code,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Fail(
        string code,
        string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Code = code,
            Message = message,
            Data = default
        };
    }
}

public class ApiPagedResponse<T>
{
    public bool Success { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public List<T> Datas { get; set; } = new();
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public int TotalPages { get; set; }

    public static ApiPagedResponse<T> Ok(
        List<T> datas,
        int pageIndex,
        int pageSize,
        int total,
        string message = "Success",
        string code = ResultCode.Success)
    {
        return new ApiPagedResponse<T>
        {
            Success = true,
            Code = code,
            Message = message,
            Datas = datas,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Total = total,
            TotalPages = pageSize <= 0 ? 0 : (int)Math.Ceiling(total / (double)pageSize)
        };
    }

    public static ApiPagedResponse<T> Empty(
        int pageIndex,
        int pageSize,
        string message = "No data found",
        string code = ResultCode.DataNotFound)
    {
        return new ApiPagedResponse<T>
        {
            Success = false,
            Code = code,
            Message = message,
            Datas = new List<T>(),
            PageIndex = pageIndex,
            PageSize = pageSize,
            Total = 0,
            TotalPages = 0
        };
    }
}

public class ApiErrorResponse
{
    public bool Success { get; set; } = false;
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public object? Errors { get; set; }

    public static ApiErrorResponse Fail(
        string code,
        string message,
        object? errors = null)
    {
        return new ApiErrorResponse
        {
            Success = false,
            Code = code,
            Message = message,
            Errors = errors
        };
    }
}

public class PagedRequest
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Keyword { get; set; }

    public int Skip => (PageIndex - 1) * PageSize;
    public int Take => PageSize;
}

public class BusinessException : Exception
{
    public string Code { get; }

    public BusinessException(string code, string message) : base(message)
    {
        Code = code;
    }
}