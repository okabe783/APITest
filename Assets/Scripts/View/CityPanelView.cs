using System;
using UnityEngine;

namespace WeatherApp.View
{
    public class CityPanelView : MonoBehaviour
    {
        [SerializeField] private UIButton _cityButton;

        private CityInfo _cityInfo;

        public event Action<CityInfo> OnClicked;

        public void SetUpCityButton(CityInfo cityInfo)
        {
            _cityInfo = cityInfo;
            _cityButton.SetText(cityInfo.Name);
            _cityButton.OnClickAddListener(() => OnClicked?.Invoke(_cityInfo));
        }
    }
}