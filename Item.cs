namespace Knapsack
{
    public class Item
    {
        public Item(string name, double weight, int value)
        {
            this.Name = name;
            this.Weight = weight;
            this.Value = value;
        }

        public string Name { get; set; }

        public double Weight { get; set; }

        public int Value { get; set; }
    }
}