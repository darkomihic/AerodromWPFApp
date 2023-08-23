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
    /// Interaction logic for frmPutnik.xaml
    /// </summary>
    public partial class frmPutnik : Window
    {

        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;

        public frmPutnik()
        {
            InitializeComponent();
            txtImePutnik.Focus();
            PopuniPadajuceListe();
        }

        public frmPutnik(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtImePutnik.Focus();
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

                string vratiPrtljag = @"Select PrtljagID, SifraPrtljag from tblPrtljag";
                SqlDataAdapter daPrt = new SqlDataAdapter(vratiPrtljag, konekcija);
                DataTable dtPrt = new DataTable();
                daPrt.Fill(dtPrt);
                cbPrtljag.ItemsSource = dtPrt.DefaultView;
                dtPrt.Dispose();
                daPrt.Dispose();

                string vratiPasos = @"Select PasosID, tblPasos.Ime + ' ' + tblPasos.Prezime + ' iz ' + tblPasos.Drzava as Pasos1 from tblPasos";
                SqlDataAdapter daP = new SqlDataAdapter(vratiPasos, konekcija);
                DataTable dtP = new DataTable();
                daP.Fill(dtP);
                cbPasos.ItemsSource = dtP.DefaultView;
                dtP.Dispose();
                daP.Dispose();

                string vratiKar = @"Select KartaID, SifraKarte from tblKarta";
                SqlDataAdapter daK = new SqlDataAdapter(vratiKar, konekcija);
                DataTable dtK = new DataTable();
                daK.Fill(dtK);
                cbKarta.ItemsSource = dtK.DefaultView;
                dtK.Dispose();
                daK.Dispose();
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
                
                cmd.Parameters.Add("@Ime", SqlDbType.NVarChar).Value = txtImePutnik.Text;
                cmd.Parameters.Add("@Prezime", SqlDbType.NVarChar).Value = txtPrezimePutnik.Text;
                cmd.Parameters.Add("@BrojTel", SqlDbType.NVarChar).Value = txtBrojTelPutnik.Text;
                cmd.Parameters.Add("@EMail", SqlDbType.NVarChar).Value = txtEMailPutnik.Text;
                cmd.Parameters.Add("@Godine", SqlDbType.NVarChar).Value = txtGodinePutnik.Text;
                cmd.Parameters.Add("@JMBG", SqlDbType.NVarChar).Value = txtJMBGPutnik.Text;
                cmd.Parameters.Add("@PrtljagID", SqlDbType.Int).Value = cbPrtljag.SelectedValue;
                cmd.Parameters.Add("@KartaID", SqlDbType.Int).Value = cbKarta.SelectedValue;
                cmd.Parameters.Add("@PasosID", SqlDbType.Int).Value = cbPasos.SelectedValue;

                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tblPutnik set Ime=@Ime, Prezime=@Prezime, BrojTel=@BrojTel, EMail=@EMail, Godine=@Godine, JMBG=@JMBG, PrtljagID=@PrtljagID, KartaID=KartaID, PasosID=PasosID where PutnikID=@id";
                    this.pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"Insert into tblPutnik(Ime, Prezime, BrojTel, EMail, Godine, JMBG, PrtljagID, KartaID, PasosID)
                                    values(@Ime, @Prezime, @BrojTel, @EMail, @Godine, @JMBG, @PrtljagID, @KartaID, @PasosID)";
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
