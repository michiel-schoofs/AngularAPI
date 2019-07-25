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
        private readonly ApplicationDBContext _context;
        #endregion

        #region Constructor
        public BadgeRepository(ApplicationDBContext context) {
            _badges = context.Badges;
            _context = context;
        }
        #endregion

        #region Interface Methods
        public void AddBadge(Badge badge) {
            _badges.Add(badge);
            SaveChanges();
        }

        public List<Badge> GiveInitialBadges() {
            return _badges.Where(b => b.IsInit).ToList();
        } 

        private void SaveChanges() {
            _context.SaveChanges();
        }
        #endregion
    }
}
