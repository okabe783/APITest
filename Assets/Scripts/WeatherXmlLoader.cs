using System.Collections.Generic;
using System.Xml;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

// 主要都市とIDを紐づけ
public class CityInfo
{
    public string Name;
    public string Id;
}

// 都道府県とその県の主要都市を取得
public class PrefInfo
{
    public string Name;
    public List<CityInfo> Cities = new();
}
// XMLを読み込む
public class WeatherXmlLoader
{
    public async UniTask<List<PrefInfo>> LoadXmlAsync(string url)
    {
        // 手紙をかく
        using UnityWebRequest request = UnityWebRequest.Get(url);
        // 郵便ポストに入れる
        await request.SendWebRequest().ToUniTask();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error: {request.error}");
            return null;
        }

        string xmlText = request.downloadHandler.text;
        // XMLパース
        XmlDocument xmlDoc = new();
        xmlDoc.LoadXml(xmlText);
        
        // 都道府県を分ける
        XmlNodeList prefList = xmlDoc.GetElementsByTagName("pref");
        List<PrefInfo> prefs = new();
        foreach (XmlNode node in prefList)
        {
            // prefNodeがTextNodeの可能性があるのでタグであることを保証するためにキャスト
            if (node is not XmlElement prefElement)
                continue;

            // 都道府県名を登録
            PrefInfo prefInfo = new()
            {
                Name = prefElement.GetAttribute("title"),
            };
            
            // 主要都市を登録
            XmlNodeList cityNodes = prefElement.GetElementsByTagName("city");
            foreach (XmlNode city in cityNodes)
            {
                XmlElement cityElement = (XmlElement)city;
                string cityId = cityElement.GetAttribute("id");
                
                prefInfo.Cities.Add(new CityInfo
                {
                    Name = cityElement.GetAttribute("title"),
                    Id = cityId,
                });
            }
            prefs.Add(prefInfo);
        }
        
        return prefs;
    }
}