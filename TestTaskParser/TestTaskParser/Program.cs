using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TestTaskParser.DB;

namespace TestTaskParser
{
    internal class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static int currentTotal { get; set; }
        private static async Task<string> PostHttpRequestAsync( int size = 10000, int number = 0, string url = "https://www.lesegais.ru/open-area/graphql", bool fordate = false)
        {
            string str;
            if (fordate) //fordate - тру когда запрашиваем тотал, и фолс в обратных, соответсвенно и строки соотв.
                str = "{\"query\":\"query SearchReportWoodDealCount($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {\\n  searchReportWoodDeal(filter: $filter, pageable: {number: $number, size: $size}, orders: $orders) {\\n    total\\n    number\\n    size\\n    overallBuyerVolume\\n    overallSellerVolume\\n    __typename\\n  }\\n}\\n\",\"variables\":{\"size\":20,\"number\":0,\"filter\":null},\"operationName\":\"SearchReportWoodDealCount\"}";
           
            else str =   $"{{\"query\":\"query SearchReportWoodDeal($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {{\\n  searchReportWoodDeal(filter: $filter, pageable: {{number: $number, size: $size}}, orders: $orders) {{\\n    content {{\\n      sellerName\\n      sellerInn\\n      buyerName\\n      buyerInn\\n      woodVolumeBuyer\\n      woodVolumeSeller\\n      dealDate\\n      dealNumber\\n      __typename\\n    }}\\n    __typename\\n  }}\\n}}\\n\",\"variables\":{{\"size\":{size},\"number\":{number},\"filter\":null,\"orders\":[{{\"property\":\"dealDate\",\"direction\":\"DESC\"}}]}},\"operationName\":\"SearchReportWoodDeal\"}}";   
            var httpContent = new StringContent(str, Encoding.UTF8, "application/json");
            try
            {
                using (var response = await client.PostAsync(url, httpContent).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                return "{}";
            }
        }
        static void Main(string[] args)
        {
            var result = PostHttpRequestAsync(fordate : true).Result; //запрос количества объектов таблицы
            var total = Int32.Parse(result.Substring(result.IndexOf("\"total\":") + 8,result.IndexOf("\"number\"") - result.IndexOf("\"total\":") - 9));//поиск поля total
            FirstParse(total);
            currentTotal = total;
            while (true)
            {
                Thread.Sleep(600000); //Сон на 10 минут
                result = PostHttpRequestAsync(fordate: true).Result;
                total = Int32.Parse(result.Substring(result.IndexOf("\"total\":") + 8, result.IndexOf("\"number\"") - result.IndexOf("\"total\":") - 9));
                if (currentTotal < total)
                {
                    Parse(total - currentTotal);
                    currentTotal = total;
                }

            }
        }
        public static async Task FirstParse(int total) //решил сделать скачивание по 10 000 объектов чтобы пока следующие 10000 скачиваются -
        {                                              //предыдущие десериализуются и сохраняются в бд, да и с оперативной памятью могут быть проблемы
            using (var db = new MyContext())           //при скачивании всей БД
            {
                int i = 0; //страница или же номер 10-ти тысяч данных (второй десяток и т.д.)
                while (total > 0) 
                {
                    var result = await PostHttpRequestAsync(10000, i++);
                    total -= 10000;
                    var reports = JsonSerializer.Deserialize<Basa>(result);
                    if (reports.Data != null)
                        db.Reports.AddRange(reports.Data.SearchReportWoodDeal.Content);
                    await db.SaveChangesAsync();
                }
            }
        }
        public static async Task Parse(int total)
        {
            using (var db = new MyContext())
            {
                var result = await PostHttpRequestAsync(total);
                var reports = JsonSerializer.Deserialize<Basa>(result);
                if (reports.Data != null)
                    db.Reports.AddRange(reports.Data.SearchReportWoodDeal.Content);
                await db.SaveChangesAsync();
            }
        }
    } 
}
