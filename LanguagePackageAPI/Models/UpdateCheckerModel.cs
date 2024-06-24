namespace LanguagePackageAPI.Models
{
    public class UpdateCheckerModel
    {
        public int Id { get; set; }
        public string Name  { get; set; }
        public string Version { get; set; }
        public DateTime LatestUpdate { get; set; }
    }
}
