using System.Net.Http.Headers;

using HotdogFrontend.ManagersAndHandlers;

using OtelCommon;

namespace HotdogFrontend.Data
{
    public class OrderService
    {

        private OrderViewModel? _orderViewModel = null;
        private string SessionId = string.Empty;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JwtTokenHandler _jwtTokenHandler;
        private readonly IConfiguration _conf;

        public OrderService(IHttpClientFactory httpClientFactor, JwtTokenHandler jwtTokenHandler, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactor;
            SessionId = Guid.NewGuid().ToString();
            _jwtTokenHandler = jwtTokenHandler;
            _conf = configuration;
            _orderViewModel = new OrderViewModel(SessionId, this);
        }

        public List<DtoOrderItem> FetchOrderItemsFromBackend()
        {
            
            using var act = TraceActivities.Source.StartActivity(EventNames.Prices);
            using HttpClient client = _httpClientFactory.CreateClient();


            var jwtTokenTask = _jwtTokenHandler.GetJwtToken();
            var jwtToken = jwtTokenTask.GetAwaiter().GetResult();
            if (jwtToken == null)
            {
                act.AddTag(TagNames.PricesSuccesful, false);
                throw new Exception("Failed to fetch order items from backend");
            }


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            string backendUrl = _conf["HotdogService:HotdogBackendUrl"]; // "http://localhost:5255/Order/GetPriceTable";
            var response = client.GetAsync(backendUrl + "/Order/GetPriceTable").Result;

            if ( response.IsSuccessStatusCode )
            {
                var result = response.Content.ReadAsStringAsync().Result;
                act.AddTag(TagNames.PricesSuccesful, true);
                return System.Text.Json.JsonSerializer.Deserialize<List<DtoOrderItem>>(result);
            } else
            {
                act.AddTag(TagNames.PricesSuccesful, false);
                throw new Exception("Failed to fetch order items from backend");
            }

        }

    }
}
