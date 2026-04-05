/***************************************************************************
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 ***************************************************************************/

namespace Ambertation.Windows.Forms
{
    // HexViewControl wraps AvaloniaHex.HexEditor — a full-featured, properly
    // virtualized hex editor control for Avalonia, already included as a package
    // dependency. Its styles are loaded in App.axaml:
    //   <StyleInclude Source="avares://AvaloniaHex/Themes/Simple/AvaloniaHex.axaml"/>
    //
    // Layout: orange column-number header row on top (matching the original SimPE
    // hex editor), then the HexEditor below. Three columns: offset | hex bytes | ASCII.
    // The header's left margin is updated after layout to align with the hex column.
    public class HexViewControl : Avalonia.Controls.Grid
    {
        public enum ViewState { Hex, SignedDec, UnsignedDec }

        private static readonly Avalonia.Media.FontFamily MonoFont =
            new Avalonia.Media.FontFamily("Courier New, Courier, monospace");
        private const double HexFontSize   = 12.5;
        private const Avalonia.Media.FontWeight HexFontWeight = Avalonia.Media.FontWeight.SemiBold;
        private static readonly Avalonia.Media.IBrush OrangeBrush =
            new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(230, 120, 0));

        private readonly AvaloniaHex.HexEditor _editor;
        private readonly AvaloniaHex.Rendering.OffsetColumn _offsetCol;
        private readonly AvaloniaHex.Rendering.HexColumn    _hexCol;
        // The TextBlock showing "00 01 02 ... 0f" in the orange header row.
        private readonly Avalonia.Controls.TextBlock _colHeader;
        private byte[] _data;

        public HexViewControl()
        {
            // Two rows: auto-height orange header, then the editor fills the rest.
            RowDefinitions.Add(new Avalonia.Controls.RowDefinition(Avalonia.Controls.GridLength.Auto));
            RowDefinitions.Add(new Avalonia.Controls.RowDefinition(new Avalonia.Controls.GridLength(1, Avalonia.Controls.GridUnitType.Star)));

            // ── Orange column-number header ───────────────────────────────────
            // Double space between bytes 07 and 08 matches AvaloniaHex's default
            // group separator (BytesPerGroup = 8).
            _colHeader = new Avalonia.Controls.TextBlock
            {
                Text       = "00 01 02 03 04 05 06 07 08 09 0a 0b 0c 0d 0e 0f",
                Foreground = Avalonia.Media.Brushes.White,
                FontFamily = MonoFont,
                FontSize   = HexFontSize,
                FontWeight = HexFontWeight,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            };

            var headerBorder = new Avalonia.Controls.Border
            {
                Background = OrangeBrush,
                Padding    = new Avalonia.Thickness(0, 3, 0, 3),
                Child      = _colHeader,
            };
            Avalonia.Controls.Grid.SetRow(headerBorder, 0);
            Children.Add(headerBorder);

            // ── Hex editor ───────────────────────────────────────────────────
            _offsetCol = new AvaloniaHex.Rendering.OffsetColumn
            {
                Background = OrangeBrush,
                Foreground = Avalonia.Media.Brushes.White,
            };
            _hexCol = new AvaloniaHex.Rendering.HexColumn();

            _editor = new AvaloniaHex.HexEditor
            {
                FontFamily = MonoFont,
                FontSize   = HexFontSize,
                FontWeight = HexFontWeight,
            };
            _editor.Columns.Add(_offsetCol);
            _editor.Columns.Add(_hexCol);
            _editor.Columns.Add(new AvaloniaHex.Rendering.AsciiColumn());

            // Set a non-null empty document so the column headers always render
            // even before any resource is selected.
            _editor.Document = new AvaloniaHex.Document.ByteArrayBinaryDocument(System.Array.Empty<byte>());

            // Fix bytes-per-line at 16 to match the original SimPE layout.
            _editor.HexView.BytesPerLine = 16;

            Avalonia.Controls.Grid.SetRow(_editor, 1);
            Children.Add(_editor);

            // Update header margin once layout is complete so the column numbers
            // align with the hex bytes below.
            _editor.LayoutUpdated += OnEditorLayoutUpdated;
        }

