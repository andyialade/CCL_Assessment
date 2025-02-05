namespace CCL.UI.Models
{
    public class UserDocument
    {
        public int Index { get; set; }

        public string FileName { get; set; } = default!;

        public string FullPath { get; set; } = default!;

        public bool IsEncrypted { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
