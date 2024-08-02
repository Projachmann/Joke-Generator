namespace Joke_Generator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("DO YOU WANT TO HERE A JOKE?");
            Console.ReadLine();

            bool anotherJoke = true;
            string apiKey = Environment.GetEnvironmentVariable("API_KEY");

            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("No Api Key found. Get your api key from: https://api-ninjas.com/profile");
                Console.Write("Api Key: ");
                apiKey = Console.ReadLine();
            }
            string url = "https://api.api-ninjas.com/v1/jokes";

            while (anotherJoke)
            {
                Console.Clear();

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();

                        var joke = System.Text.Json.JsonSerializer.Deserialize<Joke[]>(responseBody);

                        if (joke != null)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine(joke[0].joke);
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("No joke for you!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                Console.Clear();

                Console.WriteLine("Another joke?");
                string input = Console.ReadLine().ToLower();

                if (input != "yes")
                {
                    anotherJoke = false;
                }
            }
        }
    }

    public class Joke
    {
        public string joke { get; set; }
    }
}
