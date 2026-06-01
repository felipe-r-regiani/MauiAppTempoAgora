using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.Diagnostics;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked_Previsao(object sender, EventArgs e)
        {
            try
            {
                if (txt_cidade.Text != "")
                {
                    Tempo? t = await DataService.GetTempo(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n " +
                                         $"Longitude: {t.lon} \n " +
                                         $"Temperatura: {t.temp} \n " +
                                         $"Sensação Térmica: {t.feels_like} \n" +
                                         $"Temp max: {t.temp_max} \n" +
                                         $"Temp min: {t.temp_min} \n" +
                                         $"Ícone: {t.icon}; \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Pôr do Sol: {t.sunset}";

                        lbl_previsao.Text = dados_previsao;
                        img_icone.Source = $"https://openweathermap.org/img/wn/{t.icon}@2x.png";


                        string mapa = $"https://embed.windy.com/embed.html?" +
                                      $"type=map&location=coordinates&metricRain=mm&" +
                                      $"metricTemp=°C&metricWind=km/h&zoom=5&overlay=wind&" +
                                      $"product=ecmwf&level=surface&" +
                                      $"lat={t.lat.ToString().Replace(",", ".")}&" +
                                      $"lon={t.lon.ToString().Replace(",", ".")}";
                        
                        wv_mapa.Source = mapa;
                    }
                }
                else
                    throw new Exception("Informe a cidade.");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Ops", ex.Message, "OK");
            }
        }

        private async void GetCidade(double lat, double lon)
        {
            try
            {
                IEnumerable<Placemark> places = await Geocoding.Default.GetPlacemarksAsync(lat, lon);

                Placemark? place = places.FirstOrDefault();

                Debug.WriteLine(place);

                if (place != null)
                {
                    if(!string.IsNullOrWhiteSpace(place.Locality))
                    {
                        txt_cidade.Text = place.Locality;

                    } else if(!string.IsNullOrWhiteSpace(place.SubAdminArea))
                    {
                        txt_cidade.Text = place.SubAdminArea;

                    } else if(!string.IsNullOrWhiteSpace(place.FeatureName))
                    {
                        txt_cidade.Text = place.FeatureName;
                    }                    
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Erro: Obtenção do nome da cidade:", ex.Message, "OK");
            }
        }

        private async void Button_Clicked_Localizacao(object sender, EventArgs e)
        {
            try
            {
                // Código para obter geolocalização vai aqui
                GeolocationRequest request = new GeolocationRequest(
                    GeolocationAccuracy.Medium,
                    TimeSpan.FromSeconds(10));

                Location? local = await Geolocation.Default.GetLocationAsync(request);

                Debug.WriteLine(local);

                if (local != null)
                {
                    string local_disp = $"Latitude: {local.Latitude} \n" +
                                        $"Longitude: {local.Longitude}";
                    
                    lbl_previsao.Text = local_disp;

                    GetCidade(local.Latitude, local.Longitude);
                } else
                {
                    lbl_previsao.Text = "Não consegui pegar a geolocalização...";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlertAsync("Erro: Dispositivo não Suporta", fnsEx.Message, "OK");

            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlertAsync("Erro: Localização Desabilitada", fneEx.Message, "OK");

            }
            catch (PermissionException pEx)
            {
                await DisplayAlertAsync("Erro: Permissão de Localização", pEx.Message, "OK");

            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Erro:", ex.Message, "OK");
            }
        }
    }
}
