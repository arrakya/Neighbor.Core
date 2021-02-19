using System;

namespace Neighbor.Core.Domain.Models.Security
{
    public class GeneratePINResultModel
    {
        public string Reference { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
    }
}
