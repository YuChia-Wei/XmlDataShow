using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XmlDataShow
{
    public class UrlDataDownloader
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public bool DownloadFile(string url, string savingPath)
        {
            var stringAsync = _httpClient.GetStringAsync(url);

            var stringAsyncResult = stringAsync.Result;

            System.IO.Directory.CreateDirectory($"data\\");

            using (var fileStream = new FileStream(savingPath, FileMode.OpenOrCreate))
            {
                var writer = new StreamWriter(fileStream);
                writer.Write(stringAsyncResult);
            }


            return File.Exists(savingPath);
            //XmlSerializer ser = new XmlSerializer(typeof(rss));
        }

        public async Task<string> GetRssDataAsync(string path)
        {
            string rssData = null;
            HttpResponseMessage response = await _httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                rssData = await response.Content.ReadAsStringAsync();
            }
            return rssData;
        }
    }
}