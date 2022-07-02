using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace IRDInsoftService
{
    public class BillViewModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string seller_pan { get; set; }
        public string buyer_pan { get; set; }
        public string fiscal_year { get; set; }
        public string buyer_name { get; set; }
        public string invoice_number { get; set; }
        public string invoice_date { get; set; }
        public double total_sales { get; set; }
        public Nullable<double> taxable_sales_vat { get; set; }
        public Nullable<double> vat { get; set; }
        public Nullable<double> excisable_amount { get; set; }
        public Nullable<double> excise { get; set; }
        public Nullable<double> taxable_sales_hst { get; set; }
        public Nullable<double> hst { get; set; }
        public Nullable<double> amount_for_esf { get; set; }
        public Nullable<double> esf { get; set; }
        public Nullable<double> export_sales { get; set; }
        public Nullable<double> tax_exempted_sales { get; set; }
        public bool isrealtime { get; set; }
        public DateTime datetimeClient { get; set; }

    }
    class IRDDAta : Database
    {
        private HttpClient httpClient;
        string baseUrlBill = ConfigurationManager.AppSettings.Get("billURL");
        string baseUrlReturn = ConfigurationManager.AppSettings.Get("returnURL");
        string fiscalYear = ConfigurationManager.AppSettings.Get("fiscalYear");
        string username = ConfigurationManager.AppSettings.Get("user");
        string password = ConfigurationManager.AppSettings.Get("pwd");
        string sellerPan = ConfigurationManager.AppSettings.Get("sellerPan");


        #region Prepare Data
        public async void Prepare(object state)
        {
            
            DataTable _dt = new DataTable();
            DataTable _dt_sms_result = new DataTable();
            _dt = GetInfo();
            if (_dt != null)
            {           
                foreach (DataRow dr in _dt.Rows)
                {
                    if (dr["isReturn"].ToString() == "0")
                    {
                        // POST /api/bill
                        var irdData = new Dictionary<string, string>
                        {
                           { "username", username },
                           { "password", password },
                           { "seller_pan", sellerPan },
                           { "buyer_pan", dr["Customer_pan"].ToString() },
                           { "fiscal_year", dr["Fiscal_year"].ToString() },
                           { "buyer_name", dr["Customer_name"].ToString() },
                           { "invoice_number", dr["Bill_no"].ToString() },
                           { "invoice_date", dr["Bill_Date"].ToString() },
                           { "total_sales", dr["nettotal"].ToString() },
                           { "taxable_sales", dr["Taxable"].ToString() },
                           { "vat", null },
                           { "excisable_amount", null },
                           { "excise", null },
                           { "taxable_sales_hst", null },
                           { "hst", null },
                           { "amount_for_esf", null },
                           { "esf", null },
                           { "export_sales", null },
                           { "tax_exempted_sales", dr["Tax_Tx"].ToString() },
                           { "isrealtime", "false" },
                           { "datetimeClient", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") }

                        };
                       
                        httpClient = new HttpClient();
                        try
                        {
                            string resourceAddress = "http://192.168.200.4:8065/api/bill.aspx";                           
                            
                            string postBody = JsonConvert.SerializeObject(irdData);
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage wcfResponse = await httpClient.PostAsync(resourceAddress, new StringContent(postBody, Encoding.UTF8, "application/json"));
                            
                            if(wcfResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                PushInfo(dr["Fiscal_year"].ToString(), dr["Bill_no"].ToString(), dr["isReturn"].ToString());
                            }
                            else
                            {
                                if (httpClient != null)
                                {
                                    httpClient.Dispose();
                                    httpClient = null;
                                }
                            }
                        }
                        catch (HttpRequestException hre)
                        {
                            Console.WriteLine("Error:" + hre.Message);
                        }
                        catch (TaskCanceledException)
                        {
                            Console.WriteLine("Request canceled.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally
                        {
                            if (httpClient != null)
                            {
                                httpClient.Dispose();
                                httpClient = null;
                            }
                        }
                    }
                    else
                    {
                        // POST /api/billreturn
                        var irdData = new Dictionary<string, string>
                        {
                           { "username", "hello" },
                           { "password", "world" },
                           { "seller_pan", "hello" },
                           { "buyer_pan", dr["Customer_pan"].ToString() },
                           { "fiscal_year", dr["Fiscal_year"].ToString() },
                           { "buyer_name", dr["Customer_name"].ToString() },
                           { "ref_invoice_number", dr["Bill_no"].ToString() },
                           { "credit_note_number", dr["Customer_name"].ToString() },
                           { "return_date", dr["Bill_Date"].ToString() },
                           { "reason_for_return", dr["Customer_name"].ToString() },
                           { "total_sales", dr["nettotal"].ToString() },
                           { "taxable_sales_vat", dr["Taxable"].ToString() },
                           { "vat", null },
                           { "excisable_amount", null },
                           { "excise", null },
                           { "taxable_sales_hst", null },
                           { "hst", null },
                           { "amount_for_esf", null },
                           { "esf", null },
                           { "export_sales", null },
                           { "tax_exempted_sales", dr["Tax_Tx"].ToString() },
                           { "isrealtime", "false" },
                           { "datetimeClient", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") }

                        };
                        
                        httpClient = new HttpClient();
                        try
                        {
                            string resourceAddress = baseUrlReturn;

                            string postBody = JsonConvert.SerializeObject(irdData);
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage wcfResponse = await httpClient.PostAsync(resourceAddress, new StringContent(postBody, Encoding.UTF8, "application/json"));
                            
                            if (wcfResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                PushInfo(dr["Fiscal_year"].ToString(), dr["Bill_no"].ToString(), dr["isReturn"].ToString());
                            }
                            else
                            {
                                if (httpClient != null)
                                {
                                    httpClient.Dispose();
                                    httpClient = null;
                                }
                            }
                        }
                        catch (HttpRequestException hre)
                        {
                            Console.WriteLine("Error:" + hre.Message);
                        }
                        catch (TaskCanceledException)
                        {
                            Console.WriteLine("Request canceled.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally
                        {
                            if (httpClient != null)
                            {
                                httpClient.Dispose();
                                httpClient = null;
                            }
                        }
                    }
                   
                }//end foreach
                
            }
            Prepare(null);
        }

        #endregion

        #region SProc Activities

        #region Get Data From SProc

        public DataTable GetInfo()
        {
            cmd.CommandText = "ird_checkSynStatus";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@fy", fiscalYear);
            
            DataTable dt = new DataTable();

            try
            {
                Connect();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


            }
            catch (SqlException se)
            {
                Console.WriteLine(se.ToString());
                return null;
            }
            finally
            {
                Disconnect();
            }
            return dt;
            // return list;
        }

        #endregion

        #region Push data to SProc

        public void PushInfo(String fy, String bill, String ret)
        {
            int res = -1;
            cmd.CommandText = "usp_IrdSyncUpdate";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@fy", fy);
            cmd.Parameters.AddWithValue("@bill", bill);
            cmd.Parameters.AddWithValue("@rtn", ret);
            // cmd.Parameters.AddWithValue("@dt_sms_result", dt_sms_result);
            // SqlParameter tvpParam = cmd.Parameters.AddWithValue("@dt_sms_result", dt_sms_result);
            // tvpParam.SqlDbType = SqlDbType.Structured;

            try
            {
                Connect();
                res = cmd.ExecuteNonQuery();

            }
            catch (SqlException se)
            {
                Console.WriteLine(se.ToString());
                //return null;
            }
            finally
            {
                Disconnect();
            }

        }
        #endregion
        #endregion
    }
}
