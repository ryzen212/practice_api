using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace practice_api.Controllers.v2
{
    //[Authorize]
    public class BaseController : Controller
    {
        protected Dictionary<string, string[]> HandleValidationErrors()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        k => k.Key,
                        v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return errors; // You can return different response types based on your needs
            }

            return null; // No validation errors
        }
    }
}
