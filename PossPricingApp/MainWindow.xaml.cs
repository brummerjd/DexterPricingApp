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
using System.Windows.Navigation;

using EPocalipse.IFilter;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls.Primitives;

namespace DexterPricingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// 1) Is Microsoft.Win32 kosher to use
        /// 2) 
        /// 3) Separate NumberTextBox and DecimalTextBox
        /// </summary>

        public string DB_CONN_STRING;

        private DexterPricingDatabase _DB;

        public bool EditMode
        {
            get { return this._EditMode; }
            set { this._EditMode = value; OnPropertyChanged("EditMode"); }
        }
        private bool _EditMode;

        public bool ShowPartCharges
        {
            get { return this._ShowPartCharges; }
            set { this._ShowPartCharges = value; OnPropertyChanged("ShowPartCharges"); }
        }
        private bool _ShowPartCharges = true;

        public bool ShowPrintedCharges
        {
            get { return this._ShowPrintedCharges; }
            set { this._ShowPrintedCharges = value; OnPropertyChanged("ShowPrintedCharges"); }
        }
        private bool _ShowPrintedCharges = false;

        private int _OldChargeKey;

        #region Lists and Selected variables
        public string SearchCustomer
        {
            get { return this._SearchCustomer; }
            set { this._SearchCustomer = value; OnPropertyChanged("SearchCustomer"); }
        }
        private string _SearchCustomer;

        public ObservableCollection<Customer> Customers
        {
            get { return this._Customers; }
            set { this._Customers = value; OnPropertyChanged("Customers"); }
        }
        private ObservableCollection<Customer> _Customers;

        public Customer SelectedCustomer
        {
            get { return this._SelectedCustomer; }
            set { this._SelectedCustomer = value; OnPropertyChanged("SelectedCustomer"); }
        }
        private Customer _SelectedCustomer;

        public string SearchPart
        {
            get { return this._SearchPart; }
            set { this._SearchPart = value; OnPropertyChanged("SearchPart"); }
        }
        private string _SearchPart;

        public ObservableCollection<Part> Parts
        {
            get { return this._Parts; }
            set { this._Parts = value; OnPropertyChanged("Parts"); }
        }
        private ObservableCollection<Part> _Parts;

        public Part SelectedPart
        {
            get { return this._SelectedPart; }
            set { this._SelectedPart = value; OnPropertyChanged("SelectedPart"); }
        }
        private Part _SelectedPart;

        public PartCharge SelectedPartCharge
        {
            get { return this._SelectedPartCharge; }
            set { this._SelectedPartCharge = value; OnPropertyChanged("SelectedPartCharge"); }
        }
        private PartCharge _SelectedPartCharge = new PartCharge() { IsTaxable = false, Quantity = 1 };

        public MiscCharge SelectedMiscCharge
        {
            get { return this._SelectedMiscCharge; }
            set { this._SelectedMiscCharge = value; OnPropertyChanged("SelectedMiscCharge"); }
        }
        private MiscCharge _SelectedMiscCharge = new MiscCharge() { Date = DateTime.Today, IsTaxable = false };

        public ObservableCollection<Charge> Charges
        {
            get { return this._Charges; }
            set { this._Charges = value; OnPropertyChanged("Charges"); }
        }
        private ObservableCollection<Charge> _Charges;

        public Charge SelectedCharge
        {
            get { return this._SelectedCharge; }
            set { this._SelectedCharge = value; OnPropertyChanged("SelectedCharge"); }
        }
        private Charge _SelectedCharge;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = myWindow;
        }

        private void myWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //JDB.Library.DatabaseFunctions.GenerateDatabaseClassFile(
            //    "DexterPricingDatabase",
            //    @"C:\Users\Josh\Desktop\Dexter Application Contents\DexterDatabase.sdf",
            //    @"C:\Users\Josh\Desktop\Dexter Application Contents\DexterDatabase.cs"
            //);
            //return;

            if (string.IsNullOrEmpty(DexterPricingApp.Properties.Settings.Default.StorageLocation) || !Directory.Exists(DexterPricingApp.Properties.Settings.Default.StorageLocation))
            {
                string storageLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "BE Application Contents");
                if (Directory.Exists(storageLocation))
                {
                    DexterPricingApp.Properties.Settings.Default.StorageLocation = storageLocation;
                    DexterPricingApp.Properties.Settings.Default.Save();
                }
                else
                {
                    storageLocation = JDB.Library.DatabaseFunctions.GetDatabaseLocation("Select folder containing application contents.");
                    if (!string.IsNullOrEmpty(storageLocation))
                    {
                        DexterPricingApp.Properties.Settings.Default.StorageLocation = storageLocation;
                        DexterPricingApp.Properties.Settings.Default.Save();
                    }
                    else
                    {
                        this.CloseMenuItem_Click(this, null);
                    }
                }
            }

            if (Directory.GetFiles(DexterPricingApp.Properties.Settings.Default.StorageLocation, "DexterDatabase.sdf", SearchOption.TopDirectoryOnly).Length == 0)
            {
                DexterPricingApp.Properties.Settings.Default.StorageLocation = "";
                DexterPricingApp.Properties.Settings.Default.Save();

                MessageBox.Show("Database file not found in specified contents folder. Try re-opening and selecting different contents folder.");
                this.CloseMenuItem_Click(this, null);
            }

            string receiptsFolderLocation = Path.Combine(DexterPricingApp.Properties.Settings.Default.StorageLocation, "Receipts");
            if (!Directory.Exists(receiptsFolderLocation)) { Directory.CreateDirectory(receiptsFolderLocation); }

            string databaseBackupsFolderLocation = Path.Combine(DexterPricingApp.Properties.Settings.Default.StorageLocation, "Database Backups");
            if (!Directory.Exists(databaseBackupsFolderLocation)) { Directory.CreateDirectory(databaseBackupsFolderLocation); }

            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            int week = cul.Calendar.GetWeekOfYear(DateTime.Today, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            string currentDatabaseBackupPath = Path.Combine(databaseBackupsFolderLocation, string.Format("DexterDatabaseBackup_{0:00}_{1:yyyy}.sdf", week, DateTime.Today));
            if (!File.Exists(currentDatabaseBackupPath))
            {
                File.Copy(Path.Combine(DexterPricingApp.Properties.Settings.Default.StorageLocation, "DexterDatabase.sdf"), currentDatabaseBackupPath);
            }

            DB_CONN_STRING = Path.Combine(DexterPricingApp.Properties.Settings.Default.StorageLocation, "DexterDatabase.sdf");

            this._DB = new DexterPricingDatabase
            (
                DB_CONN_STRING
            );

            this.RefreshCustomerList();
            this.RefreshPartList();

            this.BetweenDatePicker1.SelectedDate = DateTime.Today.AddMonths(-1);
            this.BetweenDatePicker2.SelectedDate = DateTime.Today;
        }

        private void myWindow_Closed(object sender, EventArgs e)
        {
            this._DB.SubmitChanges();
        }

        private void RandomChargeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Random r = new Random();

            //this.PricePerTonDistillers = Math.Round(r.NextDouble(), 2);
            //this.PricePerTonHay = Math.Round(r.NextDouble(), 2);
            //this.PricePerTonCorn = Math.Round(r.NextDouble(), 2);
            //this.PricePerTonMinerals = Math.Round(r.NextDouble(), 2);
            //this.PricePerHeadYardage = Math.Round(r.NextDouble(), 2);

            //this.SelectedWorkDay = new WorkDay();
            //this.SelectedDate = DateTime.Today;
            //this.SelectedWorkDay.PoundsDistillers = r.Next(200);
            //this.SelectedWorkDay.PoundsHay = r.Next(200);
            //this.SelectedWorkDay.PoundsCorn = r.Next(200);
            //this.SelectedWorkDay.PoundsMinerals = r.Next(200);
            //this.SelectedWorkDay.NumberOfHeadYardage = r.Next(200);

            //this.AddChargeButton_Click(null, null);
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender == null) { return; }

            (sender as TextBox).SelectAll();
        }

        private void SelectedCustomer_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectedCustomer == null) { return; }

            var charges = new ObservableCollection<Charge>();

            if (this.ShowPartCharges)
            {
                foreach (PartCharge part in this._DB.PartCharge.Where(o => (this.ShowPrintedCharges ? true : o.Printed == null) && o.CustomerID == this.SelectedCustomer.CustomerID))
                {
                    charges.Add(new Charge(part));
                }
            }

            foreach (MiscCharge misc in this._DB.MiscCharge.Where(o => (this.ShowPrintedCharges ? true : o.Printed == null) && o.CustomerID == this.SelectedCustomer.CustomerID))
            {
                charges.Add(new Charge(misc));
            }

            this.Charges = new ObservableCollection<Charge>(charges.OrderByDescending(o => o.Date));
            this.SelectedPartCharge = new PartCharge() { IsTaxable = false, Quantity = 1 };
            this.SelectedMiscCharge = new MiscCharge() { Date = DateTime.Today, IsTaxable = false };
        }

        private void PartsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectedPart != null)
            {
                this.PartsGrid.ScrollIntoView(this.SelectedPart);
            }
        }

        private void EditPart_Click(object sender, RoutedEventArgs e)
        {
            Part editPart = this.SelectedPart;
            if (editPart == null) { return; }

            Part originalPart = new Part()
            {
                Code = editPart.Code,
                Description = editPart.Description,
                IsCustom = editPart.IsCustom,
                PartKey = editPart.PartKey,
                Price = editPart.Price
            };

            PartWindow win = new PartWindow("Edit Part " + editPart.Code, "Edit Part", editPart);
            win.Owner = this;

            if (win.ShowDialog() ?? false)
            {
                this._DB.SubmitChanges();
            }
            else
            {
                this.Parts[this.Parts.IndexOf(this.SelectedPart)] = originalPart;
                this.SelectedPart = originalPart;

            }
        }

        private void DeletePart_Click(object sender, RoutedEventArgs e)
        {
            Part part = this.SelectedPart;

            if (part == null) { return; }

            if (MessageBox.Show("Are you sure you want to delete this part? This action cannot be undone.", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this._DB.Part.DeleteOnSubmit(this.SelectedPart);
                this._DB.SubmitChanges();

                this.RefreshPartList();
            }
        }

        private void ShowPartChargesCheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedCustomer_Changed(this, null);
        }

        private void ShowPrintedChargesCheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedCustomer_Changed(this, null);
        }

        private void ChargesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Charge charge = this.SelectedCharge;

            if (charge == null) { return; }

            if (charge.Type == ChargeType.Part)
            {
                this.PartTab.IsSelected = true;

                this.SelectedMiscCharge = null;

                this.SelectedPartCharge = Charge.Clone(this._DB.PartCharge.Where(o => o.PartChargeKey == charge.Key).FirstOrDefault());
            }
            else
            {
                this.MiscTab.IsSelected = true;

                this.SelectedPartCharge = null;

                this.SelectedMiscCharge = Charge.Clone(this._DB.MiscCharge.Where(o => o.MiscChargeKey == charge.Key).FirstOrDefault());
            }
        }

        private void EditChargeItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedCharge == null) { return; }

            this._OldChargeKey = this.SelectedCharge.Key;
            if (this.SelectedCharge.Type == ChargeType.Part) { this.PartTab.IsEnabled = true; }
            else { this.MiscTab.IsEnabled = true; }

            this.EditMode = true;
        }

        private void DeleteChargeItem_Click(object sender, RoutedEventArgs e)
        {
            Charge charge = this.SelectedCharge;

            if (charge == null) { return; }

            if (MessageBox.Show("Are you sure you want to delete this charge? This action cannot be undone.", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (charge.Type == ChargeType.Part)
                {
                    PartCharge part = this._DB.PartCharge.Where(o => o.PartChargeKey == charge.Key).First();
                    this._DB.PartCharge.DeleteOnSubmit(part);
                    this._DB.SubmitChanges();
                }
                else
                {
                    MiscCharge misc = this._DB.MiscCharge.Where(o => o.MiscChargeKey == charge.Key).First();
                    this._DB.MiscCharge.DeleteOnSubmit(misc);
                    this._DB.SubmitChanges();
                }

                this.SelectedCustomer_Changed(null, null);
            }
        }

        private void NewCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            JDB.Library.Controls.CustomInputBox win = new JDB.Library.Controls.CustomInputBox();

            string cust = win.Show(this, "Enter name of customer:", "New Customer", "");

            if (!string.IsNullOrEmpty(cust) && win.WasOkClicked)
            {
                if (this._DB.Customer.Count(o => o.Name == cust) > 0)
                {
                    MessageBox.Show("A customer with that name already exists. Please specify a different name.");
                    return;
                }

                _DB.Customer.InsertOnSubmit(new Customer
                {
                    Name = cust
                });
                _DB.SubmitChanges();

                this.RefreshCustomerList();

                string receiptFolder = Path.Combine(DexterPricingApp.Properties.Settings.Default.StorageLocation, "Receipts", cust);
                Directory.CreateDirectory(receiptFolder);

                if (this.Customers.Count > 0)
                {
                    long maxID = this.Customers.Max(o => o.CustomerID);
                    this.SelectedCustomer = this.Customers.Where(o => o.CustomerID == maxID).FirstOrDefault();
                }
            }
        }

        private void RefreshCustomerList()
        {
            this.Customers = new ObservableCollection<Customer>(this._DB.Customer.OrderBy(o => o.Name).ToList());
        }

        private void RefreshPartList()
        {
            this.Parts = new ObservableCollection<Part>(this._DB.Part.OrderBy(o => o.Code).ToList());
        }

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedPartCharge.Quantity++;
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedPartCharge.Quantity > 1) { this.SelectedPartCharge.Quantity--; }
        }

        private void AddChargeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedCustomer == null)
            {
                MessageBox.Show("No customer selected. Please select one before adding charge information.");
                return;
            }

            bool addingPartCharge = this.PartTab.IsSelected;
            int? chargeKey;
            if (addingPartCharge)
            {
                if (this.SelectedPart == null)
                {
                    MessageBox.Show("No part selected. Please select one before adding charge information.");
                    return;
                }

                PartCharge updatePart = this._DB.PartCharge.Where(o => o.CustomerID == this.SelectedCustomer.CustomerID
                    && o.Printed == null
                    && o.Code == this.SelectedPart.Code
                    && o.Price == this.SelectedPart.Price
                    && o.IsTaxable == this.SelectedPartCharge.IsTaxable).FirstOrDefault();

                if (updatePart != null)
                {
                    chargeKey = updatePart.PartChargeKey;

                    updatePart.Quantity += this.SelectedPartCharge.Quantity;
                    this._DB.SubmitChanges();
                }
                else
                {
                    this.SelectedPartCharge.CustomerID = this.SelectedCustomer.CustomerID;
                    this.SelectedPartCharge.Code = this.SelectedPart.Code;
                    this.SelectedPartCharge.Description = this.SelectedPart.Description;
                    this.SelectedPartCharge.Price = this.SelectedPart.Price;
                    this.SelectedPartCharge.Printed = null;

                    this._DB.PartCharge.InsertOnSubmit(this.SelectedPartCharge);
                    this._DB.SubmitChanges();

                    chargeKey = this.SelectedPartCharge.PartChargeKey;
                }
            }
            else
            {
                this.SelectedMiscCharge.CustomerID = this.SelectedCustomer.CustomerID;
                this.SelectedMiscCharge.Printed = null;

                this._DB.MiscCharge.InsertOnSubmit(this.SelectedMiscCharge);
                this._DB.SubmitChanges();

                chargeKey = this.SelectedMiscCharge.MiscChargeKey;
            }

            this.SelectedCustomer_Changed(null, null);

            this.ChargesGrid.SelectedItem = this.Charges.Where(o =>
                (addingPartCharge ? o.Type == ChargeType.Part : o.Type == ChargeType.Misc)
                    && o.Key == chargeKey).FirstOrDefault();
            this.ChargesGrid.Focus();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.PartTab.IsEnabled)
            {
                PartCharge updatedPart = this._DB.PartCharge.Where(o => o.PartChargeKey == this.SelectedPartCharge.PartChargeKey).FirstOrDefault();
                Charge.Clone(this.SelectedPartCharge, updatedPart);

                this._DB.SubmitChanges();

                this.ChargesGrid.SelectedItem = this.Charges.Where(o => o.Type == ChargeType.Part && o.Key == this._OldChargeKey).FirstOrDefault();
            }
            else
            {
                MiscCharge updatedMisc = this._DB.MiscCharge.Where(o => o.MiscChargeKey == this.SelectedMiscCharge.MiscChargeKey).FirstOrDefault();
                Charge.Clone(this.SelectedMiscCharge, updatedMisc);

                this._DB.SubmitChanges();

                this.ChargesGrid.SelectedItem = this.Charges.Where(o => o.Type == ChargeType.Misc && o.Key == this._OldChargeKey).FirstOrDefault();
            }

            this.EditMode = false;
            this.ChargesGrid.Focus();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.PartTab.IsEnabled)
            {
                this.SelectedPartCharge = this._DB.PartCharge.Where(o => o.PartChargeKey == this._OldChargeKey).FirstOrDefault();

                this.ChargesGrid.SelectedItem = this.Charges.Where(o => o.Type == ChargeType.Part && o.Key == this._OldChargeKey).FirstOrDefault();
            }
            else
            {
                this.SelectedMiscCharge = this._DB.MiscCharge.Where(o => o.MiscChargeKey == this._OldChargeKey).FirstOrDefault();

                this.ChargesGrid.SelectedItem = this.Charges.Where(o => o.Type == ChargeType.Misc && o.Key == this._OldChargeKey).FirstOrDefault();
            }

            this.EditMode = false;
            this.ChargesGrid.Focus();
        }

        private void ImportPartsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Word Documents (*.doc;*.docx)|*.doc;*.docx";

            if (ofd.ShowDialog() == true)
            {
                this.ImportPartList(ofd.FileName);
                this.RefreshPartList();
            }
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedCustomer == null)
            {
                MessageBox.Show("No customer selected. Please select one before printing receipt.");
                return;
            }

            if (this.Charges.Count(o => o.Include) == 0)
            {
                MessageBox.Show("No charges have been selected to print. Select charges using the 'Include' column to print");
                return;
            }

            string receipt = DexterPricingApp.Properties.Resources.Receipt;

            receipt = receipt.Replace("{Customer}", this.SelectedCustomer.Name);
            receipt = receipt.Replace("{Date}", DateTime.Now.ToString("M/d/yyyy (dddd)"));
            receipt = receipt.Replace("{Total}", Charge.GetTotalReceiptLine(this.Charges.ToList()));

            string receiptInfoLines = "";
            bool hasMiscCharges;

            if (this.Charges.Where(o => o.Include && !o.IsTaxable).Count() > 0)
            {
                receiptInfoLines += "NON-TAXED CHARGES" + System.Environment.NewLine + System.Environment.NewLine;
                hasMiscCharges = (this.Charges.Where(o => o.Include && !o.IsTaxable).Count(o => o.Type == ChargeType.Misc) > 0);

                if (this.Charges.Where(o => o.Include && !o.IsTaxable).Count(o => o.Type == ChargeType.Part) > 0)
                {
                    receiptInfoLines += "Parts" + System.Environment.NewLine + System.Environment.NewLine;

                    List<int> partChargeKeys = this.Charges.Where(o => o.Type == ChargeType.Part && o.Include && !o.IsTaxable).Select(o => o.Key).ToList();
                    List<PartCharge> customerPartCharges = this._DB.PartCharge.Where(o => o.CustomerID == this.SelectedCustomer.CustomerID && !o.IsTaxable).ToList();
                    List<PartCharge> printPartCharges = customerPartCharges.Where(o => (partChargeKeys.IndexOf(o.PartChargeKey) != -1)).ToList();

                    receiptInfoLines += Charge.GetPartChargeLines(printPartCharges);
                    if (hasMiscCharges) { receiptInfoLines += System.Environment.NewLine + System.Environment.NewLine; }

                    foreach (PartCharge partCharge in printPartCharges)
                    {
                        partCharge.Printed = DateTime.Now;
                    }
                    this._DB.SubmitChanges();
                }

                if (hasMiscCharges)
                {
                    receiptInfoLines += "Miscellaneous" + System.Environment.NewLine + System.Environment.NewLine;

                    List<int> miscChargeKeys = this.Charges.Where(o => o.Type == ChargeType.Misc && o.Include && !o.IsTaxable).Select(o => o.Key).ToList();
                    List<MiscCharge> customerMiscCharges = this._DB.MiscCharge.Where(o => o.CustomerID == this.SelectedCustomer.CustomerID && !o.IsTaxable).ToList();
                    List<MiscCharge> printMiscCharges = customerMiscCharges.Where(o => (miscChargeKeys.IndexOf(o.MiscChargeKey) != -1)).ToList();

                    receiptInfoLines += Charge.GetMiscLines(printMiscCharges);

                    foreach (MiscCharge miscCharge in customerMiscCharges)
                    {
                        miscCharge.Printed = DateTime.Now;
                    }
                    this._DB.SubmitChanges();
                }

                receiptInfoLines += System.Environment.NewLine + System.Environment.NewLine;
            }

            if (this.Charges.Where(o => o.Include && o.IsTaxable).Count() > 0)
            {
                receiptInfoLines += "TAXED CHARGES" + System.Environment.NewLine + System.Environment.NewLine;
                hasMiscCharges = (this.Charges.Where(o => o.Include && o.IsTaxable).Count(o => o.Type == ChargeType.Misc) > 0);

                if (this.Charges.Where(o => o.Include && o.IsTaxable).Count(o => o.Type == ChargeType.Part) > 0)
                {
                    receiptInfoLines += "Parts" + System.Environment.NewLine + System.Environment.NewLine;

                    List<int> partChargeKeys = this.Charges.Where(o => o.Type == ChargeType.Part && o.Include && o.IsTaxable).Select(o => o.Key).ToList();
                    List<PartCharge> customerPartCharges = this._DB.PartCharge.Where(o => o.CustomerID == this.SelectedCustomer.CustomerID && o.IsTaxable).ToList();
                    List<PartCharge> printPartCharges = customerPartCharges.Where(o => (partChargeKeys.IndexOf(o.PartChargeKey) != -1)).ToList();

                    receiptInfoLines += Charge.GetPartChargeLines(printPartCharges);
                    if (hasMiscCharges) { receiptInfoLines += System.Environment.NewLine + System.Environment.NewLine; }

                    foreach (PartCharge partCharge in printPartCharges)
                    {
                        partCharge.Printed = DateTime.Now;
                    }
                    this._DB.SubmitChanges();
                }

                if (hasMiscCharges)
                {
                    receiptInfoLines += "Miscellaneous" + System.Environment.NewLine + System.Environment.NewLine;

                    List<int> miscChargeKeys = this.Charges.Where(o => o.Type == ChargeType.Misc && o.Include && o.IsTaxable).Select(o => o.Key).ToList();
                    List<MiscCharge> customerMiscCharges = this._DB.MiscCharge.Where(o => o.CustomerID == this.SelectedCustomer.CustomerID && o.IsTaxable).ToList();
                    List<MiscCharge> printMiscCharges = customerMiscCharges.Where(o => (miscChargeKeys.IndexOf(o.MiscChargeKey) != -1)).ToList();

                    receiptInfoLines += Charge.GetMiscLines(printMiscCharges);

                    foreach (MiscCharge miscCharge in printMiscCharges)
                    {
                        miscCharge.Printed = DateTime.Now;
                    }
                    this._DB.SubmitChanges();
                }
            }

            receipt = receipt.Replace("{Receipt Info}", receiptInfoLines.TrimEnd('\r', '\n'));

            string receiptFolder = Path.Combine(DexterPricingApp.Properties.Settings.Default.StorageLocation, "Receipts", this.SelectedCustomer.Name);
            if (!Directory.Exists(receiptFolder)) { Directory.CreateDirectory(receiptFolder); }
            string receiptLocation = Path.Combine(receiptFolder, string.Format(@"{0}_{1}.txt", this.SelectedCustomer.Name, DateTime.Now.ToString("M-d-yyyy_HHmmss")));

            using (StreamWriter file = new StreamWriter(receiptLocation))
            {
                file.Write(receipt);
                file.Close();
            }
            
            Process.Start(receiptLocation);

            this.SelectedCustomer_Changed(this, null);
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

        private void ChargesGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!this.EditMode)
            {
                this.ChargesGrid.SelectedItem = null;
                this.SelectedPartCharge = new PartCharge() { IsTaxable = false, Quantity = 1 };
                this.SelectedMiscCharge = new MiscCharge() { Date = DateTime.Today, IsTaxable = false };
            }
        }

        private void ChangeNameCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedCustomer == null) { return; }

            JDB.Library.Controls.CustomInputBox win = new JDB.Library.Controls.CustomInputBox();

            string cust = win.Show(this, "Enter name of customer:", "Rename Customer", this.SelectedCustomer.Name);

            if (win.WasOkClicked && !string.IsNullOrEmpty(cust) && !cust.Equals(this.SelectedCustomer.Name))
            {
                Directory.Move
                (
                    Path.Combine(DexterPricingApp.Properties.Settings.Default.StorageLocation, "Receipts", this.SelectedCustomer.Name),
                    Path.Combine(DexterPricingApp.Properties.Settings.Default.StorageLocation, "Receipts", cust)
                );

                Customer customer = this._DB.Customer.Where(o => o.CustomerID == this.SelectedCustomer.CustomerID).FirstOrDefault();
                customer.Name = cust;
                this._DB.SubmitChanges();

                this.RefreshCustomerList();

                this.SelectedCustomer = this.Customers.Where(o => o.Name == cust).FirstOrDefault();
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedCustomer == null) { return; }

            Customer customer = this.SelectedCustomer;

            if (MessageBox.Show("Are you sure you want to delete this customer? This action cannot be undone.", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Customer cust = this._DB.Customer.Where(o => o.CustomerID == customer.CustomerID).First();
                this._DB.Customer.DeleteOnSubmit(cust);
                this._DB.SubmitChanges();

                this.RefreshCustomerList();
            }
        }

        private void AllButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Charges == null) { return; }

            foreach (Charge c in this.Charges)
            {
                c.Include = true;
            }
        }

        private void NoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Charges == null) { return; }

            foreach (Charge c in this.Charges)
            {
                c.Include = false;
            }
        }
        
        private void BetweenButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Charges == null) { return; }

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

            foreach (Charge c in this.Charges)
            {
                if (c.Date >= afterDate && c.Date <= beforeDate)
                {
                    c.Include = true;
                }
                else
                {
                    c.Include = false;
                }
            }
        }

        private void TotalsButton_Click(object sender, RoutedEventArgs e)
        {
            CalculateTotalsWindow win = new CalculateTotalsWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        public List<Charge> GetTaxableCharges(DateTime afterDate, DateTime beforeDate)
        {
            List<Charge> charges = new List<Charge>();

            foreach (PartCharge part in this._DB.PartCharge.Where(o =>
                o.Printed != null
                && o.Printed >= afterDate
                && o.Printed <= beforeDate
                && o.IsTaxable))
            {
                charges.Add(new Charge(part));
            }

            foreach (MiscCharge part in this._DB.MiscCharge.Where(o =>
                o.Printed != null
                && o.Printed >= afterDate
                && o.Printed <= beforeDate
                && o.IsTaxable))
            {
                charges.Add(new Charge(part));
            }

            return charges;
        }

        public List<Charge> GetTotalCharges(DateTime afterDate, DateTime beforeDate)
        {
            List<Charge> charges = new List<Charge>();

            foreach (PartCharge part in this._DB.PartCharge.Where(o =>
                o.Printed != null
                && o.Printed >= afterDate
                && o.Printed <= beforeDate))
            {
                charges.Add(new Charge(part));
            }

            foreach (MiscCharge part in this._DB.MiscCharge.Where(o =>
                o.Printed != null
                && o.Printed >= afterDate
                && o.Printed <= beforeDate))
            {
                charges.Add(new Charge(part));
            }

            return charges;
        }

        private void AddCustomPartButton_Click(object sender, RoutedEventArgs e)
        {
            PartWindow win = new PartWindow("Add Custom Part", "New Part", true);
            win.Owner = this;
            if (win.ShowDialog() ?? false)
            {
                this._DB.Part.InsertOnSubmit(win.EditPart);
                this._DB.SubmitChanges();

                this.RefreshPartList();
            }
        }

        private void CustomerSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchString = ((TextBox)e.Source).Text.Trim();
            Customer selectedCustomer = this.Customers.Where(o => o.Name.StartsWith(searchString, true, null)).FirstOrDefault();

            if (selectedCustomer != null)
            { 
                this.CustomersGrid.SelectedItem = selectedCustomer;
                this.CustomersGrid.ScrollIntoView(selectedCustomer);    // Listboxes must manually be told to scroll to selected item
            }
        }

        private void PartSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchString = ((TextBox)e.Source).Text.Trim();
            Part selectedPart = this.Parts.Where(o => o.Code.StartsWith(searchString, true, null)).FirstOrDefault();

            if (selectedPart != null) { this.PartsGrid.SelectedItem = selectedPart; }
        }

        private void ImportPartList(string partListPath)
        {
            this._DB.Part.DeleteAllOnSubmit(this._DB.Part.Where(o => !o.IsCustom));
            this._DB.SubmitChanges();

            FilterReader reader = new FilterReader(partListPath);
            string text = reader.ReadToEnd();
            string[] parts = text.Split('\t');

            string failedParts = "";
            for (int i = 0; i < parts.Count() / 4; i++)
            {
                try
                {
                    Part part = new Part
                    {
                        IsCustom = false,
                        Code = parts[4 * i],
                        Description = parts[4 * i + 1],
                        Price = Math.Round(Double.Parse(parts[4 * i + 2]), 2)
                    };
                    this._DB.Part.InsertOnSubmit(part);
                    this._DB.SubmitChanges();
                }
                catch (Exception exc)
                {
                    failedParts += parts[4 * i] + "\t" + parts[4 * i + 1] + "\t" + parts[4 * i + 2] + System.Environment.NewLine;
                }
            }

            if (!string.IsNullOrEmpty(failedParts))
            {
                MessageBox.Show("The following parts were unable to be added:" + System.Environment.NewLine
                                    + failedParts + System.Environment.NewLine
                                    + "Please fix the input file and re-import, or add these parts manually.");
            }
        }
    }
}