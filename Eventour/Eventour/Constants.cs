namespace Eventour
{
    public static class Constants
    {
        public static string AppName = "Eventour";

        // OAuth
        // For Google login, configure at https://console.developers.google.com/
        public static string iOSClientId = "496912193126-76h0vv8sb3fhuolqqbhedmms9kkdji45.apps.googleusercontent.com";
        public static string AndroidClientId = "496912193126-e6dqpatgs9b5b9gtf7mcj3pn163lh1sa.apps.googleusercontent.com";

        // These values do not need changing
        public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
        public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

        // Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
        public static string iOSRedirectUrl = "com.googleusercontent.apps.496912193126-76h0vv8sb3fhuolqqbhedmms9kkdji45:/oauth2redirect";
        public static string AndroidRedirectUrl = "com.googleusercontent.apps.496912193126-e6dqpatgs9b5b9gtf7mcj3pn163lh1sa:/oauth2redirect";
    }
}
