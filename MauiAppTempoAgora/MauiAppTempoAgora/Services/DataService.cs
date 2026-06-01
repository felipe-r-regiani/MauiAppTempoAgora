using Newtonsoft.Json.Linq;
using MauiAppTempoAgora.Models;
using System.Diagnostics;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetTempo(string cidade)
        {
            Tempo? t = null;

            string chave = "6135072afe7f6cec1537d5cb08a5a1a2";
            
            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    DateTimeOffset nascer = DateTimeOffset.FromUnixTimeSeconds((long)rascunho["sys"]["sunrise"]);
                    DateTimeOffset por = DateTimeOffset.FromUnixTimeSeconds((long)rascunho["sys"]["sunset"]);

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["main"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp = (double)rascunho["main"]["temp"],
                        feels_like = (double)rascunho["main"]["feels_like"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = nascer.LocalDateTime,
                        sunset = por.LocalDateTime,
                        timezone = (int)rascunho["timezone"],
                        icon = (string)rascunho["weather"][0]["icon"]
                    };
                }
            }

            return t;
        }
    }
}
