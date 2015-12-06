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
        public List<CBWall> Walls { get; set; }

        public CBModel()
        {
            Walls = new List<CBWall>();

            Walls.Add(new CBWall("SZ1", new CBDimensions(10, 10, 10), 0.3));
            Walls.Add(new CBWall("SZ2", new CBDimensions(10, 10, 10), 0.4));
            Walls.Add(new CBWall("SZ3", new CBDimensions(10, 10, 10), 0.4));
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

    interface IHeatEchangeable
    {
        double ThermalTransmitance { get; set; }
        double Area { get; set; }
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
    
    public class CBWall : CBBuildingElement
    {
        private double _heatTransferCoeff;

        /// <summary>
        /// Overall thickness of the wall
        /// </summary>
        public double Thickness
        {
            get
            {
                return WallLayers.Sum(x => x.Item2);
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
                return Dimensions.Witdh * Dimensions.Height - AreaCorrection;
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
        public CBWallTypes WallType
        {
            get; private set;
        }

        public List<CBBuildingElement> EmbeddedElements { get; private set; }

        /// <summary>
        /// Main constructor for common initializing. 
        /// </summary>
        /// <param name="dimensions"></param>
        private CBWall(string name, CBDimensions dimensions, CBWallTypes wt)
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
        public CBWall(string name, CBDimensions dimensions, List<Tuple<CBBuilidingMaterial, double>> layers, CBWallTypes walltype = CBWallTypes.external)
            : this(name, dimensions, walltype)  
        {
            WallLayers = layers;
        }

        /// <summary>
        /// Create wall with given U value.
        /// </summary>
        /// <param name="u">Heat transfer coefficient of wall in SI Unit (W/m2) </param>      
        public CBWall(string name, CBDimensions dimensions, double u, CBWallTypes walltype = CBWallTypes.external) 
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
                return Dimensions.Witdh * Dimensions.Height;
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
        public double Witdh { get; set; }
        public double Height { get; set; }
        public double Thickness { get; set; }

        public CBDimensions(double witdh, double height, double thickness)
        {
            Witdh = witdh;
            Height = height;
            Thickness = thickness;
        }

        public bool Equals(CBDimensions other)
        {
            return Witdh == other.Witdh &&
                   Height == other.Height &&
                   Thickness == other.Thickness;
        }
    }



    public enum CBWallTypes
    {
        external,
        internatl,
        floor,
        slab,
        
    }

}
