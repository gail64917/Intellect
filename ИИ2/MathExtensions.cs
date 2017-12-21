using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ИИ2
{
    public static class MathExtensions
    {
        public static double Mean(this List<double> values)
        {
            return values.Count == 0 ? 0 : values.Mean(0, values.Count);
        }

        public static double Mean(this List<double> values, int start, int end)
        {
            double s = 0;

            for (int i = start; i < end; i++)
            {
                s += values[i];
            }

            return s / (end - start);
        }

        public static double Variance(this List<double> values)
        {
            return values.Variance(values.Mean(), 0, values.Count);
        }

        public static double Variance(this List<double> values, double mean)
        {
            return values.Variance(mean, 0, values.Count);
        }

        public static double Variance(this List<double> values, double mean, int start, int end)
        {
            double variance = 0;

            for (int i = start; i < end; i++)
            {
                variance += Math.Pow((values[i] - mean), 2);
            }

            int n = end - start;
            if (start > 0) n -= 1;

            return variance / (n);
        }

        public static double StandardDeviation(this List<double> values)
        {
            return values.Count == 0 ? 0 : values.StandardDeviation(0, values.Count);
        }

        public static double StandardDeviation(this List<double> values, int start, int end)
        {
            double mean = values.Mean(start, end);
            double variance = values.Variance(mean, start, end);

            return Math.Sqrt(variance);
        }

        public static double Covariance(List<double> values1, List<double> values2)
        {
            if (values1.Count == values2.Count)
            {
                double covar = 0;
                for (int i = 0; i<values1.Count; i++)
                {
                    covar += (values1[i] - values1.Mean()) * (values2[i] - values2.Mean());
                }
                return covar / values1.Count;
            }
            else
            {
                return 0;
            }
        }

        public static double Correlation(List<double> values1, List<double> values2)
        {
            return MathExtensions.Covariance(values1, values2) / MathExtensions.StandardDeviation(values1) / MathExtensions.StandardDeviation(values2);
        }

        public static double CoefficientK(List<double> values1)
        {
            List<double> indexes = new List<double>();
            for (int i = 0; i<values1.Count; i++)
            {
                indexes.Add(i);
            }
            double K = MathExtensions.Covariance(values1, indexes) / MathExtensions.Variance(indexes);
            return K;
        }

        public static double CoefficientA(List<Double> values1)
        {
            List<double> indexes = new List<double>();
            for (int i = 0; i < values1.Count; i++)
            {
                indexes.Add(i);
            }
            double K = MathExtensions.Covariance(values1, indexes) / MathExtensions.Variance(indexes);
            double A = values1.Mean() - K * indexes.Mean();
            return A;
        }
    }
}
