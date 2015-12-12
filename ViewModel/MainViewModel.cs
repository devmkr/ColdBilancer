using ColdBilancer.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace ColdBilancer.ViewModel
{

    public class MainViewModel : ViewModelBase
    {

        private CBModel _model;


        private RelayCommand _openEditWallsWindow;
     
        private List<IDrawable> _drawnWalls;

        //TO DO mvvm property
        public int OnsapRadius { get; set; }

        /// <summary>
        /// The <see cref="OsnapPoint" /> property's name.
        /// </summary>


        private Point? _osnapPoint;

        /// <summary>
        /// Sets and gets the SnapPoint property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Point? OsnapPoint
        {
            get
            {
                return _osnapPoint;
            }
            set
            {
                Set(nameof(OsnapPoint), ref _osnapPoint, value);
            }
        }                   


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
                        Messenger.Default.Send(MessagesTypes.OpenWallView);
                        Messenger.Default.Send(_model, MessagesTypes.PassModel);
                    }));
            }
        }

        private RelayCommand<Point> _findOsnap;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand<Point> FindOsnap
        {
            get
            {
                return _findOsnap
                    ?? (_findOsnap = new RelayCommand<Point>(
                    p =>
                    {
                        SetOnapPoint(p);
                    }));
            }
        }


        private Point? _start = null;

        private RelayCommand<Point> _drawWall;

        /// <summary>
        /// Gets the DrawWall.
        /// </summary>
        public RelayCommand<Point> DrawWall
        {
            get
            {
                return _drawWall
                    ?? (_drawWall = new RelayCommand<Point>(
                    p =>
                    {
                        if (!_start.HasValue)
                        {
                            _start = p;
                        }
                        else
                        {
                            DrawWallBetweenPoints(_start.Value, p);
                            _start = null;
                        }

                    }));
            }
        }


        public ObservableCollection<IDrawable> DrawnElements { get; set; }

        public ObservableCollection<CBWallType> WallColl { get; set; }
        public ObservableCollection<string> TypeWallColl
        {
            get
            {
                return new ObservableCollection<string>(_model.WallsType.Select(x => x.Name));
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _model = new CBModel();
            WallColl = new ObservableCollection<CBWallType>(_model.WallsType);

            //TO DO "dehard" value
            OnsapRadius = 20;
            //TO DO more elegant solution

            _drawnWalls = new List<IDrawable>();
            DrawnElements = new ObservableCollection<IDrawable>(_drawnWalls);
            RaisePropertyChanged(nameof(DrawnElements));

            //Subscribing...
            _model.PropertyChanged += (e, s) =>
            {
                RaisePropertyChanged(nameof(WallColl));
                RaisePropertyChanged(nameof(TypeWallColl));
            };
        }

        private void DrawWallBetweenPoints(Point start, Point end)
        {

            var addedWall = new CBWall(new CBWallType("sz1", 1.0, 10.0), start, end, 4.0);
            _model.AddWall(addedWall);
            _drawnWalls.Add(new CBWallViewModel(addedWall));
            DrawnElements = new ObservableCollection<IDrawable>(_drawnWalls);
            RaisePropertyChanged(nameof(DrawnElements));
        }

        public void SetOnapPoint(Point p)
        {
            if (OsnapPoint.HasValue)

                OsnapPoint = PointsOperations.IsPointInsideRadius(OsnapPoint.Value, p, OnsapRadius) ? OsnapPoint : null;

            else if (_drawnWalls.Count > 0)

                OsnapPoint = _drawnWalls.SelectMany(x => x.SnapPoints)
                                        .Where(x => PointsOperations.IsPointInsideRadius(p, x, OnsapRadius))
                                        .OrderBy(x => PointsOperations.DistanceBetweenPoints(x, p))
                                        .FirstOrDefault();

#if (DEBUG)
            Debug.WriteLine(OsnapPoint.HasValue);
#endif

        }


    }
}