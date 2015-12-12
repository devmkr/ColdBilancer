using ColdBilancer.Helpers;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static System.Math;


namespace ColdBilancer.ViewModel
{
    public interface IDrawable
    {
        /// <summary>
        /// 
        /// </summary>
        CBWall ModelElement { get; }

        /// <summary>
        /// 
        /// </summary>
        Geometry Geometry { get; }

        List<Point> SnapPoints { get; }

        double RotationAngle { get; }

    }

    /// <summary>
    /// Class representing viewmodel of Wall
    /// </summary>
    public class CBWallViewModel : ViewModelBase, IDrawable
    {
        private CBWall _modelElement;
        private List<Point> _snapPoints;

        public CBWall ModelElement
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

        public Geometry Geometry
        {
            get
            {
                return new RectangleGeometry(new Rect(new Size(_modelElement.Dimensions.Length, _modelElement.Dimensions.Thickness)), 0, 0,
                                            //Rotate and offset the rectangle:
                                            new MatrixTransform(Cos(RotationAngle), Sin(RotationAngle), -Sin(RotationAngle), Cos(RotationAngle),
                                                                _modelElement.StartEndPoints.Item1.X, _modelElement.StartEndPoints.Item1.Y));               
                
            }
        }
      

        /// <summary>
        /// 
        /// </summary>
        public double RotationAngle
        {
            get
            {
                return PointsOperations.AngleBetweenPoints(_modelElement.StartEndPoints.Item1, _modelElement.StartEndPoints.Item2);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public List<Point> SnapPoints
        {
            get
            {
                return _snapPoints;
            }
        }

        public CBWallViewModel(CBWall wall)
        {
            ModelElement = wall;
            _snapPoints = new List<Point>() { wall.StartEndPoints.Item1, wall.StartEndPoints.Item2 };

        }

    }

}
