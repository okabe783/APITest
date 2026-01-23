namespace WeatherApp.Interface
{
    public interface IForecastItem<in T>
    {
        void SetItemUI(T itemData);
    }
}