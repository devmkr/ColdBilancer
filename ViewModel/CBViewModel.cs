using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ColdBilancer.ViewModel
{
    public interface IDrawable      
    {
        /// <summary>
        /// 
        /// </summary>
        CBBuildingElement ModelElement { get; }

        /// <summary>
        /// 
        /// </summary>
        Geometry PathGeometry { get; }

        /// <summary>
        /// 
        /// </summary>
        double X { get; }

        /// <summary>
        /// 
        /// </summary>
        double Y { get; }

        double Rotation { get; }       

    }

    /// <summary>
    /// Class representing viewmodel of Wall
    /// </summary>
    public class CBWallViewModel : ViewModelBase, IDrawable
    {
        private CBBuildingElement _modelElement;
        private double _x;
        private double _y;
        private double _rotation;

        public CBBuildingElement ModelElement
        {
            get
            {
                return _modelElement;
            }
            private set
            {
                _modelElement = value;
            }
        }

        public Geometry PathGeometry
        {
            get
            {
                return System.Windows.Media.PathGeometry.CreateFromGeometry(                 
                        new RectangleGeometry(new Rect(X, Y, _modelElement.Dimensions.Length, _modelElement.Dimensions.Thickness),0,0, new RotateTransform(Rotation)));                        
                  
            }
        }
        /// <summary>
        /// Start point representing top-left
        /// </summary>
        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                RaisePropertyChanged(nameof(X));
                RaisePropertyChanged(nameof(PathGeometry));
            }
        }

        /// <summary>
        /// Start point representing top-left
        /// </summary>
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                RaisePropertyChanged(nameof(Y));
                RaisePropertyChanged(nameof(PathGeometry));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                RaisePropertyChanged(nameof(Rotation));
            }
        }
        
              
        public CBWallViewModel(CBWallType wall, double x, double y, double rotation = 0)
        {
            X = x;
            Y = y;
            _modelElement = wall;
            Rotation = rotation;
        }

        public CBWallViewModel(CBWallType wall, Point start, Point end)           
        {
            //TODO rotation calculation and etc.

        }
        
    }

}
