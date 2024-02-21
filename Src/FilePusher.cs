using gfp.Src.Framework.CommandLine;
using gfp.Src.Log;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace gfp.Src
{
    public class FilePusher
    {
        public static void Push(Options options)
        {
            try
            {
                // 将文件转换成Base64编码
                Logger.Info("Encoding local file to Base64...");
                string base64 = Convert.ToBase64String(File.ReadAllBytes(options.Local));
                Logger.Info("File " + options.Local + " Encoded.");

                // 获取API
                string api = Api(options);

                // 获取文件SHA
                Logger.Info("Retrieving SHA of the remote file...");
                string sha = Sha(api, options);

                // 更新到仓库
                Logger.Info("Pushing file to repository...");
                Push(api, sha, base64, options);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.GetType() == typeof(AggregateException))
                {
                    Exception temp = ex;
                    while (temp.InnerException != null)
                    {
                        temp = temp.InnerException;
                    }
                    message = temp.Message;
                }
                Logger.Error(message);
            }
            finally
            {
                if (options.Pause)
                {
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }
            }
        }

        private static string Api(Options options)
        {
            string[] strings = options.Repo.Split('/');
            string owner = strings[strings.Length - 2];
            string repo = strings[strings.Length - 1];
            return "https://api.github.com/repos/" + owner + "/" + repo + "/contents/" + options.File + "?ref=" + options.Branch;
        }

        private static string Sha(string api, Options options)
        {
            string sha = null;
            HttpClient(options, (client) =>
            {
                Task<HttpResponseMessage> task = client.GetAsync(api);
                if (Task.WaitAny(task, Task.Delay(options.Timoyout)) != 0) throw new Exception("HTTP-Get request timeout.");

                using (var response = task.Result)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    JObject jObject = JsonConvert.DeserializeObject<JObject>(json);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        sha = jObject.TryGetValue("sha", out var token) ? token.ToString() : null;
                        Logger.Info("File SHA:" + sha);
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        Logger.Warn("Remote file does not exist and a new file will be added.");
                    }
                }
            });
            return sha;
        }

        private static void Push(string api, string sha, string base64, Options options)
        {
            HttpClient(options, (client) =>
            {
                var map = new Dictionary<string, object>
                {
                    ["sha"] = sha,
                    ["message"] = options.Message,
                    ["committer"] = new { name = options.Committer, email = options.CommitterEmail },
                    ["content"] = base64
                };

                HttpContent content = new StringContent(JsonConvert.SerializeObject(map), Encoding.UTF8, "application/json");
                Task<HttpResponseMessage> task = client.PutAsync(api, content);
                if (Task.WaitAny(task, Task.Delay(options.Timoyout)) != 0) throw new Exception("HTTP-Put request timeout.");

                using (HttpResponseMessage response = task.Result)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Logger.Info("File " + options.File + " updated.");
                    }
                    else if (response.StatusCode == HttpStatusCode.Created)
                    {
                        Logger.Info("File " + options.File + " added.");
                    }
                    else
                    {
                        JObject jObject = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                        throw new Exception(jObject.TryGetValue("message", out var token) ? token.ToString() : "File push error");
                    }
                }
            });
        }
        private static void HttpClient(Options options, Action<HttpClient> action)
        {
            WebProxy proxy = options.Proxy ? new WebProxy(options.ProxyHost, options.ProxyPort) : null;
            using (var client = new HttpClient(new HttpClientHandler { Proxy = proxy, UseProxy = options.Proxy }) { Timeout = TimeSpan.FromSeconds(10) })
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36");
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + options.Token);
                client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
                action.Invoke(client);
            }
        }
    }
}
