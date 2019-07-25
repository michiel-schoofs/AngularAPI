using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TatsugotchiWebAPI.Model.EFClasses;
using TatsugotchiWebAPI.Model.Enums;

namespace TatsugotchiWebAPI.Model {
    public class Badge {
        #region Attributes
        private static readonly Random rand = new Random();
        private static readonly string _imagePath = "\\Images\\BadgeImages";
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

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
        #endregion

        #region Calculated Attributes
            [NotMapped]
            public bool Inheritable { get => Chance != 0;}

            [NotMapped]
            public int Value { get => 100 - (int)(Chance * 10); }
        #endregion

        #region Constructors
        //Entity Framework constructor
        protected Badge() {}
        public Badge(string description, string name,
            double chance, BadgeType type, bool isInit = false) {
                Description = description;
                Name = name;
                Chance = chance;
                Type = type;
                IsInit = isInit;
            }
        #endregion

        #region Methods
            public bool CalculateInherit() {
            var r = rand.NextDouble();
            if (Inheritable)
                return r >= Chance;

                return Inheritable;
            }
        #endregion
    }
}
