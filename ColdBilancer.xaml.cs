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
            Messenger.Default.Register<MessagesTypes>(this, NotificationMessageReceived);
        }

        //Messenger member
        private void NotificationMessageReceived(MessagesTypes msg)
        {         
            if (msg == MessagesTypes.OpenMaterialView)
            {
                var view = new MaterialsView();
                view.Show();
            }
            else if (msg == MessagesTypes.OpenWallView)
            {
                var view = new EditWallsView();
                view.Show();
            }
        }

    }
}
