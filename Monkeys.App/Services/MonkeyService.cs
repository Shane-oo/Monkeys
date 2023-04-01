using System.Net.Http.Json;
using Monkeys.App.Model;

namespace Monkeys.App.Services;

public class MonkeyService
{
    HttpClient httpClient;
    public MonkeyService()
    {
        httpClient = new HttpClient();
    }



    List<Monkey> monkeys = new();
    public async Task<List<Monkey>> GetMonkeysAsync()
    {
        if (monkeys.Count > 0)
        {
            return monkeys;
        }
        var url = "https://montemagno.com/monkeys.json";

        var respone = await httpClient.GetAsync(url);

        if (respone.IsSuccessStatusCode)
        {
            monkeys = await respone.Content.ReadFromJsonAsync<List<Monkey>>();
        }

        return monkeys;
    }
}
