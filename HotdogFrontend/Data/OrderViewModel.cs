namespace HotdogFrontend.Data
{
    public class OrderViewModel
    {
        private string _sessionId;
        private DateTime? _pricesLoaded = null;
        private OrderService _parentService;
        private Dictionary<string, float>? _prices = null;

        public OrderViewModel(string sessionId, OrderService parentService)
        {
            this._sessionId = sessionId;
            this._parentService = parentService;
        }

        public void LoadPrices()
        {
            if (_pricesLoaded == null || _prices == null)
            {
                LoadPricesFromBackend();
                return;
            }
        
            //Get minutes since _pricesLoaded was set
            TimeSpan timeSincePricesLoaded = DateTime.Now - _pricesLoaded.Value;

            //If it has been more than 5 minutes since the prices were loaded, reload them
            if (timeSincePricesLoaded.Minutes > 5)
            {
                LoadPricesFromBackend();
                return;
            }
        }

        private void LoadPricesFromBackend()
        {
            var orderItems = _parentService.FetchOrderItemsFromBackend();

            foreach (var item in orderItems)
            {
                _prices.Add(item.Name, item.Price);
            }

            
        }



    }
}
