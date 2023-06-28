namespace Web_UI.Models
{
    public class Error
    {
        public string Description { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
    }
}
