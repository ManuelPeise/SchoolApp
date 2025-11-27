using Data.Entities;
using Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface IStorageHandler<TRequest, TResponse> where TRequest : AEntityBase where TResponse : ResponseBaseModel
    {
        Task<ResponseBaseModel?> StoreData(string url, TRequest model);
    }
}
    
