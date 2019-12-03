using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Request
{
    public class WorkflowRequest
    {
        public Guid UUID { get; set; }
        public string Data { get; set; }
        public List<String> Steps { get; set; }
    }
}
