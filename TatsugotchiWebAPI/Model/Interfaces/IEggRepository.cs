using System.Collections.Generic;

namespace TatsugotchiWebAPI.Model.Interfaces {
    public interface IEggRepository {
        IEnumerable<Egg> GetEggsInNeedOfHatching();
    }
}
