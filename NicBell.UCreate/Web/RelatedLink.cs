using Newtonsoft.Json;

namespace NicBell.UCreate.Web
{
    public class RelatedLink
    {
        public string Caption { get; set; }
        public string Link { get; set; }
        public bool NewWindow { get; set; }
        public bool Edit { get; set; }
        public bool IsInternal { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
    }
}
