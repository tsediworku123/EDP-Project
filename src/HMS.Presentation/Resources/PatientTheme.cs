using System.Drawing;
using System.Drawing.Drawing2D;

namespace HMS.Core.Utils
{
    public static class PatientTheme
    {
        // ── CALM & MODERN PALETTE ────────────────────────────────────
        public static Color Background = Color.FromArgb(248, 250, 252);     // Slate 50
        public static Color Surface = Color.White;
        public static Color Border = Color.FromArgb(226, 232, 240);       // Slate 200
        public static Color SidebarBg = Color.FromArgb(15, 23, 42);       // Slate 900
        public static Color SidebarHover = Color.FromArgb(30, 41, 59);    // Slate 800
        
        // ── PRIMARY ACTIONS (VIBRANT BLUE) ──────────────────────────
        public static Color Primary = Color.FromArgb(37, 99, 235);        // Blue 600
        public static Color PrimaryHover = Color.FromArgb(29, 78, 216);   // Blue 700
        public static Color PrimaryLight = Color.FromArgb(239, 246, 255); // Blue 50
        
        // ── BRAND GRADIENTS ──────────────────────────────────────────
        public static Color GradientStart = Color.FromArgb(37, 99, 235);  // Blue 600
        public static Color GradientEnd = Color.FromArgb(124, 58, 237);    // Violet 600
        
        // ── SUCCESS (HEALTH/POSITIVE) ────────────────────────────────
        public static Color Success = Color.FromArgb(16, 185, 129);       // Emerald 500
        public static Color SuccessLight = Color.FromArgb(236, 253, 245); // Emerald 50
        
        // ── WARNING / ALERTS ─────────────────────────────────────────
        public static Color Danger = Color.FromArgb(239, 68, 68);        // Red 500
        public static Color DangerLight = Color.FromArgb(254, 242, 242);  // Red 50
        public static Color Amber = Color.FromArgb(245, 158, 11);         // Amber 500
        
        // ── TEXT ─────────────────────────────────────────────────────
        public static Color TextPrimary = Color.FromArgb(15, 23, 42);     // Slate 900
        public static Color TextSecondary = Color.FromArgb(100, 116, 139); // Slate 500
        public static Color TextMuted = Color.FromArgb(148, 163, 184);     // Slate 400
        
        // ── FONTS ────────────────────────────────────────────────────
        public static Font TitleLarge = new Font("Segoe UI Semibold", 24);
        public static Font TitleMedium = new Font("Segoe UI Semibold", 18);
        public static Font Subtitle = new Font("Segoe UI", 11, FontStyle.Regular);
        public static Font LabelBold = new Font("Segoe UI", 9, FontStyle.Bold);
        public static Font BodyRegular = new Font("Segoe UI", 10, FontStyle.Regular);
        public static Font ButtonFont = new Font("Segoe UI Semibold", 10);

        // ── UTILITIES ────────────────────────────────────────────────
        public static void ApplyShadow(Graphics g, Rectangle rect, int depth = 4)
        {
            using (SolidBrush b = new SolidBrush(Color.FromArgb(10, 0, 0, 0))) {
                for (int i = 1; i <= depth; i++) {
                    Rectangle shadowRect = new Rectangle(rect.X + i, rect.Y + i, rect.Width, rect.Height);
                    g.FillRectangle(b, shadowRect);
                }
            }
        }

        public static LinearGradientBrush GetBrandGradient(Rectangle rect)
        {
            return new LinearGradientBrush(rect, GradientStart, GradientEnd, 45f);
        }
    }
}
