namespace Trippin_Website.Logic_classes
{
    public class TemporaryLinkResponse
    {
        public string accept = "application/json";
        public string ContentType { get; set; }

        public int maxDownloads { get; set; }
        public bool autoDelete { get; set; }

        //formatul este 1d 
        public string expires { get; set; }
        public string link { get; set; }


    }
}