using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApplication1
{

    class Program
    {

        static HttpClient _client = new HttpClient();
        static string _strBaseaddress = "";
        static string _strjson = "";
        static string _strHttpRequest = "";
        static string _strSurveyId = "";
        static string _strOrgKey = "";
        static string _strPublishToken = "";
        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine("Please enter end point address");
            while (string.IsNullOrEmpty(_strBaseaddress))
            {
                try
                {
                    _client.BaseAddress = new Uri(Console.ReadLine());
                    _strBaseaddress = _client.BaseAddress.ToString();
                }
                catch (Exception)
                {
                    Console.WriteLine("Please Enter a valid endpoint");
                }
            }
            Console.WriteLine("Please select file");
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            try
            {
                string line = null;
                StreamReader sr = new StreamReader(ofd.FileName);
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.ToLower().Contains("surveyid"))
                    {
                        _strSurveyId = line.Substring(line.IndexOf("=") + 1).TrimStart('\\').TrimEnd('\\');
                    }
                    else if (line.ToLower().Contains("orgkey"))
                    {
                        _strOrgKey = line.Substring(line.IndexOf("=") + 1).TrimStart('\\').TrimEnd('\\');
                    }
                    else if (line.ToLower().Contains("publisherkey"))
                    {
                        _strPublishToken = line.Substring(line.IndexOf("=") + 1).TrimStart('\\').TrimEnd('\\');
                    }
                    else if (line.ToLower().Contains("httprequest"))
                    {
                        _strHttpRequest = line.Substring(line.IndexOf("=") + 1).Trim();
                    }
                    else if (line.ToLower().StartsWith("{"))
                    {
                        _strjson = line;
                    }
                }
                sr.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
            }

            RunAsync().GetAwaiter().GetResult();
        }
        static async Task RunAsync()
        {
            // client.BaseAddress = new Uri("http://localhost:51498/"); 
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("SurveyId", _strSurveyId);
            _client.DefaultRequestHeaders.Add("PublisherKey", _strPublishToken);
            _client.DefaultRequestHeaders.Add("OrgKey", _strOrgKey);
            try
            {
                if (_strHttpRequest.ToLower() == "post")
                {
                    var url = await CreateSurveyAnswerAsync(_strjson);
                }
                else if (_strHttpRequest.ToLower() == "patch")
                {
                    var url = await UpdateSurveyAnswer(_strjson);
                }
                else if (_strHttpRequest.ToLower() == "delete")
                {
                    var url = await DeleteSurveyAnswer(_strjson);
                }              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }


        static async Task<Uri> CreateSurveyAnswerAsync(string Jsonstring)
        {          
            var content = new StringContent(Jsonstring, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("api/surveyResponse", content);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Response generated. " + response.Headers.Location);
            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<Uri> UpdateSurveyAnswer(string Jsonstring)
        {          
            var content = new StringContent(Jsonstring, Encoding.UTF8, "application/json");
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, "api/surveyResponse")
            {
                Content = content
            };
            CancellationToken canceltoken;
            var response = await _client.SendAsync(request, canceltoken);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Response updated. " + response.Headers.Location);
            return response.Headers.Location;
        }

        static async Task<Uri> DeleteSurveyAnswer(string Jsonstring)
        {           
            var content = new StringContent(Jsonstring, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/surveyResponse");
            request.Content = content;// new StringContent(JsonConvert.SerializeObject(Jsonstring), Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Response deleted. " + response.Headers.Location);
            // return URI of the created resource.
            return response.Headers.Location;
        }

    }
}