using System.Collections.Generic;
using UnityEngine;

namespace WeatherApp.View
{
    public class HourlyForecastView : MonoBehaviour
    {
        [SerializeField] 
        private RectTransform _content;
        [SerializeField]
        private HourlyForecastItem _itemPrefab;
        
        // 初期化時に使う
        private List<HourlyForecastItem> _items = new();

        public void Show(List<HourlyForecastsData> dataList)
        {
            foreach (HourlyForecastItem item in _items)
            {
                Destroy(item);
            }
            
            foreach (HourlyForecastsData item in dataList)
            {
                HourlyForecastItem obj = Instantiate(_itemPrefab, _content);
                obj.SetItemUI(item);
                _items.Add(obj);
            }
        }
    }
}