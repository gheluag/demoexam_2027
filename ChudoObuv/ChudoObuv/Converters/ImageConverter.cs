using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ChudoObuv.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? fileName = value as string;

            string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

            if (string.IsNullOrWhiteSpace(fileName))
                fileName = "picture.png";

            string fullPath = Path.Combine(imageFolder, fileName);
            if(!File.Exists(fullPath))
                fullPath = Path.Combine(imageFolder, "picture.png");

            return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
