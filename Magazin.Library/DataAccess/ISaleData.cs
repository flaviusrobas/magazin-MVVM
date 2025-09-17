using Magazin.Library.Models;

namespace Magazin.Library.DataAccess
{
    public interface ISaleData
    {
        List<SaleReportModel> GetSaleReports();
        decimal GetTaxRate();
        void SaveSale(SaleModel saleInfo, string cashierId);
    }
}