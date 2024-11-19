using Acquirer.Sample.Api.Controllers.Base;
using Acquirer.Sample.Application.Interfaces.Services;
using Acquirer.Sample.Application.Models.Sample;
using Microsoft.AspNetCore.Http;
using System;

namespace Acquirer.Sample.Api.Controllers.V1;

[ApiVersion("1.0")]
public class SamplesController(ISampleService sampleService) : BaseController
{
    [HttpPost("")]
    public async Task<IResult> Create([FromBody] SampleCreateModel model)
    {
        var result = await sampleService.Create(model);

        return Result(result);
    }

    [HttpPut("{sampleId}")]
    public async Task<IResult> Update(Guid sampleId, [FromBody] SampleUpdateModel model)
    {
        model.SampleId = sampleId;

        var result = await sampleService.Update(model);
        return Result(result);
    }

    [HttpGet("{sampleId}")]
    public async Task<IResult> GetById(Guid sampleId)
    {
        var result = await sampleService.GetById(sampleId);
        return Result(result);
    }

    [HttpDelete("{sampleId}")]
    public async Task<IResult> Delete(Guid sampleId)
    {
        var result = await sampleService.Delete(sampleId);
        return Result(result);
    }
}
