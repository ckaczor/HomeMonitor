using System;
using System.IO;
using System.Reflection;

namespace ChrisKaczor.HomeMonitor.Weather.Service
{
    public static class ResourceReader
    {
        public static string GetString(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new Exception($"Resource {resourceName} not found in {assembly.FullName}.  Valid resources are: {string.Join(", ", assembly.GetManifestResourceNames())}.");

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}