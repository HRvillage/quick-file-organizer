namespace QuickFileOrganizer;

internal sealed class GroupedNamingDialog : Form
{
    private readonly bool _english;
    private readonly IReadOnlyList<FileInfo> _files;
    private readonly string _folder;
    private readonly AppSettings _settings;
    private readonly string _fileFilterSummary;
    private readonly NumericUpDown _groupSize = new();
    private readonly ComboBox _sortMode = new();
    private readonly TextBox _prefix = new();
    private readonly NumericUpDown _startNumber = new();
    private readonly ComboBox _digits = new();
    private readonly ComboBox _separator = new();
    private readonly RadioButton _sameName = new();
    private readonly RadioButton _differentNames = new();
    private readonly TextBox _commonName = new();
    private readonly TableLayoutPanel _itemNames = new();
    private readonly CheckBox _dateEnabled = new();
    private readonly TableLayoutPanel _dateOptions = new();
    private readonly ComboBox _dateMode = new();
    private readonly DateTimePicker _manualDate = new();
    private readonly ComboBox _datePosition = new();
    private readonly ComboBox _timeWarning = new();
    private readonly Label _summary = new();
    private readonly Label _warning = new();
    private readonly DataGridView _previewGrid = new();
    private readonly ModernButton _okButton = new();
    private readonly ModernButton _cancelButton = new();
    private readonly List<TextBox> _itemNameBoxes = [];

    public GroupedNamingOptions Options { get; private set; } = new();
    public GroupedNamingPreview Preview { get; private set; } = new();

    public GroupedNamingDialog(IReadOnlyList<FileInfo> files, string folder, AppSettings settings, bool english, string fileFilterSummary)
    {
        _files = files;
        _folder = folder;
        _settings = settings;
        _english = english;
        _fileFilterSummary = fileFilterSummary;
        Text = T("分組命名助手", "Grouped naming assistant");
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.Sizable;
        MinimizeBox = false;
        MaximizeBox = true;
        AutoScaleMode = AutoScaleMode.Dpi;
        MinimumSize = new Size(900, 620);
        ClientSize = new Size(1060, 700);
        Font = new Font("Segoe UI", 9F);
        BackColor = Color.FromArgb(246, 248, 251);
        ForeColor = Color.FromArgb(30, 41, 59);
        BuildUi();
        LoadSettings();
        UpdateItemNameRows();
        UpdatePreview();
    }

