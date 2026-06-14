using System;

namespace Game
{
    public static class NumberFormatter
    {
        private static readonly string[] Suffixes =
        {
            "",
            "K",
            "M",
            "B"
        };

        public static string Format(int value)
        {
            if (value < 1000)
                return value.ToString();

            double displayValue = value;
            int index = 0;

            while (displayValue >= 1000 && index < Suffixes.Length - 1)
            {
                displayValue /= 1000.0;
                index++;
            }

            if (displayValue >= 100)
                return Math.Floor(displayValue).ToString("0") + Suffixes[index];

            if (displayValue >= 10)
                return (Math.Floor(displayValue * 10) / 10)
                    .ToString("0.#") + Suffixes[index];

            return (Math.Floor(displayValue * 100) / 100)
                .ToString("0.##") + Suffixes[index];
        }

        public static string FormatMultiplier(int value)
        {
            if (value < 1000)
                return value.ToString();

            double displayValue = value;
            int index = 0;

            while (displayValue >= 1000 && index < Suffixes.Length - 1)
            {
                displayValue /= 1000.0;
                index++;
            }

            return Math.Floor(displayValue).ToString("0") + Suffixes[index];
        }
    }
}