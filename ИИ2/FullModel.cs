using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ИИ2
{
    public class FullModel
    {
        public string ActiveName { get; set; }
        public List<double> ActiveValues { get; set; }
        public double Mean { get { return MathExtensions.Mean(ActiveValues); } }
        public double Variance { get { return MathExtensions.Variance(ActiveValues); } }
        public double CoefficientA { get { return MathExtensions.CoefficientA(ActiveValues); } }
        public double CoefficientK { get { return MathExtensions.CoefficientK(ActiveValues); } }
    }
}
