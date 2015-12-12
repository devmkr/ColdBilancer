using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace ColdBilancer
{
    public class CBModel : INotifyPropertyChanged
    {
        public List<CBWallType> WallsType { get; private set; }
        public List<CBWall> Walls { get; private set; }

        public CBModel()
        {
            WallsType = new List<CBWallType>();
            Walls = new List<CBWall>();           
        }
        public void AddWall(CBWall wall)
        {
            Walls.Add(wall);            
        }
        public void RemoveWall(CBWall wall)
        {
            if (!Walls.Remove(wall))
                throw new ArgumentOutOfRangeException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class CBWall
    {
        /// <summary>
        /// 
        /// </summary>
        public CBWallType Type { get; private set; }

        public CBDimensions Dimensions { get; private set; }

        public Orientation Oritentation { get; private set; }

        public Tuple<Point,Point> StartEndPoints { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="height"></param>
        public CBWall(CBWallType type, Point start, Point end, double height)
        {
            StartEndPoints = new Tuple<Point, Point>(start, end);
            Type = type;

            //Distance between two points...
            var length = Math.Sqrt(Math.Pow(end.X - start.X, 2.0) + Math.Pow(end.Y - start.Y, 2.0));

            //Wrapping dimensions to one object
            Dimensions = new CBDimensions(length, height, type.Thickness);
        }

        

    }

    public class CBBuilidingMaterial
    {
        public double ThermalConducivity { get; private set; }
        public double SpecificHeat { get; private set; }
        public double Rho { get; private set; }
        public string Name { get; private set; }

    }

    /// <summary>
    /// Base class describes the basic properties of building. 
    /// </summary>
    public abstract class CBBuildingElement
    {
        /// <summary>
        /// Heat transfer coefficient of building element
        /// Value should be returned in SI Units (W/(sqm*K))
        /// </summary>
        abstract public double HeatTransferCoeff { get; protected set; }       

        /// <summary>
        /// Name of building elements
        /// Should be implement change Name.
        /// </summary>
        public string Name { get; protected set; }
      
    }

    public class CBWallType : CBBuildingElement
    {
        private double _heatTransferCoeff;     

        /// <summary>
        /// Overall thickness of the wall
        /// </summary>
        public double Thickness { get; private set; }
      
        /// <summary>
        /// List of wall layers. Index of list represents the order following order. 
        /// T2 in tuple represents the thickness of the layer.
        /// </summary>
        public List<Tuple<CBBuilidingMaterial, double>> WallLayers { get; private set; }

        /// <summary>
        /// Heat transfer coefficient of wall.
        /// Express by equation: U = 1 / Rt.
        /// Where Rt is sum of di/lambdai.       
        /// </summary>
        public override double HeatTransferCoeff
        {
            get
            {
                return (WallLayers != null) ? 1 / WallLayers.Sum(x => x.Item2 / x.Item1.ThermalConducivity)
                                            : _heatTransferCoeff;
            }
            protected set
            {
                _heatTransferCoeff = value;
            }

        }      

       
        public CBWallLocation WallType
        {
            get; private set;
        }



        /// <summary>
        /// Main constructor for common initializing. 
        /// </summary>
        /// <param name="dimensions"></param>
        private CBWallType(string name, CBWallLocation wt)
        {
            WallType = wt;
            Name = name;
        }

        /// <summary>
        /// Create wall in accordance with given layers.
        /// </summary>
        /// <param name="layers">Collection of tuple: building material and thickness of layers is SI Unit (m)</param>
        public CBWallType(string name, List<Tuple<CBBuilidingMaterial, double>> layers, CBWallLocation walltype = CBWallLocation.external)
            : this(name,  walltype)
        {
            WallLayers = layers;
            Thickness = WallLayers.Sum(x => x.Item2);            
        }

        /// <summary>
        /// Create wall with given U value and wall Thickness
        /// </summary>
        /// <param name="u">Heat transfer coefficient of wall in SI Unit (W/m2) </param>      
        public CBWallType(string name, double u, double thickness, CBWallLocation walltype = CBWallLocation.external)
            : this(name, walltype)
        {
            Thickness = thickness;
            HeatTransferCoeff = u;
        }     

    }

    public class CBDoor : CBBuildingElement
    {

        public CBDimensions Dimensions { get; private set; }
        /// <summary>
        /// Heat transfer coefficient of door
        /// </summary>
        override public double HeatTransferCoeff
        {
            get; protected set;

        }
        public CBWall PatternElement { get; private set; }

        /// <summary>
        /// Create new door
        /// </summary>
        /// <param name="dimensions">dimensions of the door in SI Unit</param>
        /// <param name="heatTransferCoeef">Heat transfer coefficient of door in SI Unit (W/m2) </param>
        public CBDoor(CBDimensions dimensions, double heatTransferCoeef)
        {
            Dimensions = dimensions;
            HeatTransferCoeff = heatTransferCoeef;         
        }
    }
    public class CBSpace
    {
        public int Id { get; set; }
        public string Name { get; private set; }
        //TO DO write class
    }

    public class CBDimensions : IEquatable<CBDimensions>
    {
        public double Length { get; set; }
        public double Height { get; set; }
        public double Thickness { get; set; }

        public CBDimensions(double length, double height, double thickness)
        {
            Length = length;
            Height = height;
            Thickness = thickness;
        }

        public bool Equals(CBDimensions other)
        {
            return Length == other.Length &&
                   Height == other.Height &&
                   Thickness == other.Thickness;
        }
    }

    public enum CBWallLocation
    {
        external,
        internatl,
        floor,
        slab,

    }

    public enum Orientation
    {
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW
    }

}
