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
    /// Interaction logic for frmPrtljag.xaml
    /// </summary>
    
    public partial class frmPrtljag : Window
    {

        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public frmPrtljag()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtTezinaPrtljag.Focus();
        }
        public frmPrtljag(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtTezinaPrtljag.Focus();
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
                cmd.Parameters.Add("@Tezina", SqlDbType.NVarChar).Value = txtTezinaPrtljag.Text;
                cmd.Parameters.Add("@SifraPrtljag", SqlDbType.NVarChar).Value = txtSifraPrtljaga.Text;
                cmd.Parameters.Add("@PredjenaKontrola", SqlDbType.Bit).Value = Convert.ToInt32(cbxPredjenaKontrolaPrtljag.IsChecked);
                cmd.Parameters.Add("@RucniPrtljag", SqlDbType.Bit).Value = Convert.ToInt32(cbxRucniPrtljag.IsChecked);

                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tblPrtljag set Tezina=@Tezina, SifraPrtljag=@SifraPrtljag, PredjenaKontrola=@PredjenaKontrola, RucniPrtljag=@RucniPrtljag where PrtljagID=@id";
                    this.pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"Insert into tblPrtljag(Tezina, PredjenaKontrola, RucniPrtljag, SifraPrtljag)
                                        values(@Tezina, @PredjenaKontrola, @RucniPrtljag, @SifraPrtljag)";

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
