using System.Diagnostics;

namespace QuickFileOrganizer;

internal sealed class MainForm : Form
{
    private const int TargetClientWidth = 1040;
    private const int TargetClientHeight = 680;
    private const int MinimumClientWidth = 860;
    private const int MinimumClientHeight = 560;
    private const int FileTypesCardLogicalHeight = 244;
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
    private readonly ModernButton _groupedNamingButton = new();
    private readonly CheckBox _allFilesCheck = new();
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
        ["heic"] = [".heic", ".heif"], ["tiff"] = [".tif", ".tiff"],
        ["raw"] = [".raw", ".dng", ".cr2", ".cr3", ".nef", ".arw", ".raf", ".orf", ".rw2", ".pef", ".srw"],
        ["mp4"] = [".mp4"], ["mov"] = [".mov"], ["avi"] = [".avi"],
        ["mp3"] = [".mp3"], ["wav"] = [".wav"], ["ogg"] = [".ogg"], ["wma"] = [".wma"], ["aac"] = [".aac"],
        ["pdf"] = [".pdf"], ["word"] = [".doc", ".docx"], ["excel"] = [".xls", ".xlsx"],
        ["ppt"] = [".ppt", ".pptx"], ["txt"] = [".txt"],
        ["zip"] = [".zip"], ["rar"] = [".rar"], ["7z"] = [".7z"],
        ["psd"] = [".psd"], ["ai"] = [".ai"], ["indd"] = [".indd"], ["sketch"] = [".sketch"], ["fig"] = [".fig"],
        ["dwg"] = [".dwg"], ["dwf"] = [".dwf"], ["dxf"] = [".dxf"],
        ["step"] = [".step", ".stp"], ["iges"] = [".iges", ".igs"], ["stl"] = [".stl"], ["3ds"] = [".3ds"]
    };

    private static readonly Dictionary<string, string[]> CategoryMembers = new(StringComparer.OrdinalIgnoreCase)
    {
        ["image"] = ["jpg", "png", "gif", "webp", "bmp"],
        ["advancedImage"] = ["heic", "tiff", "raw"],
        ["video"] = ["mp4", "mov", "avi"],
        ["audio"] = ["mp3", "wav", "ogg", "wma", "aac"],
        ["document"] = ["pdf", "word", "excel", "ppt", "txt"],
        ["archive"] = ["zip", "rar", "7z"],
        ["design"] = ["psd", "ai", "indd", "sketch", "fig"],
        ["cad"] = ["dwg", "dwf", "dxf"],
        ["model3d"] = ["step", "iges", "stl", "3ds"]
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
        SizeButtonToText(_groupedNamingButton, 118);
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
        var titleRow = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Margin = new Padding(0, 0, 0, 6) };
        titleRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        titleRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        ConfigureSectionTitle(_namingTitle);
        _namingTitle.Margin = new Padding(0);
        _groupedNamingButton.ApplySecondary();
        _groupedNamingButton.Margin = new Padding(8, 0, 0, 0);
        titleRow.Controls.Add(_namingTitle, 0, 0);
        titleRow.Controls.Add(_groupedNamingButton, 1, 0);
        outer.Controls.Add(titleRow, 0, 0);
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
        var outer = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 1, RowCount = 3 };
        outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        outer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        ConfigureSectionTitle(_typesTitle);
        outer.Controls.Add(_typesTitle, 0, 0);
        _allFilesCheck.AutoSize = true;
        _allFilesCheck.FlatStyle = FlatStyle.Flat;
        _allFilesCheck.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        _allFilesCheck.ForeColor = TextPrimary;
        _allFilesCheck.Margin = new Padding(0, 0, 0, 4);
        outer.Controls.Add(_allFilesCheck, 0, 1);
        var stack = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 2, RowCount = 5, Margin = new Padding(0) };
        stack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        stack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        stack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        stack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        stack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        stack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        stack.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        AddCategoryRow(stack, 0, 0, "image", ["jpg", "png", "gif", "webp", "bmp"]);
        AddCategoryRow(stack, 1, 0, "video", ["mp4", "mov", "avi"]);
        AddCategoryRow(stack, 0, 1, "document", ["pdf", "word", "excel", "ppt", "txt"]);
        AddCategoryRow(stack, 1, 1, "archive", ["zip", "rar", "7z"]);
        AddCategoryRow(stack, 0, 2, "audio", ["mp3", "wav", "ogg", "wma", "aac"]);
        AddCategoryRow(stack, 1, 2, "advancedImage", ["heic", "tiff", "raw"]);
        AddCategoryRow(stack, 0, 3, "design", ["psd", "ai", "indd", "sketch", "fig"]);
        AddCategoryRow(stack, 1, 3, "cad", ["dwg", "dwf", "dxf"]);
        AddCategoryRow(stack, 0, 4, "model3d", ["step", "iges", "stl", "3ds"]);
        outer.Controls.Add(stack, 0, 2);
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
        _allFilesCheck.Checked = _settings.IncludeAllFileTypes;
        UpdateFileTypeControlsEnabled();
        if (DateTime.TryParse(_settings.ManualDate, out var date)) _manualDatePicker.Value = date;
        _languageToggle.SetLanguage(_english);
    }

    private void ApplyLanguage()
    {
        Text = _english ? "Quick File Organizer" : "檔案快速整理器｜Quick File Organizer";
        _titleLabel.Text = _english ? "Quick File Organizer" : "檔案快速整理器";
        _subtitleLabel.Text = _english ? "Fast, safe batch renaming for everyday files" : "Quick File Organizer，快速、安全地整理日常檔案名稱";
        _languageToggle.SetLanguage(_english);
        _folderTitle.Text = T("資料夾位置", "Folder");
        _namingTitle.Text = T("命名設定", "Naming");
        _typesTitle.Text = T("檔案類型", "File types");
        _previewTitle.Text = T("即時預覽", "Live preview");
        _folderText.PlaceholderText = T("請選擇要重新命名的資料夾", "Choose a folder to rename");
        _browseButton.Text = T("選擇資料夾", "Browse");
        _folderOpenButton.Text = T("開啟", "Open");
        _groupedNamingButton.Text = T("分組命名...", "Grouped naming...");
        _dragHint.Text = T("提示：也可以直接把一個資料夾拖曳到此視窗。", "Tip: drag one folder anywhere onto this window.");
        _name1Label.Text = T("名稱 1", "Name 1");
        _name2Label.Text = T("名稱 2", "Name 2");
        _dateSourceLabel.Text = T("日期來源", "Date source");
        ReplaceItems(_dateModeCombo, [T("不加入日期", "No date"), T("今天日期", "Today"), T("手動輸入", "Manual"), T("檔案日期", "File date")], DateModeIndex(_settings.DateMode));
        ReplaceItems(_fileDateCombo, [T("修改時間", "Modified date"), T("建立時間", "Created date")], _settings.FileDateField == "created" ? 1 : 0);
        _datePositionLabel.Text = T("檔名格式", "Filename format");
        ReplaceItems(_datePositionCombo, FilenameFormatItems(), InitialFilenameFormat());
        _startLabel.Text = T("起始號碼", "Start number");
        _digitsLabel.Text = T("流水號位數", "Digits");
        _continueCheck.Text = T("接續相同命名設定的上一個號碼", "Continue the same naming sequence");
        _refreshButton.Text = T("重新整理預覽", "Refresh preview");
        _undoButton.Text = T("還原上一次", "Undo last");
        _renameButton.Text = T("開始重新命名", "Rename files");
        _actionOpenButton.Text = T("開啟資料夾", "Open folder");
        _copyrightLabel.Text = "© 2026 HeroRaye  ·  v1.1.0";

        SetCategoryText("image", T("圖片", "Images"));
        SetCategoryText("advancedImage", T("進階圖片", "Advanced images"));
        SetCategoryText("video", T("影片", "Videos"));
        SetCategoryText("audio", T("音訊", "Audio"));
        SetCategoryText("document", T("文件", "Documents"));
        SetCategoryText("archive", T("壓縮檔", "Archives"));
        SetCategoryText("design", T("設計檔", "Design files"));
        SetCategoryText("cad", T("CAD 圖面", "CAD drawings"));
        SetCategoryText("model3d", T("3D 模型", "3D models"));
        _allFilesCheck.Text = T("Any：包含資料夾內所有檔案", "Any: include all files in the folder");
        SetTypeText("jpg", "JPG/JPEG"); SetTypeText("png", "PNG"); SetTypeText("gif", "GIF"); SetTypeText("webp", "WEBP"); SetTypeText("bmp", "BMP");
        SetTypeText("heic", "HEIC"); SetTypeText("tiff", "TIFF"); SetTypeText("raw", "RAW");
        SetTypeText("mp4", "MP4"); SetTypeText("mov", "MOV"); SetTypeText("avi", "AVI");
        SetTypeText("mp3", "MP3"); SetTypeText("wav", "WAV"); SetTypeText("ogg", "OGG"); SetTypeText("wma", "WMA"); SetTypeText("aac", "AAC");
        SetTypeText("pdf", "PDF"); SetTypeText("word", "Word"); SetTypeText("excel", "Excel"); SetTypeText("ppt", "PowerPoint"); SetTypeText("txt", "TXT");
        SetTypeText("zip", "ZIP"); SetTypeText("rar", "RAR"); SetTypeText("7z", "7Z");
        SetTypeText("psd", "PSD"); SetTypeText("ai", "AI"); SetTypeText("indd", "INDD"); SetTypeText("sketch", "Sketch"); SetTypeText("fig", "FIG");
        SetTypeText("dwg", "DWG"); SetTypeText("dwf", "DWF"); SetTypeText("dxf", "DXF");
        SetTypeText("step", "STEP"); SetTypeText("iges", "IGES"); SetTypeText("stl", "STL"); SetTypeText("3ds", "3DS");
        ResizeCommandButtons();
        UpdateDateControls();
        if (!_applyingLanguage) RefreshPreview();
    }

    private void WireEvents()
    {
        _languageToggle.LanguageChanged += (_, _) => ToggleLanguage();
        _browseButton.Click += (_, _) => SelectFolder();
        _folderOpenButton.Click += (_, _) => OpenCurrentFolder();
        _groupedNamingButton.Click += (_, _) => OpenGroupedNaming();
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
        _allFilesCheck.CheckedChanged += AllFilesCheckChanged;

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
        UpdateCategoryStates();
        _updatingChecks = false;
        RefreshPreview();
    }

    private void AllFilesCheckChanged(object? sender, EventArgs e)
    {
        if (_updatingChecks) return;
        if (_allFilesCheck.Checked)
        {
            string message = T("Any 會忽略下方檔案類型清單，將資料夾內所有非隱藏、非系統檔案都納入重新命名。\n\n這個功能適合用在你要處理的副檔名不在預設清單時。請確認資料夾內沒有不想重新命名的檔案。", "Any ignores the file type list below and includes every non-hidden, non-system file in the folder.\n\nUse this when the extension you need is not in the preset list. Make sure the folder does not contain files you do not want to rename.");
            if (MessageBox.Show(this, message, T("確認包含所有檔案", "Confirm include all files"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
            {
                _updatingChecks = true;
                _allFilesCheck.Checked = false;
                _updatingChecks = false;
                return;
            }
        }
        _settings.IncludeAllFileTypes = _allFilesCheck.Checked;
        UpdateFileTypeControlsEnabled();
        RefreshPreview();
    }

    private void UpdateFileTypeControlsEnabled()
    {
        bool enabled = !_allFilesCheck.Checked;
        foreach (var check in _typeChecks.Values) check.Enabled = enabled;
        foreach (var check in _categoryChecks.Values) check.Enabled = enabled;
    }

    private void UpdateCategoryStates()
    {
        foreach (var category in CategoryMembers)
        {
            int selected = category.Value.Count(x => _typeChecks[x].Checked);
            _categoryChecks[category.Key].CheckState = selected == 0 ? CheckState.Unchecked : selected == category.Value.Length ? CheckState.Checked : CheckState.Indeterminate;
        }
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
    private int InitialFilenameFormat() => Math.Clamp(_settings.FilenameFormat ?? (_settings.DatePosition == "after" ? 1 : 0), 0, 5);
    private string[] FilenameFormatItems() =>
    [
        T("名稱1_名稱2_日期_流水號", "Name1_Name2_Date_Number"),
        T("名稱1_名稱2_流水號_日期", "Name1_Name2_Number_Date"),
        T("名稱1_名稱2流水號_日期", "Name1_Name2Number_Date"),
        T("日期_名稱1_名稱2_流水號", "Date_Name1_Name2_Number"),
        T("日期_名稱1_名稱2流水號", "Date_Name1_Name2Number"),
        T("流水號_名稱1_名稱2_日期", "Number_Name1_Name2_Date")
    ];

    private void UpdateDateControls()
    {
        _manualDatePicker.Visible = CurrentDateMode() == "manual";
        _fileDateCombo.Visible = CurrentDateMode() == "file";
        _datePositionCombo.Enabled = true;
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

    private void OpenGroupedNaming()
    {
        RefreshPreview();
        string folder = _folderText.Text.Trim();
        if (!Directory.Exists(folder)) { ShowInfo(T("請先選擇有效的資料夾。", "Choose a valid folder first.")); return; }
        if (_currentFiles.Count == 0) { ShowInfo(T("目前沒有符合條件的檔案，請先勾選檔案類型。", "No matching files. Select at least one file type first.")); return; }
        using var dialog = new GroupedNamingDialog(_currentFiles, folder, _settings, _english, BuildGroupedFilterSummary());
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        _settings.Save();
        ExecuteGroupedRename(dialog.Preview);
    }

    private string BuildGroupedFilterSummary()
    {
        var extensions = _currentFiles.Select(x => x.Extension.ToUpperInvariant()).Where(x => x.Length > 0).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x).ToList();
        string extText = extensions.Count == 0 ? T("無副檔名", "No extension") : string.Join(_english ? ", " : "、", extensions);
        string scope = _allFilesCheck.Checked ? T("Any 模式", "Any mode") : T("主畫面目前篩選結果", "current main-screen filter");
        return T($"使用{scope}：共 {_currentFiles.Count} 個檔案，副檔名：{extText}", $"Using {scope}: {_currentFiles.Count} files, extensions: {extText}");
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
        if (_allFilesCheck.Checked)
        {
            try
            {
                return new DirectoryInfo(folder).EnumerateFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden) && !f.Attributes.HasFlag(FileAttributes.System))
                    .OrderBy(f => f.CreationTimeUtc).ThenBy(f => f.Name, StringComparer.OrdinalIgnoreCase).ToList();
            }
            catch { return []; }
        }
        var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in _typeChecks.Where(x => x.Value.Checked))
        {
            foreach (string ext in FileTypeGroups[pair.Key]) extensions.Add(ext);
        }
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
        return $"{SanitizePart(_name1Text.Text).ToLowerInvariant()}|{SanitizePart(_name2Text.Text).ToLowerInvariant()}|{CurrentDateMode()}|{CurrentFilenameFormat()}";
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
        string numberText = FormatNumber(number);
        List<string> parts = CurrentFilenameFormat() switch
        {
            1 => [.. names, numberText, includeDate ? date : string.Empty],
            2 => [.. AppendNumberToLastName(names, numberText), includeDate ? date : string.Empty],
            3 => [includeDate ? date : string.Empty, .. names, numberText],
            4 => [includeDate ? date : string.Empty, .. AppendNumberToLastName(names, numberText)],
            5 => [numberText, .. names, includeDate ? date : string.Empty],
            _ => [.. names, includeDate ? date : string.Empty, numberText]
        };
        return string.Join("_", parts.Where(x => !string.IsNullOrWhiteSpace(x))) + extension.ToLowerInvariant();
    }

    private int CurrentFilenameFormat() => Math.Clamp(_datePositionCombo.SelectedIndex, 0, 5);

    private static List<string> AppendNumberToLastName(List<string> names, string numberText)
    {
        var parts = names.ToList();
        if (parts.Count == 0) parts.Add(numberText);
        else parts[^1] += numberText;
        return parts;
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

        if (plan.GroupBy(x => x.FinalPath, StringComparer.OrdinalIgnoreCase).Any(g => g.Count() > 1)) { ShowWarning(T("\u547d\u540d\u7d50\u679c\u767c\u751f\u91cd\u8907\uff0c\u8acb\u8abf\u6574\u8a2d\u5b9a\u3002", "Duplicate target names were generated. Adjust the settings.")); return; }
        var sources = plan.Select(x => x.SourcePath).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var conflicts = plan.Where(x => File.Exists(x.FinalPath) && !sources.Contains(x.FinalPath)).Take(5).Select(x => Path.GetFileName(x.FinalPath)).ToList();
        if (conflicts.Count > 0) { ShowWarning(T("\u5df2\u6709\u540c\u540d\u6a94\u6848\uff0c\u7a0b\u5f0f\u4e0d\u6703\u8986\u84cb\uff1a\n\n", "Existing files will not be overwritten:\n\n") + string.Join("\n", conflicts)); return; }
        if (!ConfirmMultipleFileTypes(_currentFiles)) return;

        string confirm = T($"\u5373\u5c07\u91cd\u65b0\u547d\u540d {plan.Count} \u500b\u6a94\u6848\u3002\n\n\u7b2c\u4e00\u7b46\u7bc4\u4f8b\uff1a{Path.GetFileName(plan[0].FinalPath)}\n\u6700\u5f8c\u4e00\u7b46\u7bc4\u4f8b\uff1a{Path.GetFileName(plan[^1].FinalPath)}\n\n\u78ba\u5b9a\u7e7c\u7e8c\u55ce\uff1f", $"Rename {plan.Count} files?\n\nFirst: {Path.GetFileName(plan[0].FinalPath)}\nLast: {Path.GetFileName(plan[^1].FinalPath)}");
        if (MessageBox.Show(this, confirm, T("\u78ba\u8a8d\u91cd\u65b0\u547d\u540d", "Confirm rename"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

        var temp = new List<RenamePlan>(); var done = new List<RenamePlan>();
        try
        {
            foreach (var item in plan) { File.Move(item.SourcePath, item.TemporaryPath); temp.Add(item); }
            foreach (var item in plan) { File.Move(item.TemporaryPath, item.FinalPath); done.Add(item); }
            _settings.LastRename = plan.Select(x => new RenameLogEntry { OldPath = x.SourcePath, NewPath = x.FinalPath }).ToList();
            _settings.LastNumbers[BuildSequenceKey()] = start + plan.Count - 1;
            SaveUiSettings(); RefreshPreview();
            var result = MessageBox.Show(this, T($"\u5df2\u6210\u529f\u91cd\u65b0\u547d\u540d {plan.Count} \u500b\u6a94\u6848\u3002\n\u6700\u5f8c\u6d41\u6c34\u865f\uff1a{FormatNumber(start + plan.Count - 1)}\n\n\u662f\u5426\u958b\u555f\u8cc7\u6599\u593e\uff1f", $"Renamed {plan.Count} files.\nLast sequence: {FormatNumber(start + plan.Count - 1)}\n\nOpen the folder?"), T("\u91cd\u65b0\u547d\u540d\u5b8c\u6210", "Rename complete"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes) OpenCurrentFolder();
        }
        catch (Exception ex)
        {
            RollbackRename(done, temp);
            MessageBox.Show(this, T("\u91cd\u65b0\u547d\u540d\u672a\u5b8c\u6210\uff0c\u7a0b\u5f0f\u5df2\u5617\u8a66\u6062\u5fa9\u539f\u59cb\u6a94\u540d\u3002\n\n", "Rename failed. The app attempted to restore the original names.\n\n") + ex.Message, T("\u57f7\u884c\u5931\u6557", "Operation failed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            RefreshPreview();
        }
    }

    private void ExecuteGroupedRename(GroupedNamingPreview preview)
    {
        if (!preview.CanRename) return;
        var plan = preview.Items.Select(item => new RenamePlan
        {
            SourcePath = item.SourceFile.FullName,
            FinalPath = item.FinalPath,
            TemporaryPath = Path.Combine(Path.GetDirectoryName(item.SourceFile.FullName)!, $".__qfo_{Guid.NewGuid():N}{item.SourceFile.Extension}")
        }).ToList();

        if (plan.GroupBy(x => x.FinalPath, StringComparer.OrdinalIgnoreCase).Any(g => g.Count() > 1)) { ShowWarning(T("\u547d\u540d\u7d50\u679c\u767c\u751f\u91cd\u8907\uff0c\u8acb\u8abf\u6574\u8a2d\u5b9a\u3002", "Duplicate target names were generated. Adjust the settings.")); return; }
        var sources = plan.Select(x => x.SourcePath).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var conflicts = plan.Where(x => File.Exists(x.FinalPath) && !sources.Contains(x.FinalPath)).Take(5).Select(x => Path.GetFileName(x.FinalPath)).ToList();
        if (conflicts.Count > 0) { ShowWarning(T("\u5df2\u6709\u540c\u540d\u6a94\u6848\uff0c\u7a0b\u5f0f\u4e0d\u6703\u8986\u84cb\uff1a\n\n", "Existing files will not be overwritten:\n\n") + string.Join("\n", conflicts)); return; }
        if (!ConfirmMultipleFileTypes(preview.Items.Select(x => x.SourceFile))) return;

        if (preview.WarningCount > 0)
        {
            string text = T($"\u5206\u7d44\u9810\u89bd\u4e2d\u6709 {preview.WarningCount} \u500b\u6642\u9593\u9593\u9694\u63d0\u9192\u3002\n\n\u9019\u901a\u5e38\u4ee3\u8868\u53ef\u80fd\u6709\u6f0f\u62cd\u3001\u63d2\u5165\u7121\u95dc\u6a94\u6848\uff0c\u6216\u6392\u5e8f\u65b9\u5f0f\u4e0d\u9069\u5408\u76ee\u524d\u6a94\u6848\u3002\u8acb\u78ba\u8a8d\u9810\u89bd\u4e2d\u7684\u63d0\u9192\u5217\u518d\u7e7c\u7e8c\u3002", $"The preview has {preview.WarningCount} time gap warning(s).\n\nThis may mean a missing file, an unrelated file in the folder, or the wrong sort order. Review the highlighted rows before continuing.");
            if (MessageBox.Show(this, text, T("\u78ba\u8a8d\u6642\u9593\u63d0\u9192", "Confirm time warnings"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) return;
        }

        string confirm = T($"\u5373\u5c07\u4f7f\u7528\u5206\u7d44\u547d\u540d\u91cd\u65b0\u547d\u540d {plan.Count} \u500b\u6a94\u6848\u3002\n\n\u7b2c\u4e00\u7b46\uff1a{Path.GetFileName(plan[0].FinalPath)}\n\u6700\u5f8c\u4e00\u7b46\uff1a{Path.GetFileName(plan[^1].FinalPath)}\n\n\u78ba\u5b9a\u7e7c\u7e8c\u55ce\uff1f", $"Rename {plan.Count} files with grouped naming?\n\nFirst: {Path.GetFileName(plan[0].FinalPath)}\nLast: {Path.GetFileName(plan[^1].FinalPath)}");
        if (MessageBox.Show(this, confirm, T("\u78ba\u8a8d\u5206\u7d44\u547d\u540d", "Confirm grouped rename"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

        var temp = new List<RenamePlan>(); var done = new List<RenamePlan>();
        try
        {
            foreach (var item in plan) { File.Move(item.SourcePath, item.TemporaryPath); temp.Add(item); }
            foreach (var item in plan) { File.Move(item.TemporaryPath, item.FinalPath); done.Add(item); }
            _settings.LastRename = plan.Select(x => new RenameLogEntry { OldPath = x.SourcePath, NewPath = x.FinalPath }).ToList();
            _settings.Save(); RefreshPreview();
            var result = MessageBox.Show(this, T($"\u5df2\u6210\u529f\u91cd\u65b0\u547d\u540d {plan.Count} \u500b\u6a94\u6848\u3002\n\n\u662f\u5426\u958b\u555f\u8cc7\u6599\u593e\uff1f", $"Renamed {plan.Count} files.\n\nOpen the folder?"), T("\u91cd\u65b0\u547d\u540d\u5b8c\u6210", "Rename complete"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes) OpenCurrentFolder();
        }
        catch (Exception ex)
        {
            RollbackRename(done, temp);
            MessageBox.Show(this, T("\u91cd\u65b0\u547d\u540d\u672a\u5b8c\u6210\uff0c\u7a0b\u5f0f\u5df2\u5617\u8a66\u6062\u5fa9\u539f\u59cb\u6a94\u540d\u3002\n\n", "Rename failed. The app attempted to restore the original names.\n\n") + ex.Message, T("\u57f7\u884c\u5931\u6557", "Operation failed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            RefreshPreview();
        }
    }

    private bool ConfirmMultipleFileTypes(IEnumerable<FileInfo> files)
    {
        var extensions = files.Select(x => string.IsNullOrWhiteSpace(x.Extension) ? T("\u7121\u526f\u6a94\u540d", "No extension") : x.Extension.ToUpperInvariant()).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x).ToList();
        if (extensions.Count <= 1) return true;
        string text = T($"\u9019\u6b21\u5305\u542b {extensions.Count} \u7a2e\u526f\u6a94\u540d\uff1a{string.Join("\u3001", extensions)}\n\n\u8acb\u78ba\u8a8d\u9019\u4e9b\u6a94\u6848\u53ef\u4ee5\u4e00\u8d77\u4f7f\u7528\u76f8\u540c\u547d\u540d\u898f\u5247\u3002", $"This batch includes {extensions.Count} file types: {string.Join(", ", extensions)}\n\nMake sure these files should use the same naming rule.");
        return MessageBox.Show(this, text, T("\u78ba\u8a8d\u6a94\u6848\u985e\u578b", "Confirm file types"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK;
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
        _settings.FilenameFormat = CurrentFilenameFormat();
        _settings.DatePosition = CurrentFilenameFormat() is 3 or 4 ? "before" : "after";
        _settings.IncludeAllFileTypes = _allFilesCheck.Checked;
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
