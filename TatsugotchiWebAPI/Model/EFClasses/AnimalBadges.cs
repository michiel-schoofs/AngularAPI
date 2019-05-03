namespace TatsugotchiWebAPI.Model.EFClasses {
    public class AnimalBadges {
        #region Attributes
            public int BadgeID { get; set; }
            public Badge Badge { get; set; }

            public int AnimalID { get; set; }
            public Animal Animal { get; set; }
        #endregion

        #region Constructor
            //EF Constructor
            protected AnimalBadges() { }

            public AnimalBadges(Badge badge, Animal animal) {
                Badge = badge;
                Animal = animal;
            } 
        #endregion
    }
}
