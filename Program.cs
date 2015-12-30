using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Knapsack
{
    public class Program
    {
        private const int MAX_GENERATIONS_COUNT = 1000;

        static void Main(string[] args)
        {
            var items = new List<Item>();
            items.Add(new Item("map", 0.09, 150));
            items.Add(new Item("compass", 0.13, 35));
            items.Add(new Item("water", 1.53, 200));
            items.Add(new Item("sandwich", 0.5, 160));
            items.Add(new Item("glucose", 0.15, 60));
            items.Add(new Item("tin", 0.68, 45));
            items.Add(new Item("banana", 0.27, 60));
            items.Add(new Item("apple", 0.39, 40));
            items.Add(new Item("cheese", 0.23, 30));
            items.Add(new Item("beer", 0.52, 10));
            items.Add(new Item("suntan cream", 0.11, 70));
            items.Add(new Item("camera", 0.32, 30));
            items.Add(new Item("T-shirt", 0.24, 15));
            items.Add(new Item("trusers", 0.48, 10));
            items.Add(new Item("umbrella", 0.73, 40));
            items.Add(new Item("waterproof trousers", 0.42, 70));
            items.Add(new Item("waterproof overclothes", 0.43, 75));
            items.Add(new Item("note-case", 0.22, 80));
            items.Add(new Item("sunglasses", 0.07, 20));
            items.Add(new Item("towel", 0.18, 12));
            items.Add(new Item("socks", 0.04, 50));
            items.Add(new Item("book", 0.3, 10));
            items.Add(new Item("notebook", 0.9, 1));
            items.Add(new Item("tent", 2, 150));

            var itemsManager = new ItemsManager(items, 5);
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var population = itemsManager.GeneratePopulation();
            var generationsCount = 1;
            var populationMeasures = itemsManager.EvaluateFitnessOfPopulation(population);
            var percentageOfEqualFitnessValue = itemsManager.CheckPercentageOfEqualFitnessValue(populationMeasures);

            while (true)
            {
                if (percentageOfEqualFitnessValue > 80)
                {
                    stopWatch.Stop();
                    double elapsedSeconds = stopWatch.ElapsedMilliseconds / 1000;

                    var maxValue = populationMeasures.Select(m => m.Value).Max();

                    for (int i = 0; i < population.Count; i++)
                    {
                        if (populationMeasures[i].Value == maxValue)
                        {
                            var chromosome = population[i];
                            var chromosomeWeight = populationMeasures[i].Weight;

                            for (int j = 0; j < chromosome.Length; j++)
                            {
                                if (chromosome[j] == '1')
                                {
                                    Console.WriteLine("{0} -> Weight: {1}; Value: {2}", items[j].Name, items[j].Weight, items[j].Value);
                                }
                            }

                            Console.WriteLine();
                            Console.WriteLine("Max weight: {0}", chromosomeWeight);
                            Console.WriteLine("Max value: {0}", maxValue);
                            Console.WriteLine();
                            Console.WriteLine("Elapsed time: {0} sec", elapsedSeconds);
                            Console.WriteLine();
                            break;
                        }
                    }

                    break;
                }

                if (generationsCount >= MAX_GENERATIONS_COUNT)
                {
                    Console.WriteLine("No solution was found!");
                    break;
                }

                population = itemsManager.GenerateNewPopulation(population, populationMeasures);

                generationsCount++;

                populationMeasures = itemsManager.EvaluateFitnessOfPopulation(population);
                percentageOfEqualFitnessValue = itemsManager.CheckPercentageOfEqualFitnessValue(populationMeasures);
            }
        }
    }
}