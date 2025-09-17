using Magazin.Library.Internal.DataAccess;
using Magazin.Library.Models;
using MagDesktopUI.Library.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Magazin.Library.DataAccess
{
    public class SaleData : ISaleData
    {
        
        private readonly IProductData _productData;
        private readonly ISqlDataAccess _sql;
        private readonly IConfiguration _config;
        public SaleData(IProductData productData, ISqlDataAccess sql, IConfiguration config)
        {
            _productData = productData;
            _sql = sql;
            _config = config;

        }

        public decimal GetTaxRate()
        {
            string rateText = _config.GetValue<string>("TaxRate"); //ConfigurationManager.AppSettings["TaxRate"];


            bool IsValidTaxRate = Decimal.TryParse(rateText, out decimal output);

            if (IsValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("The Tax Rate is not set up properly");
            }

            output /= 100;

            return output;
        }
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //TODO: Make this SOLID/DRY/Better
            // Start filling in the sale details models we will save to the DB
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            //ProductData products = new ProductData(_config);
            //var newtaxRate = new ConfigHelper();
            var taxRate = GetTaxRate();

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                //Get the information about this product
                var productInfo = _productData.GetProductById(detail.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"Product with Id {detail.ProductId} could not be found in the database.");
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

            //using (SqlDataAccess sql = new SqlDataAccess(_config)) {}

            try
            {
                _sql.StartTransaction("MagData");

                //Save the sale model
                _sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                // Get the Id from the sale model
                sale.Id = _sql.LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();

                //Finish filling in the sale detail models
                foreach (var item in details)
                {
                    item.SaleId = sale.Id;
                    // Save the sale detail models
                    _sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                }

                _sql.CommitTransaction();
            }
            catch
            {

                _sql.RollbackTransaction();
                throw;
            }

        }

        public List<SaleReportModel> GetSaleReports()
        {
            //SqlDataAccess sql = new SqlDataAccess(_config);
            var output = _sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "MagData");
            return output;
        }

    }
}
