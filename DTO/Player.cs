using System;
using System.Collections.Generic;
using System.Text;

namespace CowsAndChicken.DTO
{
    public class Player
    {
        public string Name { get; set; }

        public int YearOfBirth { get; set; }

        public int Age { get; set; }

        public Guid Id { get; set; }
    }
}
