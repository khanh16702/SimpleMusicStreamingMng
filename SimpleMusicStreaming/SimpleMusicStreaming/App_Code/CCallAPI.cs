using CoreLib.Entities;
using NuGet.Protocol;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using System.Net.Http.Headers;

namespace SimpleMusicStreaming.App_Code
{
    public class CCallAPI
    {

        public static async Task<CResponseMessage> PostTemplateAsync(Object obj, string LinkAPI, IConfiguration configuration)
        {
            HttpClient client = new HttpClient();
            string APILINK = configuration["UrlAPI"];
            
            try
            {
                client.BaseAddress = new Uri(APILINK);
                client.Timeout = TimeSpan.FromMinutes(30);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            CResponseMessage cResult = new CResponseMessage();
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(LinkAPI, obj);  
                if (response.IsSuccessStatusCode)
                {
                    cResult = await response.Content.ReadAsAsync<CResponseMessage>();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return cResult;
        }

        public static async Task<Object> SearchTemplateAsync(string LinkAPI, IConfiguration configuration)
        {
            HttpClient client = new HttpClient();
            string APILINK = configuration["UrlAPI"];

            try
            {
                client.BaseAddress = new Uri(APILINK);
                client.Timeout = TimeSpan.FromMinutes(30);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            Object obj = new Object();
            try
            {
                HttpResponseMessage response = await client.GetAsync(LinkAPI);

                if (response.IsSuccessStatusCode)
                {
                    obj = await response.Content.ReadAsAsync<Object>();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return obj;
        }

        public static async Task<CResponseMessage> DeleteTemplateAsync(string LinkAPI, IConfiguration configuration)
        {
            HttpClient client = new HttpClient();
            string APILINK = configuration["UrlAPI"];

            try
            {
                client.BaseAddress = new Uri(APILINK);
                client.Timeout = TimeSpan.FromMinutes(30);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            CResponseMessage cResult = new CResponseMessage();

            try
            {
                HttpResponseMessage response = await client.DeleteAsync(LinkAPI);
                if (response.IsSuccessStatusCode)
                {
                    cResult = await response.Content.ReadAsAsync<CResponseMessage>();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return cResult;
        }
    }
}
