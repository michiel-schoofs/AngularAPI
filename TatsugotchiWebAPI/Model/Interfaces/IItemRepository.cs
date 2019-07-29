using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TatsugotchiWebAPI.Model.Interfaces
{
    public interface IItemRepository{
        void AddItem(Item item);
        void RemoveItem(Item item);
        Item GetItem(int id);
        void SaveChanges();
    }
}
