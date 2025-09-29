using System.Configuration;

namespace MagDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        // To Do this from config to the API
        public decimal GetTaxRate()
        {
            string? rateText = ConfigurationManager.AppSettings["TaxRate"];
            

            bool IsValidTaxRate = Decimal.TryParse(rateText, out decimal output);

            if (IsValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("The Tax Rate is not set up properly");
            }

            return output;


        }
    }
}
