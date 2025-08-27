namespace practice_api.Models.Shared
{
    public class ValidationResult
    {   
        public Dictionary<string, string[]> Errors { get; set; } = new();
        public bool Error => Errors.Any();
    }
}
