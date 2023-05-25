namespace Pishi_Wash__Store.Assets.Converters
{
    internal class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            return !string.IsNullOrWhiteSpace(str) && str.All(char.IsDigit) ? str : "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            return !string.IsNullOrWhiteSpace(str) && str.All(char.IsDigit) ? str : "0";
        }
    }
}
