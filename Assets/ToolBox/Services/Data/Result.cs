 
namespace ToolBox.Services.Data
{
    public readonly struct Result
    {
        public bool Success { get; }
        public string Error { get; }

        public Result(bool success, string error = null)
        {
            Success = success;
            Error = error;
        }

        public static Result Ok() => new Result(true);
        public static Result Fail(string error) => new Result(false, error);
    }
    
    public readonly struct Result<T>
    {
        public bool Success { get; }
        public string Error { get; }
        public T Value { get; }

        public Result(T value)
        {
            Success = true;
            Error = null;
            Value = value;
        }

        public Result(string error)
        {
            Success = false;
            Error = error;
            Value = default;
        }

        public static Result<T> Ok(T value) => new Result<T>(value);
        public static Result<T> Fail(string error) => new Result<T>(error);
    }
}