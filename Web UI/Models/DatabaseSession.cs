using System.Reflection.Metadata.Ecma335;

namespace Web_UI.Models
{
    public class DatabaseSession
    {
        private const string ErrorsKey = "Errors";

        private ISession Session { get; set; }
        public DatabaseSession(ISession session) => Session = session;
        private List<Error> Errors = new();

        public List<Error> GetErrors() => Session.GetObject<List<Error>>(ErrorsKey) ?? new();
        public void AddError(Error err)
        {
            Errors = Session.GetObject<List<Error>>(ErrorsKey) ?? new();
            Errors.Add(err);
            Session.SetObject(ErrorsKey, Errors);
        }

        public void SetErrors(List<Error> errors) => Session.SetObject(ErrorsKey, errors);
        public void ClearErrors() => Session.SetObject(ErrorsKey, new List<string>());
        public bool ContainErrors()
        {
            var errors = Session.GetObject<List<Error>>(ErrorsKey) ?? new();
            return errors.Count==0 ? false :  true;
        }
    }
}
