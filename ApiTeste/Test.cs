using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiTester
{
    public class Test
    {
        //microservice api
        //private static readonly string baseUrl = "http://localhost:8469/products/addProduct";
        //monolith api
        private static readonly string baseUrl = "https://localhost:44392/products/addProduct";
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting to add products...");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                Random random = new Random();
                Stopwatch stopwatch = new Stopwatch();

                Console.WriteLine("Enter the number of products to add:");
                if (!int.TryParse(Console.ReadLine(), out int numberOfProducts) || numberOfProducts <= 0)
                {
                    Console.WriteLine("Please enter a valid positive number.");
                    return;
                }

                stopwatch.Start();

                for (int i = 1; i <= numberOfProducts; i++)
                {
                    var product = new ProductDto
                    {
                        Name = $"Product {i}",
                        Ingredients = "",  
                        Price = random.NextDouble() * 99 + 1 
                    };

                    var jsonContent = JsonSerializer.Serialize(product);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    try
                    {
                        HttpResponseMessage response = await client.PostAsync("", content);

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Product {i} added successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to add Product {i}. Status Code: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception occurred while adding Product {i}: {ex.Message}");
                    }
                }

                stopwatch.Stop();

                Console.WriteLine($"Finished adding products. Total time elapsed: {stopwatch.Elapsed}");
            }
        }
    }
}