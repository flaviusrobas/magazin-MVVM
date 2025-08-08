using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagDesktopUI.Views
{
    public class SalesViewModel :Screen
    {
       private BindingList<string> _products;

       public BindingList<string> Products
        {
            get { return _products; }
            set { _products = value;
            NotifyOfPropertyChange(() => Products);
            }
        }

        private BindingList<string> _cart;

        public BindingList<string> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        private string _itemQuantity;

        public string ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }

        public string SubTotal
        {
            //TODO - replace this with a real calculation
            get { return "$0.00"; }

        }

        public string Tax
        {
            //TODO - replace this with a real calculation
            get { return "$0.00"; }

        }

        public string Total
        {
            //TODO - replace this with a real calculation
            get { return "$0.00"; }

        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                //Make sure ItemQuantity is not null or empty
                // Check if ItemQuantity is not null or empty and is a valid number
                return output;
            }
        }

        public void AddToCart()
        { }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                //Make sure ItemQuantity is not null or empty
               
                return output;
            }
        }

        public void RemoveFromCart()
        { }

        public bool CanCeckOut
        {
            get
            {
                bool output = false;

                //Make sure ItemQuantity is not null or empty
                
                return output;
            }
        }
    }
}
