using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.Data.Repository {
    public class BadgeRepository : IBadgeRepository {
        
        #region Attributes
        private readonly DbSet<Badge> _badges;
        #endregion

        public BadgeRepository(ApplicationDBContext context) {
            _badges = context.Badges;
        }

        public ICollection<Badge> GiveInitialBadges() {
            return _badges.Where(b=>b.IsInit).ToList();
        }
    }
}
