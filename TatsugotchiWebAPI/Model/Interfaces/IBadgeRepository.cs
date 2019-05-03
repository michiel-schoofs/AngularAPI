using System.Collections.Generic;

namespace TatsugotchiWebAPI.Model.Interfaces {
    public interface IBadgeRepository {
        #region Interface Methods
        ICollection<Badge> GiveInitialBadges();
        void AddBadge(Badge badge);
        #endregion
    }
}
