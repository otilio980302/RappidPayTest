using System;

namespace RapidPayTest.Infrastructure.Services
{
    public class UfeService
    {
        private static UfeService instance;
        private Random random;
        private decimal lastFeeAmount;

        private UfeService()
        {
            random = new Random();
            lastFeeAmount = 0.01m; // Initial fee amount (you can set it to any value)
        }

        public static UfeService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UfeService();
                }
                return instance;
            }
        }

        public decimal GetRandomDecimal()
        {
            return (decimal)random.NextDouble() * 2;
        }

        public void UpdateFee()
        {
            decimal randomDecimal = GetRandomDecimal();
            lastFeeAmount *= randomDecimal;
        }

        public decimal LastFeeAmount
        {
            get { return lastFeeAmount; }
        }
    }
}