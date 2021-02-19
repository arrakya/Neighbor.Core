namespace Neighbor.Core.Domain.Models.Security
{
    public class VerifyPINResultModel
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
    }
}
