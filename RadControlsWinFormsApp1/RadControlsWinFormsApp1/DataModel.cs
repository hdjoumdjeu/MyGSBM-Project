using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace ALCHANYSCHOOL
{
    public class DataModel
    {
        public List<KeyValuePair<double, object>> GetPieData()
        {
            List<KeyValuePair<double, object>> data = new List<KeyValuePair<double, object>>();

            data.Add(new KeyValuePair<double, object>(46, "Samsung"));
            data.Add(new KeyValuePair<double, object>(43.5, "Apple"));
            data.Add(new KeyValuePair<double, object>(19, "RIM"));
            data.Add(new KeyValuePair<double, object>(15, "Huawei"));
            data.Add(new KeyValuePair<double, object>(19.6, "Other"));
            data.Add(new KeyValuePair<double, object>(12, "Siemens"));
            data.Add(new KeyValuePair<double, object>(11.5, "Panasonic"));
            data.Add(new KeyValuePair<double, object>(8, "Nokia"));
            data.Add(new KeyValuePair<double, object>(6.5, "Sony"));
            data.Add(new KeyValuePair<double, object>(87, "Fujitsu"));
            data.Add(new KeyValuePair<double, object>(98, "HTC"));
            data.Add(new KeyValuePair<double, object>(20, "Motorola"));

            return data;
        }

        public List<KeyValuePair<double, object>> GetBarData(SqlDataReader dataReader)
        {
            List<KeyValuePair<double, object>> data = new List<KeyValuePair<double, object>>();
            double Key;
            String Value;
           KeyValuePair<double, object> point ;//= new KeyValuePair<double, object>(20.55, "Samsung");           
            while (dataReader.Read())
            {
                Key = double.Parse(dataReader["INSCRIT_PAR_CLASSE"].ToString());
                Value = dataReader["Intitule"].ToString();
                point = new KeyValuePair<double, object>(Key, Value);
                data.Add(point);
            }
            return data;
        }

        public List<KeyValuePair<double, object>> GetBar_PensionData(SqlDataReader dataReader)
        {
            List<KeyValuePair<double, object>> data = new List<KeyValuePair<double, object>>();
            double MontantNetTT = 0;
            double MontantRecuTT = 0;
            double MontantResteTT=0, RemiseAmount = 0;
            double CumulRemisePlusAvance=0;
            KeyValuePair<double, object> point;           
            while (dataReader.Read())
            {               
                //Cumul des montant TT ici
                MontantNetTT = MontantNetTT + double.Parse(dataReader["Montant_Net"].ToString());
                MontantRecuTT = MontantRecuTT + double.Parse(dataReader["Avance"].ToString());
                if (dataReader["MontantRemise"].ToString() != "")
                {
                    RemiseAmount = double.Parse(dataReader["MontantRemise"].ToString());
                }
                else
                {
                    RemiseAmount = 0;
                }
                CumulRemisePlusAvance = RemiseAmount + double.Parse(dataReader["Avance"].ToString());
                MontantResteTT = MontantResteTT + double.Parse(dataReader["Montant_Net"].ToString()) - CumulRemisePlusAvance;
            }
            point = new KeyValuePair<double, object>(MontantRecuTT, "Pension Payer");
            data.Add(point);
            point = new KeyValuePair<double, object>(MontantResteTT, "Pension Restante");
            data.Add(point);
            return data;
        }

        public List<KeyValuePair<double, object>> GetLineOneData()
        {
            List<KeyValuePair<double, object>> data = new List<KeyValuePair<double, object>>();

            KeyValuePair<double, object> point = new KeyValuePair<double, object>(400, 2007);
            data.Add(point);
            point = new KeyValuePair<double, object>(550, 2008);
            data.Add(point);
            point = new KeyValuePair<double, object>(740, 2009);
            data.Add(point);
            point = new KeyValuePair<double, object>(940, 2010);
            data.Add(point);
            point = new KeyValuePair<double, object>(1170, 2011);
            data.Add(point);
            point = new KeyValuePair<double, object>(1388, 2012);
            data.Add(point);
            point = new KeyValuePair<double, object>(1540, 2013);
            data.Add(point);
            point = new KeyValuePair<double, object>(1720, 2014);
            data.Add(point);
            point = new KeyValuePair<double, object>(2000, 2015);
            data.Add(point);

            return data;
        }

        public List<KeyValuePair<double, object>> GetLineTwoData()
        {
            List<KeyValuePair<double, object>> data = new List<KeyValuePair<double, object>>();

            KeyValuePair<double, object> point = new KeyValuePair<double, object>(1120, 2007);
            data.Add(point);
            point = new KeyValuePair<double, object>(1240, 2008);
            data.Add(point);
            point = new KeyValuePair<double, object>(1350, 2009);
            data.Add(point);
            point = new KeyValuePair<double, object>(1400, 2010);
            data.Add(point);
            point = new KeyValuePair<double, object>(1490, 2011);
            data.Add(point);
            point = new KeyValuePair<double, object>(1535, 2012);
            data.Add(point);
            point = new KeyValuePair<double, object>(1610, 2013);
            data.Add(point);
            point = new KeyValuePair<double, object>(1680, 2014);
            data.Add(point);
            point = new KeyValuePair<double, object>(1700, 2015);
            data.Add(point);

            return data;
        }
    }
}
