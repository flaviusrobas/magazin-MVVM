using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagDesktopUI.Models
{
    public class CartItemDisplayModel : INotifyPropertyChanged
    {
        public ProductDisplayModel? Product { get; set; }

        private int _quantityInCart;

        public int QuantityInCart
        {
            get { return _quantityInCart; }
            set 
            { 
                _quantityInCart = value;
                OnPropertyChanged(nameof(QuantityInCart));
                OnPropertyChanged(nameof(DisplayText));
                
            }
        }

        public string DisplayText
        {
            get
            {
                return $"{Product.ProductName} ({QuantityInCart})";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
