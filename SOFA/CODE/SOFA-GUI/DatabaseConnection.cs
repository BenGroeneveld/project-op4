using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace Pinautomaat
{
    public class DatabaseConnection
    {
        public static async Task<bool> isDatabaseConnected()
        {
            Pas pas = new Pas();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            using(var client = new HttpClient(new HttpClientHandler { UseProxy = false, ClientCertificateOptions = ClientCertificateOption.Automatic }))
            {
                string loadURL = string.Concat("/api/pass/", "123");
                client.BaseAddress = new Uri("https://hrsqlapp.tk");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.GetAsync(loadURL).Wait();
                HttpResponseMessage response = await client.GetAsync(loadURL).ConfigureAwait(false);

                if(response.IsSuccessStatusCode)
                {
                    try
                    {
                        pas = await response.Content.ReadAsAsync<Pas>();
                        if(pas != null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public static async Task<Pas> getPasFromDatabase()
        {
            Pas pas = new Pas();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            using(var client = new HttpClient(new HttpClientHandler { UseProxy = false, ClientCertificateOptions = ClientCertificateOption.Automatic }))
            {
                string loadURL = string.Concat("/api/pass/", Program.PasID);
                client.BaseAddress = new Uri("https://hrsqlapp.tk");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.GetAsync(loadURL).Wait();
                HttpResponseMessage response = await client.GetAsync(loadURL).ConfigureAwait(false);

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

        public static async Task<Rekening> getRekeningFromDatabase()
        {
            Rekening rekening = new Rekening();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            using(var client = new HttpClient(new HttpClientHandler { UseProxy = false, ClientCertificateOptions = ClientCertificateOption.Automatic }))
            {
                string loadURL = string.Concat("/api/rekenings/", Program.RekeningID);
                client.BaseAddress = new Uri("https://hrsqlapp.tk");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.GetAsync(loadURL).Wait();
                HttpResponseMessage response = await client.GetAsync(loadURL).ConfigureAwait(false);

                if(response.IsSuccessStatusCode)
                {
                    try
                    {
                        rekening = await response.Content.ReadAsAsync<Rekening>();
                        return rekening;
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

        public static void setPasFromDatabase(Pas updateDataObject)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            using(var client = new HttpClient(new HttpClientHandler { UseProxy = false, ClientCertificateOptions = ClientCertificateOption.Automatic }))
            {
                string loadURL = string.Concat("/api/pass/", updateDataObject.RekeningID);
                client.BaseAddress = new Uri("https://hrsqlapp.tk");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PutAsJsonAsync(loadURL, updateDataObject).Result;
            }
        }

        public static void setRekeningFromDatabase(Rekening updateDataObject)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            using(var client = new HttpClient(new HttpClientHandler { UseProxy = false, ClientCertificateOptions = ClientCertificateOption.Automatic }))
            {
                string loadURL = string.Concat("/api/rekenings/", updateDataObject.RekeningID);
                client.BaseAddress = new Uri("https://hrsqlapp.tk");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PutAsJsonAsync(loadURL, updateDataObject).Result;
            }
        }
    }
}
