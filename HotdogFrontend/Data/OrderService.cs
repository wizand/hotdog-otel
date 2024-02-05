using OtelCommon;

namespace HotdogFrontend.Data
{
    public class OrderService
    {

        private OrderViewModel? _orderViewModel = null;
        private string SessionId = string.Empty;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderService(IHttpClientFactory httpClientFactor)
        {
            _httpClientFactory = httpClientFactor;
            SessionId = Guid.NewGuid().ToString();
            _orderViewModel = new OrderViewModel(SessionId, this);
        }

        public List<DtoOrderItem> FetchOrderItemsFromBackend()
        {
            
            using var act = TraceActivities.Source.StartActivity(EventNames.Prices);
            using HttpClient client = _httpClientFactory.CreateClient();

            var response = client.GetAsync("https://localhost:5001/Order/GetPriceTable").Result;

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
