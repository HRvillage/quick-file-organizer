namespace QuickFileOrganizer;

internal enum GroupedNamingSortMode
{
    TakenTime,
    ModifiedTime,
    FileName
}

internal enum GroupedNamingDateMode
{
    None,
    Today,
    Manual,
    FileTime
}

internal enum GroupedNamingDatePosition
{
    Before,
    Middle,
    After
}

internal sealed class GroupedNamingOptions
{
    public int GroupSize { get; set; } = 3;
    public GroupedNamingSortMode SortMode { get; set; } = GroupedNamingSortMode.FileName;
    public string Prefix { get; set; } = "A";
    public int StartNumber { get; set; } = 1;
    public int Digits { get; set; } = 3;
    public string Separator { get; set; } = "_";
    public bool SameName { get; set; }
    public string CommonName { get; set; } = "工地現場";
    public List<string> ItemNames { get; set; } = ["施工前", "施工中", "施工後"];
    public GroupedNamingDateMode DateMode { get; set; } = GroupedNamingDateMode.Today;
    public DateTime ManualDate { get; set; } = DateTime.Today;
    public GroupedNamingDatePosition DatePosition { get; set; } = GroupedNamingDatePosition.Middle;
    public int? TimeWarningMinutes { get; set; } = 5;
}

internal sealed class GroupedRenameItem
{
    public FileInfo SourceFile { get; init; } = null!;
    public int GroupIndex { get; init; }
    public int ItemIndex { get; init; }
    public string GroupCode { get; init; } = string.Empty;
    public string FinalFileName { get; init; } = string.Empty;
    public string FinalPath { get; init; } = string.Empty;
    public int? GapFromPreviousMinutes { get; init; }
    public bool HasTimeWarning { get; init; }
}

internal sealed class GroupedNamingPreview
{
    public List<GroupedRenameItem> Items { get; init; } = [];
    public List<string> Errors { get; init; } = [];
    public int WarningCount { get; init; }
    public bool CanRename => Errors.Count == 0 && Items.Count > 0;
}
