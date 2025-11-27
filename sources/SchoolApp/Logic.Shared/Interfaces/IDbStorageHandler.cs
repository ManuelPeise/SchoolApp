using Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface IDbStorageHandler<TRequest> where TRequest : class
    {
        Task<bool> StoreData(
            TRequest model,
            string currentUser,
            Func<TRequest, Task<ResponseBaseModel>> mySqlUpdateFunction,
            Func<TRequest, string, bool, Task> sqLiteUpdateFunction);
    }
}
