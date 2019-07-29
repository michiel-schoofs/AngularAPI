using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.Data.Repository
{
    public class ItemRepository : IItemRepository{
        private readonly DbSet<Item> _items;
        private readonly ApplicationDBContext _context;

        public ItemRepository(ApplicationDBContext context){
            _context = context;
            _items = context.Items;
        }

        public void AddItem(Item item){
            _items.Add(item);
        }

        public Item GetItem(int id){
            return _items.FirstOrDefault(i => i.ID == id);
        }

        public void RemoveItem(Item item) {
            _items.Remove(item);
        }

        public void SaveChanges(){
            _context.SaveChanges();
        }
    }
}
