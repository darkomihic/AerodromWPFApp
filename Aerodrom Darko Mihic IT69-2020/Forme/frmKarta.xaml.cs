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
    /// Interaction logic for frmKarta.xaml
    /// </summary>
    public partial class frmKarta : Window
    {

        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;

        public frmKarta()
        {
            InitializeComponent();
            txtBrojSedista.Focus();
            PopuniPadajuceListe();
        }

        public frmKarta(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtBrojSedista.Focus();
            PopuniPadajuceListe();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
        }

        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija = kon.KreirajKonekciju();
                konekcija.Open();

                string vratiLet = @"Select LetID, SifraLeta from tblLet";
                SqlDataAdapter daLet = new SqlDataAdapter(vratiLet, konekcija);
                DataTable dtLet = new DataTable();
                daLet.Fill(dtLet);
                cbLet.ItemsSource = dtLet.DefaultView;
                dtLet.Dispose();
                daLet.Dispose();

                


            }
            catch (SqlException)
            {
                MessageBox.Show("Liste nisu popunjene", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
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
                cmd.Parameters.Add("@CenaKarte", SqlDbType.Int).Value = txtCenaKarte.Text;
                cmd.Parameters.Add("@BrojKredKartice", SqlDbType.NVarChar).Value = txtBrojKreditne.Text;
                cmd.Parameters.Add("@Klasa", SqlDbType.NVarChar).Value = txtKlasaKarta.Text; 
                cmd.Parameters.Add("@Sediste", SqlDbType.Int).Value = txtBrojSedista.Text;
                cmd.Parameters.Add("@SifraKarte", SqlDbType.NVarChar).Value = txtSifraKarte.Text;
                cmd.Parameters.Add("@LetID", SqlDbType.Int).Value = cbLet.SelectedValue;
                   
               

                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tblKarta
                                                    set CenaKarte=@CenaKarte, BrojKredKartice=@BrojKredKartice, Klasa=@Klasa, Sediste=@Sediste, SifraKarte=@SifraKarte, LetID=@LetID where KartaID=@id";
                    this.pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"Insert into tblKarta(CenaKarte, BrojKredKartice, Klasa, Sediste, SifraKarte, LetID) values(@CenaKarte, @BrojKredKartice, @Klasa, @Sediste, @SifraKarte, @LetID)";
                }
                
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch(SqlException)
            {
                MessageBox.Show("Unos nije validan", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if(konekcija != null)
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
