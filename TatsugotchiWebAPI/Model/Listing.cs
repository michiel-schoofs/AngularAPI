using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.Model.Enums;
using TatsugotchiWebAPI.Model.Exceptions;

namespace TatsugotchiWebAPI.Model
{
    public class Listing{
        #region Simple Properties
            public int ID { get; set; }
            public bool IsAdoptable { get; set; }
            public bool IsBreedable { get; set; }
            public int AdoptAmount { get; set; }
            public int BreedAmount { get; set; }
        #endregion

        #region Associations
            public Animal Animal { get; set; }
            public int AnimalID { get; set; }

            [NotMapped]
            public PetOwner Owner { get=>Animal.Owner; }
        #endregion
        
        //EF be retarded
        protected Listing(){}

        //Constructor if you don't provide custom values
        public Listing(Animal an, bool forAdoption,bool forBreeding) :
            this(an, forAdoption, forBreeding,an.AnimalValue, an.AnimalValue / 2) {}


        public Listing(Animal an, bool forAdoption, bool forBreeding,
            int adoptAmount,int breedAmount){

            if (!an.CanBreed && forBreeding)
                throw new InvalidListingException("This animal can't breed");

            if(forAdoption)
                CheckAmount(an, adoptAmount,true);
            if (forBreeding)
                CheckAmount(an, breedAmount, false);

            if (forBreeding == false && forAdoption == false)
                throw new InvalidListingException("You need to at least list for adoption or for breeding");

            //You can't put up females for breeding otherwise if you close up the account the egg is deleted.
            if(forBreeding && an.Gender == AnimalGender.Female)
                throw new InvalidListingException("You can't put up females for breeding");

            Animal = an;

            IsAdoptable = forAdoption;
            IsBreedable = forBreeding;

            AdoptAmount = adoptAmount;
            BreedAmount = breedAmount;
        }

        public void ChangeBreedingAmount(int amount) {
            if (!IsBreedable)
                throw new InvalidListingException("This listing isn't for a breeding");

            CheckAmount(Animal, amount,false);

            BreedAmount = amount;
        }

        public void ChangeAdoptionAmount(int amount){
            if (!IsAdoptable)
                throw new InvalidListingException("This listing isn't for an adoption");

            CheckAmount(Animal, amount,true);

            AdoptAmount = amount;
        }

        private void CheckAmount(Animal an,double amount,bool forAdoption){
            if (amount <= 0)
                throw new InvalidListingException("That's an invalid amount");

            if (forAdoption){
                if (an.AnimalValue < amount)
                    throw new InvalidListingException("You can't put it up for adoption/breeding more then it's worth");
            }else {
                if(an.AnimalValue / 2 < amount)
                    throw new InvalidListingException("You can't put it up for breeding more then it's half of it's worth");
            }
        }

        public void AcceptAdoption(PetOwner po){
            if(!IsAdoptable)
                throw new InvalidListingException("This animal wasn't put up for adoption");

            if (Owner == po)
                throw new InvalidListingException("You are the owner of the animal so you can't adopt it");

            if (Owner.WalletAmount < AdoptAmount)
                throw new InvalidListingException("You don't have enough funds to make this adoption");

            this.Animal.Owner.WalletAmount += AdoptAmount;
            po.WalletAmount -= AdoptAmount;

            Animal.Owner = po;
        }

        public Egg AcceptBreeding(PetOwner po, Animal an, string name)
        {
            if (!IsBreedable)
                throw new InvalidListingException("This animal wasn't put up for breeding");

            if (Owner == po)
                throw new InvalidListingException("You are the owner of the animal so you can't breed trough a listing");

            if (po.WalletAmount < BreedAmount)
                throw new InvalidListingException("You don't have enough funds to breed with this animal");

            var egg = an.Breed(Animal, name);

            this.Animal.Owner.WalletAmount += BreedAmount;
            po.WalletAmount -= BreedAmount;

            return egg;
        }
    }
}
