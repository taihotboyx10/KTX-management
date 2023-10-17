using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKTX.Util
{
    class Validate
    {
        public bool PhoneValid(string phone)
        {
            string pattern = @"^(?:\+?(\d{1,3}))?[-.\s]?(\d{3})[-.\s]?(\d{3})[-.\s]?(\d{4})$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(phone);
        }

        public bool EmailValid(string mail)
        {
            string pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)*(\.[a-z]{2,4})$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(mail);
        }

        public bool CheckNullInTextBox(ErrorProvider errorProvider, Guna2TextBox textBox, string text, string message )
        {
            if (string.IsNullOrEmpty(text))
            {
                errorProvider.SetError(textBox, message);
                textBox.Focus();
                return false;
            }
            else
            {
                errorProvider.Clear();
            }
            return true;
        }

        public bool CheckNullInComboBox(ErrorProvider errorProvider, Guna2ComboBox comboBox, string text, string message)
        {
            if (comboBox.SelectedIndex == -1)//SelectedItem == null
            {
                errorProvider.SetError(comboBox, message);
                return false;
            }
            else
            {
                errorProvider.Clear();
            }
            return true;
        }

    }
}
