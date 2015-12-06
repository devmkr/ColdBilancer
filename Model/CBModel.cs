using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdBilancer
{
    public class CBModel : INotifyPropertyChanged
    {
        public List<CBWallType> Walls { get; set; }

        public CBModel()
        {
            Walls = new List<CBWallType>();

            Walls.Add(new CBWallType("SZ1", new CBDimensions(100, 100, 10), 10));
            Walls.Add(new CBWallType("SZ2", new CBDimensions(100, 100, 10), 0.4));
            Walls.Add(new CBWallType("SZ3", new CBDimensions(100, 100, 10), 0.4));
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
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
        /// Area of heat exchange.
        /// Value should be returned in SI Units (sqm)
        /// </summary>
        abstract public double Area { get; }
        
        /// <summary>
        /// Name of building elements
        /// Should be implement change Name.
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// Dimensions of builidng elements.
        /// Value should be returned in SI Units (m,m,m)
        /// </summary>
        public CBDimensions Dimensions { get; protected set; }
    }
    
    public class CBWallType : CBBuildingElement
    {
        private double _heatTransferCoeff;

        /// <summary>
        /// Overall thickness of the wall
        /// </summary>
        public double Thickness
        {
            get
            {
                return  Dimensions.Thickness;
            }
        }        
        /// <summary>
        /// List of wall layers. Index of list represents the order following order. 
        /// T2 in tuple represents the thickness of the layer.
        /// </summary>
        public List<Tuple<CBBuilidingMaterial,double>> WallLayers { get; private set; }

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

        /// <summary>
        /// Area of heat exchange.
        /// </summary>
        public override double Area
        {
            get
            {
                return Dimensions.Length * Dimensions.Height - AreaCorrection;
            }
        }

        /// <summary>
        /// Area of openings, doors, windows embedded in the wall. 
        /// </summary>
        public double AreaCorrection
        {
            get
            {
                return EmbeddedElements.Sum(x => x.Area);
            }
        }
        public CBWallLocation WallType
        {
            get; private set;
        }

        public List<CBBuildingElement> EmbeddedElements { get; private set; }

        /// <summary>
        /// Main constructor for common initializing. 
        /// </summary>
        /// <param name="dimensions"></param>
        private CBWallType(string name, CBDimensions dimensions, CBWallLocation wt)
        {
            Dimensions = dimensions;
            WallType = wt;
            Name = name;

            EmbeddedElements = new List<CBBuildingElement>();
        }

        /// <summary>
        /// Create wall in accordance with given layers.
        /// </summary>
        /// <param name="layers">Collection of tuple: building material and thickness of layers is SI Unit (m)</param>
        public CBWallType(string name, CBDimensions dimensions, List<Tuple<CBBuilidingMaterial, double>> layers, CBWallLocation walltype = CBWallLocation.external)
            : this(name, dimensions, walltype)  
        {            
            WallLayers = layers;
            Dimensions.Thickness = WallLayers.Sum(x => x.Item2);
        }

        /// <summary>
        /// Create wall with given U value.
        /// </summary>
        /// <param name="u">Heat transfer coefficient of wall in SI Unit (W/m2) </param>      
        public CBWallType(string name, CBDimensions dimensions, double u, CBWallLocation walltype = CBWallLocation.external) 
            : this(name, dimensions,walltype)
        {
            HeatTransferCoeff = u; 
        }

        internal void AddEmbeddedElement(CBBuildingElement embeddedelement) => EmbeddedElements.Add(embeddedelement);
    
        internal void RemoveEmbeddedElement(CBBuildingElement embeddeelement)
        {
            if(!EmbeddedElements.Contains(embeddeelement))            
                throw new ArgumentException(embeddeelement.ToString());

           EmbeddedElements.Remove(embeddeelement);
        }       
        
    }

    public class CBDoor : CBBuildingElement
    {
        /// <summary>
        /// Door area
        /// </summary>
        public override double Area
        {
            get
            {
                return Dimensions.Length * Dimensions.Height;
            }
        }

        /// <summary>
        /// Heat transfer coefficient of door
        /// </summary>
        override public double HeatTransferCoeff
        {
            get; protected set;
           
        }
        public CBBuildingElement PatternElement { get; private set; }

        /// <summary>
        /// Create new door
        /// </summary>
        /// <param name="dimensions">dimensions of the door in SI Unit</param>
        /// <param name="heatTransferCoeef">Heat transfer coefficient of door in SI Unit (W/m2) </param>
        public CBDoor(CBDimensions dimensions, double heatTransferCoeef, CBBuildingElement patternElement)
        {
            Dimensions = dimensions;
            HeatTransferCoeff = heatTransferCoeef;
            PatternElement = patternElement;
        }
    }
    public class CBSpace
    {
        public int Id { get; set; }
        public string Name { get; private set; }     


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
