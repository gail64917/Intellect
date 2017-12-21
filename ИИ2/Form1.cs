using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ИИ2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public List<List<double>> Actives = new List<List<double>>();
        public List<string> names = new List<string>();
        public List<FullModel> MainActives = new List<FullModel>();
        public List<ActivePrefer> ResultList = new List<ActivePrefer>();

        private void button1_Click(object sender, EventArgs e)
        {
            using (var reader = new StreamReader(textBox1.Text + ".csv"))
            {
                List<double> listA = new List<double>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    foreach (var item in values)
                    {
                        var duals = item.Split('.');
                        listA.Add(Convert.ToDouble(duals[0]));
                    }
                        
                }
                //names.Add(textBox1.Text.Substring(0, textBox1.Text.Length - 4));
                names.Add(textBox1.Text);
                Actives.Add(listA);

                FullModel current = new FullModel() { ActiveName = textBox1.Text, ActiveValues = listA };
                MainActives.Add(current);

                string valuesActives = "";
                foreach(var value in listA)
                {
                    valuesActives += value + ";";
                }

                //label2.Text += "\r\n" + textBox1.Text.Substring(0, textBox1.Text.Length - 4) + ":   " + valuesActives;
                label2.Text += "\r\n" + textBox1.Text + ":   " + valuesActives;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            matrixCov.Columns.Add("", "");
            //string fullnames
            for (int i = 0; i<Actives.Count; i++)
            {
                if (i != 0)
                    matrixCov.Rows.Add();
                matrixCov.Columns.Add(names[i], names[i]);
            }

            for (int i = 0; i < Actives.Count; i++)
            {
                matrixCov[0, i].Value = names[i];
            }

            for (int i = 0; i < Actives.Count; i++)
            {
                for (int j = 0; j < Actives.Count; j++)
                {
                    matrixCov[i+1, j].Value = MathExtensions.Covariance(Actives[i], Actives[j]) / MathExtensions.StandardDeviation(Actives[i]) / MathExtensions.StandardDeviation(Actives[j]);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Actives.Count; i++)
            {
                functions.Text += "\r\n" + MainActives[i].ActiveName + ": y = " + MainActives[i].CoefficientA + " + (" + MainActives[i].CoefficientK + ") * x";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
           ResultList.Clear();
            string res = "";
            Result.Text = "";
            if (ResultList.Count == 0)
            {
                for (int i = 0; i < MainActives.Count; i++)
                {
                    double prefer = 0;
                    for (int j = 0; j < MainActives.Count; j++)
                    {
                        prefer += MathExtensions.Correlation(MainActives[i].ActiveValues, MainActives[j].ActiveValues) * MainActives[i].CoefficientK / MainActives[i].Variance;
                    }
                    ActivePrefer current = new ActivePrefer() { active = MainActives[i], prefer = prefer };
                    ResultList.Add(current);
                }
            }
            if (Risk.Text == "Min")
            {
                double minVar = ResultList[0].active.Variance;
                for (int i = 0; i < ResultList.Count; i++)
                {
                    if (ResultList[i].active.Variance < minVar)
                    {
                        minVar = ResultList[i].active.Variance;
                    }
                }
                for (int i = 0; i < ResultList.Count; i++)
                {
                    if (ResultList[i].active.Variance == minVar)
                    {
                        ResultList[i].prefer *= 5;
                    }
                }
            }
            if (Risk.Text == "Mean")
            {
                //
            }
            if (Risk.Text == "Max")
            {
                double maxVar = ResultList[0].active.Variance;
                for (int i = 0; i < ResultList.Count; i++)
                {
                    if (ResultList[i].active.Variance > maxVar)
                    {
                        maxVar = ResultList[i].active.Variance;
                    }
                }
                for (int i = 0; i < ResultList.Count; i++)
                {
                    if (ResultList[i].active.Variance == maxVar)
                    {
                        ResultList[i].prefer *= 5;
                    }
                }
            }

            if (Profit.Text == "Min")
            {
                double minK = ResultList[0].active.CoefficientK;
                for (int i = 0; i < ResultList.Count; i++)
                {
                    if (ResultList[i].active.CoefficientK < minK)
                    {
                        minK = ResultList[i].active.CoefficientK;
                    }
                }
                for (int i = 0; i < ResultList.Count; i++)
                {
                    if (ResultList[i].active.CoefficientK == minK)
                    {
                        ResultList[i].prefer *= 5;
                    }
                }
            }
            if (Profit.Text == "Mean")
            {
                //
            }
            if (Profit.Text == "Max")
            {
                double maxK = ResultList[0].active.CoefficientK;
                for (int i = 0; i < ResultList.Count; i++)
                {
                    if (ResultList[i].active.CoefficientK > maxK)
                    {
                        maxK = ResultList[i].active.CoefficientK;
                    }
                }
                for (int i = 0; i < ResultList.Count; i++)
                {
                    if (ResultList[i].active.CoefficientK == maxK)
                    {
                        ResultList[i].prefer *= 5;
                    }
                }
            }

            for (int i = 0; i < ResultList.Count; i++)
            {
                if (ResultList[i].active.ActiveName == Prefer.Text)
                {
                    if (ResultList[i].prefer > 0)
                        ResultList[i].prefer *= 5;
                    else
                        ResultList[i].prefer *= -5;
                }
            }

            for (int i = 0; i < ResultList.Count; i++)
            {
                Random r = new Random();
                double numb1 = ((double)r.Next(5, 10))/10;
                double numb2 = ((double)r.Next(10, 15)) / 10;
                res += "\r\n" + ResultList[i].active.ActiveName + " : " + (float)(ResultList[i].prefer * 1000*numb1) + "      <      " + ResultList[i].prefer*1000 + "      <       " + ResultList[i].prefer * 1000*numb2;
            }
            Result.Text = res;
        }
    }
}
