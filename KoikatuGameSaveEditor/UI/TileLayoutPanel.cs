using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace KGSE.UI {
    public partial class TileLayoutPanel : UserControl {

        public TileLayoutPanel() {
            InitializeComponent();

            _tileSize = DefaultTileSize;

            scroll.ValueChanged += Scroll_ValueChanged;
            panel.MouseWheel += Panel_MouseWheel;
        }

        ~TileLayoutPanel() {
            scroll.ValueChanged -= Scroll_ValueChanged;
            panel.MouseWheel -= Panel_MouseWheel;
        }

        [Description("Size of each tile.")]
        [Category("Layout")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Size TileSize {
            get => _tileSize;
            set {
                var oldSize = _tileSize;
                _tileSize = value;

                if (oldSize != value) {
                    LayoutTiles();
                }
            }
        }

        [Description("Is the scrollbar always visible.")]
        [Category("Appearance")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(false)]
        public bool ScrollAlwaysVisible { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [NotNull, ItemNotNull]
        public ControlCollection Tiles {
            get => panel.Controls;
        }

        public void AddControl([NotNull] Control control) {
            panel.Controls.Add(control);

            if (!_isInBatchMode) {
                LayoutTiles();
            }
        }

        public void AddControls([NotNull, ItemNotNull] params Control[] controls) {
            panel.Controls.AddRange(controls);

            if (!_isInBatchMode) {
                LayoutTiles();
            }
        }

        public void ClearControls() {
            panel.Controls.Clear();

            if (!_isInBatchMode) {
                LayoutTiles();
            }
        }

        public void LayoutTiles() {
            _isInTileLayout = true;

            try {
                var clientSize = GetActualClientSize();

                if (clientSize.Width <= 0 || clientSize.Height <= 0) {
                    return;
                }

                var tileSize = TileSize;

                if (tileSize.Width == 0 || tileSize.Height == 0) {
                    return;
                }

                var columnCount = clientSize.Width / tileSize.Width;

                if (columnCount < 1) {
                    columnCount = 1;
                }

                _columnCount = columnCount;

                var controlCount = panel.Controls.Count;
                var rowCount = (controlCount + columnCount - 1) / columnCount;

                if (rowCount == 0) {
                    scroll.Value = 0;
                    scroll.Maximum = 0;
                    scroll.Visible = ScrollAlwaysVisible;
                } else {
                    var totalContentHeight = rowCount * tileSize.Height;

                    if (totalContentHeight > clientSize.Height) {
                        var contentBottom = totalContentHeight - scroll.Maximum;

                        if (contentBottom < clientSize.Height) {
                            scroll.Maximum -= clientSize.Height - contentBottom;
                        } else {
                            scroll.Maximum = totalContentHeight - clientSize.Height;
                        }

                        scroll.Visible = true;
                    } else {
                        scroll.Value = 0;
                        scroll.Maximum = 0;
                        scroll.Visible = ScrollAlwaysVisible;
                    }
                }
            } finally {
                _isInTileLayout = false;
            }

            MoveControls();
        }

        public void EnterBatchMode() {
            _isInBatchMode = true;
        }

        public void ExitBatchMode() {
            ExitBatchMode(true);
        }

        public void ExitBatchMode(bool performLayout) {
            _isInBatchMode = false;

            if (performLayout) {
                LayoutTiles();
            }
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            if (!_isInBatchMode) {
                LayoutTiles();
            }
        }

        protected override CreateParams CreateParams {
            get {
                var os = Environment.OSVersion;
                bool isWindows;

                switch (os.Platform) {
                    case PlatformID.Win32NT:
                    case PlatformID.Win32Windows:
                    case PlatformID.Win32S:
                        isWindows = true;
                        break;
                    default:
                        isWindows = false;
                        break;
                }

                var cp = base.CreateParams;

                if (isWindows) {
                    // WS_CLIPCHILDREN
                    cp.ExStyle |= 0x02000000;
                }

                return cp;
            }
        }

        private void MoveControls() {
            var clientRect = GetActualClientRectangle();
            var controls = panel.Controls;
            var tileSize = TileSize;
            var columnCount = _columnCount;
            var topOffset = _topOffset;

            var rowIndex = 0;
            var columnIndex = 0;

            foreach (var c in controls) {
                if (!(c is Control control)) {
                    continue;
                }

                var location = new Point(columnIndex * tileSize.Width, topOffset + rowIndex * tileSize.Height);

                var newControlRect = new Rectangle(location, tileSize);

                if (newControlRect.IntersectsWith(clientRect)) {
                    // Control will be visible
                    control.Location = location;

                    if (!control.Visible) {
                        control.Visible = true;
                    }
                } else {
                    // Control will be invisible
                    if (control.Visible) {
                        control.Visible = false;
                    }
                }

                columnIndex += 1;

                if (columnIndex >= columnCount) {
                    columnIndex = 0;
                    rowIndex += 1;
                }
            }
        }

        private void Scroll_ValueChanged(object sender, EventArgs e) {
            if (_isInTileLayout) {
                return;
            }

            var newValue = -scroll.Value;

            if (_topOffset == newValue) {
                return;
            } else {
                _topOffset = newValue;
            }

            MoveControls();
        }

        private void Panel_MouseWheel(object sender, MouseEventArgs e) {
            int delta;
            var mods = ModifierKeys;

            if ((mods & Keys.Shift) != 0) {
                delta = scroll.LargeChange;
            } else {
                delta = scroll.SmallChange;
            }

            if (e.Delta > 0) {
                delta = -delta;
            }

            var newValue = scroll.Value + delta;
            var clamped = Clamp(newValue, scroll.Minimum, scroll.Maximum);

            if (clamped != scroll.Value) {
                scroll.Value = clamped;
            }
        }

        private Size GetActualClientSize() {
            var clientSize = ClientSize;
            var scrollSize = scroll.Size;
            var availableWidth = Math.Max(clientSize.Width - scrollSize.Width, 0);

            return new Size(availableWidth, clientSize.Height);
        }

        private Rectangle GetActualClientRectangle() {
            var size = GetActualClientSize();
            return new Rectangle(Point.Empty, size);
        }

        private static int Clamp(int value, int min, int max) {
            return value < min ? min : (value > max ? max : value);
        }

        private static readonly Size DefaultTileSize = new Size(600, 380);

        private int _columnCount;
        private bool _isInBatchMode;
        private int _topOffset;

        private Size _tileSize;
        private bool _isInTileLayout;

    }
}
