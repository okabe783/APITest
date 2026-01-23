using System.Collections.Generic;
using UnityEngine;
using WeatherApp.Interface;

namespace WeatherApp.View
{
    /// <summary>
    /// 3日分の天気予報を表示する
    /// </summary>
    public class DailyForecastView : MonoBehaviour, IForecastView<DailyForecastData>
    {
        [SerializeField] 
        private RectTransform _content;
        [SerializeField] 
        private DailyForecastItem _itemPrefab;
        
        private readonly List<DailyForecastItem> _items = new();

        public void Show(List<DailyForecastData> dataList)
        {
            Clear();

            foreach (DailyForecastData item in dataList)
            {
                DailyForecastItem obj = Instantiate(_itemPrefab,_content);
                obj.SetItemUI(item);
                _items.Add(obj);
            }
        }

        public void Clear()
        {
            foreach (DailyForecastItem item in _items)
            {
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }
            _items.Clear();
        }
    }
}