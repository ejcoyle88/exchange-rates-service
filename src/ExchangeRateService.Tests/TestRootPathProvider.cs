using System.IO;
using Nancy;

namespace ExchangeRateService.Tests
{
    public class TestRootPathProvider : IRootPathProvider
    {
        private static readonly string RootPath;

        static TestRootPathProvider()
        {
            var directoryName = Path.GetDirectoryName(typeof(Bootstrapper).Assembly.CodeBase);

            if (directoryName != null)
            {
                var assemblyPath = directoryName.Replace(@"file:\", string.Empty);

                RootPath = Path.Combine(assemblyPath, "..", "..", "..", "ExchangeRateService");
            }
        }

        /// <summary>
        /// So we can locate our views during tests.
        /// </summary>
        public string GetRootPath()
        {
            return RootPath;
        }
    }
}
