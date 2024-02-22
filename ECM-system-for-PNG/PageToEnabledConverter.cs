using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace ECM_system_for_PNG
{
    public class PageToEnabledConverter : IValueConverter
    {
        // Метод для конвертации из Page в bool
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверяем, что value - это объект Page, а parameter - это строка
            if (value is Page page && parameter is string pageName)
            {
                // Возвращаем false, если имя типа страницы совпадает с параметром
                return page.GetType().Name != pageName;
            }
            // В противном случае возвращаем true
            return true;
        }

        // Метод для обратной конвертации из bool в Page
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Этот метод не нужен, поэтому возвращаем DependencyProperty.UnsetValue
            return DependencyProperty.UnsetValue;
        }
    }
}
