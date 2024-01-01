namespace CachProject.CashService
{
    public interface ICache
    {
        Task<TResult> GetOrCreateAsync<TResult, TParameter>(string key, Func<TParameter, Task<TResult>> func, TParameter parameter);
        Task<TResult> GetOrCreateAsync_UsingBuiltInFunction<TResult, TParameter>(string key, Func<TParameter, Task<TResult>> func, TParameter parameter);
    }
}
