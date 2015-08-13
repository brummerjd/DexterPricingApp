using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace DexterPricingApp
{
    public class Charge : INotifyPropertyChanged
    {
        private const int RECEIPT_WIDTH = 70;
        public const double TAX_PERCENT = 0.055;

        public int Key;
        public ChargeType Type;
        public bool IsTaxable;
        public DateTime? Printed;

        public bool Include
        {
            get { return this._Include; }
            set { this._Include = value; OnPropertyChanged("Include"); }
        }
        private bool _Include;

        public DateTime? Date
        {
            get { return this._Date; }
            set { this._Date = value; OnPropertyChanged("Date"); }
        }
        private DateTime? _Date;

        public String Description
        {
            get { return this._Description; }
            set { this._Description = value; OnPropertyChanged("Description"); }
        }
        private String _Description;
        
        public double Total
        {
            get { return this._Total; }
            set { this._Total = value; OnPropertyChanged("Total"); }
        }
        private double _Total;

        public Charge(Charge oldCharge)
        {
            this.Key = oldCharge.Key;
            this.Type = oldCharge.Type;
            this.IsTaxable = oldCharge.IsTaxable;
            this.Printed = oldCharge.Printed;
            this.Include = oldCharge.Include;
            this.Date = oldCharge.Date;
            this.Description = oldCharge.Description;
            this.Total = oldCharge.Total;
        }

        public Charge(PartCharge part)
        {
            this.Key = part.PartChargeKey;
            this.Type = ChargeType.Part;
            this.IsTaxable = part.IsTaxable;
            this.Printed = part.Printed;
            this.Include = part.Printed == null;
            this.Date = null;
            this.Description = part.Code + " x " + part.Quantity + " - " + part.Description;
            this.Total = part.Price * part.Quantity;
        }

        public Charge(MiscCharge misc)
        {
            this.Key = misc.MiscChargeKey;
            this.Type = ChargeType.Misc;
            this.IsTaxable = misc.IsTaxable;
            this.Printed = misc.Printed;
            this.Include = misc.Printed == null;
            this.Date = misc.Date;
            this.Description = misc.Description;
            this.Total = misc.Amount;
        }

        public static PartCharge Clone(PartCharge part)
        {
            PartCharge newPart = new PartCharge();

            return Clone(part, newPart);
        }

        public static PartCharge Clone(PartCharge oldPart, PartCharge newPart)
        {
            newPart.PartChargeKey = oldPart.PartChargeKey;
            newPart.CustomerID = oldPart.CustomerID;
            newPart.Code = oldPart.Code;
            newPart.Description = oldPart.Description;
            newPart.Quantity = oldPart.Quantity;
            newPart.Price = oldPart.Price;
            newPart.IsTaxable = oldPart.IsTaxable;
            newPart.Printed = oldPart.Printed;

            return newPart;
        }

        public static MiscCharge Clone(MiscCharge misc)
        {
            MiscCharge newMisc = new MiscCharge();

            return Clone(misc, newMisc);
        }

        public static MiscCharge Clone(MiscCharge oldMisc, MiscCharge newMisc)
        {
            newMisc.MiscChargeKey = oldMisc.MiscChargeKey;
            newMisc.CustomerID = oldMisc.CustomerID;
            newMisc.Date = oldMisc.Date;
            newMisc.Amount = oldMisc.Amount;
            newMisc.Description = oldMisc.Description;
            newMisc.IsTaxable = oldMisc.IsTaxable;
            newMisc.Printed = oldMisc.Printed;

            return newMisc;
        }

        public static string GetPartChargeLines(List<PartCharge> partCharges)
        {
            string partChargeLines = "";
            string itemInfo;
            string totalAmountString;
            int itemLength;

            foreach (PartCharge charge in partCharges)
            {
                itemInfo = "   " + charge.Code + " - " + charge.Description;
                totalAmountString = String.Format("{0} x ${1:0.00} = ${2:0.00}", charge.Quantity, charge.Price, charge.Quantity*charge.Price);

                itemLength = itemInfo.Length;

                if (itemLength > 43)
                {
                    itemInfo = itemInfo.Substring(0, 40) + "...";
                    itemLength = 43;
                }

                for (int i = itemLength; i < RECEIPT_WIDTH - totalAmountString.Length; i++) { itemInfo += " "; }
                itemInfo += totalAmountString + System.Environment.NewLine + System.Environment.NewLine;

                partChargeLines += itemInfo;
            }

            return partChargeLines.TrimEnd('\r', '\n');
        }

        public static string GetMiscLines(List<MiscCharge> miscCharges)
        {
            string miscChargeLines = "";
            string miscInfo;
            int itemLength;

            foreach (MiscCharge misc in miscCharges.OrderByDescending(o => o.Date))
            {
                miscInfo = "   " + (misc.Date ?? DateTime.Today).ToString("M/d/yy") + " - " + misc.Description;
                itemLength = miscInfo.Length;

                if (itemLength > 58)
                {
                    miscInfo = miscInfo.Substring(0, 55) + "...";
                    itemLength = 58;
                }

                string totalAmount = String.Format("${0:0.00}", Math.Round(misc.Amount, 2));

                for (int i = itemLength; i < RECEIPT_WIDTH - totalAmount.Length; i++) { miscInfo += " "; }

                miscChargeLines += miscInfo + totalAmount + System.Environment.NewLine + System.Environment.NewLine;
            }

            return miscChargeLines.TrimEnd('\r', '\n');
        }

        public static string GetTotalReceiptLine(List<Charge> charges)
        {
            double subtotal;
            double total = 0;

            string totalLines = "";
            string totalLine;
            string totalAmount;

            if (charges.Count(o => o.Include && !o.IsTaxable) > 0)
            {
                subtotal = charges.Where(o => o.Include && !o.IsTaxable).Sum(o => o.Total);
                total += subtotal;
                totalAmount = String.Format("${0:0.00}", charges.Where(o => o.Include && !o.IsTaxable).Sum(o => o.Total));

                totalLine = "Non-taxable Subtotal:";
                for (int i = 21; i < RECEIPT_WIDTH - totalAmount.Length; i++) { totalLine += " "; }
                totalLine += totalAmount;

                totalLines += totalLine + System.Environment.NewLine;
            }

            if (charges.Count(o => o.Include && o.IsTaxable) > 0)
            {
                subtotal = charges.Where(o => o.Include && o.IsTaxable).Sum(o => o.Total);
                total += subtotal;
                totalAmount = String.Format("${0:0.00}", subtotal);

                totalLine = "Taxable Subtotal:";
                for (int i = 17; i < RECEIPT_WIDTH - totalAmount.Length; i++) { totalLine += " "; }
                totalLine += totalAmount;

                totalLines += totalLine + System.Environment.NewLine;

                subtotal = Math.Round(charges.Where(o => o.Include && o.IsTaxable).Sum(o => o.Total) * TAX_PERCENT, 2);
                total += subtotal;
                totalAmount = String.Format("${0:0.00}", subtotal);

                totalLine = "Tax Amount:";
                for (int i = 11; i < RECEIPT_WIDTH - totalAmount.Length; i++) { totalLine += " "; }
                totalLine += totalAmount;

                totalLines += totalLine + System.Environment.NewLine;
            }

            totalAmount = String.Format("${0:0.00}", total);

            totalLine = "Total:";
            for (int i = 6; i < RECEIPT_WIDTH - totalAmount.Length; i++) { totalLine += " "; }
            totalLine += totalAmount;

            totalLines += System.Environment.NewLine + totalLine;

            return totalLines;
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

    public enum ChargeType
    {
        Part,
        Misc
    }
}
