using System.Collections.Generic;

namespace MagDesktopUI
{
    public interface ICalculations
    {
        List<string> Register { get; set; }

        double Add(double x, double y);
    }
}