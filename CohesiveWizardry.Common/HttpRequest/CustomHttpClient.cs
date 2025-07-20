using System.Diagnostics;
using System.Net;
using CohesiveWizardry.Common.Diagnostics;

namespace CohesiveWizardry.Common.HttpRequest
{
    /// <summary>
    /// Wrapper around HTTP Client for simplified use cases and integration within CohesiveRp.
    /// </summary>
    public static class CustomHttpClient
    {
        private static readonly HttpClient httpClient = null;

        static CustomHttpClient()
        {
            var clientHandler = new HttpClientHandler();

            // Ignore SSL Cert validation
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            httpClient = new HttpClient(clientHandler);
            httpClient.Timeout = new TimeSpan(0, 30, 0);
        }

        public static async Task<(string result, HttpStatusCode? resultCode)> TryGetAsync(string url)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return (responseBody, response.StatusCode);
            } catch (HttpRequestException e) when (e.StatusCode != HttpStatusCode.NotFound)
            {
                LoggingManager.LogToFile("4a7026bd-ad4a-4ba1-ac87-9ccae6d32b23", $"GET HttpRequest to url [{url}] failed. Response status code [{response?.StatusCode}].", e);
            } catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                // Ignore
            } finally
            {
                response?.Dispose();
            }

            return (null, response?.StatusCode);
        }

        public static async Task<(string result, HttpStatusCode? resultCode)> TryPostAsync(string url, HttpContent payload)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.PostAsync(url, payload, CancellationToken.None);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return (responseBody, response.StatusCode);
            } catch (HttpRequestException e)
            {
                LoggingManager.LogToFile("82c83942-8b87-4375-9f53-623ffa66f806", $"POST HttpRequest to url [{url}] failed. Response status code [{response?.StatusCode}].", e);
            } finally
            {
                response?.Dispose();
            }

            return (null, response?.StatusCode);
        }

        public static async Task<(string result, HttpStatusCode? resultCode)> TryPutAsync(string url, StringContent payload)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.PutAsync(url, payload, CancellationToken.None);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return (responseBody, response.StatusCode);
            } catch (HttpRequestException e)
            {
                LoggingManager.LogToFile("65bf0ab5-1112-45ef-8d55-5fd316890838", $"PUT HttpRequest to url [{url}] failed. Response status code [{response?.StatusCode}].", e);
            } finally
            {
                response?.Dispose();
            }

            return (null, response?.StatusCode);
        }

        public static async Task<(List<string> resultLines, HttpStatusCode? resultCode)> TryPostStreamAsync(string url, HttpContent payload, StreamWriter streamToWriteTo, int minMsToKeepStreamOpened = 3000)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, url) { Content = payload }, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None);
                response.EnsureSuccessStatusCode();
                using var responseStream = new StreamReader(await response.Content.ReadAsStreamAsync());

                Stopwatch stopWatch = Stopwatch.StartNew();
                List<string> resultLines = new();
                while (!responseStream.EndOfStream || stopWatch.ElapsedMilliseconds < minMsToKeepStreamOpened)
                {
                    if (responseStream.EndOfStream)
                    {
                        await Task.Delay(200);
                        continue;
                    }

                    string nextLine = await responseStream.ReadLineAsync();

                    if (nextLine == null)
                    {
                        await Task.Delay(200);
                        continue;
                    }

                    await streamToWriteTo.WriteLineAsync(nextLine);
                    resultLines.Add(nextLine);
                    await streamToWriteTo.FlushAsync();
                }

                // Quirk of SillyTavern, may make it specific later
                await streamToWriteTo.WriteLineAsync("");
                await streamToWriteTo.FlushAsync();

                stopWatch.Stop();
                return (resultLines, response.StatusCode);
            } catch (HttpRequestException e)
            {
                LoggingManager.LogToFile("1a1b124f-b2a6-4e41-8be0-e21af0a6c05b", $"POST HttpRequest to url [{url}] failed. Response status code [{response?.StatusCode}].", e);
            } finally
            {
                response?.Dispose();
            }

            return (null, response?.StatusCode);
        }

        /// <summary>
        /// Custom sendAsync to be able to customize headers, payload, etc
        /// </summary>
        /// <param name="httpMessage"></param>
        /// <returns></returns>
        public static async Task<(string result, HttpStatusCode? resultCode)> TrySendAsync(HttpRequestMessage httpMessage)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            HttpResponseMessage response = null;
            try
            {
                response = await httpClient.SendAsync(httpMessage, CancellationToken.None);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return (responseBody, response.StatusCode);
            } catch (HttpRequestException e)
            {
                LoggingManager.LogToFile("d4beb727-beca-4180-b583-dac3cb093bb5", $"POST HttpRequest to url [{httpMessage.RequestUri.AbsoluteUri}] failed. Response status code [{response?.StatusCode}].", e);
            } finally
            {
                response?.Dispose();
            }

            return (null, response?.StatusCode);
        }
    }
}