        private void OnEditorLayoutUpdated(object sender, System.EventArgs e)
        {
            double offsetWidth = _offsetCol.Width;
            if (offsetWidth <= 0) return;

            double colPadding = _editor.HexView.ColumnPadding;
            _colHeader.Margin = new Avalonia.Thickness(offsetWidth + colPadding, 0, 0, 0);
        }

        // ── Data property ─────────────────────────────────────────────────────
        public byte[] Data
        {
            get => _data;
            set
            {
                _data = value;
                _editor.Document = (value != null && value.Length > 0)
                    ? new AvaloniaHex.Document.ByteArrayBinaryDocument(value)
                    : null;
                DataChanged?.Invoke(this, System.EventArgs.Empty);
            }
        }

        // ── Stub properties kept for callers ─────────────────────────────────
        public System.Drawing.Color BackGroundColour    { get; set; }
        public byte                  Blocks             { get; set; }
        public int                   CharBoxWidth        { get; set; }
        public int                   CurrentRow          { get; set; }
        public System.Drawing.Color  FocusedForeColor   { get; set; }
        public System.Drawing.Color  GridColor           { get; set; }
        public System.Drawing.Color  HeadColor           { get; set; }
        public System.Drawing.Color  HeadForeColor       { get; set; }
        public System.Drawing.Color  HighlightColor      { get; set; }
        public System.Drawing.Color  HighlightForeColor  { get; set; }
        public bool                  HighlightZeros      { get; set; }
        public int                   Offset              { get; set; }
        public int                   OffsetBoxWidth      { get; set; }
        public byte                  SelectedByte        { get; set; }
        public char                  SelectedChar        { get; set; }
        public double                SelectedDouble      { get; set; }
        public float                 SelectedFloat       { get; set; }
        public int                   SelectedInt         { get; set; }
        public long                  SelectedLong        { get; set; }
        public short                 SelectedShort       { get; set; }
        public uint                  SelectedUInt        { get; set; }
        public ulong                 SelectedULong       { get; set; }
        public ushort                SelectedUShort      { get; set; }
        public byte[]                Selection           { get; set; }
        public System.Drawing.Color  SelectionColor      { get; set; }
        public System.Drawing.Color  SelectionForeColor  { get; set; }
        public System.Drawing.Color  ZeroCellColor       { get; set; }
        public bool                  ShowGrid            { get; set; }
        public ViewState             View                { get; set; }
        public int                   SelectionLength     => 0;
        public bool                  Visible             { get => IsVisible; set => IsVisible = value; }

        public event System.EventHandler DataChanged;
        public event System.EventHandler SelectionChanged;

        public void Highlight(byte[] data)
        {
            if (_data == null || _data.Length == 0 || data == null || data.Length == 0)
            {
                _editor.Selection.Range = AvaloniaHex.Document.BitRange.Empty;
                return;
            }
            // Search for the first occurrence of the byte sequence in _data.
            int limit = _data.Length - data.Length;
            for (int i = 0; i <= limit; i++)
            {
                bool match = true;
                for (int j = 0; j < data.Length; j++)
                    if (_data[i + j] != data[j]) { match = false; break; }
                if (!match) continue;

                var start = new AvaloniaHex.Document.BitLocation((ulong)i);
                var end   = new AvaloniaHex.Document.BitLocation((ulong)(i + data.Length));
                _editor.Selection.Range = new AvaloniaHex.Document.BitRange(start, end);
                _editor.Caret.Location  = start;
                _editor.HexView.BringIntoView(start);
                _editor.Focus();
                return;
            }
            // Not found — clear selection.
            _editor.Selection.Range = AvaloniaHex.Document.BitRange.Empty;
        }
        public void Refresh(bool force = false) { }

        public static string SetLength(string s, int len, char pad)
        {
            while (s.Length < len) s = pad + s;
            return s;
        }
    }

    public class HexEditControl : Avalonia.Controls.Control
    {
        public System.Drawing.Font        LabelFont   { get; set; }
        public System.Drawing.Font        TextBoxFont { get; set; }
        public bool                       Vertical    { get; set; }
        public HexViewControl.ViewState   View        { get; set; }
        public HexViewControl             Viewer      { get; set; }
    }
}
