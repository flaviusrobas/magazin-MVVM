using Caliburn.Micro;
using MagDesktopUI.Library.Api;
using MagDesktopUI.Library.Helpers;
using MagDesktopUI.Library.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MagDesktopUI.Views
{
    public class SalesViewModel : Screen
    {
        IProductEndpoint _productEndpoint;
        IConfigHelper _configHelper;
        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;

        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            // Load the products from the API
            await LoadProducts();

        }

        private async Task LoadProducts()
        {

            var productList = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(productList.ToList());
            //Products = new BindingList<ProductModel>(productList.OrderBy(p => p.ProductName).ToList());

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
            set
            {
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
                return CalculateSubTotal().ToString("C");
            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            foreach (var item in Cart)
            {
                subTotal += (item.Product.RetailPrice * item.QuantityInCart);
            }
            return subTotal;
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate() / 100;

            //Using Linq to calculate tax
            taxAmount = Cart.
                Where(i => i.Product.IsTaxable)
                .Sum(i => i.Product.RetailPrice * i.QuantityInCart * taxRate);

            //foreach (var item in Cart)
            //{
            //    if (item.Product.IsTaxable)
            //    {
            //        taxAmount += (item.Product.RetailPrice * item.QuantityInCart * taxRate);
            //    }
            //}

            return taxAmount;
        }

        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }

        }

        public string Total
        {
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax();
                return total.ToString("C");
            }

        }

        /*does not activate the button [Add to Cart] at initial load*/
        public bool CanAddToCart 
        {
            get
            {
                bool output = false;

                
                    //if (SelectedProduct != null && ItemQuantity > 0)
                    //{
                    //    if (SelectedProduct.QuantityInStock >= ItemQuantity)
                    //    output = true;                        
                    //}



                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
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
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);

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
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
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
