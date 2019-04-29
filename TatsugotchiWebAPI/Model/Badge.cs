using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TatsugotchiWebAPI.Model {
    public class Badge {
        #region Attributes
        private static readonly Random rand = new Random();
        private static readonly string _imagePath = "\\Images\\BadgeImages";
        #endregion

        #region Properties
        public int ID { get; set; }

        [Required]
        [Range(1, 100)]
        public int Value { get; protected set; }

        [Required]
        [StringLength(200, MinimumLength = 4)]
        public string Description { get; protected set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; protected set; }

        [Required]
        [Range(0.0, 1.0)]
        public double Chance { get; protected set; }
        
        //An initial badge can be radomly given on Animal Generation
        [Required]
        public bool IsInit { get; protected set; }
        #endregion

        #region Association
        [Required]
        public BadgeType Type { get; protected set; }
        [Required]
        public ICollection<Animal> Animals { get; private set; }
        #endregion

        #region Calculated Attributes
            [NotMapped]
            public bool Inheritable { get => Chance != 0;}
            [NotMapped]
            public string IconLink {
                get {
                    string ico = Enum.GetName(typeof(BadgeType), Type);
                    return $"{_imagePath}\\{ico}.png";
                }
            }
        #endregion

        #region Constructors
        //Entity Framework constructor
        protected Badge() {}
        public Badge(int value, string description, string name,
            double chance, bool isInit = false) {
                Value = value;
                Description = description;
                Name = name;
                Chance = chance;
                IsInit = isInit;
            }
        #endregion

        #region Methods
            public bool CalculateInherit() {
                if (Inheritable)
                    return rand.NextDouble() >= Chance;

                return Inheritable;
            }
        #endregion
    }
}
