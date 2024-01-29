using System.Diagnostics;

namespace OtelCommon;

    public static class TagNames
    {
        public const string UserLoggedIn = "user.logged_in";
        public const string UserLockedOut = "user.logged_out";
        public const string UserName = "user.name";
        public const string CurrentOrderCount = "orders.current.count";
        public const string OrderClientName = "order.client_name";

        //Login and logout
        public const string Login = "login";
        public const string Logout = "logout";
        public const string LoginSuccesful = "login.successful";

        //Register user
        public const string RegisterUser = "register";
        public const string RegisterUserSuccessful = "register.successful";
    }

public static class ServiceNames
    {
        public const string HotDogService = "hotdog-service";
        public const string HotDogFrontend = "hotdog-frontend";
        public const string HotDogBackend = "hotdog-backend";
    }

    public static class TraceActivities
    {
    public static ActivitySource? Source = null;
    }

