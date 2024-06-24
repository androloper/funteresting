namespace LanguagePackageAPI.Models
{
    public class FixedTextModel
    {
        public long Id { get; set; }
        public string KeyName { get; set; }
        public string Value { get; set; }
        public string LanguageCode { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
 