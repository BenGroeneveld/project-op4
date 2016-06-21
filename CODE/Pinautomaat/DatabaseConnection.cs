using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Pinautomaat
{
    public class DatabaseConnection
    {
        public static async Task<Pas> getPasFromDatabase(string type)
        {
            Pas pas = new Pas();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            using(var client = new HttpClient(new HttpClientHandler { UseProxy = false, ClientCertificateOptions = ClientCertificateOption.Automatic }))
            {
                client.BaseAddress = new Uri("https://hrsqlapp.tk");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(string.Concat("/api/pass/", type)).ConfigureAwait(false);

                if(response.IsSuccessStatusCode)
                {
                    try
                    {
                        pas = await response.Content.ReadAsAsync<Pas>();
                        return pas;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task getRekeningFromDatabase(string type)
        {
            Rekening rekening = new Rekening();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            using(var client = new HttpClient(new HttpClientHandler { UseProxy = false, ClientCertificateOptions = ClientCertificateOption.Automatic }))
            {
                client.BaseAddress = new Uri("https://hrsqlapp.tk");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(string.Concat("/api/rekenings/", type)).ConfigureAwait(false);

                if(response.IsSuccessStatusCode)
                {
                    try
                    {
                        rekening = await response.Content.ReadAsAsync<Rekening>();
                    }
                    catch
                    {
                    }
                }
            }
        }

        public async Task getKlantFromDatabase(string type)
        {
            Klant klant = new Klant();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            using(var client = new HttpClient(new HttpClientHandler { UseProxy = false, ClientCertificateOptions = ClientCertificateOption.Automatic }))
            {
                client.BaseAddress = new Uri("https://hrsqlapp.tk");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(string.Concat("/api/klants/", type)).ConfigureAwait(false);

                if(response.IsSuccessStatusCode)
                {
                    try
                    {
                        klant = await response.Content.ReadAsAsync<Klant>();
                    }
                    catch
                    {
                    }
                }
            }
        }

        public async Task setPasFromDatabase(string type, string update)
        {
            Pas pas = new Pas();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            using(var client = new HttpClient(new HttpClientHandler { UseProxy = false, ClientCertificateOptions = ClientCertificateOption.Automatic }))
            {
                client.BaseAddress = new Uri("https://hrsqlapp.tk");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PutAsJsonAsync(string.Concat("/api/pass/", type), update).ConfigureAwait(false);

                if(response.IsSuccessStatusCode)
                {
                    try
                    {
                        pas = await response.Content.ReadAsAsync<Pas>();
                    }
                    catch
                    {
                    }
                }
            }
        }

        public async Task setRekeningFromDatabase(string type, string update)
        {
            Rekening rekening = new Rekening();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            using(var client = new HttpClient(new HttpClientHandler { UseProxy = false, ClientCertificateOptions = ClientCertificateOption.Automatic }))
            {
                client.BaseAddress = new Uri("https://hrsqlapp.tk");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PutAsJsonAsync(string.Concat("/api/pass/", type), update).ConfigureAwait(false);

                if(response.IsSuccessStatusCode)
                {
                    try
                    {
                        rekening = await response.Content.ReadAsAsync<Rekening>();
                    }
                    catch
                    {
                    }
                }
            }
        }

        public async Task setKlantFromDatabase(string type, string update)
        {
            Klant klant = new Klant();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            using(var client = new HttpClient(new HttpClientHandler { UseProxy = false, ClientCertificateOptions = ClientCertificateOption.Automatic }))
            {
                client.BaseAddress = new Uri("https://hrsqlapp.tk");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PutAsJsonAsync(string.Concat("/api/pass/", type), update).ConfigureAwait(false);

                if(response.IsSuccessStatusCode)
                {
                    try
                    {
                        klant = await response.Content.ReadAsAsync<Klant>();
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
