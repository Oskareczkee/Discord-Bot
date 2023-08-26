namespace Web_UI.Models
{
    /// <summary>
    /// Redirection Data is class for sending data to views, which will use redirection <br />
    /// You can put any Data in and and then cast it to whatever type you wish
    /// </summary>
    public class RedirectionData
    {
        /*
         Do not change names here, simpler names like: Action, Controller, RedirectAction, RedirectController
         somehow gives undefined behaviour idk why, but changing names to these makes everything working 
        */
        public string RedirectionAction { get; set; } = string.Empty;
        public string RedirectionController { get; set; } = string.Empty;
        public object? Data { get; set; } = null; //additional data if needed
    }
}
