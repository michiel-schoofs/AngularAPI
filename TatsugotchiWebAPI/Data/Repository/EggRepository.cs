using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.Data.Repository {
    public class EggRepository : IEggRepository {
        private readonly DbSet<Egg> _eggs;

        public EggRepository(ApplicationDBContext context) {
            _eggs = context.Eggs;
        }

        public IEnumerable<Egg> GetEggsInNeedOfHatching() {
            return _eggs.Where(e => e.TimeRemaining.Milliseconds <= 0).ToList();
        }
    }
}
