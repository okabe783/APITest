using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ForecastTemp
{
    public string celsius;
    public string fahrenheit;
}

[Serializable]
public class ForecastImage
{
    public string title;
    public string url;
    public string width;
    public string height;
}

[Serializable]
public class ForecastTemperature
{
    public ForecastTemp max;
    public ForecastTemp min;
}

[Serializable]
public class Forecast
{
    public string date;
    public ForecastImage image;
    public string dateLabel;
    public string telop;
    public ForecastTemperature temperature;
}

[Serializable]
public class Root
{
    public string publicTime;
    public Forecast[] forecasts;
}

public class WeatherApiService
{
    public async UniTask<Root> Request(string url,CancellationToken token)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        await webRequest.SendWebRequest().ToUniTask(cancellationToken: token);

        if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            throw new Exception(webRequest.error);
        }
        
        return JsonUtility.FromJson<Root>(webRequest.downloadHandler.text);
    }

    public async UniTask<Sprite> LoadImage(string imageUrl, CancellationToken token)
    {
        string url = imageUrl;
        if (url.EndsWith(".svg"))
        {
            url = url.Replace(".svg", ".png");
        }

        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);
        await webRequest.SendWebRequest().ToUniTask(cancellationToken: token);

        if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            throw new Exception(webRequest.error);
        }
        Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
        
        return Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2(0.5f,0.5f));
    }
}