namespace CCL.Domain.Entities;

public class UserDocument : BaseEntity
{
    public int Index { get; set; }

    public string FileName { get; set; } = default!;

    public string FullPath { get; set; } = default!;

    public bool IsEncrypted { get; set; }
}
