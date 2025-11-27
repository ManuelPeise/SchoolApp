using Shared.Models;
using System.Security.Claims;

namespace Logic.Shared.Interfaces
{
    public interface ILogicBase
    {
        public CurrentUser CurrentUser { get; }
    }
}
