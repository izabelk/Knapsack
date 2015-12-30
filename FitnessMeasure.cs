namespace Knapsack
{
    public class FitnessMeasure
    {
        public FitnessMeasure(double weight, int value)
        {
            this.Weight = weight;
            this.Value = value;
        }

        public double Weight { get; set; }

        public int Value { get; set; }
    }
}