    private void BuildUi()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(16, 14, 16, 12) };
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Controls.Add(root);

        var body = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 420));
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.Controls.Add(body, 0, 0);

        var settingsPanel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = BackColor, Padding = new Padding(0, 0, 10, 0) };
        var settingsStack = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 1, RowCount = 4 };
        settingsStack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        settingsPanel.Controls.Add(settingsStack);
        body.Controls.Add(settingsPanel, 0, 0);
        body.Controls.Add(BuildPreviewPanel(), 1, 0);

        settingsStack.Controls.Add(BuildGroupSection(), 0, 0);
        settingsStack.Controls.Add(BuildNamesSection(), 0, 1);
        settingsStack.Controls.Add(BuildDateSection(), 0, 2);
        settingsStack.Controls.Add(BuildHelpSection(), 0, 3);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, Padding = new Padding(0, 10, 0, 0) };
        _okButton.Text = T("確認後重新命名", "Review and rename");
        _cancelButton.Text = T("取消", "Cancel");
        _okButton.ApplyPrimary();
        _cancelButton.ApplySecondary();
        SizeButton(_okButton, 150);
        SizeButton(_cancelButton, 90);
        _okButton.Click += (_, _) => Confirm();
        _cancelButton.Click += (_, _) => DialogResult = DialogResult.Cancel;
        actions.Controls.Add(_okButton);
        actions.Controls.Add(_cancelButton);
        root.Controls.Add(actions, 0, 1);
    }

    private Control BuildGroupSection()
    {
        var section = CreateSection(T("1  設定分組", "1  Group setup"));
        var grid = CreateTwoColumnGrid();
        AddField(grid, T("每組檔案數量", "Group size"), _groupSize, 0, 0);
        AddField(grid, T("排序依據", "Sort by"), _sortMode, 1, 0);
        AddField(grid, T("主體前綴", "Prefix"), _prefix, 0, 1);
        AddField(grid, T("起始編號", "Start no."), _startNumber, 1, 1);
        AddField(grid, T("編號位數", "Digits"), _digits, 0, 2);
        AddField(grid, T("分隔符", "Separator"), _separator, 1, 2);
        section.Controls.Add(grid, 0, 1);
        _groupSize.Minimum = 2;
        _groupSize.Maximum = 8;
        _startNumber.Minimum = 1;
        _startNumber.Maximum = 999999;
        SetupCombo(_sortMode, [T("\u5efa\u7acb\u6642\u9593", "Created time"), T("\u4fee\u6539\u6642\u9593", "Modified time"), T("\u6a94\u6848\u540d\u7a31", "File name")]);
        SetupCombo(_digits, ["2", "3", "4"]);
        SetupCombo(_separator, ["_", "-"]);
        WirePreviewUpdates(_groupSize, _sortMode, _prefix, _startNumber, _digits, _separator);
        return section;
    }

    private Control BuildNamesSection()
    {
        var section = CreateSection(T("2  設定組內名稱", "2  Item names"));
        var modeRow = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, WrapContents = true, Margin = new Padding(0, 2, 0, 8) };
        _sameName.Text = T("每張使用相同名稱", "Same name");
        _differentNames.Text = T("每張使用不同名稱", "Different names");
        _sameName.AutoSize = true;
        _differentNames.AutoSize = true;
        _sameName.Margin = new Padding(0, 0, 18, 4);
        _differentNames.Margin = new Padding(0, 0, 0, 4);
        modeRow.Controls.Add(_sameName);
        modeRow.Controls.Add(_differentNames);
        section.Controls.Add(modeRow, 0, 1);

        var commonGrid = CreateOneColumnGrid();
        AddField(commonGrid, T("共同名稱", "Common name"), _commonName, 0, 0);
        section.Controls.Add(commonGrid, 0, 2);

        _itemNames.Dock = DockStyle.Top;
        _itemNames.AutoSize = true;
        _itemNames.ColumnCount = 2;
        _itemNames.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
        _itemNames.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        section.Controls.Add(_itemNames, 0, 3);

        _sameName.CheckedChanged += (_, _) => { UpdateItemNameRows(); UpdatePreview(); };
        _differentNames.CheckedChanged += (_, _) => { UpdateItemNameRows(); UpdatePreview(); };
        _commonName.TextChanged += (_, _) => UpdatePreview();
        return section;
    }

    private Control BuildDateSection()
    {
        var section = CreateSection(T("3  日期與檢查提醒", "3  Date and checks"));
        _dateEnabled.Text = T("加入日期標記", "Add date");
        _dateEnabled.AutoSize = true;
        _dateEnabled.Margin = new Padding(0, 2, 0, 8);
        section.Controls.Add(_dateEnabled, 0, 1);

        _dateOptions.Dock = DockStyle.Top;
        _dateOptions.AutoSize = true;
        _dateOptions.ColumnCount = 2;
        _dateOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        _dateOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        AddField(_dateOptions, T("日期來源", "Date source"), _dateMode, 0, 0);
        AddField(_dateOptions, T("日期位置", "Date position"), _datePosition, 1, 0);
        AddField(_dateOptions, T("手動日期", "Manual date"), _manualDate, 0, 1);
        _dateOptions.SetColumnSpan(_manualDate, 2);
        section.Controls.Add(_dateOptions, 0, 2);

        var warningGrid = CreateOneColumnGrid();
        AddField(warningGrid, T("時間間隔提醒", "Time gap warning"), _timeWarning, 0, 0);
        var hint = new Label { Dock = DockStyle.Top, AutoSize = true, ForeColor = Color.FromArgb(100, 116, 139), Text = T("若同一組內相鄰檔案時間差太大，會提醒可能漏拍、排序錯誤或混入無關檔案。", "Warns when nearby files in a group are far apart in time, which may mean missing files, wrong sorting, or unrelated files.") };
        warningGrid.Controls.Add(hint, 0, 2);
        section.Controls.Add(warningGrid, 0, 3);

        SetupCombo(_dateMode, [T("今日日期", "Today"), T("手動日期", "Manual"), T("依檔案時間", "File time")]);
        SetupCombo(_datePosition, [T("前方", "Before"), T("中間", "Middle"), T("後方", "After")]);
        SetupCombo(_timeWarning, [T("檔案間隔超過 3 分鐘", "File gap over 3 min"), T("檔案間隔超過 5 分鐘", "File gap over 5 min"), T("檔案間隔超過 10 分鐘", "File gap over 10 min"), T("不提醒", "Off")]);
        _manualDate.Format = DateTimePickerFormat.Custom;
        _manualDate.CustomFormat = "yyyy/MM/dd";
        _dateEnabled.CheckedChanged += (_, _) => { UpdateDateControls(); UpdatePreview(); };
        _dateMode.SelectedIndexChanged += (_, _) => { UpdateDateControls(); UpdatePreview(); };
        _manualDate.ValueChanged += (_, _) => UpdatePreview();
        _datePosition.SelectedIndexChanged += (_, _) => UpdatePreview();
        _timeWarning.SelectedIndexChanged += (_, _) => UpdatePreview();
        return section;
    }

    private Control BuildHelpSection()
    {
        var section = CreateSection(T("4  檢查後執行", "4  Review before rename"));
        var text = new Label
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ForeColor = Color.FromArgb(100, 116, 139),
            Text = T("若有時間異常，請回到資料夾移除無關檔案、確認排序，或調整提醒門檻。", "If a time warning appears, remove unrelated files, check sorting, or adjust the warning limit.")
        };
        section.Controls.Add(text, 0, 1);
        return section;
    }

    private Control BuildPreviewPanel()
    {
        var panel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(12, 0, 0, 0) };
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _summary.Dock = DockStyle.Top;
        _summary.AutoSize = true;
        _summary.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        _summary.Margin = new Padding(0, 0, 0, 8);
        _warning.Dock = DockStyle.Top;
        _warning.AutoSize = true;
        _warning.ForeColor = Color.FromArgb(154, 52, 18);
        _warning.Margin = new Padding(0, 0, 0, 8);
        _previewGrid.Dock = DockStyle.Fill;
        _previewGrid.AllowUserToAddRows = false;
        _previewGrid.AllowUserToDeleteRows = false;
        _previewGrid.AllowUserToResizeRows = false;
        _previewGrid.ReadOnly = true;
        _previewGrid.RowHeadersVisible = false;
        _previewGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _previewGrid.BackgroundColor = Color.White;
        _previewGrid.BorderStyle = BorderStyle.FixedSingle;
        _previewGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _previewGrid.Columns.Add("group", T("組別", "Group"));
        _previewGrid.Columns.Add("source", T("原檔名", "Source"));
        _previewGrid.Columns.Add("target", T("新檔名", "Target"));
        _previewGrid.Columns.Add("status", T("狀態", "Status"));
        _previewGrid.Columns[0].FillWeight = 16;
        _previewGrid.Columns[1].FillWeight = 31;
        _previewGrid.Columns[2].FillWeight = 37;
        _previewGrid.Columns[3].FillWeight = 26;
        panel.Controls.Add(_summary, 0, 0);
        panel.Controls.Add(_warning, 0, 1);
        panel.Controls.Add(_previewGrid, 0, 2);
        return panel;
    }

    private TableLayoutPanel CreateSection(string title)
    {
        var section = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 1, Padding = new Padding(0, 0, 0, 12), Margin = new Padding(0, 0, 0, 10) };
        section.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        var label = new Label { AutoSize = true, Dock = DockStyle.Top, Text = title, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Margin = new Padding(0, 0, 0, 8) };
        section.Controls.Add(label, 0, 0);
        return section;
    }

    private static TableLayoutPanel CreateTwoColumnGrid()
    {
        var grid = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Margin = new Padding(0) };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        return grid;
    }

    private static TableLayoutPanel CreateOneColumnGrid()
    {
        var grid = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 1, Margin = new Padding(0) };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        return grid;
    }

    private static void AddField(TableLayoutPanel grid, string labelText, Control control, int column, int row)
    {
        while (grid.RowStyles.Count <= row * 2 + 1)
        {
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }
        var label = new Label { Text = labelText, AutoSize = true, Dock = DockStyle.Top, ForeColor = Color.FromArgb(100, 116, 139), Margin = new Padding(0, 0, 8, 2) };
        control.Dock = DockStyle.Fill;
        control.Margin = new Padding(0, 0, 8, 8);
        grid.Controls.Add(label, column, row * 2);
        grid.Controls.Add(control, column, row * 2 + 1);
    }

    private static void SetupCombo(ComboBox combo, string[] items)
    {
        combo.DropDownStyle = ComboBoxStyle.DropDownList;
        combo.Items.AddRange(items);
        combo.SelectedIndex = 0;
    }

    private static void WirePreviewUpdates(params Control[] controls)
    {
        foreach (var control in controls)
        {
            if (control is TextBox textBox) textBox.TextChanged += (_, _) => ((GroupedNamingDialog)textBox.FindForm()!).UpdatePreview();
            else if (control is NumericUpDown number) number.ValueChanged += (_, _) => ((GroupedNamingDialog)number.FindForm()!).UpdatePreview();
            else if (control is ComboBox combo) combo.SelectedIndexChanged += (_, _) => ((GroupedNamingDialog)combo.FindForm()!).UpdatePreview();
        }
    }

    private void LoadSettings()
    {
        _groupSize.Value = Math.Clamp(_settings.GroupedGroupSize, 2, 8);
        _sortMode.SelectedIndex = Math.Clamp(_settings.GroupedSortMode, 0, 2);
        _prefix.Text = string.IsNullOrWhiteSpace(_settings.GroupedPrefix) ? "A" : _settings.GroupedPrefix;
        _startNumber.Value = Math.Clamp(_settings.GroupedStartNumber, 1, 999999);
        _digits.SelectedIndex = Math.Clamp(_settings.GroupedDigits - 2, 0, 2);
        _separator.SelectedIndex = _settings.GroupedSeparator == "-" ? 1 : 0;
        _sameName.Checked = _settings.GroupedSameName;
        _differentNames.Checked = !_settings.GroupedSameName;
        _commonName.Text = string.IsNullOrWhiteSpace(_settings.GroupedCommonName) ? T("工地現場", "Site") : _settings.GroupedCommonName;
        _dateEnabled.Checked = _settings.GroupedDateMode != "none";
        _dateMode.SelectedIndex = _settings.GroupedDateMode == "manual" ? 1 : _settings.GroupedDateMode == "file" ? 2 : 0;
        if (DateTime.TryParse(_settings.GroupedManualDate, out var manualDate)) _manualDate.Value = manualDate;
        _datePosition.SelectedIndex = _settings.GroupedDatePosition == "before" ? 0 : _settings.GroupedDatePosition == "after" ? 2 : 1;
        _timeWarning.SelectedIndex = _settings.GroupedTimeWarningMinutes switch { 3 => 0, 10 => 2, 0 => 3, _ => 1 };
        UpdateDateControls();
    }

    private void SaveSettings()
    {
        _settings.GroupedGroupSize = (int)_groupSize.Value;
        _settings.GroupedSortMode = _sortMode.SelectedIndex;
        _settings.GroupedPrefix = _prefix.Text.Trim();
        _settings.GroupedStartNumber = (int)_startNumber.Value;
        _settings.GroupedDigits = int.Parse((string)_digits.SelectedItem!);
        _settings.GroupedSeparator = (string)_separator.SelectedItem!;
        _settings.GroupedSameName = _sameName.Checked;
        _settings.GroupedCommonName = _commonName.Text.Trim();
        _settings.GroupedItemNames = _itemNameBoxes.Select(x => x.Text.Trim()).ToList();
        _settings.GroupedDateMode = _dateEnabled.Checked ? DateModeValue() : "none";
        _settings.GroupedManualDate = _manualDate.Value.ToString("yyyy-MM-dd");
        _settings.GroupedDatePosition = DatePositionValue();
        _settings.GroupedTimeWarningMinutes = TimeWarningMinutes() ?? 0;
    }

    private GroupedNamingOptions CurrentOptions()
    {
        return new GroupedNamingOptions
        {
            GroupSize = (int)_groupSize.Value,
            SortMode = (GroupedNamingSortMode)Math.Clamp(_sortMode.SelectedIndex, 0, 2),
            Prefix = _prefix.Text.Trim(),
            StartNumber = (int)_startNumber.Value,
            Digits = int.Parse((string)_digits.SelectedItem!),
            Separator = (string)_separator.SelectedItem!,
            SameName = _sameName.Checked,
            CommonName = _commonName.Text.Trim(),
            ItemNames = _itemNameBoxes.Select(x => x.Text.Trim()).ToList(),
            DateMode = _dateEnabled.Checked ? (GroupedNamingDateMode)Math.Clamp(_dateMode.SelectedIndex + 1, 1, 3) : GroupedNamingDateMode.None,
            ManualDate = _manualDate.Value.Date,
            DatePosition = (GroupedNamingDatePosition)Math.Clamp(_datePosition.SelectedIndex, 0, 2),
            TimeWarningMinutes = TimeWarningMinutes()
        };
    }

    private void UpdateItemNameRows()
    {
        _itemNames.Visible = _differentNames.Checked;
        _commonName.Parent!.Visible = _sameName.Checked;
        var current = _itemNameBoxes.Select(x => x.Text).ToList();
        _itemNames.Controls.Clear();
        _itemNames.RowStyles.Clear();
        _itemNameBoxes.Clear();
        int count = (int)_groupSize.Value;
        var saved = current.Count > 0 ? current : _settings.GroupedItemNames.Count > 0 ? _settings.GroupedItemNames : DefaultItemNames();
        for (int i = 0; i < count; i++)
        {
            _itemNames.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var label = new Label { Text = T($"第 {i + 1} 張", $"Item {i + 1}"), Dock = DockStyle.Fill, AutoSize = true, Margin = new Padding(0, 6, 8, 6) };
            var text = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 8, 6), Text = i < saved.Count ? saved[i] : T($"項目{i + 1}", $"Item{i + 1}") };
            text.TextChanged += (_, _) => UpdatePreview();
            _itemNameBoxes.Add(text);
            _itemNames.Controls.Add(label, 0, i);
            _itemNames.Controls.Add(text, 1, i);
        }
    }

    private List<string> DefaultItemNames() => _english ? ["Before", "During", "After"] : ["施工前", "施工中", "施工後"];

    private void UpdateDateControls()
    {
        _dateOptions.Visible = _dateEnabled.Checked;
        _manualDate.Visible = _dateEnabled.Checked && _dateMode.SelectedIndex == 1;
    }

    private void UpdatePreview()
    {
        UpdateDateControls();
        Options = CurrentOptions();
        Preview = GroupedNamingPlanner.Build(_files, _folder, Options, _english);
        _previewGrid.Rows.Clear();
        foreach (var item in Preview.Items)
        {
            string status = item.HasTimeWarning
                ? T($"請確認：與上一個檔案相隔 {item.GapFromPreviousMinutes} 分鐘", $"Check: {item.GapFromPreviousMinutes} min gap from previous file")
                : T("正常", "OK");
            int row = _previewGrid.Rows.Add(item.GroupCode, item.SourceFile.Name, item.FinalFileName, status);
            if (item.HasTimeWarning) _previewGrid.Rows[row].DefaultCellStyle.ForeColor = Color.FromArgb(154, 52, 18);
        }
        string errorText = string.Join(Environment.NewLine, Preview.Errors);
        _summary.Text = Preview.CanRename
            ? $"{_fileFilterSummary}{Environment.NewLine}{T($"\u5171 {Preview.Items.Count} \u500b\u6a94\u6848\uff0c{Preview.WarningCount} \u500b\u63d0\u9192\u3002", $"{Preview.Items.Count} files, {Preview.WarningCount} warning(s).")}"
            : $"{_fileFilterSummary}{Environment.NewLine}{T("\u5c1a\u672a\u6e96\u5099\u597d\u91cd\u65b0\u547d\u540d\u3002", "Rename is not ready.")}";
        _warning.Text = Preview.Errors.Count > 0 ? errorText : Preview.WarningCount > 0
            ? T("請先確認時間間隔提醒列，再決定是否繼續。", "Review the time gap warnings before continuing.")
            : string.Empty;
        _okButton.Enabled = Preview.CanRename;
    }

    private void Confirm()
    {
        UpdatePreview();
        if (!Preview.CanRename) return;
        SaveSettings();
        DialogResult = DialogResult.OK;
    }

    private string DateModeValue() => _dateMode.SelectedIndex == 1 ? "manual" : _dateMode.SelectedIndex == 2 ? "file" : "today";
    private string DatePositionValue() => _datePosition.SelectedIndex == 0 ? "before" : _datePosition.SelectedIndex == 2 ? "after" : "middle";
    private int? TimeWarningMinutes() => _timeWarning.SelectedIndex switch { 0 => 3, 2 => 10, 3 => null, _ => 5 };
    private string T(string zh, string en) => _english ? en : zh;

    private static void SizeButton(ModernButton button, int minimumWidth)
    {
        var measured = TextRenderer.MeasureText(button.Text, button.Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.SingleLine);
        int width = Math.Max(minimumWidth, measured.Width + button.Padding.Horizontal + 24);
        button.AutoSize = false;
        button.MinimumSize = Size.Empty;
        button.Size = new Size(width, 36);
        button.MinimumSize = button.Size;
    }
}
