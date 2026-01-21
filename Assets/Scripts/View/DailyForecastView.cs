using System.Collections.Generic;
using UnityEngine;

namespace WeatherApp.View
{
    /// <summary>
    /// 3日分の天気を表示する
    /// </summary>
    public class DailyForecastView : MonoBehaviour
    {
        [SerializeField] 
        private RectTransform _content;
        [SerializeField] 
        private DailyForecastItem _itemPrefab;
        
        private List<DailyForecastItem> _items = new();

        public void Show(List<DailyForecastData> dataList)
        {
            foreach (DailyForecastItem item in _items)
            {
                Destroy(item);
            }

            foreach (DailyForecastData item in dataList)
            {
                DailyForecastItem obj = Instantiate(_itemPrefab,_content);
                obj.SetItemUI(item);
                _items.Add(obj);
            }
        }
    }
}