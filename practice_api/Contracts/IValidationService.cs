using System.Collections.Generic;
using System.Threading.Tasks;

namespace practice_api.Contracts
{
    public interface IValidationService
    {
        Task<Dictionary<string, string[]>?> ValidateAsync<T>(T dto) where T : class;
        Dictionary<string, string[]>? Validate<T>(T dto) where T : class;
    }
}
