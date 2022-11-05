namespace Scrapper.CLI
{
    public static class WebService
    {
        public static async Task<string> GetPage(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}