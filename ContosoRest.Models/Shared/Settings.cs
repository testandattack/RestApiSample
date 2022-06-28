using Microsoft.Extensions.Options;
using Moq;

namespace ContosoRest.Models.Shared
{
    public class Settings
    {
        public SQL SQL { get; set; }
        public Core Core { get; set; }
        public BrickLinkApiSettings brickLinkApiSettings { get; set; }

        public Settings() { }

        public static IOptionsSnapshot<Settings> CreateIOptionSnapshotMock(Settings settings)
        {
            var mock = new Mock<IOptionsSnapshot<Settings>>();
            mock.Setup(m => m.Value).Returns(settings);
            return mock.Object;
        }
    }

    public class Core
    {
        public string SwaggerUrl { get; set; }

        public bool UseBasicAuth { get; set; }
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

    public class BrickLinkApiSettings
    {
        public string ConsumerKey { get; set; }

        public string ConsumerSecret { get; set; }

        public string TokenValue { get; set; }

        public string TokenSecret { get; set; }

    }
}
