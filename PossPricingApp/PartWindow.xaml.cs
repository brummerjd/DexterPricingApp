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
    /// Interaction logic for AddCustomPartWindow.xaml
    /// </summary>
    public partial class PartWindow : Window, INotifyPropertyChanged
    {
        public string Header
        {
            get { return this._Header; }
            set { this._Header = value; OnPropertyChanged("Header"); }
        }
        private string _Header;

        public Part EditPart
        {
            get { return this._EditPart; }
            set { this._EditPart = value; OnPropertyChanged("EditPart"); }
        }
        private Part _EditPart;

        public PartWindow(string title, string header, bool isCustom)
        {
            InitializeComponent();

            this.Title = title;
            this.Header = header;
            this.EditPart = new Part() { IsCustom = isCustom };
        }

        public PartWindow(string title, string header, Part editPart)
        {
            InitializeComponent();

            this.Title = title;
            this.Header = header;
            this.EditPart = editPart;
        }

        private void myWindow_Closing(object sender, CancelEventArgs e)
        {
            this.EditPart.Price = Math.Round(this.EditPart.Price, 2);
        }

        private void NumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender == null) { return; }

            (sender as TextBox).SelectAll();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.EditPart.Code))
            {
                MessageBox.Show("You must specify a code for new custom part.");
                return;
            }
            
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
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
