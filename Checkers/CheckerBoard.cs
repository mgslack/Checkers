using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

/*
 * Component implementing a checker (or chess) board. Allows for changing
 * square colors and border color.
 * Note, resizing does work, though the effect may not be seen until the
 * project is rebuilt. Using the mouse and moving slow works best to keep
 * the board looking right during resizing.
 * 
 * Author: Michael G. Slack
 * Date Written: 2014-03-28
 * 
 * ----------------------------------------------------------------------------
 * 
 * Revised: 2014-06-16 - Fixed another bug with the mouse-click to start the
 *                       drag-n-drop operation.
 * 
 */
namespace Checkers
{
    public delegate bool CanDragTo(int startLoc, int currentLoc);
    public delegate void DragDropped(int startLoc, int endLoc, Bitmap image);
    public delegate void DragMoveComplete(int endLoc);

    public partial class CheckerBoard : UserControl
    {
        public const int NUM_SQUARES = 64;
        public const int SQUARES_ROW = 8;
        public static readonly Color DEF_BORDER_COLOR = Color.Yellow;
        public static readonly Color DEF_COLOR_SQ1 = Color.Red;
        public static readonly Color DEF_COLOR_SQ2 = Color.Black;

        #region Private Consts
        private const int DEF_SIZE = 489;
        private const int MIN_SIZE = 169;
        private const int SIZE_DELTA = 8;
        private const int HALF_DELTA = SIZE_DELTA / 2;
        private static readonly int[] C2_SQUARES = {  1,  3,  5,  7,
                                                      8, 10, 12, 14,
                                                     17, 19, 21, 23,
                                                     24, 26, 28, 30,
                                                     33, 35, 37, 39,
                                                     40, 42, 44, 46,
                                                     49, 51, 53, 55,
                                                     56, 58, 60, 62 };
        #endregion

        // --------------------------------------------------------------------

        #region Properties
        private Color _squareColor1 = DEF_COLOR_SQ1;
        public Color SquareColor1 {
            get { return _squareColor1; }
            set { _squareColor1 = value; UpdateColors(false); }
        }

        private Color _squareColor2 = DEF_COLOR_SQ2;
        public Color SquareColor2 {
            get { return _squareColor2; }
            set { _squareColor2 = value; UpdateColors(true); }
        }

        private Color _borderColor = DEF_BORDER_COLOR;
        public Color BorderColor { 
            get { return _borderColor; }
            set { _borderColor = value;
                  borderPen = new Pen(_borderColor);
                  Invalidate(); }
        }

        public Cursor DragNoneCursor { set { checkerNoneCursor = value; } }

        private CanDragTo _canDragTo = null;
        public CanDragTo CanDragTo {
            get { return _canDragTo; }
            set { _canDragTo = value; }
        }

        private DragDropped _dragDropped = null;
        public DragDropped DragDropped {
            get { return _dragDropped; }
            set { _dragDropped = value; }
        }

        private DragMoveComplete _dragMoveComplete = null;
        public DragMoveComplete DragMoveComplete {
            get { return _dragMoveComplete; }
            set { _dragMoveComplete = value; }
        }

        private bool _autoMove = false;
        public bool AutoMove {
            get { return _autoMove; }
            set { _autoMove = value; }
        }
        #endregion

        // --------------------------------------------------------------------

        private Pen borderPen = new Pen(DEF_BORDER_COLOR);
        private BoardSquare[] squares = new BoardSquare[NUM_SQUARES];
        private int OldWidth = 0, OldHeight = 0;

        #region Drag-n-Drop Fields (variables)
        private Bitmap origDragImage = null;
        private bool useCustomCursors = false;
        private bool dragStarting = false;
        private int dragStartLoc = 0;
        private int dragEndLoc = 0;
        private Cursor checkerMoveCursor = null;
        private Cursor checkerNoneCursor = null;
        #endregion

        // --------------------------------------------------------------------

        public CheckerBoard()
        {
            InitializeComponent();
            CreateSquares();
            OldWidth = this.Width; OldHeight = this.Height;
            AllowDrop = true;
        }

        // --------------------------------------------------------------------

        #region Private Methods
        private Color SquareColor(int idx)
        {
            Color color = _squareColor1;

            if (C2_SQUARES.Contains(idx)) color = _squareColor2;

            return color;
        }

