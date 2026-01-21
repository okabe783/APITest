using UnityEngine;
using UnityEngine.UI;

// 1日分の予報（UI表示用）
public class DailyForecastData
{
    /// <summary>
    ///  今日か明日か明後日
    /// </summary>
    public string Date;
    /// <summary>
    /// 天気画像
    /// </summary>
    public Sprite WeatherIcon;
    /// <summary>
    /// 気温
    /// </summary>
    public string Temperature;
    public string RainText;
}

namespace WeatherApp.View
{
    /// <summary>
    /// 今日、明日、明後日の天気のパネル
    /// </summary>
    public class DailyForecastItem : MonoBehaviour
    {
        [SerializeField]
        private Text _date;
        [SerializeField] 
        private Image _icon;
        [SerializeField] 
        private Text _temperature;
        [SerializeField] 
        private Text _chanceOfRain;

        // ToDo : あとでInterfaceに
        public void SetItemUI(DailyForecastData data)
        {
            _date.text = data.Date;
            _icon.sprite = data.WeatherIcon;
            _temperature.text = data.Temperature;
            _chanceOfRain.text = data.RainText;
        }
    }
}