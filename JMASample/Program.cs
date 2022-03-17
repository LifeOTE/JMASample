using Newtonsoft.Json.Linq;


namespace JMASample
{
    class Program
    {
        static readonly HttpClient client = new();
        static async Task Main(string[] args)
        {
            string latest_Time = await client.GetStringAsync("https://www.jma.go.jp/bosai/amedas/data/latest_time.txt");
            Console.WriteLine(latest_Time);
            DateTime dateTime = DateTime.Parse(latest_Time);
            Console.WriteLine(dateTime);
            string url = "https://www.jma.go.jp/bosai/amedas/data/map/" + dateTime.ToString("yyyyMMddHHmmss") + ".json";
            Console.WriteLine(url);
            string amedas_Data = await client.GetStringAsync(url);
            Console.WriteLine(amedas_Data);
            JObject amedas = JObject.Parse(amedas_Data);
            var amedas_List = new List<JMA_amedas>();
            foreach (var pair in amedas)
            {
                
                JValue? wind_content = null;
                wind_content = (JValue?)pair.Value?[key: "wind"]?[0];
                if (wind_content?.Value != null)
                {
                    amedas_List.Add(new());
                    amedas_List[amedas_List.Count - 1].id = Convert.ToInt32(pair.Key);
                    amedas_List[amedas_List.Count - 1].wind = (double)wind_content;
                }

            }
            var amedas_Ranking = amedas_List.OrderByDescending(x => x.wind).ToList();
            foreach(var content in amedas_Ranking)
            {
                Console.WriteLine(content.id);
                Console.WriteLine(content.wind);
            }
        }
    }

    class JMA_amedas
    {
        public int? id { get; set; }
        public double? pressure { get; set; }
        public double? normalPressure { get; set; }
        public double? temp { get; set; }
        public int? humidity { get; set; }
        public int? visibility { get; set; }
        public int? snow { get; set; }
        public int? weather { get; set; }
        public int? snow1h { get; set; }
        public int? snow6h { get; set; }
        public int? snow12h { get; set; }
        public int? snow24h { get; set; }
        public int? sun10m { get; set; }
        public int? sun1h { get; set; }
        public int? precipitation10m { get; set; }
        public int? precipitation1h { get; set; }
        public int? precipitation3h { get; set; }
        public int? precipitation24h { get; set; }
        public int? windDirection { get; set; }
        public double? wind { get; set; }
    }
}