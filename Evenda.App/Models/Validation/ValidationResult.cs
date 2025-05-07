namespace Evenda.App.Models.Validation
{
    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public IDictionary<string, IList<string>> Errors { get; set; } = new Dictionary<string, IList<string>>();

        public void AddError(string key, string error)
        {
            if (Errors.ContainsKey(key))
            {
                Errors[key].Add(error);
            }
            else
            {
                Errors.Add(key, new List<string> { error });
            }
        }
    }
}
