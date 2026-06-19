using System;
using Microsoft.Spark.Sql;
using static Microsoft.Spark.Sql.Functions;

namespace WordCountSpark
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a Spark session
            SparkSession spark = SparkSession
                .Builder()
                .AppName("WordCountSpark")
                .GetOrCreate();

            // Read the input text file into a DataFrame
            DataFrame inputData = spark.Read().Text("input.txt");

            // Split the lines into words and explode the array to get individual words
            DataFrame words = inputData
                .Select(Explode(Split(inputData["value"], " ")).Alias("word"));

            // Group by the word and count the occurrences
            DataFrame wordCounts = words
                .GroupBy("word")
                .Count()
                .OrderBy(Desc("count"));

            // Show the results
            wordCounts.Show();

            // Stop the Spark session
            spark.Stop();
        }
    }
}