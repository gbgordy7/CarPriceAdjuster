using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPriceAdjuster
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayCarValue(25313.40m, 35000m, 3 * 12, 50000, 1, 1);
            DisplayCarValue(19688.20m, 35000m, 3 * 12, 150000, 1, 1);
            DisplayCarValue(19688.20m, 35000m, 3 * 12, 250000, 1, 1);
            DisplayCarValue(20090.00m, 35000m, 3 * 12, 250000, 1, 0);
            DisplayCarValue(21657.02m, 35000m, 3 * 12, 250000, 0, 1);
            Console.ReadLine();
        }

        public class Car
        {
            public decimal PurchaseValue { get; set; }
            public int AgeInMonths { get; set; }
            public int NumberOfMiles { get; set; }
            public int NumberOfPreviousOwners { get; set; }
            public int NumberOfCollisions { get; set; }
        }

        public class PriceDeterminator
        {

            /// <summary>
            /// loop to remove value from the car
            /// </summary>
            /// <param name="maxValue">the point at which one should stop decreasing the value</param>
            /// <param name="value">the value to be checked ie age, number of miles</param>
            /// <param name="carValue">current value of the car</param>
            /// <param name="reduction">amount to reduce the value by</param>
            /// <param name="increment">how much to increment the check by ie number of miles is every 1000, age is every 1 month</param>
            /// <returns></returns>
            public decimal carPriceReduction(int maxValue, int value, decimal carValue, decimal reduction, int increment)
            {
                //determine number of times to loop
                int numberOfLoops = (value > maxValue) ? maxValue : value;

                //reduce amount by "reduction" for each iteration of the loop
                for (int i = 0; i < numberOfLoops; i += increment)
                {
                    carValue = carValue * (1 - reduction);
                }

                return carValue;
            }

            public decimal DetermineCarPrice(Car car)
            {
                //set sell price to the purchase price then reduce
                decimal sellPrice = car.PurchaseValue;

                int age = (car.AgeInMonths >= 120) ? 120 : car.AgeInMonths;
                int mileage = (car.NumberOfMiles >= 150000) ? 150000 : car.NumberOfMiles;
                int collisions = (car.NumberOfCollisions >= 5) ? 5 : car.NumberOfCollisions;

                sellPrice = sellPrice * (1 - (age * .005m));
                sellPrice = sellPrice * (1 - (mileage / 1000 * .002m));

                //reduce car to 75% of value if more than two previous owners
                if (car.NumberOfPreviousOwners > 2) sellPrice = sellPrice * .75m;

                sellPrice = sellPrice * (1 - (collisions * .02m));

                //increase car value by 10% if zero previous owners
                if (car.NumberOfPreviousOwners == 0) sellPrice = sellPrice * 1.1m;

                return sellPrice;
            }

        }


        private static void DisplayCarValue(decimal expectValue, decimal purchaseValue,
        int ageInMonths, int numberOfMiles, int numberOfPreviousOwners, int
        numberOfCollisions)
        {
            Car car = new Car
            {
                AgeInMonths = ageInMonths,
                NumberOfCollisions = numberOfCollisions,
                NumberOfMiles = numberOfMiles,
                NumberOfPreviousOwners = numberOfPreviousOwners,
                PurchaseValue = purchaseValue
            };
            PriceDeterminator priceDeterminator = new PriceDeterminator();
            var carPrice = priceDeterminator.DetermineCarPrice(car);
            Console.WriteLine("Expected Value: " + expectValue.ToString() + " Actual Value: " + carPrice.ToString());

        }



    }
}
