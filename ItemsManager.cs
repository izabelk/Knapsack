using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knapsack
{
    public class ItemsManager
    {
        public const int CHROMOSOMES_COUNT_IN_POPULATION = 200;

        private readonly List<Item> items;

        public ItemsManager(List<Item> items, int maxWeight)
        {
            if (items != null)
            {
                this.items = items;
            }
            else
            {
                items = new List<Item>();
            }

            this.MaxWeight = maxWeight;
        }

        public int MaxWeight { get; set; }

        public List<StringBuilder> GeneratePopulation()
        {
            var population = new List<StringBuilder>();
            var randomGenerator = new Random();

            for (int i = 0; i < CHROMOSOMES_COUNT_IN_POPULATION; i++)
            {
                var chromosome = new StringBuilder();

                for (int j = 0; j < this.items.Count; j++)
                {
                    var gene = randomGenerator.Next(0, 2);

                    if (gene == 0)
                    {
                        chromosome.Append("0");
                    }
                    else
                    {
                        chromosome.Append("1");
                    }
                }

                population.Add(chromosome);
            }

            return population;
        }

        public List<FitnessMeasure> EvaluateFitnessOfPopulation(List<StringBuilder> population)
        {
            var fitnessMeasures = new List<FitnessMeasure>();

            foreach (var chromosome in population)
            {
                var fitnessMeasure = this.EvaluateFitnessOfChromosome(chromosome);

                while (fitnessMeasure == null)
                {
                    fitnessMeasure = this.EvaluateFitnessOfChromosome(chromosome);
                }

                fitnessMeasures.Add(fitnessMeasure);
            }

            return fitnessMeasures;
        }

        public double CheckPercentageOfEqualFitnessValue(List<FitnessMeasure> measures)
        {
            var fitnesses = measures.Select(m => m.Value).ToList();

            double mostCommonFitnessValue = fitnesses.GroupBy(f => f)
                                                  .OrderByDescending(g => g.Count())
                                                  .First()
                                                  .Count();

            return (mostCommonFitnessValue / CHROMOSOMES_COUNT_IN_POPULATION) * 100;
        }

        public void PerformCrossover(StringBuilder firstChromosome, StringBuilder secondChromosome)
        {
            var randomGenerator = new Random();
            var crossoverPoint = randomGenerator.Next(0, this.items.Count);

            for (int i = crossoverPoint; i < firstChromosome.Length; i++)
            {
                var gene = firstChromosome[i];
                firstChromosome[i] = secondChromosome[i];
                secondChromosome[i] = gene;
            }
        }

        public void PerformMutation(StringBuilder chromosome)
        {
            var randomGenerator = new Random();

            for (int i = 0; i < chromosome.Length; i++)
            {
                var randomNumber = randomGenerator.NextDouble();

                if (randomNumber < 0.1)
                {
                    chromosome[i] = chromosome[i] == '1' ? '0' : '1';
                }
            }
        }

        public int GetParentIndexOfNextGeneration(List<FitnessMeasure> populationMeasures)
        {
            var totalValue = populationMeasures.Select(m => m.Value).Sum();
            var randomGenerator = new Random();
            double maxValue = randomGenerator.NextDouble() * totalValue;

            int i = 0;
            double sum = 0;

            do
            {
                sum += populationMeasures[i].Value;
                i++;
            }
            while (sum < maxValue);

            return --i;
        }

        public List<StringBuilder> GenerateNewPopulation(List<StringBuilder> population, List<FitnessMeasure> populationMeasures)
        {
            var values = populationMeasures.Select(m => m.Value).ToList();
            var maxValuesIndexes = Utils.GetTwoMaxValuesIndexes(values);
            var firstMaxValueIndex = maxValuesIndexes[0];
            var secondMaxValueIndex = maxValuesIndexes[1];

            var firstBestChromosome = population[firstMaxValueIndex];
            var secondBestChromosome = population[secondMaxValueIndex];

            var newGeneration = new List<StringBuilder>() { firstBestChromosome, secondBestChromosome };

            var firstChromosomeIndex = this.GetParentIndexOfNextGeneration(populationMeasures);
            var secondChromosomeIndex = this.GetParentIndexOfNextGeneration(populationMeasures);

            while (firstChromosomeIndex == secondChromosomeIndex ||
                firstChromosomeIndex == firstMaxValueIndex ||
                firstChromosomeIndex == secondMaxValueIndex ||
                secondChromosomeIndex == firstMaxValueIndex ||
                secondChromosomeIndex == secondMaxValueIndex)
            {
                firstChromosomeIndex = this.GetParentIndexOfNextGeneration(populationMeasures);
                secondChromosomeIndex = this.GetParentIndexOfNextGeneration(populationMeasures);
            }

            this.PerformCrossover(population[firstChromosomeIndex], population[secondChromosomeIndex]);

            for (int i = 0; i < population.Count; i++)
            {
                if (i != firstMaxValueIndex && i != secondMaxValueIndex)
                {
                    newGeneration.Add(population[i]);
                }
            }

            foreach (var chromosome in newGeneration)
            {
                this.PerformMutation(chromosome);
            }

            return newGeneration;
        }

        private FitnessMeasure EvaluateFitnessOfChromosome(StringBuilder chromosome)
        {
            double totalWeight = 0;
            var totalValue = 0;

            for (int i = 0; i < chromosome.Length; i++)
            {
                if (chromosome[i] == '1')
                {
                    totalWeight += this.items[i].Weight;
                    totalValue += this.items[i].Value;
                }
            }

            if (totalWeight > this.MaxWeight)
            {
                var randomGenerator = new Random();
                var index = randomGenerator.Next(chromosome.Length);
                var item = chromosome[index];

                while (item != '1')
                {
                    index = randomGenerator.Next(chromosome.Length);
                    item = chromosome[index];
                }

                chromosome[index] = '0';
            }
            else
            {
                return new FitnessMeasure(totalWeight, totalValue);
            }

            return null;
        }
    }
}