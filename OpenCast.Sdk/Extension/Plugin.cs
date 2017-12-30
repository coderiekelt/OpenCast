namespace OpenCast.Sdk.Extension
{
    public abstract class Plugin
    {
        public string Name;

        public string Version;

        public string Website;

        public string Author;

        public Plugin()
        {
            this.Name = "Unknown Plugin";
            this.Version = "v0.0.0";
            this.Website = "http://example.com/";
            this.Author = "OpenCast";
        }

        public abstract void Initialize();
    }
}