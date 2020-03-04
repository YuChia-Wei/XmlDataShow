using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            using (HttpContent content = response.Content)
            {
                // 將 httpcontent 轉為 string
                result = await content.ReadAsStringAsync();
            }

            return result;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var urlDataDownloader = new UrlDataDownloader();
            var dataPath = $"data\\testdata.txt";
            var url = "http://www.bbc.co.uk/zhongwen/trad/index.xml";

            //urlDataDownloader.GetRssDataAsync(url).GetAwaiter().GetResult();

            string result = await GetResult(url);

            saveXmlFile(dataPath, result);
        }

        private bool saveXmlFile(string path, string dataResult)
        {
            System.IO.Directory.CreateDirectory($"data\\");

            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                var writer = new StreamWriter(fileStream);

                writer.AutoFlush = true;

                foreach (var s in dataResult.Split('\n'))
                {
                    writer.WriteLine(s);
                }
            }

            return File.Exists(path);
        }

        private void ShowDataButton_Click(object sender, EventArgs e)
        {
            dataSet1.ReadXml($"data\\testdata.txt");
            dataGridView1.DataSource = dataSet1;

            //    XmlSerializer ser = new XmlSerializer(typeof(rss));

            //    if (ser.Deserialize(new FileStream(dataPath, FileMode.Open)) is rss processResult)
            //    {
            //        dataGridView1.DataSource = processResult.channel;
            //        dataGridView1.Show();
            //    }
        }
    }
}