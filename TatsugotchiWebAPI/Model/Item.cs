using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Model.Enums;

namespace TatsugotchiWebAPI.Model
{
    public class Item{
        #region Properties
            public int ID { get; set; }
            [Range(0,Int16.MaxValue)]
            public int MoneyValue { get; set; }
            [Required(AllowEmptyStrings =false)]
            public string Name { get; set; }
            [Range(0,100)]
            public int Value { get; set; }
            [Required(AllowEmptyStrings = false)]
            public string URL { get; set; }
        #endregion

        #region Associations
            [Required]
            public ItemCategory Category { get; set; }
        #endregion

        #region Constructor
            //EF Constructor
            protected Item() { }

            public Item(int moneyValue, int value, ItemCategory cat, string url,string name){
                MoneyValue = moneyValue;
                Value = value;
                Category = cat;
                URL = url;
                Name = name;
            } 
        #endregion
    }
}
