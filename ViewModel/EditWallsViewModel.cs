using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ColdBilancer.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class EditWallsViewModel : ViewModelBase
    {
        private CBModel _model;

        private RelayCommand _openMaterialsWindow;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand OpenMaterialsWindow
        {
            get
            {
                return _openMaterialsWindow
                    ?? (_openMaterialsWindow = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send(MessagesType.OpenMaterialView);
                        
                    }));
            }
        }

        public ObservableCollection<CBWallType> WallColl { get; set; }

        /// <summary>
        /// Initializes a new instance of the EditWallsViewModel class.
        /// </summary>
        public EditWallsViewModel()
        {
            MessengerInstance.Register<CBModel>(this,MessagesType.PassModel,  x => Init(x));

         }

        private void Init(CBModel model)
        {
            _model = model;
            WallColl = new ObservableCollection<CBWallType>(_model.Walls);
            _model.PropertyChanged += (s, e) => { RaisePropertyChanged(nameof(WallColl)); };
            RaisePropertyChanged(nameof(WallColl));
        }
    }
}