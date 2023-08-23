using Aerodrom_Darko_Mihic_IT69_2020.Forme;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Configuration;

namespace Aerodrom_Darko_Mihic_IT69_2020
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ucitanaTabela;
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocnired;

        public MainWindow()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            UcitajPodatke(dataGridCentralni, avionSelect);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void BindGrid()
        {
            DataTable dt = new DataTable();
            string queryString = "select * from tbl_EmpDetails";
            string conn = ConfigurationManager.ConnectionStrings["TestConnectionString"].ToString();
            var table = new DataTable();
            using (SqlConnection sql = new SqlConnection(conn))
            {
                SqlCommand command = new SqlCommand(queryString, sql);
                sql.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(table);
                sql.Close();
                da.Dispose();
            }
            dt = table;
            .DataSource = dt;
            GridView1.DataBind();
            Session["Data"] = dt;
        }

        private void UcitajPodatke(DataGrid dataGrid, string selectUpit)
        {
            try
            {
                konekcija.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(selectUpit, konekcija);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                if(dataGrid != null)
                {
                    dataGrid.ItemsSource = dt.DefaultView;
                }
                ucitanaTabela = selectUpit;
                dt.Dispose();
                dataAdapter.Dispose();
            }
            catch(SqlException)
            {
                MessageBox.Show("Neuspešno učitani podaci","Greška",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                if(konekcija!=null)
                {
                    konekcija.Close();
                }
            }
        }

        #region Buttons top
        private void Button_Click(object sender, RoutedEventArgs e)
                {
                    UcitajPodatke(dataGridCentralni, kartaSelect);
                }

                private void Button_Click_1(object sender, RoutedEventArgs e)
                {
                     UcitajPodatke(dataGridCentralni, letSelect);
                }

                private void Button_Click_2(object sender, RoutedEventArgs e)
                {
                     UcitajPodatke(dataGridCentralni, putnikSelect);
                }

                private void Button_Click_3(object sender, RoutedEventArgs e)
                {
                    UcitajPodatke(dataGridCentralni, pasosSelect);
                }

                private void Button_Click_4(object sender, RoutedEventArgs e)
                {
                    UcitajPodatke(dataGridCentralni, poletanjeSelect);
                }

                private void Button_Click_5(object sender, RoutedEventArgs e)
                {
                     UcitajPodatke(dataGridCentralni, sletanjeSelect);
                }

                private void Button_Click_6(object sender, RoutedEventArgs e)
                {
                    UcitajPodatke(dataGridCentralni, prtljagSelect);
                }

                 private void btnAvion_Click(object sender, RoutedEventArgs e)
                 {
                      UcitajPodatke(dataGridCentralni, avionSelect);
                   }
        #endregion

        #region Select upiti
        static string letSelect = @"Select tblLet.LetID as ID, NazivLinije as 'Naziv Linije', SifraLeta as 'Šifra leta', tblPoletanje.Grad as 'Grad polaska', tblSletanje.Grad as 'Grad dolaska' from tblLet 
                                                                  
                                                                   join tblAvion on tblLet.AvionID = tblAvion.AvionID
                                                                     join tblSletanje on tblLet.SletanjeID = tblSletanje.SletanjeID
                                                                        join tblPoletanje on tblLet.PoletanjeID = tblPoletanje.PoletanjeID";
        static string avionSelect = @"Select tblAvion.AvionID as ID, Model as 'Model aviona', BrojLeta as 'Šifra leta',
                                        PutniciEconomy as 'Broj putnika u economy klasi', PutniciBusiness as 'Broj putnika u business klasi',
                                        BrojClanovaPosade as 'Broj članova posade', Vlasnik from tblAvion";
                                                
        static string kartaSelect = @"Select tblKarta.KartaID as ID,CenaKarte as Cena, BrojKredKartice as 'Broj kreditne kartice', Klasa, Sediste, SifraKarte as 'Sifra karte' from tblKarta
                                                            join tblLet on tblKarta.LetID = tblLet.LetID";
        static string pasosSelect = @"Select tblPasos.PasosID as ID, tblPasos.Ime as Ime, tblPasos.Prezime as Prezime, Viza, Drzava, Godine from tblPasos";
        static string poletanjeSelect = @"Select tblPoletanje.PoletanjeID as ID, tblPoletanje.Grad as Grad, tblPoletanje.Vreme as Vreme, tblPoletanje.Gate as Gate from tblPoletanje";
                                                                           
        static string sletanjeSelect = @"Select tblSletanje.SletanjeID as ID, tblSletanje.Grad as Grad, tblSletanje.Vreme as Vreme, tblSletanje.Gate as Gate from tblSletanje";
        static string prtljagSelect = @"Select tblPrtljag.prtljagID as ID, Tezina as 'Težina', PredjenaKontrola as 'Pređena kontrola',
                                                RucniPrtljag as 'Ručni prtljag', SifraPrtljag as 'Sifra prtljaga' from tblPrtljag";
        static string putnikSelect = @"Select tblPutnik.PutnikID as ID, tblPutnik.Ime, tblPutnik.Prezime, BrojTel as 'Broj telefona',EMail as 'E-mail',
                                            tblPutnik.Godine, JMBG from tblPutnik join tblPrtljag on tblPutnik.PrtljagID = tblPrtljag.PrtljagID
                                                                        join tblPasos on tblPutnik.PasosID = tblPasos.PasosID
                                                                       join tblKarta on tblPutnik.KartaID = tblKarta.KartaID";
        #endregion

        #region Select sa uslovom

        string selectUslovLet = @"Select * from tblLet where LetID=";
        string selectUslovAvion = @"Select * from tblAvion where AvionID=";
        string selectUslovKarta = @"Select * from tblKarta where KartaID=";
        string selectUslovPasos = @"Select * from tblPasos where PasosID=";
        string selectUslovPoletanje = @"Select * from tblPoletanje where PoletanjeID=";
        string selectUslovSletanje = @"Select * from tblSletanje where SletanjeID=";
        string selectUslovPrtljag = @"Select * from tblPrtljag where PrtljagID=";
        string selectUslovPutnik = @"Select * from tblPutnik where PutnikID=";

        #endregion

        #region Delete
        string letDelete = @"Delete from tblLet where LetID=";
        string avionDelete = @"Delete from tblAvion where AvionID=";
        string kartaDelete = @"Delete from tblKarta where KartaID=";
        string pasosDelete= @"Delete from tblPasos where PasosID=";
        string poletanjeDelete = @"Delete from tblPoletanje where PoletanjeID=";
        string sletanjeDelete = @"Delete from tblSletanje where SletanjeID=";
        string prtljagDelete = @"Delete from tblPrtljag where PrtljagID=";
        string putnikDelete = @"Delete from tblPutnik where PutnikID=";

        #endregion

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window w1;

            if (ucitanaTabela.Equals(letSelect, StringComparison.Ordinal))
            {
                w1 = new frmLet();
                w1.ShowDialog();
                UcitajPodatke(dataGridCentralni, letSelect);
            }
            else if(ucitanaTabela.Equals(kartaSelect, StringComparison.Ordinal))
            {
                w1 = new frmKarta();
                w1.ShowDialog();
                UcitajPodatke(dataGridCentralni, kartaSelect);
            }
            else if (ucitanaTabela.Equals(prtljagSelect, StringComparison.Ordinal))
            {
                w1 = new frmPrtljag();
                w1.ShowDialog();
                UcitajPodatke(dataGridCentralni, prtljagSelect);
            }
            else if (ucitanaTabela.Equals(pasosSelect, StringComparison.Ordinal))
            {
                w1 = new frmPasos();
                w1.ShowDialog();
                UcitajPodatke(dataGridCentralni, pasosSelect);
            }
            else if (ucitanaTabela.Equals(poletanjeSelect, StringComparison.Ordinal))
            {
                w1 = new frmPoletanje();
                w1.ShowDialog();
                UcitajPodatke(dataGridCentralni, poletanjeSelect);
            }
            else if (ucitanaTabela.Equals(sletanjeSelect, StringComparison.Ordinal))
            {
                w1 = new frmSletanje();
                w1.ShowDialog();
                UcitajPodatke(dataGridCentralni, sletanjeSelect);
            }
            else if (ucitanaTabela.Equals(putnikSelect, StringComparison.Ordinal))
            {
                w1 = new frmPutnik();
                w1.ShowDialog();
                UcitajPodatke(dataGridCentralni, putnikSelect);
            }
            else if (ucitanaTabela.Equals(avionSelect, StringComparison.Ordinal))
            {
                w1 = new frmAvion();
                w1.ShowDialog();
                UcitajPodatke(dataGridCentralni, avionSelect);
            }

        }

        private void PopuniFormu(DataGrid grid, string selectUslov)
        {
            try
            {
                konekcija.Open();
                azuriraj = true;
                DataRowView red = (DataRowView)grid.SelectedItems[0];
                pomocnired = red;
                SqlCommand komanda = new SqlCommand
                {
                    Connection = konekcija
                };
                komanda.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                komanda.CommandText = selectUslov + "@id";
                SqlDataReader citac = komanda.ExecuteReader();
                komanda.Dispose();
                while(citac.Read())
                {
                    if (ucitanaTabela.Equals(avionSelect))
                    {
                        frmAvion prozorAvion = new frmAvion(azuriraj, pomocnired);
                        prozorAvion.txtModel.Text = citac["Model"].ToString();
                        prozorAvion.txtBrojLeta.Text = citac["BrojLeta"].ToString();
                        prozorAvion.txtBrojEconomy.Text = citac["PutniciEconomy"].ToString();
                        prozorAvion.txtBrojBusiness.Text = citac["PutniciBusiness"].ToString();
                        prozorAvion.txtBrojPosade.Text = citac["BrojClanovaPosade"].ToString();
                        prozorAvion.txtVlasnik.Text = citac["Vlasnik"].ToString();
                        prozorAvion.ShowDialog();
                    } else if (ucitanaTabela.Equals(kartaSelect))
                    {
                        frmKarta prozorKarta = new frmKarta(azuriraj, pomocnired);
                        prozorKarta.txtCenaKarte.Text = citac["CenaKarte"].ToString();
                        prozorKarta.txtBrojSedista.Text = citac["Sediste"].ToString();
                        prozorKarta.txtBrojKreditne.Text = citac["BrojKredKartice"].ToString();
                        prozorKarta.txtKlasaKarta.Text = citac["Klasa"].ToString();
                        prozorKarta.txtSifraKarte.Text = citac["SifraKarte"].ToString();
                        prozorKarta.cbLet.SelectedValue = citac["LetID"].ToString();

                        prozorKarta.ShowDialog();

                    } else if (ucitanaTabela.Equals(letSelect))
                    {
                        frmLet prozorLet = new frmLet(azuriraj, pomocnired);
                        prozorLet.txtNazivLinije.Text = citac["NazivLinije"].ToString();
                        prozorLet.txtSifraLeta.Text = citac["SifraLeta"].ToString();
                        prozorLet.cbAvion.SelectedValue = citac["AvionID"].ToString();
                        prozorLet.cbPoletanje.SelectedValue = citac["PoletanjeID"].ToString();
                        prozorLet.cbSletanje.SelectedValue = citac["SletanjeID"].ToString();

                        prozorLet.ShowDialog();
                    } else if (ucitanaTabela.Equals(putnikSelect))
                    {
                        frmPutnik prozorPutnik = new frmPutnik(azuriraj, pomocnired);
                        prozorPutnik.txtImePutnik.Text = citac["Ime"].ToString();
                        prozorPutnik.txtPrezimePutnik.Text = citac["Prezime"].ToString();
                        prozorPutnik.txtBrojTelPutnik.Text = citac["BrojTel"].ToString();
                        prozorPutnik.txtEMailPutnik.Text = citac["EMail"].ToString();
                        prozorPutnik.txtGodinePutnik.Text = citac["Godine"].ToString();
                        prozorPutnik.txtJMBGPutnik.Text = citac["JMBG"].ToString();
                        prozorPutnik.cbKarta.SelectedValue = citac["KartaID"].ToString();
                        prozorPutnik.cbPasos.SelectedValue = citac["PasosID"].ToString();
                        prozorPutnik.cbPrtljag.SelectedValue = citac["PrtljagID"].ToString();
                        prozorPutnik.ShowDialog();
                    } else if (ucitanaTabela.Equals(pasosSelect))
                    {
                        frmPasos prozorPasos = new frmPasos(azuriraj, pomocnired);
                        prozorPasos.txtImePasos.Text = citac["Ime"].ToString();
                        prozorPasos.txtPrezimePasos.Text = citac["Prezime"].ToString();
                        prozorPasos.txtDrzavaPasos.Text = citac["Drzava"].ToString();
                        prozorPasos.txtGodinePasos.Text = citac["Godine"].ToString();
                        prozorPasos.cbxVizaPasos.IsChecked = (bool)citac["Viza"];
                        prozorPasos.ShowDialog();
                    } else if (ucitanaTabela.Equals(poletanjeSelect))
                    {
                        frmPoletanje prozorPoletanje = new frmPoletanje(azuriraj, pomocnired);
                        prozorPoletanje.txtGradPoletanje.Text = citac["Grad"].ToString();
                        prozorPoletanje.txtVremePoletanje.Text = citac["Vreme"].ToString();
                        prozorPoletanje.txtGatePoletanje.Text = citac["Gate"].ToString();
                        prozorPoletanje.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(sletanjeSelect))
                    {
                        frmSletanje prozorSletanje = new frmSletanje(azuriraj, pomocnired);
                        prozorSletanje.txtGradSletanje.Text = citac["Grad"].ToString();
                        prozorSletanje.txtVremeSletanje.Text = citac["Vreme"].ToString();
                        prozorSletanje.txtGateSletanje.Text = citac["Gate"].ToString();
                        prozorSletanje.ShowDialog();
                    } else if (ucitanaTabela.Equals(prtljagSelect))
                    {
                        frmPrtljag prozorPrtljag = new frmPrtljag(azuriraj, pomocnired);
                        prozorPrtljag.txtTezinaPrtljag.Text = citac["Tezina"].ToString();
                        prozorPrtljag.txtSifraPrtljaga.Text = citac["SifraPrtljag"].ToString();
                        prozorPrtljag.cbxPredjenaKontrolaPrtljag.IsChecked = (bool)citac["PredjenaKontrola"];
                        prozorPrtljag.cbxRucniPrtljag.IsChecked = (bool)citac["RucniPrtljag"];
                        prozorPrtljag.ShowDialog();


                    }

                }

            }
            catch(ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
                azuriraj = false;

            }
            
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            Window w1;

            if (ucitanaTabela.Equals(avionSelect, StringComparison.Ordinal))
            {
                PopuniFormu(dataGridCentralni, selectUslovAvion);
                UcitajPodatke(dataGridCentralni, avionSelect);
            }
            else if (ucitanaTabela.Equals(kartaSelect, StringComparison.Ordinal))
            {
                PopuniFormu(dataGridCentralni, selectUslovKarta);
                UcitajPodatke(dataGridCentralni, kartaSelect);
            }
            else if (ucitanaTabela.Equals(letSelect, StringComparison.Ordinal))
            {
                PopuniFormu(dataGridCentralni, selectUslovLet);
                UcitajPodatke(dataGridCentralni, letSelect);
            }
            else if (ucitanaTabela.Equals(putnikSelect, StringComparison.Ordinal))
            {
                PopuniFormu(dataGridCentralni, selectUslovPutnik);
                UcitajPodatke(dataGridCentralni, putnikSelect);
            }
            else if (ucitanaTabela.Equals(pasosSelect, StringComparison.Ordinal))
            {
                PopuniFormu(dataGridCentralni, selectUslovPasos);
                UcitajPodatke(dataGridCentralni, pasosSelect);
            }
            else if (ucitanaTabela.Equals(poletanjeSelect, StringComparison.Ordinal))
            {
                PopuniFormu(dataGridCentralni, selectUslovPoletanje);
                UcitajPodatke(dataGridCentralni, poletanjeSelect);
            }
            else if (ucitanaTabela.Equals(sletanjeSelect, StringComparison.Ordinal))
            {
                PopuniFormu(dataGridCentralni, selectUslovSletanje);
                UcitajPodatke(dataGridCentralni, sletanjeSelect);
            }
            else if (ucitanaTabela.Equals(prtljagSelect, StringComparison.Ordinal))
            {
                PopuniFormu(dataGridCentralni, selectUslovPrtljag);
                UcitajPodatke(dataGridCentralni, prtljagSelect);
            }

        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {

            if (ucitanaTabela.Equals(avionSelect, StringComparison.Ordinal))
            {
                ObrisiZapis(dataGridCentralni, avionDelete);
                UcitajPodatke(dataGridCentralni, avionSelect);
            }
            else if (ucitanaTabela.Equals(kartaSelect, StringComparison.Ordinal))
            {
                ObrisiZapis(dataGridCentralni, kartaDelete);
                UcitajPodatke(dataGridCentralni, kartaSelect);
            }
            else if (ucitanaTabela.Equals(letSelect, StringComparison.Ordinal))
            {
                ObrisiZapis(dataGridCentralni, letDelete);
                UcitajPodatke(dataGridCentralni, letSelect);
            }
            else if (ucitanaTabela.Equals(pasosSelect, StringComparison.Ordinal))
            {
                ObrisiZapis(dataGridCentralni, pasosDelete);
                UcitajPodatke(dataGridCentralni, pasosSelect);
            }
            else if (ucitanaTabela.Equals(poletanjeSelect, StringComparison.Ordinal))
            {
                ObrisiZapis(dataGridCentralni, poletanjeDelete);
                UcitajPodatke(dataGridCentralni, poletanjeSelect);
            }
            else if (ucitanaTabela.Equals(prtljagSelect, StringComparison.Ordinal))
            {
                ObrisiZapis(dataGridCentralni, prtljagDelete);
                UcitajPodatke(dataGridCentralni, prtljagSelect);
            }
            else if (ucitanaTabela.Equals(putnikSelect, StringComparison.Ordinal))
            {
                ObrisiZapis(dataGridCentralni, putnikDelete);
                UcitajPodatke(dataGridCentralni, putnikSelect);
            }
            else if (ucitanaTabela.Equals(sletanjeSelect, StringComparison.Ordinal))
            {
                ObrisiZapis(dataGridCentralni, sletanjeDelete);
                UcitajPodatke(dataGridCentralni, sletanjeSelect);
            }



        }

        private void ObrisiZapis(DataGrid grid, string deleteUpit)
        {
            try
            {
                konekcija.Open();
                DataRowView red = (DataRowView)grid.SelectedItems[0];
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da zelite da obrisete?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    SqlCommand komanda = new SqlCommand
                    {
                        Connection = konekcija
                    };
                    komanda.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    komanda.CommandText = deleteUpit + "@id";
                    komanda.ExecuteNonQuery();
                    komanda.Dispose();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SqlException)
            {
                MessageBox.Show("Postoji povezani podaci sa drugim tabelama", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                konekcija.Close();
            }
        }
    }
}
