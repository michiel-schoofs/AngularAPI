namespace TatsugotchiWebAPI.Model.EFClasses {
    //Tussen tabel voor EF
    public class ChildParentAnimal {
        #region Properties
        public int IDParent { get; set; }
        public Animal Parent { get; set; }

        public int IDChild { get; set; }
        public Animal Child { get; set; }
        #endregion

        #region Constructors
        //EF Constructor
        protected ChildParentAnimal() { }

        public ChildParentAnimal(Animal parent, Animal child) {
            Parent = parent;
            Child = child;
        } 
        #endregion
    }
}
