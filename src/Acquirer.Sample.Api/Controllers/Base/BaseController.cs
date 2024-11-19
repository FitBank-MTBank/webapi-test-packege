using Acquirer.Shared.Messages;
using Microsoft.AspNetCore.Http;

namespace Acquirer.Sample.Api.Controllers.Base;

[ApiController]
[Route("sample-api/api/v{version:apiVersion}/[controller]")]

public class BaseController : ControllerBase
{
    [NonAction]
    public IResult Result(Result message)
    {
        if (message.IsFailure)
            return message.ToBadRequest(string.Empty);

        return message.ToSuccess();
    }
}