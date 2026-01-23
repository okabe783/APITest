using System.Collections.Generic;

namespace WeatherApp.Interface
{
    public interface IForecastView<T>
    {
        void Show(List<T> data);
        void Clear();
    }
}