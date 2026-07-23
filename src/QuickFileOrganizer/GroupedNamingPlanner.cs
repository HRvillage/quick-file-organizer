namespace QuickFileOrganizer;

internal static class GroupedNamingPlanner
{
    public static GroupedNamingPreview Build(IReadOnlyList<FileInfo> files, string folder, GroupedNamingOptions options, bool english)
    {
        var errors = new List<string>();
        var sorted = SortFiles(files, options.SortMode);
        if (sorted.Count == 0) errors.Add(english ? "No matching files were found." : "目前沒有符合條件的檔案。");
        if (options.GroupSize < 2) errors.Add(english ? "Group size must be at least 2." : "每組檔案數量至少需要 2。");
        if (sorted.Count > 0 && options.GroupSize > 0 && sorted.Count % options.GroupSize != 0)
        {
            int remainder = sorted.Count % options.GroupSize;
            errors.Add(english
                ? $"{sorted.Count} files cannot be divided into groups of {options.GroupSize}. {remainder} file(s) would be left over."
                : $"{sorted.Count} 個檔案無法整除每組 {options.GroupSize} 個，會剩下 {remainder} 個檔案。");
        }

        var items = new List<GroupedRenameItem>();
        int warningCount = 0;
        if (errors.Count == 0)
        {
            for (int i = 0; i < sorted.Count; i++)
            {
                int groupIndex = i / options.GroupSize;
                int itemIndex = i % options.GroupSize;
                var file = sorted[i];
                var previous = itemIndex > 0 ? sorted[i - 1] : null;
                int? gap = previous is null ? null : (int)Math.Round((GetSortTime(file, options.SortMode) - GetSortTime(previous, options.SortMode)).TotalMinutes);
                bool hasWarning = gap.HasValue && options.TimeWarningMinutes.HasValue && gap.Value > options.TimeWarningMinutes.Value;
                if (hasWarning) warningCount++;
                string finalName = BuildFileName(file, groupIndex, itemIndex, options);
                items.Add(new GroupedRenameItem
                {
                    SourceFile = file,
                    GroupIndex = groupIndex,
                    ItemIndex = itemIndex,
                    GroupCode = BuildGroupCode(groupIndex, options),
                    FinalFileName = finalName,
                    FinalPath = Path.Combine(folder, finalName),
                    GapFromPreviousMinutes = gap,
                    HasTimeWarning = hasWarning
                });
            }
        }

        if (items.GroupBy(x => x.FinalPath, StringComparer.OrdinalIgnoreCase).Any(g => g.Count() > 1))
            errors.Add(english ? "Duplicate target names were generated." : "命名結果發生重複。");

        return new GroupedNamingPreview { Items = items, Errors = errors, WarningCount = warningCount };
    }

    private static List<FileInfo> SortFiles(IReadOnlyList<FileInfo> files, GroupedNamingSortMode mode)
    {
        return mode switch
        {
            GroupedNamingSortMode.FileName => files.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase).ToList(),
            GroupedNamingSortMode.ModifiedTime => files.OrderBy(x => x.LastWriteTimeUtc).ThenBy(x => x.Name, StringComparer.OrdinalIgnoreCase).ToList(),
            _ => files.OrderBy(x => x.CreationTimeUtc).ThenBy(x => x.Name, StringComparer.OrdinalIgnoreCase).ToList()
        };
    }

    private static DateTime GetSortTime(FileInfo file, GroupedNamingSortMode mode)
    {
        return mode == GroupedNamingSortMode.ModifiedTime ? file.LastWriteTime : file.CreationTime;
    }

    private static string BuildFileName(FileInfo file, int groupIndex, int itemIndex, GroupedNamingOptions options)
    {
        var parts = new List<string>();
        string date = ResolveDate(file, options).ToString("yyyyMMdd");
        bool includeDate = options.DateMode != GroupedNamingDateMode.None;
        if (includeDate && options.DatePosition == GroupedNamingDatePosition.Before) parts.Add(date);
        parts.Add(BuildGroupCode(groupIndex, options));
        if (includeDate && options.DatePosition == GroupedNamingDatePosition.Middle) parts.Add(date);
        parts.Add(BuildItemName(itemIndex, options));
        if (includeDate && options.DatePosition == GroupedNamingDatePosition.After) parts.Add(date);
        return string.Join(options.Separator, parts.Select(SanitizePart).Where(x => !string.IsNullOrWhiteSpace(x))) + file.Extension.ToLowerInvariant();
    }

    private static string BuildGroupCode(int groupIndex, GroupedNamingOptions options)
    {
        return $"{SanitizePart(options.Prefix)}{(options.StartNumber + groupIndex).ToString($"D{options.Digits}")}";
    }

    private static string BuildItemName(int itemIndex, GroupedNamingOptions options)
    {
        if (options.SameName) return $"{options.CommonName}{itemIndex + 1}";
        return itemIndex < options.ItemNames.Count ? options.ItemNames[itemIndex] : $"Item{itemIndex + 1}";
    }

    private static DateTime ResolveDate(FileInfo file, GroupedNamingOptions options)
    {
        return options.DateMode switch
        {
            GroupedNamingDateMode.Manual => options.ManualDate.Date,
            GroupedNamingDateMode.FileTime => file.CreationTime,
            _ => DateTime.Today
        };
    }

    public static string SanitizePart(string value)
    {
        string result = value.Trim();
        foreach (char c in Path.GetInvalidFileNameChars()) result = result.Replace(c, '-');
        while (result.Contains("__", StringComparison.Ordinal)) result = result.Replace("__", "_", StringComparison.Ordinal);
        return result.Trim(' ', '.', '_');
    }
}
