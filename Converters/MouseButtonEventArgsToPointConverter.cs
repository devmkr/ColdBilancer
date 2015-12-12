using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFExtensions.Controls;

namespace ColdBilancer
{
    class MouseButtonEventArgsToPointConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {

            var args = value as MouseEventArgs;
            var zc = parameter as ZoomControl;

            if (args == null || zc == null || zc.Content == null)
                throw new ArgumentException();                   

            var point = args.GetPosition(zc);
#if (DEBUG)
            Debug.WriteLine(zc.TranslatePoint(point, (UIElement)zc.Content));
#endif
            return zc.TranslatePoint(point, (UIElement)zc.Content);

        }
    }
}
