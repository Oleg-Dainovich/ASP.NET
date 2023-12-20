
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLab.Domain.Entities;

namespace WebLab.Domain
{
    public class Cart
    {
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        public virtual void AddToCart(Movie movie)
        {
            if (CartItems.ContainsKey(movie.Id))
            {
                CartItems[movie.Id].IncrementAmount();
            }
            else
            {
                CartItems.Add(movie.Id, new CartItem(movie));
            }
        }
        public virtual void RemoveItems(int id)
        {
            CartItems.Remove(id);
        }
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        public int Count { get => CartItems.Sum(item => item.Value.Amount); }
        public decimal TotalPrice
        {
            get => CartItems.Sum(item => item.Value.TotalPrice);
        }
    }
}