using System;

namespace CowsAndChicken.DTO
{
    public class GameInfo
    {
        public Guid GameId { get; set; }

        public string PlayerName { get; set; }

        public int PlayerYearOfBirth { get; set; }
    }
}
