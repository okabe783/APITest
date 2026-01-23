using UnityEngine;
using UnityEngine.UI;
using WeatherApp.Interface;

public class CurrentForecastData
{
    public string CityName;
    public Sprite WeatherIcon;
    public string Temperature;
    public string Telop;
}

namespace WeatherApp.View
{
    public class CurrentForecastItem : MonoBehaviour,IForecastItem<CurrentForecastData>
    {
        [SerializeField] 
        private Text _cityNameText;
        [SerializeField] 
        private Image _currentWeatherIcon;
        [SerializeField]
        private Text _currentTempText;
        [SerializeField]
        private Text _currentTelopText;

        public void SetItemUI(CurrentForecastData data)
        {
            _cityNameText.text = data.CityName;
            _currentWeatherIcon.sprite = data.WeatherIcon;
            _currentTempText.text = data.Temperature;
            _currentTelopText.text = data.Telop;
        }
    }
}