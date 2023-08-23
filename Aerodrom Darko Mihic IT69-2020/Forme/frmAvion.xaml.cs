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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace Aerodrom_Darko_Mihic_IT69_2020.Forme
{
    /// <summary>
    /// Interaction logic for frmAvion.xaml
    /// </summary>
    public partial class frmAvion : Window
    {

        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;


        public frmAvion()
        {
            InitializeComponent();
            txtModel.Focus();
            konekcija = kon.KreirajKonekciju();

        }

        public frmAvion(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtModel.Focus();
            konekcija = kon.KreirajKonekciju();
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
                cmd.Parameters.Add("@Model", SqlDbType.NVarChar).Value = txtModel.Text;
                cmd.Parameters.Add("@BrojLeta", SqlDbType.NVarChar).Value = txtBrojLeta.Text;
                cmd.Parameters.Add("@PutniciEconomy", SqlDbType.Int).Value = txtBrojEconomy.Text;
                cmd.Parameters.Add("@PutniciBusiness", SqlDbType.Int).Value = txtBrojBusiness.Text;
                cmd.Parameters.Add("@BrojClanovaPosade", SqlDbType.Int).Value = txtBrojPosade.Text;
                cmd.Parameters.Add("@Vlasnik", SqlDbType.NVarChar).Value = txtVlasnik.Text;
                if(this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tblAvion
                                                set Model=@Model, BrojLeta=@BrojLeta, PutniciEconomy=@PutniciEconomy, PutniciBusiness=@PutniciBusiness, BrojClanovaPosade=@BrojClanovaPosade, Vlasnik=@Vlasnik where AvionID=@id";
                    this.pomocniRed = null;                                          
                }
                else
                {
                    cmd.CommandText = @"Insert into tblAvion(Model, BrojLeta, PutniciEconomy, PutniciBusiness, BrojClanovaPosade, Vlasnik) values(@Model, @BrojLeta, @PutniciEconomy, @PutniciBusiness, @BrojClanovaPosade, @Vlasnik)";
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
                if(konekcija != null )
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
