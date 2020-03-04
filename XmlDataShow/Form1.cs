using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace XmlDataShow
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static async Task<string> GetResult(string urlStr)
        {
            string result;
            //建立 HttpClient
            HttpClient client = new HttpClient(); //{BaseAddress = new Uri(urlStr)};
            //使用 async 方法從網路 url 上取得回應
            using (HttpResponseMessage response = await client.GetAsync(urlStr))
            // 將網路取得回應的內容設定給 httpcontent，可省略，直接使用 response.Content
            {
                using (HttpContent content = response.Content)
                {
                    // 將 httpcontent 轉為 string
                    result = await content.ReadAsStringAsync();
                }
            }

            return result;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var dataPath = $"data\\testdata.txt";
            var url = "http://www.bbc.co.uk/zhongwen/trad/index.xml";

            var result = await GetResult(url);

            SaveXmlFile(dataPath, result);
        }

        private bool SaveXmlFile(string path, string dataResult)
        {
            System.IO.Directory.CreateDirectory($"data\\");

            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                var writer = new StreamWriter(fileStream);

                writer.Write(dataResult);
                writer.AutoFlush = true;
            }

            return File.Exists(path);
        }

        private void ShowDataButton_Click(object sender, EventArgs e)
        {
            var ser = new XmlSerializer(typeof(rss));
            if (ser.Deserialize(new FileStream($"data\\testdata.txt", FileMode.Open)) is rss result)
            {
                dataGridView1.DataSource = result.channel.item.OrderBy(o => o.pubDate).ToArray();
            }
        }
    }
}