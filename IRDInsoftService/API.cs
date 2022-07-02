using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace IRDInsoftService
{
    class API
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task SendData(Dictionary<string, string> irdData)
        {
            var content = new FormUrlEncodedContent(irdData);
            var response = await client.PostAsync("http://localhost:3000/api/bill", content);

            var responseString = await response.Content.ReadAsStringAsync();
            
        }
        

    }
}
