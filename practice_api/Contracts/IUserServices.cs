using practice_api.Models.Auth;
using practice_api.Models.Shared;
using practice_api.Models.Users;

namespace practice_api.Contracts
{
    public interface IUserServices
    {

        public Task<TableResponse<UserDto>> Table(TableRequest request);

        public Task<ServiceResult> Create(UserCreateDto request);
    }
}
