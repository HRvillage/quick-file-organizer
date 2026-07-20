using System.Drawing.Drawing2D;

namespace QuickFileOrganizer;

internal sealed class RoundedPanel : Panel
{
    public int CornerRadius { get; set; } = 16;
    public Color BorderColor { get; set; } = Color.FromArgb(226, 232, 240);
    public int BorderThickness { get; set; } = 1;

    public RoundedPanel()
    {
        DoubleBuffered = true;
        BackColor = Color.White;
        Padding = new Padding(18);
    }

    protected override void OnResize(EventArgs eventargs)
    {
        base.OnResize(eventargs);
        using var path = CreatePath(ClientRectangle, CornerRadius);
        Region = new Region(path);
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var path = CreatePath(new Rectangle(0, 0, Width - 1, Height - 1), CornerRadius);
        using var pen = new Pen(BorderColor, BorderThickness);
        e.Graphics.DrawPath(pen, path);
        base.OnPaint(e);
    }

    private static GraphicsPath CreatePath(Rectangle bounds, int radius)
    {
        int d = Math.Max(2, radius * 2);
        var path = new GraphicsPath();
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}

internal sealed class ModernButton : Button
{
    public Color NormalBackColor { get; set; } = Color.White;
    public Color HoverBackColor { get; set; } = Color.FromArgb(241, 245, 249);
    public Color PressedBackColor { get; set; } = Color.FromArgb(226, 232, 240);
    public Color BorderColor { get; set; } = Color.FromArgb(203, 213, 225);
    public int CornerRadius { get; set; } = 10;
    public int BorderThickness { get; set; } = 1;

    public ModernButton()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        AutoEllipsis = false;
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        UseVisualStyleBackColor = false;
        Cursor = Cursors.Hand;
        MinimumSize = new Size(80, 34);
        Height = 34;
        Padding = new Padding(10, 0, 10, 0);
        BackColor = NormalBackColor;
        ForeColor = Color.FromArgb(30, 41, 59);
        Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        MouseEnter += (_, _) => BackColor = HoverBackColor;
        MouseLeave += (_, _) => BackColor = NormalBackColor;
        MouseDown += (_, _) => BackColor = PressedBackColor;
        MouseUp += (_, _) => BackColor = HoverBackColor;
    }

    public void ApplySecondary()
    {
        NormalBackColor = Color.FromArgb(234, 242, 253);
        HoverBackColor = Color.FromArgb(219, 234, 254);
        PressedBackColor = Color.FromArgb(191, 219, 254);
        BorderColor = Color.FromArgb(147, 197, 253);
        ForeColor = Color.FromArgb(30, 87, 153);
        BackColor = NormalBackColor;
    }

    public void ApplyPrimary()
    {
        NormalBackColor = Color.FromArgb(47, 128, 237);
        HoverBackColor = Color.FromArgb(35, 106, 210);
        PressedBackColor = Color.FromArgb(28, 90, 180);
        BorderColor = NormalBackColor;
        ForeColor = Color.White;
        BackColor = NormalBackColor;
    }

    public void ApplyWarning()
    {
        NormalBackColor = Color.FromArgb(255, 247, 237);
        HoverBackColor = Color.FromArgb(255, 237, 213);
        PressedBackColor = Color.FromArgb(254, 215, 170);
        BorderColor = Color.FromArgb(251, 146, 60);
        ForeColor = Color.FromArgb(154, 52, 18);
        BackColor = NormalBackColor;
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        UpdateRegion();
        Parent?.Invalidate(Bounds, false);
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        using var brush = new SolidBrush(Parent?.BackColor ?? SystemColors.Control);
        pevent.Graphics.FillRectangle(brush, ClientRectangle);
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        OnPaintBackground(pevent);
        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var bounds = new Rectangle(1, 1, Math.Max(1, Width - 3), Math.Max(1, Height - 3));
        using var path = CreatePath(bounds, CornerRadius);
        using var brush = new SolidBrush(BackColor);
        using var pen = new Pen(BorderColor, BorderThickness);
        pevent.Graphics.FillPath(brush, path);
        pevent.Graphics.DrawPath(pen, path);
        TextRenderer.DrawText(pevent.Graphics, Text, Font, ClientRectangle, ForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
    }

    private void UpdateRegion()
    {
        if (Width <= 0 || Height <= 0) return;
        using var path = CreatePath(new Rectangle(0, 0, Width, Height), CornerRadius);
        var oldRegion = Region;
        Region = new Region(path);
        oldRegion?.Dispose();
    }

    private static GraphicsPath CreatePath(Rectangle bounds, int radius)
    {
        int d = Math.Max(2, radius * 2);
        var path = new GraphicsPath();
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}

internal sealed class LanguageToggle : UserControl
{
    private readonly Button _zh = new();
    private readonly Button _en = new();
    public event EventHandler? LanguageChanged;
    public bool English { get; private set; }

    public LanguageToggle()
    {
        Width = 122;
        Height = 32;
        BackColor = Color.FromArgb(226, 232, 240);
        Padding = new Padding(3);
        _zh.Text = "中文";
        _en.Text = "EN";
        foreach (var button in new[] { _zh, _en })
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
            button.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            button.Dock = DockStyle.Left;
            button.Width = 58;
            Controls.Add(button);
        }
        _en.BringToFront();
        _zh.Click += (_, _) => SetLanguage(false, true);
        _en.Click += (_, _) => SetLanguage(true, true);
        Resize += (_, _) => ApplyRegion();
        SetLanguage(false, false);
    }

    public void SetLanguage(bool english, bool notify = false)
    {
        English = english;
        StyleButton(_zh, !english);
        StyleButton(_en, english);
        if (notify) LanguageChanged?.Invoke(this, EventArgs.Empty);
    }

    private static void StyleButton(Button button, bool selected)
    {
        button.BackColor = selected ? Color.White : Color.Transparent;
        button.ForeColor = selected ? Color.FromArgb(47, 128, 237) : Color.FromArgb(100, 116, 139);
    }

    private void ApplyRegion()
    {
        using var path = new GraphicsPath();
        int r = 10, d = r * 2;
        var b = ClientRectangle;
        path.AddArc(b.X, b.Y, d, d, 180, 90);
        path.AddArc(b.Right - d, b.Y, d, d, 270, 90);
        path.AddArc(b.Right - d, b.Bottom - d, d, d, 0, 90);
        path.AddArc(b.X, b.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        Region = new Region(path);
    }
}
