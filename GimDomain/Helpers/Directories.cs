    using System.IO;

namespace Gim.Domain.Helpers
{
    public static class Directories
    {

        public static readonly string UserHome
            = EnvVar.ByName("USERPROFILE");

        public static readonly string Desktop
            = Path.Combine(UserHome, "Desktop");

        public static readonly string LocalAppData
            = EnvVar.ByName("LOCALAPPDATA");
    }
}