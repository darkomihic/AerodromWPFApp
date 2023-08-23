using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Aerodrom_Darko_Mihic_IT69_2020.Forme
{
    /// <summary>
    /// Interaction logic for frmPoletanje.xaml
    /// </summary>
    public partial class frmPoletanje : Window
    {

        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;

        public frmPoletanje()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtGradPoletanje.Focus();
        }

        public frmPoletanje(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtGradPoletanje.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
        }

        private void btnSačuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@Grad", SqlDbType.NVarChar).Value = txtGradPoletanje.Text;
                cmd.Parameters.Add("@Vreme", SqlDbType.NVarChar).Value = txtVremePoletanje.Text;
                cmd.Parameters.Add("@Gate", SqlDbType.NVarChar).Value = txtGatePoletanje.Text;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tblPoletanje set Grad=@Grad, Vreme=@Vreme, Gate=@Gate where PoletanjeID=@id";
                    this.pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"Insert into tblPoletanje(Grad, Vreme, Gate)
                                        values(@Grad, @Vreme, @Gate)";
                }


                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();

            }
            catch (SqlException)
            {
                MessageBox.Show("Unos nije validan", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
