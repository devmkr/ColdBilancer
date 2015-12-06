using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
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


        public ObservableCollection<IDrawable> DrawnElements { get; set; }

        public ObservableCollection<CBWallType> WallColl { get; set; }        
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
            WallColl = new ObservableCollection<CBWallType>(_model.Walls);

            //Just for test purpose:

            var l = new List<IDrawable>();
            foreach(var z in _model.Walls)
            {
                l.Add(new CBWallViewModel(z, 10, 10));
            }

            l.Add(new CBWallViewModel(new CBWallType("a", new CBDimensions(100, 20, 10), 0.1), 10, 20, 45));

            DrawnElements = new ObservableCollection<IDrawable>(l);

            RaisePropertyChanged(nameof(DrawnElements));

            //Subscribing...
            _model.PropertyChanged += (e, s) => { RaisePropertyChanged(nameof(WallColl));
                                                  RaisePropertyChanged(nameof(TypeWallColl));};
        }
    }
}