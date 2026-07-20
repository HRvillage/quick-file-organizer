using System.Diagnostics;

namespace QuickFileOrganizer;

internal sealed class MainForm : Form
{
    private const int TargetClientWidth = 1040;
    private const int TargetClientHeight = 680;
    private const int MinimumClientWidth = 860;
    private const int MinimumClientHeight = 560;
    private const int FileTypesCardLogicalHeight = 128;
    private static readonly Padding RootPadding = new(16, 10, 16, 8);

    private readonly AppSettings _settings;
    private bool _english;
    private bool _updatingChecks;
    private bool _applyingLanguage;
    private FormWindowState _lastWindowState = FormWindowState.Normal;
    private readonly TableLayoutPanel _mainLayout = new();
    private readonly Panel _contentPanel = new();
    private readonly TableLayoutPanel _contentLayout = new();
    private readonly Panel _contentSpacer = new();

    private readonly Label _titleLabel = new();
    private readonly Label _subtitleLabel = new();
    private readonly LanguageToggle _languageToggle = new();
    private readonly RoundedPanel _folderCard = new();
    private readonly RoundedPanel _namingCard = new();
    private readonly RoundedPanel _typesCard = new();
    private readonly RoundedPanel _previewCard = new();
    private readonly Label _folderTitle = new();
    private readonly Label _namingTitle = new();
    private readonly Label _typesTitle = new();
    private readonly Label _previewTitle = new();
    private readonly TextBox _folderText = new();
    private readonly Label _dragHint = new();
    private readonly FlowLayoutPanel _folderButtons = new();
    private readonly ModernButton _browseButton = new();
    private readonly ModernButton _folderOpenButton = new();
    private readonly TextBox _name1Text = new();
    private readonly TextBox _name2Text = new();
    private readonly Label _name1Label = new();
    private readonly Label _name2Label = new();
    private readonly Label _dateSourceLabel = new();
    private readonly ComboBox _dateModeCombo = new();
    private readonly DateTimePicker _manualDatePicker = new();
    private readonly ComboBox _fileDateCombo = new();
    private readonly Label _datePositionLabel = new();
    private readonly ComboBox _datePositionCombo = new();
    private readonly Label _startLabel = new();
    private readonly NumericUpDown _startNumber = new();
    private readonly Label _digitsLabel = new();
    private readonly ComboBox _digitsCombo = new();
    private readonly CheckBox _continueCheck = new();
    private readonly Label _continueHint = new();
    private readonly Label _summaryLabel = new();
    private readonly TextBox _previewText = new();
    private readonly ModernButton _refreshButton = new();
    private readonly ModernButton _undoButton = new();
    private readonly ModernButton _renameButton = new();
    private readonly ModernButton _actionOpenButton = new();
    private readonly LinkLabel _copyrightLabel = new();

    private readonly Dictionary<string, CheckBox> _typeChecks = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, CheckBox> _categoryChecks = new(StringComparer.OrdinalIgnoreCase);
    private List<FileInfo> _currentFiles = [];

    private static readonly Color PageBack = Color.FromArgb(246, 248, 251);
    private static readonly Color TextPrimary = Color.FromArgb(30, 41, 59);
    private static readonly Color TextSecondary = Color.FromArgb(100, 116, 139);
    private static readonly Color Accent = Color.FromArgb(47, 128, 237);

    private static readonly Dictionary<string, string[]> FileTypeGroups = new(StringComparer.OrdinalIgnoreCase)
    {
        ["jpg"] = [".jpg", ".jpeg"], ["png"] = [".png"], ["gif"] = [".gif"],
        ["webp"] = [".webp"], ["bmp"] = [".bmp"],
        ["mp4"] = [".mp4"], ["mov"] = [".mov"], ["avi"] = [".avi"],
        ["pdf"] = [".pdf"], ["word"] = [".doc", ".docx"], ["excel"] = [".xls", ".xlsx"],
        ["ppt"] = [".ppt", ".pptx"], ["txt"] = [".txt"],
        ["zip"] = [".zip"], ["rar"] = [".rar"], ["7z"] = [".7z"]
    };

    private static readonly Dictionary<string, string[]> CategoryMembers = new(StringComparer.OrdinalIgnoreCase)
    {
        ["image"] = ["jpg", "png", "gif", "webp", "bmp"],
        ["video"] = ["mp4", "mov", "avi"],
        ["document"] = ["pdf", "word", "excel", "ppt", "txt"],
        ["archive"] = ["zip", "rar", "7z"]
    };

    public MainForm()
    {
        _settings = AppSettings.Load();
        _english = string.Equals(_settings.Language, "en", StringComparison.OrdinalIgnoreCase);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        MinimizeBox = true;
        WindowState = FormWindowState.Normal;
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(TargetClientWidth, TargetClientHeight);
        MinimumSize = SizeFromClientSize(new Size(MinimumClientWidth, MinimumClientHeight));
        Font = new Font("Segoe UI", 9F);
        BackColor = PageBack;
        ForeColor = TextPrimary;
        Icon = LoadWindowIcon();
        AllowDrop = true;
        DoubleBuffered = true;

        BuildUi();
        LoadSettingsIntoUi();
        ApplyLanguage();
        ConfigureInitialWindowSize();
        WireEvents();
        RefreshPreview();
        UpdateFileTypesCardHeight();
    }

