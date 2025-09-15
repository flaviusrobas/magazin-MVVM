using Magazin.Library.Models;

namespace Magazin.Library.DataAccess
{
    public interface ISaleData
    {
        List<SaleReportModel> GetSaleReports();
        void SaveSale(SaleModel saleInfo, string cashierId);
    }
}