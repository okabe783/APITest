using UnityEngine;
using UnityEngine.UI;
using WeatherApp.Interface;

// 時間別予報図のデータ　// 不変ではないのでClass
public class HourlyForecastsData
{
    public int Hour;
    public Sprite WeatherIcon;
    public string Temperature;
    public string Rain;
}

namespace WeatherApp.View
{
    public class HourlyForecastItem : MonoBehaviour,IForecastItem<HourlyForecastsData>
    {
        [Header("UI Elements")]
        [SerializeField]
        private Text _hourText;
        [SerializeField]
        private Image _weatherIcon;
        [SerializeField]
        private Text _rainText;
        [SerializeField]
        private Text _temperature;
        
        public void SetItemUI(HourlyForecastsData data)
        {
            _hourText.text = data.Hour.ToString();
            _weatherIcon.sprite = data.WeatherIcon;
            _rainText.text = data.Rain;
            _temperature.text = $"{data.Temperature}℃";
        }
    }
}