//using Magazin.Library.Internal.DataAccess;
//using Magazin.Library.Internal.DataAccess;
using Magazin.Library.Internal.DataAccess;
using Magazin.Library.Models;
using MagDesktopUI.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Magazin.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //TODO: Make this SOLID/DRY/Better
            // Start filling in the sale details models we will save to the DB
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData();
            var newtaxRate = new ConfigHelper();
            decimal taxRate = newtaxRate.GetTaxRate() / 100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                //Get the information about this product
                var productInfo = products.GetProductById(item.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"Product with Id {item.ProductId} could not be found in the database.");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);

                if (productInfo.IsTaxable)
                {
                    detail.Tax = Math.Round((detail.PurchasePrice * taxRate), 2);
                }
                else
                {
                    detail.Tax = 0;
                }

                details.Add(detail);
            }


            // Create the sale model            
            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId,  // transmitem id-ul casierului pentru a afla cine sa conectat
            };

            sale.Total = sale.SubTotal + sale.Tax;

            using (SqlDataAccess sql = new SqlDataAccess())
            {
                try
                {
                    sql.StartTransaction("MagData");

                    //Save the sale model
                    sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                    // Get the Id from the sale model
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();

                    //Finish filling in the sale detail models
                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        // Save the sale detail models
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }

                    sql.CommitTransaction();
                }
                catch 
                {

                    sql.RollbackTransaction();
                    throw;
                }

                
            }
        }

        //public List<ProductModel> GetProductsById(string Id)
        //{
        //    SqlDataAccess sql = new SqlDataAccess();
        //    var p = new { Id = Id };
        //    var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "MagData");
        //    return output;
        //}
    }
}
