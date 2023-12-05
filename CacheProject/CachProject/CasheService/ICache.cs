
namespace CachProject.CashService
{
    public interface ICache
    {
        Task<T> GetOrCreateAsync<T>(string key, Func<string, Task<T>> func, string strParameter);
        Task<T> GetOrCreateAsync<T>(string key, Func<int, Task<T>> func, int intParameter);

        //Using generic parameter ,So the previous two functions are not needed
        Task<T> GetOrCreateWithGenericAsync<T, TParameter>(string key, Func<TParameter, Task<T>> func, TParameter parameter);
        object GetMemoryValueByKey(string key);
    }
}
