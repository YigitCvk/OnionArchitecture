namespace OA.Domain.Settings
{
    public class Utils
    {
        private static OaSettings _settings;
        public static OaSettings Settings { get { return _settings; } }
        public static void SetBvysSettings(OaSettings oaSettings) { _settings = oaSettings; }
    }
}
