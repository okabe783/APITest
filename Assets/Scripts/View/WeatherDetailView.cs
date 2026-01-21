using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace WeatherApp.View
{
    public class WeatherDetailView : MonoBehaviour
    {
        [Header("Current Weather")]
        [SerializeField] 
        private Text _cityNameText;
        [SerializeField]
        private Text _dateText;
        [SerializeField] 
        private Image _currentWeatherIcon;
        [SerializeField]
        private Text _currentTempText;
        [SerializeField]
        private Text _currentTelopText;

        [Header("Forecast View")]
        [SerializeField] private HourlyForecastView _hourlyForecastView; 
        [SerializeField] private DailyForecastView _dailyForecastView;
        
        public void ShowCurrentWeatherDetail(Root weather, string cityName)
        {
            _cityNameText.text = cityName;
            
            // 日付表示
            DateTime currentTime = DateTime.ParseExact(
                weather.publicTimeFormatted,
                "yyyy/MM/dd HH:mm:ss",
                CultureInfo.InvariantCulture
            );
            
            _dateText.text = currentTime.ToString("M/d (ddd)");

            Forecast today = weather.forecasts[0];
            _currentTelopText.text = today.telop;

            _currentTempText.text = today.temperature.max?.celsius != null ? $"{today.temperature.max.celsius}℃" : "℃";
        }

        /// <summary>
        ///  現在天気アイコン
        /// </summary>
        /// <param name="sprite"></param>
        public void SetIcon(Sprite sprite)
        {
            if (sprite != null)
            {
                _currentWeatherIcon.sprite = sprite;
            }
        }
        
        /// <summary>
        /// 1時間ごとの天気予想図
        /// </summary>
        /// <param name="hourlyList"></param>
        public void ShowHourlyForecastView(List<HourlyForecastsData> hourlyList)
        {
            _hourlyForecastView.Show(hourlyList);
        }

        public void ShowDailyForecastView(List<DailyForecastData> dailyList)
        {
            _dailyForecastView.Show(dailyList);
        }
    }
}