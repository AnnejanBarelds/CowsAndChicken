using System;
using System.Collections.Generic;
using System.Text;

namespace CowsAndChicken.DomainModel
{
    public class NumberGenerator : INumberGenerator
    {
        public int Generate(int length)
        {
            if (length < 4 || length > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Value must be between 4 and 8");
            }

            // This works because we know that this will never be out of int range since length is never > 8. Revisit this code when the max length changes
            int min = Convert.ToInt32(Math.Pow(10d, length - 1));
            int max = Convert.ToInt32(Math.Pow(10d, length));

            return new Random().Next(min, max);
        }
    }
}
