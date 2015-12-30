using System.Collections.Generic;

namespace Knapsack
{
    public static class Utils
    {
        public static int[] GetTwoMaxValuesIndexes(List<int> array)
        {
            var firstMaxValue = int.MinValue;
            var secondMaxValue = int.MinValue;

            var firstMaxValueIndex = -1;
            var secondMaxValueIndex = -1;

            for (int i = 0; i < array.Count; i++)
            {
                if (array[i] > firstMaxValue)
                {
                    secondMaxValue = firstMaxValue;
                    firstMaxValue = array[i];

                    secondMaxValueIndex = firstMaxValueIndex;
                    firstMaxValueIndex = i;
                }
                else if (array[i] > secondMaxValue && array[i] < firstMaxValue)
                {
                    secondMaxValue = array[i];
                    secondMaxValueIndex = i;
                }
            }

            return new int[] { firstMaxValueIndex, secondMaxValueIndex };
        }
    }
}