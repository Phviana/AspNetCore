using AspNetCoreMVC.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreMVC
{

    public interface IReportHelper
    {
        Task CreateReport(Order order);
    }
    public class ReportHelper : IReportHelper
    {
        private readonly IConfiguration configuration;
        private const string URISTRING = "api/report";
        private readonly HttpClient httpClient; // httpClientFactory.Create

        // do not detect DNS change
        // private static HttpClient httpClient;

        public ReportHelper(IConfiguration configuration, HttpClient httpClient)
        {
            this.configuration = configuration;
            this.httpClient = httpClient;
        }
        public async Task CreateReport(Order order)
        {

            string reportLine = await GetReportLine(order);

            // Problem : socket exhaustion
            // using (HttpClient httpClient = new HttpClient())


            // content text (JSON)
            var content = JsonConvert.SerializeObject(reportLine);
            // HttpContent that pack the text
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            // URI = Uniform Resource Identifier

            // get endpoint's token
            var discoveryResponse = 
            await httpClient.GetDiscoveryDocumentAsync(configuration["AspNetCoreMVC.IdentityServer_Url"]);

            if (discoveryResponse.IsError)
            {
                throw new ApplicationException(discoveryResponse.Error);
            }


            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryResponse.TokenEndpoint,
                    ClientId = "ASPNETCOREMVC",
                    ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0",
                    Scope = "AspNetCoreMVC.ReportWebApi"
                }
                );

            if (tokenResponse.IsError)
            {
                Debug.WriteLine(tokenResponse.IsError);
                return;
            }

            httpClient.SetBearerToken(tokenResponse.AccessToken);
            

            Uri baseUrl = new Uri(configuration["BaseUrl"]);
            Uri uri = new Uri(baseUrl, URISTRING);
            HttpResponseMessage httpResponse = await httpClient.PostAsync(uri, httpContent);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new ApplicationException(httpResponse.ReasonPhrase);
            }

        }

        private async Task<string> GetReportLine(Order order)
        {
            StringBuilder sb = new StringBuilder();
            string orderTemplate =
                    await System.IO.File.ReadAllTextAsync("TemplateOrder.txt");

            string templateOrderItem =
                await System.IO.File.ReadAllTextAsync("TemplateOrderItem.txt");

            string orderLine =
                string.Format(orderTemplate,
                    order.Id,
                    order.Register.Name,
                    order.Register.Address,
                    order.Register.Complement,
                    order.Register.Neighborhood,
                    order.Register.County,
                    order.Register.UF,
                    order.Register.Telephone,
                    order.Register.Email,
                    order.Itens.Sum(i => i.Subtotal));

            sb.AppendLine(orderLine);

            foreach (var i in order.Itens)
            {
                string lineOrderItem =
                    string.Format(
                        templateOrderItem,
                        i.Product.Code,
                        i.UnitPrice,
                        i.Product.Name,
                        i.Amount,
                        i.Subtotal);

                sb.AppendLine(lineOrderItem);
            }
            sb.AppendLine($@"=============================================");

            return sb.ToString();
        }
    }
}

