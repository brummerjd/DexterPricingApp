using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.ComponentModel;

namespace DexterPricingApp
{
    /// <summary>
    /// Interaction logic for CalculateTaxWindow.xaml
    /// </summary>
    public partial class CalculateTotalsWindow : Window, INotifyPropertyChanged
    {
        public double TaxAmount
        {
            get { return this._TaxAmount; }
            set { this._TaxAmount = value; OnPropertyChanged("TaxAmount"); }
        }
        private double _TaxAmount;

        public double TotalCharges
        {
            get { return this._TotalCharges; }
            set { this._TotalCharges = value; OnPropertyChanged("TotalCharges"); }
        }
        private double _TotalCharges;

        public CalculateTotalsWindow()
        {
            InitializeComponent();
        }

        private void myWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.BetweenDatePicker1.SelectedDate = DateTime.Today.AddMonths(-1);
            this.BetweenDatePicker2.SelectedDate = DateTime.Today;

            this.CalculateButton_Click(this, null);
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime date1 = BetweenDatePicker1.SelectedDate ?? DateTime.Today;
            DateTime date2 = BetweenDatePicker2.SelectedDate ?? DateTime.Today;

            DateTime afterDate, beforeDate;
            if (DateTime.Compare(date1, date2) < 0)
            {
                afterDate = date1;
                beforeDate = date2;
            }
            else
            {
                afterDate = date2;
                beforeDate = date1;
            }
            beforeDate = beforeDate.AddDays(1);

            // Calculate taxable charges
            List<Charge> taxableCharges = ((MainWindow)this.Owner).GetTaxableCharges(afterDate, beforeDate);

            List<DateTime?> taxablePrintDates = taxableCharges.Select(o => o.Printed).Distinct().ToList();

            double taxAmount = 0;
            foreach (DateTime? date in taxablePrintDates)
            {
                taxAmount += Math.Round(taxableCharges.Where(o => o.Printed == date).Sum(o => o.Total) * Charge.TAX_PERCENT, 2);
            }

            this.TaxAmount = taxAmount;

            // Calculate total charges
            this.TotalCharges = ((MainWindow)this.Owner).GetTotalCharges(afterDate, beforeDate).Sum(o => o.Total);
        }

        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
