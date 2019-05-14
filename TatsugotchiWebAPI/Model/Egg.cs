﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TatsugotchiWebAPI.Model.Enums;

namespace TatsugotchiWebAPI.Model {
    public class Egg {
        public int ID { get; set; }
        public Animal Mother { get; set; }
        public Animal Father { get; set; }
        public DateTime DateConceived { get; set; }
        public AnimalType Type { get; set; }



        [NotMapped]
        public TimeSpan TimeRemaining { get => (DateConceived.Add(Span)).Subtract(DateTime.Now); }
        [NotMapped]
        public TimeSpan Span { get{
                TimeSpan var = new TimeSpan();

                switch (this.Type) {
                    case AnimalType.Alpaca:
                        var = new TimeSpan(5,0,0,0);
                        break;
                    case AnimalType.Capybara:
                        var = new TimeSpan(2, 12, 0, 0);
                        break;
                    case AnimalType.Tapir:
                        var = new TimeSpan(5, 0, 0, 0);
                        break;
                }

                return var;
            }
        }

        public Egg(Animal mother,Animal father) {
            if (mother == null || father == null)
                throw new ArgumentException("Neither of the parents can be null");

            Mother = mother;
            Father = father;
            DateConceived = DateTime.Now;
            Type = Mother.Type;
        }

        //EF Constructor
        protected Egg() {}

    }
}
