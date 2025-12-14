namespace HolookorBackend.Core.Application.Responses
{
    public class BaseResponse<T>
    {
        public bool Status { get; init; }
        public string? Message { get; init; }
        public T? Data { get; init; }
        public int TotalCount { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}
