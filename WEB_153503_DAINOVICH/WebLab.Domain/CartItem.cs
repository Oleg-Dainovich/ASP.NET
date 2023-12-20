using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLab.Domain.Entities;

namespace WebLab.Domain
{
    public class CartItem
    {
        public Movie Movie { get; }
        public int Amount { get; private set; }

        public decimal TotalPrice { get => Amount * Movie.Price; }
        public void IncrementAmount()
        {
            Amount += 1;
        }

        public CartItem(Movie movie, int amount = 1)
        {
            Movie = movie;
            Amount = amount;
        }
    }
}