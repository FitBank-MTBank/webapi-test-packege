using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquirer.Sample.Application.Models.Clients
{
    public class SampleExternalApiRequestModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }

    public class SampleExternalApiResponseModel
    {
        public string Response { get; set; }
    }
}
