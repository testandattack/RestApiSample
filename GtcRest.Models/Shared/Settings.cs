namespace GtcRest.Models.Shared
{
    public class Settings
    {
        public SQL SQL { get; set; }

        public Settings() { }
    }

    public class SQL
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string SqlConn_User { get; set; }
        public string SqlConn_Admin { get; set; }
    }
}