        private BoardSquare CreateSquare(int x, int y, int idx)
        {
            BoardSquare square = new BoardSquare();

            square.Name = "BorderSquare" + Convert.ToString(idx);
            square.BackColor = SquareColor(idx);
            square.Location = new Point(x, y);
            square.BoardLocation = idx;
            square.AssignDragDropEvents(this);

            return square;
        }

        private void CreateSquares()
        {
            int x = 1, y = 1;

            for (int i = 0; i < NUM_SQUARES; i++) {
                if ((i > 0) && ((i % SQUARES_ROW) == 0)) {
                    x = 1; y += squares[i-1].Height + 1;
                }
                squares[i] = CreateSquare(x, y, i);
                x += squares[i].Width + 1;
                this.Controls.Add(squares[i]);
            }
        }

        private void UpdateColors(bool C2_Squares)
        {
            for (int i = 0; i < NUM_SQUARES; i++)
                if ((C2_Squares) && (C2_SQUARES.Contains(i))) {
                    squares[i].BackColor = _squareColor2;
                    squares[i].Invalidate();
                }
                else if ((!C2_Squares) && (!C2_SQUARES.Contains(i))) {
                    squares[i].BackColor = _squareColor1;
                    squares[i].Invalidate();
                }
        }

        private void AdjustSquares(int hDelt, int wDelt)
        {
            int x = 1, y = 1;

            for (int i = 0; i < NUM_SQUARES; i++) {
                squares[i].Height += hDelt;
                squares[i].Width += wDelt;
                if ((i > 0) && ((i % SQUARES_ROW) == 0)) {
                    x = 1; y += squares[i].Height + 1;
                }
                squares[i].Location = new Point(x, y);
                x += squares[i].Width + 1;
            }
        }

        private int AdjustSize(int oldSz, int newSz, ref int delt)
        {
            int adjSz = newSz, dif = 0, deltDif = 0;
            bool minus = false;

            if (newSz < MIN_SIZE) newSz = MIN_SIZE;
            if (newSz == oldSz) return newSz;

            if (newSz < oldSz) { dif = oldSz - newSz; minus = true; } else dif = newSz - oldSz;
            deltDif = dif % SIZE_DELTA;
            delt = dif / SIZE_DELTA;
            if (deltDif >= HALF_DELTA) {
                if (minus)
                    adjSz = newSz - SIZE_DELTA + deltDif;
                else
                    adjSz = newSz + SIZE_DELTA - deltDif;
            }
            else {
                if (minus)
                    adjSz = newSz + deltDif;
                else
                    adjSz = newSz - deltDif;
            }
            if (adjSz < MIN_SIZE) adjSz = MIN_SIZE;
            if (minus) delt *= -1;

            return adjSz;
        }
        #endregion

        #region Drag-n-Drop Support
        private void DragCreateCursor(int location)
        {
            Bitmap piece = GetSquareImage(location);

            if (piece != null) {
                checkerMoveCursor = CursorUtil.CreateCursor(piece, piece.Width / 2, piece.Height / 2);
            }
            useCustomCursors = ((checkerMoveCursor != null) && (checkerNoneCursor != null));
        }

        private void DragDisposeCursor()
        {
            useCustomCursors = false;
            if (checkerMoveCursor != null) checkerMoveCursor.Dispose();
        }

        public void CheckerBoard_MouseDown(object sender, MouseEventArgs e)
        {
            if ((sender is BoardSquare) && (!_autoMove) && (e.Button != MouseButtons.Right)) {
                int loc = (sender as BoardSquare).BoardLocation;
                origDragImage = SquareAt(loc).Image;
                if (origDragImage != null) {
                    dragStartLoc = loc;
                    DragCreateCursor(loc);
                    dragStarting = true;
                    if (DoDragDrop(origDragImage, DragDropEffects.Move) == DragDropEffects.Move) {
                        if (_dragMoveComplete != null) _dragMoveComplete(dragEndLoc);
                    }
                    else {
                        DragDisposeCursor();
                        SquareAt(dragStartLoc).Image = origDragImage;
                    }
                }
            }
        }

