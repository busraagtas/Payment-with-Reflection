using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReflectionIleOdeme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            var odemeYontemleri = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IOdemeYontemi).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();

            cmbOdemeYontemi.Items.Clear();
            foreach (var yontem in odemeYontemleri)
            {
                cmbOdemeYontemi.Items.Add(yontem.FullName); 
            }

            if (cmbOdemeYontemi.Items.Count > 0)
                cmbOdemeYontemi.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbOdemeYontemi.SelectedItem == null || string.IsNullOrWhiteSpace(txtTutar.Text))
            {
                lblSonuc.Text = "Lütfen ödeme yöntemi ve tutar giriniz.";
                return;
            }

            try
            {
                decimal tutar = decimal.Parse(txtTutar.Text);
                string secilenSinif = cmbOdemeYontemi.SelectedItem.ToString();

                Type type = Type.GetType(secilenSinif);
                if (type != null)
                {
                    IOdemeYontemi odemeYontemi = (IOdemeYontemi)Activator.CreateInstance(type);
                    string sonuc = odemeYontemi.Ode(tutar);
                    lblSonuc.Text = sonuc;
                }
                else
                {
                    lblSonuc.Text = "Ödeme yöntemi oluşturulamadı.";
                }
            }
            catch (Exception ex)
            {
                lblSonuc.Text = "Hata: " + ex.Message;
            }
        }
    }
}
