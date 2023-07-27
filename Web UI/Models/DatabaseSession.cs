using System.Reflection.Metadata.Ecma335;
using Web_UI.Models.Filters;

namespace Web_UI.Models
{
    public class DatabaseSession
    {
        private const string ErrorsKey = "Errors";
        private const string ItemFiltersKey = "ItemFilters";
        private const string MobFiltersKey = "MobFilters";

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
        public bool ContainsErrors()
        {
            var errors = Session.GetObject<List<Error>>(ErrorsKey) ?? new();
            return errors.Count==0 ? false :  true;
        }


        public void SetItemFilters(ItemFilters filter) => Session.SetObject(ItemFiltersKey, filter);
        public ItemFilters GetItemFilters() => Session.GetObject<ItemFilters>(ItemFiltersKey) ?? new();
        public void SetMobFilters(MobFilters filter) => Session.SetObject(MobFiltersKey, filter);
        public MobFilters GetMobFilters() => Session.GetObject<MobFilters>(MobFiltersKey) ?? new();
    }
}
