namespace HotdogFrontend.Data
{
    public class OrderViewModel
    {
        private string _sessionId;
        private DateTime? _pricesLoaded = null;
        private OrderService _parentService;

        public OrderViewModel(string sessionId, OrderService parentService)
        {
            this._sessionId = sessionId;
            this._parentService = parentService;
        }

        public void LoadPrices()
        {
            if (_pricesLoaded == null )
            {


                
                //Load the prices from the backend
                _pricesLoaded = DateTime.Now;
            }
        
            //Get minutes since _pricesLoaded was set
            TimeSpan timeSincePricesLoaded = DateTime.Now - _pricesLoaded.Value;

            //If it has been more than 5 minutes since the prices were loaded, reload them
            if (timeSincePricesLoaded.Minutes > 5)
            {
                //Load the prices from the backend
                _pricesLoaded = DateTime.Now;


            }
        }



    }
}
