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
    /// Interaction logic for frmLet.xaml
    /// </summary>
    public partial class frmLet : Window
    {


        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;


        public frmLet()
        {
            InitializeComponent();
            txtNazivLinije.Focus();
            PopuniPadajuceListe();
        }

        public frmLet(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtNazivLinije.Focus();
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

                string vratiAvion = @"Select AvionID, BrojLeta from tblAvion";
                SqlDataAdapter daAvion = new SqlDataAdapter(vratiAvion, konekcija);
                DataTable dtAvion = new DataTable();
                daAvion.Fill(dtAvion);
                cbAvion.ItemsSource = dtAvion.DefaultView;
                dtAvion.Dispose();
                daAvion.Dispose();

                string vratiPoletanje = @"Select PoletanjeID, tblPoletanje.Grad + ' ' + tblPoletanje.Vreme as Poletanje from tblPoletanje";
                SqlDataAdapter daPoletanje = new SqlDataAdapter(vratiPoletanje, konekcija);
                DataTable dtPoletanje = new DataTable();
                daPoletanje.Fill(dtPoletanje);
                cbPoletanje.ItemsSource = dtPoletanje.DefaultView;
                dtPoletanje.Dispose();
                daPoletanje.Dispose();

                string vratiSletanje = @"Select SletanjeID, tblSletanje.Grad + ' ' + tblSletanje.Vreme as Sletanje from tblSletanje";
                SqlDataAdapter daSletanje = new SqlDataAdapter(vratiSletanje, konekcija);
                DataTable dtSletanje = new DataTable();
                daSletanje.Fill(dtSletanje);
                cbSletanje.ItemsSource = dtSletanje.DefaultView;
                dtSletanje.Dispose();
                daSletanje.Dispose();


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

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@NazivLinije", SqlDbType.NVarChar).Value = txtNazivLinije.Text;
                cmd.Parameters.Add("@SifraLeta", SqlDbType.NVarChar).Value = txtSifraLeta.Text;
                cmd.Parameters.Add("@AvionID", SqlDbType.Int).Value = cbAvion.SelectedValue;
                cmd.Parameters.Add("@PoletanjeID", SqlDbType.Int).Value = cbPoletanje.SelectedValue;
                cmd.Parameters.Add("@SletanjeID", SqlDbType.Int).Value = cbSletanje.SelectedValue;

                if(this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tblLet set NazivLinije=@NazivLinije, SifraLeta=@SifraLeta, AvionID=@AvionID, PoletanjeID=@PoletanjeID, SletanjeID=@SletanjeID where LetID=@id";
                    this.pomocniRed = null;               
                }
                else
                {
                    cmd.CommandText = @"Insert into tblLet(NazivLinije, SifraLeta, AvionID, PoletanjeID, SletanjeID) values(@NazivLinije, @SifraLeta, @AvionID, @PoletanjeID, @SletanjeID)";
                }
                    

                
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (Exception)
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
