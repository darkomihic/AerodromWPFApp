using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Aerodrom_Darko_Mihic_IT69_2020
{
    public class Konekcija
    {
        public SqlConnection KreirajKonekciju()
        {
            SqlConnectionStringBuilder scSb = new SqlConnectionStringBuilder
            {
                DataSource = @"ZORAN\SQLEXPRESS",
                InitialCatalog = "Aerodrom",
                IntegratedSecurity = true,
            };

            string conn = scSb.ToString();
            SqlConnection konekcija = new SqlConnection(conn);
            return konekcija;
        }


    }
}
