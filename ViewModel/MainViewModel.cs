using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;

namespace ColdBilancer.ViewModel
{
  
    public class MainViewModel : ViewModelBase
    {

        private CBModel _model;
              

        private RelayCommand _openEditWallsWindow;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand OpenEditWallsWindow
        {
            get
            {
                return _openEditWallsWindow
                    ?? (_openEditWallsWindow = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send(MessagesType.OpenWallView);
                        Messenger.Default.Send(_model, MessagesType.PassModel);
                    }));
            }
        }

        public ObservableCollection<CBWall> WallColl { get; set; }
        
        public ObservableCollection<string> TypeWallColl
        {
            get
            {
                return new ObservableCollection<string>(_model.Walls.Select(x => x.Name));
            }
        }


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _model = new CBModel();
            WallColl = new ObservableCollection<CBWall>(_model.Walls);

            //Subscribing...
            _model.PropertyChanged += (e, s) => { RaisePropertyChanged(nameof(WallColl)); RaisePropertyChanged(nameof(TypeWallColl)); };
        }
    }
}