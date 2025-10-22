using Api.GRRInnovations.Memorix.Application.Wrappers.In;
using Api.GRRInnovations.Memorix.Domain.Interfaces;

namespace Api.GRRInnovations.Memorix.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<IUser> RegisterAsync(WrapperInRegister wrapperInRegister);
    }
}
