using System.Drawing;
using System.Windows.Forms;

/*
 * Component implementing a checker (or chess) board square. This component
 * should only be created/used by the CheckerBoard component. Has the
 * BoardLocation index as a property (will be a value between 0 and 63).
 * If the Image property is set, will draw the image on a paint event. The
 * image will be centered in the square if possible.
 * 
 * Author: Michael G. Slack
 * Date Written: 2014-03-28
 * 
 * ----------------------------------------------------------------------------
 * 
 * Revised: yyyy-mm-dd - XXXX.
 * 
 */
namespace Checkers
{
    public partial class BoardSquare : UserControl
    {
        #region Properties
        private int _boardLocation = 0;
        public int BoardLocation {
            get { return _boardLocation; }
            set { _boardLocation = value; }
        }

        private Bitmap _image = null;
        public Bitmap Image {
            get { return _image; }
            set { _image = value; this.Invalidate(); }
        }
        #endregion

        // --------------------------------------------------------------------

        public BoardSquare()
        {
            InitializeComponent();
        }

        // --------------------------------------------------------------------

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;

            if (_image != null) {
                int x = 0, y = 0;

                if (_image.Width < this.Width) x = (this.Width - _image.Width) / 2;
                if (_image.Height < this.Height) y = (this.Height - _image.Height) / 2;

                graphics.DrawImage(_image, new Point(x, y));
            }
        }

        // --------------------------------------------------------------------

        /*
         * Method used to pass along the drag-n-drop events to the parent
         * board component. Mainly called by the board component when it
         * initializes the squares on the board.
         */
        public void AssignDragDropEvents(Control parent)
        {
            AllowDrop = true;
            this.GiveFeedback += ((CheckerBoard) parent).CheckerBoard_GiveFeedback;
            this.MouseDown += ((CheckerBoard) parent).CheckerBoard_MouseDown;
            this.DragEnter += ((CheckerBoard) parent).CheckerBoard_DragEnter;
            this.DragDrop += ((CheckerBoard) parent).CheckerBoard_DragDrop;
        }
    }
}
