using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace TallerDIA.Utils
{
    public class BooleanToTBColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolean)
            {
                return boolean ? Brushes.Purple : Brushes.Gray; // Verde si es verdadero, rojo si es falso
            }
            return Brushes.Black; // Color predeterminado
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}