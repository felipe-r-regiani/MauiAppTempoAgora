# AppTempoAgora

Aplicativo de clima tempo real desenvolvido em .NET MAUI. Consulta a API OpenWeatherMap para exibir condições meteorológicas atuais de qualquer cidade, com suporte a localização por GPS e mapa interativo Windy.

## Funcionalidades

- Pesquisa de clima por nome da cidade
- Geolocalização automática via GPS com reverse geocoding
- Exibição de temperatura, sensação térmica, mínima, máxima
- Horários de nascer e pôr do sol
- Ícone ilustrativo do clima (ícone oficial OpenWeatherMap)
- Mapa interativo de vento/tempo embutido (Windy.com via WebView)
- Tratamento de exceções específicas (GPS desligado, permissão negada, recurso não suportado)

## Como funciona

O app possui uma única tela (`MainPage`) com duas formas de consulta:

### Busca por cidade
1. Usuário digita o nome da cidade no `Entry` e toca em "Pesquisar"
2. `DataService.GetTempo(cidade)` cria um `HttpClient` dentro de `using` e faz GET na API:
   `https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={chave}`
3. O JSON de resposta é parseado com `JObject` (Newtonsoft.Json)
4. Timestamps Unix de nascer/pôr do sol são convertidos com `DateTimeOffset.FromUnixTimeSeconds()`
5. Os dados são exibidos em um `Label` e o ícone é carregado em um `Image`
6. Um `WebView` carrega o mapa Windy.com centralizado nas coordenadas da cidade

### Localização por GPS
1. Usuário toca em "Obter Minha Localização"
2. `Geolocation.Default.GetLocationAsync()` obtém as coordenadas atuais
3. `Geocoding.Default.GetPlacemarksAsync(lat, lon)` faz reverse geocoding para obter o nome da cidade
4. O campo de cidade é preenchido automaticamente e o usuário pode pesquisar o clima

### Estrutura do Projeto

```
Models/Tempo.cs               → Modelo com lon, lat, temp, feels_like, temp_min, temp_max, sunrise, sunset, description, icon
Services/DataService.cs       → Serviço com HttpClient para chamada à API OpenWeatherMap
MainPage.xaml(.cs)            → Interface única com Entry, Button, Label, Image e WebView
```

## Conceitos novos aprendidos

1. **HttpClient com using** — uso de `using (HttpClient client = new HttpClient())` para garantir o descarte correto de recursos de rede após a requisição à API.
2. **Requisição a API REST externa** — chamada GET à OpenWeatherMap API com parâmetros de query string (`q`, `units`, `appid`).
3. **Parsing de JSON com JObject (Newtonsoft.Json)** — manipulação de resposta JSON aninhada usando `JObject.Parse()` e acesso a propriedades como `response["main"]["temp"]`.
4. **Geolocation API** — `Geolocation.Default.GetLocationAsync()` para obter coordenadas GPS do dispositivo, com configuração de precisão e timeout via `GeolocationRequest`.
5. **Reverse Geocoding** — `Geocoding.Default.GetPlacemarksAsync(lat, lon)` para converter coordenadas em nome de cidade/distrito.
6. **Conversão de Unix timestamp** — `DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime.ToLocalTime()` para exibir nascer/pôr do sol em horário local.
7. **WebView com URL dinâmica** — carregamento de mapa interativo do Windy.com em um `WebView`, com URL construída a partir das coordenadas: `https://embed.windy.com/embed2.html?lat={lat}&lon={lon}`.
8. **Tratamento de exceções de geolocalização** — captura específica de `FeatureNotSupportedException`, `FeatureNotEnabledException` e `PermissionException` com mensagens diferentes para cada caso.
9. **Carregamento de imagem de URL** — `ImageSource.FromUri()` para baixar e exibir o ícone do clima do OpenWeatherMap.
10. **Declaração de permissões de localização no Android** — `ACCESS_FINE_LOCATION`, `ACCESS_COARSE_LOCATION`, `ACCESS_BACKGROUND_LOCATION` no `AndroidManifest.xml`.
11. **Bing Maps token no Windows** — configuração de `MapServiceToken` específica para a plataforma Windows em `Platforms/Windows/App.xaml.cs`.
