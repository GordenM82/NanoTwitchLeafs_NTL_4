using System;

namespace NanoTwitchLeafs.Objects
{
    public sealed class DonationEvent
    {
        public string EventId { get; set; }
        public string Provider { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTimeOffset ReceivedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
