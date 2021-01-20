using System;

namespace Neighbor.Core.Domain.Models.Security
{
    public class UserPINModel
    {
        public int Id { get; set; }
        public string PIN { get; set; }
        public string Reference { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? EnterOn { get; set; }
        public int InvalidAttempt { get; set; }
        public string ChannelType { get; set; }
        public string ChannelAddress { get; set; }
        public DateTime? ChannelSendOn { get; set; }
        public bool IsEnable { get; set; }
    }
}
