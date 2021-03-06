﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Enums;

namespace TatsugotchiWebAPI.DTO
{
    public class ItemDTO{
        public int ID { get; set; }
        public int MoneyVal { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public string URL { get; set; }
        public string CategoryEnum { get; set; }

        public ItemDTO(Item item) {
            ID = item.ID;
            MoneyVal = item.MoneyValue;
            Name = item.Name;
            Value = item.Value;
            URL = item.URL;

            CategoryEnum = Enum.GetName(typeof(ItemCategory), item.Category);
        }
    }
}
