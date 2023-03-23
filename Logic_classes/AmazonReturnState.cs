namespace Trippin_Website.Logic_classes
{
    public class AmazonReturnState
    {
        public string success { get; set; }
        public string error { get; set; }
        public AmazonReturnState()
        {
            success = string.Empty;
            error = string.Empty;
        }
    }
}