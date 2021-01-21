namespace Neighbor.Core.Domain.Models.Security
{
    public class ResetPasswordResultModel
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
    }
}
