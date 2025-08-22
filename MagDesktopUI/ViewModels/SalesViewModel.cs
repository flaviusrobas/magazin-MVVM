using Caliburn.Micro;
using MagDesktopUI.Library.Api;
using MagDesktopUI.Library.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MagDesktopUI.Views
{
    public class SalesViewModel : Screen
    {
        IProductEndpoint _productEndpoint;
        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
            
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            // Load the products from the API
            await LoadProducts();
            //// Initialize the cart
            //Cart = new BindingList<ProductModel>();
            //ItemQuantity = 1; // Set default quantity to 1
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(productList.ToList());
            //Products = new BindingList<ProductModel>(productList.OrderBy(p => p.ProductName).ToList());
            //NotifyOfPropertyChange(() => Products);
        }
        
        private BindingList<ProductModel> _products;

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set 
            { 
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
            }
        }


        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();

        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        private int _itemQuantity = 1; // Caliburn.Micro let use int in XAML

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string SubTotal
        {
            get
            {
                decimal subTotal = 0;
                foreach (var item in Cart)
                {
                    subTotal += (item.Product.RetailPrice * item.QuantityInCart);
                }
                return subTotal.ToString("C");
            }
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
                if(ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }

                return output;
            }
        }

        public void AddToCart()
        {
            CartItemModel existingItem = Cart.FirstOrDefault(i => i.Product.Id == SelectedProduct.Id);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                //Hack - There should be a better way or refreshing the cart display
                Cart.Remove(existingItem);
                Cart.Add(existingItem);

            }
            else
            {
                CartItemModel cartItem = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(cartItem);
            }
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1; // Reset quantity to 1 after adding to cart
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Cart);
            //NotifyOfPropertyChange(() => existingItem.DisplayText); //Hack - not working correctly
        }

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
        {
            NotifyOfPropertyChange(() => SubTotal);
        }

        public bool CanCheckOut
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
