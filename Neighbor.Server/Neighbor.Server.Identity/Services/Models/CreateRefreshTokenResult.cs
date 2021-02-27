namespace Neighbor.Server.Identity.Services.Models
{
    public class CreateRefreshTokenResult
    {
        public enum FailCases
        {
            WrongCredential, 
            FailAccountNotConfirm,
            UserNotFound
        }

        public bool IsSuccess => !FailCase.HasValue;
        public FailCases? FailCase { get; set; }
        public string Message { get; set; }
        public string Context { get; set; }
    }
}
