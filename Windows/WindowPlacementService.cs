using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Forms = System.Windows.Forms;
using Drawing = System.Drawing;

namespace NanoTwitchLeafs.Windows
{
    internal static class WindowPlacementService
    {
        internal static Rect RestoreMainWindow(Window window, Rect savedBounds, out string monitorDescription)
        {
            Forms.Screen screen = ScreenForLogicalBounds(window, savedBounds);
            Rect workArea = LogicalWorkingArea(window, screen);
            DpiScale dpi = VisualTreeHelper.GetDpi(window);
            monitorDescription = $"{screen.DeviceName}, work area {workArea.Width:0}x{workArea.Height:0}, DPI {dpi.PixelsPerInchX:0}x{dpi.PixelsPerInchY:0}";
            return Clamp(savedBounds, workArea, window.MinWidth, window.MinHeight);
        }

        internal static void PrepareOwnedWindow(Window window, Window owner)
        {
            if (window == null) return;
            owner ??= Application.Current?.MainWindow;
            window.Owner = owner;
            window.WindowStartupLocation = WindowStartupLocation.Manual;

            window.SourceInitialized += (_, _) => PlaceOwnedWindow(window, owner);
        }

        private static void PlaceOwnedWindow(Window window, Window owner)
        {
            Forms.Screen screen = owner != null && new WindowInteropHelper(owner).Handle != IntPtr.Zero
                ? Forms.Screen.FromHandle(new WindowInteropHelper(owner).Handle)
                : Forms.Screen.PrimaryScreen;
            Rect workArea = LogicalWorkingArea(owner ?? window, screen);

            window.MaxWidth = workArea.Width;
            window.MaxHeight = workArea.Height;
            double width = Math.Min(window.Width, workArea.Width);
            double height = Math.Min(window.Height, workArea.Height);
            window.Width = Math.Max(Math.Min(window.MinWidth, workArea.Width), width);
            window.Height = Math.Max(Math.Min(window.MinHeight, workArea.Height), height);

            Rect centered = new Rect(
                workArea.Left + (workArea.Width - window.Width) / 2,
                workArea.Top + (workArea.Height - window.Height) / 2,
                window.Width,
                window.Height);
            Rect placed = Clamp(centered, workArea, Math.Min(window.MinWidth, workArea.Width), Math.Min(window.MinHeight, workArea.Height));
            window.Left = placed.Left;
            window.Top = placed.Top;
        }

        private static Forms.Screen ScreenForLogicalBounds(Window window, Rect bounds)
        {
            Point center = new Point(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
            PresentationSource source = PresentationSource.FromVisual(window);
            if (source?.CompositionTarget != null)
                center = source.CompositionTarget.TransformToDevice.Transform(center);
            return Forms.Screen.FromPoint(new Drawing.Point((int)Math.Round(center.X), (int)Math.Round(center.Y)));
        }

        private static Rect LogicalWorkingArea(Window reference, Forms.Screen screen)
        {
            Drawing.Rectangle area = (screen ?? Forms.Screen.PrimaryScreen).WorkingArea;
            Matrix fromDevice = Matrix.Identity;
            PresentationSource source = reference == null ? null : PresentationSource.FromVisual(reference);
            if (source?.CompositionTarget != null)
                fromDevice = source.CompositionTarget.TransformFromDevice;

            Point topLeft = fromDevice.Transform(new Point(area.Left, area.Top));
            Point bottomRight = fromDevice.Transform(new Point(area.Right, area.Bottom));
            return new Rect(topLeft, bottomRight);
        }

        private static Rect Clamp(Rect requested, Rect workArea, double minimumWidth, double minimumHeight)
        {
            double width = Math.Min(Math.Max(requested.Width, Math.Min(minimumWidth, workArea.Width)), workArea.Width);
            double height = Math.Min(Math.Max(requested.Height, Math.Min(minimumHeight, workArea.Height)), workArea.Height);
            double left = Math.Max(workArea.Left, Math.Min(requested.Left, workArea.Right - width));
            double top = Math.Max(workArea.Top, Math.Min(requested.Top, workArea.Bottom - height));
            return new Rect(left, top, width, height);
        }
    }
}
