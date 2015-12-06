using ColdBilancer.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ColdBilancer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<MessagesType>(this, NotificationMessageReceived);

        }
        private void NotificationMessageReceived(MessagesType msg)
        {
            if (msg == MessagesType.OpenMaterialView)
            {
                var view = new MaterialsView();
                view.Show();
            }
            else if(msg == MessagesType.OpenWallView)
            {
                var view = new EditWallsView();
                view.Show();
            }
        }
    }
}
