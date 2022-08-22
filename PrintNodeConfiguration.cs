using System;

namespace PrintNodeNet
{
    public static class PrintNodeConfiguration
    {
        public static Uri BaseAddress { get; set; } = new Uri("https://api.printnode.com");
        public static string ApiKey { get; set; }
    }
}
