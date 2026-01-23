using System.Collections.Generic;
using UnityEngine;
using WeatherApp.Interface;

namespace WeatherApp.View
{
    /// <summary>
    ///  一日の天気予報を表示する
    /// </summary>
    public class HourlyForecastView : MonoBehaviour,IForecastView<HourlyForecastsData>
    {
        [SerializeField] 
        private RectTransform _content;
        [SerializeField]
        private HourlyForecastItem _itemPrefab;
        
        // 初期化時に使う
        private readonly List<HourlyForecastItem> _items = new();

        public void Show(List<HourlyForecastsData> dataList)
        {
            Clear();
            
            foreach (HourlyForecastsData item in dataList)
            {
                HourlyForecastItem obj = Instantiate(_itemPrefab, _content);
                obj.SetItemUI(item);
                _items.Add(obj);
            }
        }

        public void Clear()
        {
            foreach (HourlyForecastItem item in _items)
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