using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TatsugotchiWebAPI.Model {
    public class Animal {

        #region Basic attributes
            public string Name { get; private set; }
            public int Speed { get; private set; }
            public int Hunger { get; private set; }
            public int Charisma { get; private set; }
            public int Boredom { get; private set; } 
            public string Image { get; private set; }
            public DateTime BirthDate { get; private set; }
        #endregion




    }
}
