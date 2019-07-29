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

        //Called from multithreaded enviroment
        public ICollection<Item> GetItems(){
            using (ApplicationDBContext context = new ApplicationDBContext()) {
                context.Items.Load();
                return context.Items.ToList();
            }
        }

        public void RemoveItem(Item item) {
            _items.Remove(item);
        }

        public void SaveChanges(){
            _context.SaveChanges();
        }
    }
}
