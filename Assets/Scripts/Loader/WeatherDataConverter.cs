using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace WeatherApp.Loader
{
    // Jsonで取得したデータを変換する
    public class WeatherDataConverter
    {
        /// <summary>
        /// 今日明日明後日のデータを格納する
        /// </summary>
        /// <param name="root"></param>
        /// <param name="icons">IconList</param>
        /// <returns></returns>
        public List<DailyForecastData> ConvertToDaily(Root root, List<Sprite> icons)
        {
            List<DailyForecastData> dataList = new();

            for (int i = 0; i < root.forecasts.Length; i++)
            {
                Forecast forecast = root.forecasts[i];
                
                dataList.Add(new DailyForecastData
                {
                    Date = forecast.dateLabel,
                    WeatherIcon = icons[i],
                    Temperature = forecast.temperature.max?.celsius,
                    // ToDo : 午前と午後つくる？
                    RainText = forecast.chanceOfRain.T00_06,
                });
            }

            return dataList;
        }
        
        /// <summary>
        /// 1時間ごとのデータを格納する
        /// </summary>
        /// <param name="root"></param>
        /// <param name="icon">単体</param>
        /// <returns></returns>
        public List<HourlyForecastsData> ConvertToHourly(Root root, Sprite icon)
        {
            List<HourlyForecastsData> datalist = new();
            // フォーマットが固定ならこれが安全に取得できる
            DateTime baseTime = DateTime.ParseExact(root.publicTimeFormatted, "yyyy/MM/dd HH:mm:ss",
                CultureInfo.InvariantCulture);

            Forecast today = root.forecasts[0];
            for (int i = baseTime.Hour; i < 24; i++)
            {
                string rain = GetRainKey(today.chanceOfRain,i);

                datalist.Add(new HourlyForecastsData
                {
                    Hour = i,
                    // 時間ごとのアイコンは存在しない
                    WeatherIcon = icon,
                    // 時間ごとの気温なし
                    Temperature = today.temperature.max.celsius,
                    Rain = rain
                });
            }
            
            return datalist;
        }

        private string GetRainKey(ForecastChanceOfRain rain,int hour)
        {
            return hour switch
            {
                < 6 => rain.T00_06,
                < 12 => rain.T06_12,
                < 18 => rain.T12_18,
                _ => rain.T18_24
            };
        }
    }
}