    private void BuildUi()
    {
        _mainLayout.Dock = DockStyle.Fill;
        _mainLayout.ColumnCount = 1;
        _mainLayout.RowCount = 3;
        _mainLayout.BackColor = PageBack;
        _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Controls.Add(_mainLayout);

        _contentPanel.Dock = DockStyle.Fill;
        _contentPanel.AutoScroll = true;
        _contentPanel.BackColor = PageBack;
        _mainLayout.Controls.Add(_contentPanel, 0, 0);

        _contentLayout.Dock = DockStyle.Top;
        _contentLayout.AutoSize = false;
        _contentLayout.Padding = RootPadding;
        _contentLayout.ColumnCount = 1;
        _contentLayout.RowCount = 6;
        _contentLayout.BackColor = PageBack;
        _contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _contentPanel.Controls.Add(_contentLayout);

        var header = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 2, Margin = new Padding(0, 0, 0, 8) };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        var titleStack = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Margin = new Padding(0) };
        _titleLabel.AutoSize = true;
        _titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        _titleLabel.ForeColor = TextPrimary;
        _subtitleLabel.AutoSize = true;
        _subtitleLabel.Font = new Font("Segoe UI", 9F);
        _subtitleLabel.ForeColor = TextSecondary;
        titleStack.Controls.Add(_titleLabel);
        titleStack.Controls.Add(_subtitleLabel);
        _languageToggle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _languageToggle.Margin = new Padding(12, 5, 0, 0);
        header.Controls.Add(titleStack, 0, 0);
        header.Controls.Add(_languageToggle, 1, 0);
        _contentLayout.Controls.Add(header, 0, 0);

        BuildFolderSection(); _contentLayout.Controls.Add(_folderCard, 0, 1);
        BuildNamingSection(); _contentLayout.Controls.Add(_namingCard, 0, 2);
        BuildTypesSection(); _contentLayout.Controls.Add(_typesCard, 0, 3);
        BuildPreviewSection(); _contentLayout.Controls.Add(_previewCard, 0, 4);
        _contentSpacer.Dock = DockStyle.Fill;
        _contentSpacer.BackColor = PageBack;
        _contentLayout.Controls.Add(_contentSpacer, 0, 5);
        _mainLayout.Controls.Add(BuildActionSection(), 0, 1);

        _copyrightLabel.AutoSize = true;
        _copyrightLabel.LinkColor = Color.FromArgb(59, 104, 170);
        _copyrightLabel.ActiveLinkColor = Accent;
        _copyrightLabel.VisitedLinkColor = Color.FromArgb(59, 104, 170);
        _copyrightLabel.LinkBehavior = LinkBehavior.HoverUnderline;
        _copyrightLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _copyrightLabel.Margin = new Padding(0, 2, 0, 0);
        _copyrightLabel.Cursor = Cursors.Hand;
        _copyrightLabel.LinkClicked += (_, _) => OpenExternalUrl("https://www.youtube.com/@HeroRaye");
        _mainLayout.Controls.Add(_copyrightLabel, 0, 2);
    }

    private void ConfigureInitialWindowSize()
    {
        ClientSize = new Size(TargetClientWidth, TargetClientHeight);
        WindowState = FormWindowState.Normal;
        CenterToScreen();
        ResizeContentRoot();
    }

    private void ResizeContentRoot()
    {
        int width = Math.Max(0, _contentPanel.ClientSize.Width - _contentPanel.Padding.Horizontal);
        int preferredHeight = _contentLayout.GetPreferredSize(new Size(width, 0)).Height;
        if (preferredHeight > _contentPanel.ClientSize.Height)
            width = Math.Max(0, width - SystemInformation.VerticalScrollBarWidth);
        preferredHeight = _contentLayout.GetPreferredSize(new Size(width, 0)).Height;
        int height = Math.Max(_contentPanel.ClientSize.Height, preferredHeight);
        _contentLayout.Size = new Size(width, height);
        _contentPanel.AutoScrollMinSize = new Size(0, height);
    }

    private void ResizeCommandButtons()
    {
        SizeButtonToText(_browseButton, 98);
        SizeButtonToText(_folderOpenButton, 82);
        _folderButtons.PerformLayout();
        SizeButtonToText(_refreshButton, 124);
        SizeButtonToText(_undoButton, 112);
        SizeButtonToText(_renameButton, 142);
        SizeButtonToText(_actionOpenButton, 112);
    }

    private void RefreshResponsiveLayout()
    {
        if (_contentPanel.Parent is null) return;
        _mainLayout.SuspendLayout();
        try
        {
            ResizeCommandButtons();
            ResizeContentRoot();
            _mainLayout.PerformLayout();
        }
        finally
        {
            _mainLayout.ResumeLayout(true);
        }
    }

    private static void SizeButtonToText(ModernButton button, int minimumWidth)
    {
        var measured = TextRenderer.MeasureText(button.Text, button.Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.SingleLine);
        int width = Math.Max(minimumWidth, measured.Width + button.Padding.Horizontal + 24);
        button.AutoEllipsis = false;
        button.AutoSize = false;
        button.MinimumSize = Size.Empty;
        button.Size = new Size(width, 36);
        button.MinimumSize = button.Size;
    }

    private static Icon LoadWindowIcon()
    {
        try
        {
            return Icon.ExtractAssociatedIcon(Application.ExecutablePath) ?? SystemIcons.Application;
        }
        catch
        {
            return SystemIcons.Application;
        }
    }

    protected override void OnDpiChanged(DpiChangedEventArgs e)
    {
        base.OnDpiChanged(e);
        UpdateFileTypesCardHeight();
        RefreshResponsiveLayout();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (WindowState == _lastWindowState) return;
        _lastWindowState = WindowState;
        RefreshResponsiveLayout();
    }

    private void UpdateFileTypesCardHeight()
    {
        int height = LogicalToDevicePixels(FileTypesCardLogicalHeight);
        _typesCard.AutoSize = false;
        _typesCard.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _typesCard.Dock = DockStyle.Fill;
        _typesCard.Height = height;
        _typesCard.MinimumSize = new Size(0, height);
        _typesCard.MaximumSize = new Size(0, height);
    }

    private int LogicalToDevicePixels(int value) => (int)Math.Ceiling(value * DeviceDpi / 96F);

    private static void ConfigureFixedCard(RoundedPanel card, Padding padding, Padding margin)
    {
        card.Dock = DockStyle.Fill;
        card.AutoSize = true;
        card.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        card.Margin = margin;
        card.MinimumSize = Size.Empty;
        card.Padding = padding;
    }

    private static void ConfigureSectionTitle(Label label)
    {
        label.AutoSize = true;
        label.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label.ForeColor = TextPrimary;
        label.Margin = new Padding(0, 0, 0, 6);
    }

    private static void StyleInput(Control control)
    {
        control.Font = new Font("Segoe UI", 9F);
        control.Height = 30;
        control.Margin = new Padding(0, 2, 8, 3);
    }

    private void BuildFolderSection()
    {
        ConfigureFixedCard(_folderCard, new Padding(12, 8, 12, 8), new Padding(0, 0, 0, 6));
        var outer = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 1, RowCount = 2 };
        outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        ConfigureSectionTitle(_folderTitle);
        outer.Controls.Add(_folderTitle, 0, 0);
        var table = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 2, RowCount = 2 };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        _folderText.Dock = DockStyle.Fill;
        _folderText.BorderStyle = BorderStyle.FixedSingle;
        StyleInput(_folderText);
        _folderText.Margin = new Padding(0, 2, 12, 3);
        _folderButtons.AutoSize = true;
        _folderButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _folderButtons.FlowDirection = FlowDirection.LeftToRight;
        _folderButtons.WrapContents = false;
        _folderButtons.Dock = DockStyle.None;
        _folderButtons.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _folderButtons.Margin = new Padding(0);
        _folderButtons.Padding = new Padding(0);
        _browseButton.MinimumSize = new Size(98, 34);
        _folderOpenButton.MinimumSize = new Size(82, 34);
        _browseButton.Margin = new Padding(0, 2, 8, 3);
        _folderOpenButton.Margin = new Padding(0, 2, 0, 3);
        _browseButton.ApplySecondary();
        _folderOpenButton.ApplySecondary();
        _folderButtons.Controls.Add(_browseButton);
        _folderButtons.Controls.Add(_folderOpenButton);
        _dragHint.AutoSize = true;
        _dragHint.ForeColor = TextSecondary;
        _dragHint.Margin = new Padding(0, 4, 0, 0);
        table.Controls.Add(_folderText, 0, 0);
        table.Controls.Add(_folderButtons, 1, 0);
        table.Controls.Add(_dragHint, 0, 1);
        table.SetColumnSpan(_dragHint, 2);
        outer.Controls.Add(table, 0, 1);
        _folderCard.Controls.Add(outer);
    }

    private void BuildNamingSection()
    {
        ConfigureFixedCard(_namingCard, new Padding(12, 8, 12, 8), new Padding(0, 0, 0, 6));
        var outer = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 1, RowCount = 2 };
        outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        ConfigureSectionTitle(_namingTitle);
        outer.Controls.Add(_namingTitle, 0, 0);
        var table = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4, RowCount = 5, Margin = new Padding(0) };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        for (int i = 0; i < 5; i++) table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        AddPair(table, _name1Label, _name1Text, 0, 0);
        AddPair(table, _name2Label, _name2Text, 2, 0);
        _dateModeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        AddPair(table, _dateSourceLabel, _dateModeCombo, 0, 1);
        _datePositionCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        AddPair(table, _datePositionLabel, _datePositionCombo, 2, 1);
        _manualDatePicker.Format = DateTimePickerFormat.Custom;
        _manualDatePicker.CustomFormat = "yyyy/MM/dd";
        AddInputOnly(table, _manualDatePicker, 0, 2);
        _fileDateCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        AddInputOnly(table, _fileDateCombo, 2, 2);
        _startNumber.Minimum = 0; _startNumber.Maximum = 999999999;
        AddPair(table, _startLabel, _startNumber, 0, 3);
        _digitsCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        AddPair(table, _digitsLabel, _digitsCombo, 2, 3);
        _continueCheck.AutoSize = true;
        _continueCheck.FlatStyle = FlatStyle.Flat;
        _continueCheck.ForeColor = TextPrimary;
        _continueCheck.Margin = new Padding(0, 4, 8, 0);
        table.Controls.Add(_continueCheck, 0, 4);
        table.SetColumnSpan(_continueCheck, 2);
        _continueHint.AutoSize = true;
        _continueHint.ForeColor = TextSecondary;
        _continueHint.Margin = new Padding(0, 6, 0, 0);
        table.Controls.Add(_continueHint, 2, 4);
        table.SetColumnSpan(_continueHint, 2);
        outer.Controls.Add(table, 0, 1);
        _namingCard.Controls.Add(outer);
    }

    private void BuildTypesSection()
    {
        ConfigureFixedCard(_typesCard, new Padding(12, 8, 12, 8), new Padding(0, 0, 0, 6));
        UpdateFileTypesCardHeight();
        var outer = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 1, RowCount = 2 };
        outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        ConfigureSectionTitle(_typesTitle);
        outer.Controls.Add(_typesTitle, 0, 0);
        var stack = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 2, RowCount = 2, Margin = new Padding(0) };
        stack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        stack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        stack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        stack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        AddCategoryRow(stack, 0, 0, "image", ["jpg", "png", "gif", "webp", "bmp"]);
        AddCategoryRow(stack, 1, 0, "video", ["mp4", "mov", "avi"]);
        AddCategoryRow(stack, 0, 1, "document", ["pdf", "word", "excel", "ppt", "txt"]);
        AddCategoryRow(stack, 1, 1, "archive", ["zip", "rar", "7z"]);
        outer.Controls.Add(stack, 0, 1);
        _typesCard.Controls.Add(outer);
    }

    private void AddCategoryRow(TableLayoutPanel stack, int column, int rowIndex, string categoryKey, string[] members)
    {
        var row = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, WrapContents = true, Margin = new Padding(0, 1, 8, 2), Padding = new Padding(0) };
        var category = new CheckBox { ThreeState = true, AutoCheck = false, AutoSize = true, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = TextPrimary, Tag = categoryKey };
        category.Click += CategoryClicked;
        _categoryChecks[categoryKey] = category;
        row.Controls.Add(category);
        foreach (string key in members)
        {
            var check = new CheckBox { AutoSize = true, FlatStyle = FlatStyle.Flat, Tag = key, Margin = new Padding(6, 2, 0, 2), ForeColor = TextSecondary };
            check.CheckedChanged += IndividualTypeChanged;
            _typeChecks[key] = check;
            row.Controls.Add(check);
        }
        stack.Controls.Add(row, column, rowIndex);
    }

    private void BuildPreviewSection()
    {
        ConfigureFixedCard(_previewCard, new Padding(12, 8, 12, 8), new Padding(0, 0, 0, 6));
        var outer = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 1, RowCount = 3 };
        outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        ConfigureSectionTitle(_previewTitle);
        outer.Controls.Add(_previewTitle, 0, 0);
        _summaryLabel.AutoSize = true;
        _summaryLabel.ForeColor = TextSecondary;
        _summaryLabel.Padding = new Padding(0, 0, 0, 6);
        _previewText.ReadOnly = true;
        _previewText.Dock = DockStyle.Top;
        _previewText.Height = 32;
        _previewText.TabStop = false;
        _previewText.BorderStyle = BorderStyle.FixedSingle;
        _previewText.BackColor = Color.FromArgb(248, 250, 252);
        _previewText.ForeColor = TextPrimary;
        _previewText.Font = new Font("Consolas", 9F);
        outer.Controls.Add(_summaryLabel, 0, 1);
        outer.Controls.Add(_previewText, 0, 2);
        _previewCard.Controls.Add(outer);
    }

    private Control BuildActionSection()
    {
        var row = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 5, Margin = new Padding(16, 2, 16, 2), Padding = new Padding(0, 6, 0, 6) };
        row.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        row.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        row.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        row.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        foreach (var button in new[] { _refreshButton, _undoButton, _renameButton, _actionOpenButton })
        {
            button.AutoSize = false;
            button.Anchor = AnchorStyles.Top;
            button.Margin = new Padding(0, 0, 8, 0);
        }
        _actionOpenButton.Margin = new Padding(0);
        _refreshButton.ApplySecondary();
        _actionOpenButton.ApplySecondary();
        _renameButton.ApplyPrimary();
        _undoButton.ApplyWarning();
        row.Controls.Add(_refreshButton, 0, 0);
        row.Controls.Add(_undoButton, 1, 0);
        row.Controls.Add(_renameButton, 3, 0);
        row.Controls.Add(_actionOpenButton, 4, 0);
        return row;
    }

    private static void AddPair(TableLayoutPanel table, Label label, Control control, int column, int row)
    {
        label.AutoSize = true;
        label.Anchor = AnchorStyles.Left;
        label.ForeColor = TextSecondary;
        label.Margin = new Padding(0, 7, 7, 3);
        control.Dock = DockStyle.Fill;
        StyleInput(control);
        table.Controls.Add(label, column, row);
        table.Controls.Add(control, column + 1, row);
    }

    private static void AddInputOnly(TableLayoutPanel table, Control control, int column, int row)
    {
        control.Dock = DockStyle.Fill;
        StyleInput(control);
        table.Controls.Add(control, column, row);
        table.SetColumnSpan(control, 2);
    }

    private void LoadSettingsIntoUi()
    {
        _folderText.Text = _settings.LastFolder;
        _name1Text.Text = _settings.Name1;
        _name2Text.Text = _settings.Name2;
        _startNumber.Value = Math.Clamp(_settings.StartNumber, 0, 999999999);
        _digitsCombo.Items.AddRange(["2", "3", "4", "5", "6"]);
        _digitsCombo.SelectedIndex = Math.Clamp(_settings.Digits - 2, 0, 4);
        _continueCheck.Checked = _settings.ContinueLastNumber;
        if (DateTime.TryParse(_settings.ManualDate, out var date)) _manualDatePicker.Value = date;
        _languageToggle.SetLanguage(_english);
    }

    private void ApplyLanguage()
    {
        Text = _english ? "Quick File Organizer" : "檔案整理工具｜Quick File Organizer";
        _titleLabel.Text = _english ? "Quick File Organizer" : "檔案整理工具";
        _subtitleLabel.Text = _english ? "Fast, safe batch renaming for everyday files" : "Quick File Organizer · 快速、安全的批次檔案命名";
        _languageToggle.SetLanguage(_english);
        _folderTitle.Text = T("檔案位置", "Folder");
        _namingTitle.Text = T("命名設定", "Naming");
        _typesTitle.Text = T("檔案類型", "File types");
        _previewTitle.Text = T("即時預覽", "Live preview");
        _folderText.PlaceholderText = T("請選擇要重新命名的資料夾", "Choose a folder to rename");
        _browseButton.Text = T("選擇資料夾", "Browse");
        _folderOpenButton.Text = T("開啟", "Open");
        _dragHint.Text = T("提示：也可以直接把一個資料夾拖曳到此視窗。", "Tip: drag one folder anywhere onto this window.");
        _name1Label.Text = T("名稱 1", "Name 1");
        _name2Label.Text = T("名稱 2", "Name 2");
        _dateSourceLabel.Text = T("日期來源", "Date source");
        ReplaceItems(_dateModeCombo, [T("不加入日期", "No date"), T("今日日期", "Today"), T("手動輸入", "Manual"), T("依檔案時間", "File date")], DateModeIndex(_settings.DateMode));
        ReplaceItems(_fileDateCombo, [T("修改時間", "Modified date"), T("建立時間", "Created date")], _settings.FileDateField == "created" ? 1 : 0);
        _datePositionLabel.Text = T("日期位置", "Date position");
        ReplaceItems(_datePositionCombo, [T("名稱之前", "Before names"), T("名稱之後", "After names")], _settings.DatePosition == "after" ? 1 : 0);
        _startLabel.Text = T("起始號碼", "Start number");
        _digitsLabel.Text = T("流水號位數", "Digits");
        _continueCheck.Text = T("延續相同命名組合的下一個號碼", "Continue the same naming sequence");
        _refreshButton.Text = T("重新整理預覽", "Refresh preview");
        _undoButton.Text = T("還原上一次", "Undo last");
        _renameButton.Text = T("開始重新命名", "Rename files");
        _actionOpenButton.Text = T("開啟資料夾", "Open folder");
        _copyrightLabel.Text = "© 2026 HeroRaye  ·  v1.0.1";

        SetCategoryText("image", T("圖片", "Images"));
        SetCategoryText("video", T("影片", "Videos"));
        SetCategoryText("document", T("文件", "Documents"));
        SetCategoryText("archive", T("壓縮檔", "Archives"));
        SetTypeText("jpg", "JPG/JPEG"); SetTypeText("png", "PNG"); SetTypeText("gif", "GIF"); SetTypeText("webp", "WEBP"); SetTypeText("bmp", "BMP");
        SetTypeText("mp4", "MP4"); SetTypeText("mov", "MOV"); SetTypeText("avi", "AVI");
        SetTypeText("pdf", "PDF"); SetTypeText("word", "Word"); SetTypeText("excel", "Excel"); SetTypeText("ppt", "PowerPoint"); SetTypeText("txt", "TXT");
        SetTypeText("zip", "ZIP"); SetTypeText("rar", "RAR"); SetTypeText("7z", "7Z");
        ResizeCommandButtons();
        UpdateDateControls();
        if (!_applyingLanguage) RefreshPreview();
    }

    private void WireEvents()
    {
        _languageToggle.LanguageChanged += (_, _) => ToggleLanguage();
        _browseButton.Click += (_, _) => SelectFolder();
        _folderOpenButton.Click += (_, _) => OpenCurrentFolder();
        _actionOpenButton.Click += (_, _) => OpenCurrentFolder();
        _refreshButton.Click += (_, _) => RefreshPreview();
        _undoButton.Click += (_, _) => UndoLastRename();
        _renameButton.Click += (_, _) => RenameFiles();
        ResizeEnd += (_, _) => RefreshResponsiveLayout();

        _folderText.TextChanged += (_, _) => RefreshPreview();
        _name1Text.TextChanged += (_, _) => RefreshPreview();
        _name2Text.TextChanged += (_, _) => RefreshPreview();
        _dateModeCombo.SelectedIndexChanged += (_, _) => { if (_applyingLanguage) return; UpdateDateControls(); RefreshPreview(); };
        _manualDatePicker.ValueChanged += (_, _) => RefreshPreview();
        _fileDateCombo.SelectedIndexChanged += (_, _) => { if (!_applyingLanguage) RefreshPreview(); };
        _datePositionCombo.SelectedIndexChanged += (_, _) => { if (!_applyingLanguage) RefreshPreview(); };
        _startNumber.ValueChanged += (_, _) => RefreshPreview();
        _digitsCombo.SelectedIndexChanged += (_, _) => { if (!_applyingLanguage) RefreshPreview(); };
        _continueCheck.CheckedChanged += (_, _) => RefreshPreview();

        DragEnter += (_, e) =>
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true && e.Data.GetData(DataFormats.FileDrop) is string[] p && p.Length == 1 && Directory.Exists(p[0]))
                e.Effect = DragDropEffects.Copy;
            else e.Effect = DragDropEffects.None;
        };
        DragDrop += (_, e) =>
        {
            if (e.Data?.GetData(DataFormats.FileDrop) is string[] p && p.Length == 1 && Directory.Exists(p[0])) _folderText.Text = p[0];
        };
        FormClosing += (_, _) => SaveUiSettings();
    }

    private void CategoryClicked(object? sender, EventArgs e)
    {
        if (sender is not CheckBox category || category.Tag is not string key) return;
        bool selectAll = category.CheckState != CheckState.Checked;
        _updatingChecks = true;
        foreach (string member in CategoryMembers[key]) _typeChecks[member].Checked = selectAll;
        category.CheckState = selectAll ? CheckState.Checked : CheckState.Unchecked;
        _updatingChecks = false;
        RefreshPreview();
    }

    private void IndividualTypeChanged(object? sender, EventArgs e)
    {
        if (_updatingChecks) return;
        _updatingChecks = true;
        foreach (var category in CategoryMembers)
        {
            int selected = category.Value.Count(x => _typeChecks[x].Checked);
            _categoryChecks[category.Key].CheckState = selected == 0 ? CheckState.Unchecked : selected == category.Value.Length ? CheckState.Checked : CheckState.Indeterminate;
        }
        _updatingChecks = false;
        RefreshPreview();
    }

    private void ToggleLanguage()
    {
        if (_applyingLanguage) return;
        SaveUiSettings();
        _english = _languageToggle.English;
        _settings.Language = _english ? "en" : "zh-TW";
        _settings.Save();

        _applyingLanguage = true;
        SuspendLayout();
        _contentLayout.SuspendLayout();
        try
        {
            ApplyLanguage();
        }
        finally
        {
            _contentLayout.ResumeLayout(false);
            ResumeLayout(false);
            _applyingLanguage = false;
            RefreshResponsiveLayout();
            RefreshPreview();
            Invalidate(true);
            Update();
        }
    }

    private string T(string zh, string en) => _english ? en : zh;
    private void SetCategoryText(string key, string value) => _categoryChecks[key].Text = value;
    private void SetTypeText(string key, string value) => _typeChecks[key].Text = value;

    private static void ReplaceItems(ComboBox combo, string[] items, int selected)
    {
        combo.BeginUpdate(); combo.Items.Clear(); combo.Items.AddRange(items); combo.SelectedIndex = Math.Clamp(selected, 0, items.Length - 1); combo.EndUpdate();
    }

    private static int DateModeIndex(string mode) => mode == "none" ? 0 : mode == "manual" ? 2 : mode == "file" ? 3 : 1;
    private string CurrentDateMode() => _dateModeCombo.SelectedIndex == 0 ? "none" : _dateModeCombo.SelectedIndex == 2 ? "manual" : _dateModeCombo.SelectedIndex == 3 ? "file" : "today";

    private void UpdateDateControls()
    {
        _manualDatePicker.Visible = CurrentDateMode() == "manual";
        _fileDateCombo.Visible = CurrentDateMode() == "file";
        _datePositionCombo.Enabled = CurrentDateMode() != "none";
    }

    private void SelectFolder()
    {
        using var dialog = new FolderBrowserDialog { Description = T("選擇要批次重新命名的資料夾", "Choose a folder to batch rename"), UseDescriptionForTitle = true, SelectedPath = Directory.Exists(_folderText.Text) ? _folderText.Text : string.Empty };
        if (dialog.ShowDialog(this) == DialogResult.OK) _folderText.Text = dialog.SelectedPath;
    }

    private void OpenCurrentFolder()
    {
        string folder = _folderText.Text.Trim();
        if (!Directory.Exists(folder)) { ShowInfo(T("請先選擇有效的資料夾。", "Choose a valid folder first.")); return; }
        Process.Start(new ProcessStartInfo("explorer.exe", $"\"{folder}\"") { UseShellExecute = true });
    }

    private void RefreshPreview()
    {
        _currentFiles = GetEligibleFiles();
        int start = GetEffectiveStartNumber();
        if (_currentFiles.Count == 0)
        {
            _summaryLabel.Text = Directory.Exists(_folderText.Text.Trim())
                ? T("目前沒有符合條件的檔案，請勾選檔案類型。", "No matching files. Select at least one file type.")
                : T("尚未選擇有效資料夾，以下顯示命名範例。", "No valid folder selected; showing a sample name.");
            _previewText.Text = T("範例：", "Sample: ") + BuildNewFileName(null, start, ".jpg");
        }
        else
        {
            var first = _currentFiles[0];
            _summaryLabel.Text = T($"共 {_currentFiles.Count} 個檔案｜流水號 {FormatNumber(start)} ～ {FormatNumber(start + _currentFiles.Count - 1)}", $"{_currentFiles.Count} files | Sequence {FormatNumber(start)}–{FormatNumber(start + _currentFiles.Count - 1)}");
            _previewText.Text = $"{first.Name}  →  {BuildNewFileName(first, start, first.Extension)}";
        }
        _renameButton.Enabled = _currentFiles.Count > 0;
        _undoButton.Enabled = _settings.LastRename.Count > 0;
        UpdateContinueHint();
    }

    private List<FileInfo> GetEligibleFiles()
    {
        string folder = _folderText.Text.Trim();
        if (!Directory.Exists(folder)) return [];
        var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in _typeChecks.Where(x => x.Value.Checked))
            foreach (string ext in FileTypeGroups[pair.Key]) extensions.Add(ext);
        if (extensions.Count == 0) return [];
        try
        {
            return new DirectoryInfo(folder).EnumerateFiles("*", SearchOption.TopDirectoryOnly)
                .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden) && !f.Attributes.HasFlag(FileAttributes.System))
                .Where(f => extensions.Contains(f.Extension))
                .OrderBy(f => f.CreationTimeUtc).ThenBy(f => f.Name, StringComparer.OrdinalIgnoreCase).ToList();
        }
        catch { return []; }
    }

    private int GetEffectiveStartNumber()
    {
        int manual = (int)_startNumber.Value;
        if (!_continueCheck.Checked) return manual;
        return _settings.LastNumbers.TryGetValue(BuildSequenceKey(), out int last) ? last + 1 : manual;
    }

    private string BuildSequenceKey()
    {
        return $"{SanitizePart(_name1Text.Text).ToLowerInvariant()}|{SanitizePart(_name2Text.Text).ToLowerInvariant()}|{CurrentDateMode()}|{(_datePositionCombo.SelectedIndex == 1 ? "after" : "before")}";
    }

    private void UpdateContinueHint()
    {
        if (!_continueCheck.Checked) { _continueHint.Text = string.Empty; return; }
        _continueHint.Text = _settings.LastNumbers.TryGetValue(BuildSequenceKey(), out int last)
            ? T($"上次到 {FormatNumber(last)}，本次從 {FormatNumber(last + 1)} 開始", $"Last: {FormatNumber(last)}; starts at {FormatNumber(last + 1)}")
            : T("尚無相同組合紀錄", "No previous sequence found");
    }

    private string BuildNewFileName(FileInfo? file, int number, string extension)
    {
        var names = new List<string>();
        string n1 = SanitizePart(_name1Text.Text), n2 = SanitizePart(_name2Text.Text);
        if (n1.Length > 0) names.Add(n1); if (n2.Length > 0) names.Add(n2);
        string date = ResolveDate(file).ToString("yyyyMMdd");
        bool includeDate = CurrentDateMode() != "none";
        var parts = new List<string>();
        if (includeDate && _datePositionCombo.SelectedIndex == 0) parts.Add(date);
        parts.AddRange(names);
        if (includeDate && _datePositionCombo.SelectedIndex == 1) parts.Add(date);
        parts.Add(FormatNumber(number));
        return string.Join("_", parts.Where(x => !string.IsNullOrWhiteSpace(x))) + extension.ToLowerInvariant();
    }

    private DateTime ResolveDate(FileInfo? file)
    {
        return CurrentDateMode() switch
        {
            "manual" => _manualDatePicker.Value.Date,
            "file" when file is not null => _fileDateCombo.SelectedIndex == 1 ? file.CreationTime : file.LastWriteTime,
            _ => DateTime.Today
        };
    }

    private string FormatNumber(int number)
    {
        int digits = _digitsCombo.SelectedIndex >= 0 ? _digitsCombo.SelectedIndex + 2 : 3;
        return number.ToString($"D{digits}");
    }

    private static string SanitizePart(string value)
    {
        string result = value.Trim();
        foreach (char c in Path.GetInvalidFileNameChars()) result = result.Replace(c, '-');
        while (result.Contains("__", StringComparison.Ordinal)) result = result.Replace("__", "_", StringComparison.Ordinal);
        return result.Trim(' ', '.', '_');
    }

    private void RenameFiles()
    {
        RefreshPreview(); if (_currentFiles.Count == 0) return;
        int start = GetEffectiveStartNumber(); string folder = _folderText.Text.Trim();
        var plan = _currentFiles.Select((file, i) => new RenamePlan
        {
            SourcePath = file.FullName,
            FinalPath = Path.Combine(folder, BuildNewFileName(file, start + i, file.Extension)),
            TemporaryPath = Path.Combine(folder, $".__qfo_{Guid.NewGuid():N}{file.Extension}")
        }).ToList();

        if (plan.GroupBy(x => x.FinalPath, StringComparer.OrdinalIgnoreCase).Any(g => g.Count() > 1)) { ShowWarning(T("命名結果發生重複，請調整設定。", "Duplicate target names were generated. Adjust the settings.")); return; }
        var sources = plan.Select(x => x.SourcePath).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var conflicts = plan.Where(x => File.Exists(x.FinalPath) && !sources.Contains(x.FinalPath)).Take(5).Select(x => Path.GetFileName(x.FinalPath)).ToList();
        if (conflicts.Count > 0) { ShowWarning(T("已有同名檔案，程式不會覆蓋：\n\n", "Existing files will not be overwritten:\n\n") + string.Join("\n", conflicts)); return; }

        string confirm = T($"即將重新命名 {plan.Count} 個檔案。\n\n第一筆：{Path.GetFileName(plan[0].FinalPath)}\n最後一筆：{Path.GetFileName(plan[^1].FinalPath)}\n\n確定繼續嗎？", $"Rename {plan.Count} files?\n\nFirst: {Path.GetFileName(plan[0].FinalPath)}\nLast: {Path.GetFileName(plan[^1].FinalPath)}");
        if (MessageBox.Show(this, confirm, T("確認重新命名", "Confirm rename"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

        var temp = new List<RenamePlan>(); var done = new List<RenamePlan>();
        try
        {
            foreach (var item in plan) { File.Move(item.SourcePath, item.TemporaryPath); temp.Add(item); }
            foreach (var item in plan) { File.Move(item.TemporaryPath, item.FinalPath); done.Add(item); }
            _settings.LastRename = plan.Select(x => new RenameLogEntry { OldPath = x.SourcePath, NewPath = x.FinalPath }).ToList();
            _settings.LastNumbers[BuildSequenceKey()] = start + plan.Count - 1;
            SaveUiSettings(); RefreshPreview();
            var result = MessageBox.Show(this, T($"已成功重新命名 {plan.Count} 個檔案。\n最後流水號：{FormatNumber(start + plan.Count - 1)}\n\n是否開啟資料夾？", $"Renamed {plan.Count} files.\nLast sequence: {FormatNumber(start + plan.Count - 1)}\n\nOpen the folder?"), T("重新命名完成", "Rename complete"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes) OpenCurrentFolder();
        }
        catch (Exception ex)
        {
            RollbackRename(done, temp);
            MessageBox.Show(this, T("重新命名未完成，程式已嘗試恢復原始檔名。\n\n", "Rename failed. The app attempted to restore the original names.\n\n") + ex.Message, T("執行失敗", "Operation failed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            RefreshPreview();
        }
    }

    private static void RollbackRename(List<RenamePlan> completed, List<RenamePlan> movedToTemp)
    {
        foreach (var item in completed.AsEnumerable().Reverse()) try { if (File.Exists(item.FinalPath) && !File.Exists(item.TemporaryPath)) File.Move(item.FinalPath, item.TemporaryPath); } catch { }
        foreach (var item in movedToTemp.AsEnumerable().Reverse()) try { if (File.Exists(item.TemporaryPath) && !File.Exists(item.SourcePath)) File.Move(item.TemporaryPath, item.SourcePath); } catch { }
    }

    private void UndoLastRename()
    {
        var entries = _settings.LastRename; if (entries.Count == 0) return;
        int available = entries.Count(x => File.Exists(x.NewPath) && !File.Exists(x.OldPath));
        int unavailable = entries.Count - available;
        if (unavailable > 0) { ShowWarning(T($"可還原：{available} 個\n無法還原：{unavailable} 個（檔案不存在或原檔名已被占用）\n\n為避免部分還原造成混亂，本次不執行。", $"Restorable: {available}\nUnavailable: {unavailable}\n\nUndo was cancelled to avoid a partial restore.")); return; }
        if (MessageBox.Show(this, T($"確定還原上一次的 {entries.Count} 個檔案嗎？", $"Undo the last rename for {entries.Count} files?"), T("確認還原", "Confirm undo"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;
        var work = entries.Select(x => new UndoPlan { Entry = x, Temp = Path.Combine(Path.GetDirectoryName(x.NewPath)!, $".__qfo_undo_{Guid.NewGuid():N}{Path.GetExtension(x.NewPath)}") }).ToList();
        try
        {
            foreach (var item in work) File.Move(item.Entry.NewPath, item.Temp);
            foreach (var item in work) File.Move(item.Temp, item.Entry.OldPath);
            _settings.LastRename = []; _settings.Save(); RefreshPreview();
            ShowInfo(T("已還原上一次重新命名。", "The last rename was undone."));
        }
        catch (Exception ex)
        {
            foreach (var item in work) try { if (File.Exists(item.Temp) && !File.Exists(item.Entry.NewPath)) File.Move(item.Temp, item.Entry.NewPath); } catch { }
            MessageBox.Show(this, T("還原失敗：\n\n", "Undo failed:\n\n") + ex.Message, T("還原失敗", "Undo failed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SaveUiSettings()
    {
        _settings.LastFolder = _folderText.Text.Trim();
        _settings.Name1 = _name1Text.Text; _settings.Name2 = _name2Text.Text;
        _settings.Language = _english ? "en" : "zh-TW";
        _settings.DateMode = CurrentDateMode();
        _settings.ManualDate = _manualDatePicker.Value.ToString("yyyy-MM-dd");
        _settings.FileDateField = _fileDateCombo.SelectedIndex == 1 ? "created" : "modified";
        _settings.DatePosition = _datePositionCombo.SelectedIndex == 1 ? "after" : "before";
        _settings.StartNumber = (int)_startNumber.Value;
        _settings.Digits = _digitsCombo.SelectedIndex >= 0 ? _digitsCombo.SelectedIndex + 2 : 3;
        _settings.ContinueLastNumber = _continueCheck.Checked;
        _settings.Save();
    }

    private void ShowInfo(string text) => MessageBox.Show(this, text, T("提示", "Information"), MessageBoxButtons.OK, MessageBoxIcon.Information);
    private void ShowWarning(string text) => MessageBox.Show(this, text, T("注意", "Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

    private sealed class RenamePlan { public string SourcePath { get; init; } = ""; public string TemporaryPath { get; init; } = ""; public string FinalPath { get; init; } = ""; }
    private sealed class UndoPlan { public RenameLogEntry Entry { get; init; } = new(); public string Temp { get; init; } = ""; }
    private static void OpenExternalUrl(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch
        {
        }
    }


}
