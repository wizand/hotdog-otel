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
            

            using HttpClient client = _httpClientFactory.CreateClient();

            var response = client.GetAsync("https://localhost:5001/Order/GetOrderItems").Result;


            return new List<DtoOrderItem>
            {
                new DtoOrderItem { Name = "Hotdog normal", Price = 9.50f },
                new DtoOrderItem { Name = "Hotdog extra", Price = 13.50f },
                new DtoOrderItem { Name = "Hotdog vege", Price = 10.50f },
                new DtoOrderItem { Name = "Hotdog kids", Price = 7.50f },
                new DtoOrderItem { Name = "Hotdog gluten free", Price = 13.50f }

            };
        }

    }
}