        public void CheckerBoard_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (useCustomCursors) {
                // Sets the custom cursor based upon the effect.
                e.UseDefaultCursors = false;
                if ((e.Effect & DragDropEffects.Move) == DragDropEffects.Move)
                    Cursor.Current = checkerMoveCursor;
                else
                    Cursor.Current = checkerNoneCursor;
                if (dragStarting) {
                    // remove checker from start loc
                    SquareAt(dragStartLoc).Image = null;
                    dragStarting = false;
                }
            }
            else
                e.UseDefaultCursors = true;
        }

        public void CheckerBoard_DragEnter(object sender, DragEventArgs e)
        {
            if (sender is BoardSquare) {
                int loc = (sender as BoardSquare).BoardLocation;
                if ((_canDragTo != null) && (_canDragTo(dragStartLoc, loc)))
                    e.Effect = DragDropEffects.Move;
                else
                    e.Effect = DragDropEffects.None;
            }
        }

        public void CheckerBoard_DragDrop(object sender, DragEventArgs e)
        {
            if (sender is BoardSquare) {
                dragEndLoc = (sender as BoardSquare).BoardLocation;
                if (_dragDropped != null) _dragDropped(dragStartLoc, dragEndLoc, origDragImage);
            }
        }
        #endregion

        // --------------------------------------------------------------------

        #region Protected Methods (OnXXXX events)
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;

            // Clear component in the border color, board border and square
            // borders are all this color.
            graphics.Clear(_borderColor);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if ((OldWidth == 0) || (OldHeight == 0)) return;

            int hDelt = 0, wDelt = 0;

            if (OldHeight != this.Height) {
                this.Height = AdjustSize(OldHeight, this.Height, ref hDelt);
                OldHeight = this.Height;
            }

            if (OldWidth != this.Width) {
                this.Width = AdjustSize(OldWidth, this.Width, ref wDelt);
                OldWidth = this.Width;
            }

            AdjustSquares(hDelt, wDelt);
        }
        #endregion

        // --------------------------------------------------------------------

        /*
         * Method returns the BoardSquare component at the given boardLocation
         * value. The boardLocation value needs to be between 0 and 63. If
         * the range is out of bounds, the board square at location 0 is
         * returned (the first square).
         * Board locations are as the following diagram:
         *  +---+---+---+---+---+---+---+---+
         *  | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 |
         *  +---+---+---+---+---+---+---+---+
         *  | 8 | 9 | 10| 11| 12| 13| 14| 15|
         *  +---+---+---+---+---+---+---+---+
         *  | 16| 17| 18| 19| 20| 21| 22| 23|
         *  +---+---+---+---+---+---+---+---+
         *  | 24| 25| 26| 27| 28| 29| 30| 31|
         *  +---+---+---+---+---+---+---+---+
         *  | 32| 33| 34| 35| 36| 37| 38| 39|
         *  +---+---+---+---+---+---+---+---+
         *  | 40| 41| 42| 43| 44| 45| 46| 47|
         *  +---+---+---+---+---+---+---+---+
         *  | 48| 49| 50| 51| 52| 53| 54| 55|
         *  +---+---+---+---+---+---+---+---+
         *  | 56| 57| 58| 59| 60| 61| 62| 63|
         *  +---+---+---+---+---+---+---+---+
         *  
         */
        public BoardSquare SquareAt(int boardLocation)
        {
            BoardSquare square = squares[0];

            // already have squares[0], don't need to re-assign
            if ((boardLocation > 0) && (boardLocation < NUM_SQUARES))
                square = squares[boardLocation];

            return square;
        }

        /*
         * Method used to retrieve the image (if any) on the given board
         * square location. Will return a null if no image is currently
         * at that location. Will return the image (if any) at location
         * 0 if the squareLocation value is not between 0 and 63.
         */
        public Bitmap GetSquareImage(int squareLocation)
        {
            return SquareAt(squareLocation).Image;
        }

        /*
         * Method sets an image at a given square location. Will not set the
         * image if the squareLocation is not between 0 and 63.
         */
        public void SetSquareImage(Bitmap image, int squareLocation)
        {
            if ((squareLocation < 0) || (squareLocation > NUM_SQUARES - 1))
                return;

            SquareAt(squareLocation).Image = image;
        }

        /*
         * Method used to clear the board of all pieces.
         */
        public void ClearBoard()
        {
            for (int i = 0; i < NUM_SQUARES; i++) SetSquareImage(null, i);
        }
    }
}
