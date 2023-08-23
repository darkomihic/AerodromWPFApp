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
    /// Interaction logic for frmPasos.xaml
    /// </summary>
    public partial class frmPasos : Window
    {

        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;

        public frmPasos()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtImePasos.Focus();
        }

        public frmPasos(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtImePasos.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;

        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                { 
                    Connection = konekcija
                };
                cmd.Parameters.Add("@Ime", SqlDbType.NVarChar).Value = txtImePasos.Text;
                cmd.Parameters.Add("@Prezime", SqlDbType.NVarChar).Value = txtPrezimePasos.Text;
                cmd.Parameters.Add("@Drzava", SqlDbType.NVarChar).Value = txtDrzavaPasos.Text;
                cmd.Parameters.Add("@Godine", SqlDbType.Int).Value = txtGodinePasos.Text;
                cmd.Parameters.Add("@Viza", SqlDbType.Bit).Value = Convert.ToBoolean(cbxVizaPasos.IsChecked);
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tblPasos set Ime=@Ime, Prezime=@Prezime, Drzava=@Drzava, Godine=@Godine, Viza=@Viza where PasosID=@id";
                    this.pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"Insert into tblPasos(Ime, Prezime, Viza, Drzava, Godine) values(@Ime, @Prezime, @Viza, @Drzava, @Godine)";

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

