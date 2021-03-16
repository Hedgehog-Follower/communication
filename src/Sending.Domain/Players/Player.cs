using System;

namespace Sending.Domain.Players
{
    public class Player
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
