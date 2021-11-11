using System;
using System.Collections.Generic;

namespace Checkers
{
    /*
     * Enum representing the 'color' of the checkers for the checker rule
     * engine / piece tracker. Black is player1, top three rows of
     * checkers at game start. Red is player2, bottom three rows of checkers
     * at game start.
     */
    public enum PlayerColors { Black, Red }

    public class CheckerPiece
    {
        public const char RED_CHAR = 'r';
        public const char RED_KING_CHAR = 'R';
        public const char BLACK_CHAR = 'b';
        public const char BLACK_KING_CHAR = 'B';
        public const char NO_PIECE = ' ';
        
        #region Move Arrays
        private static readonly int[,] DefBlackMoves = new[,] {
            {-1, -1}, {8, 10},  {-1, -1}, {10, 12}, {-1, -1}, {12, 14}, {-1, -1}, {14, -1},
            {-1, 17}, {-1, -1}, {17, 19}, {-1, -1}, {19, 21}, {-1, -1}, {21, 23}, {-1, -1},
            {-1, -1}, {24, 26}, {-1, -1}, {26, 28}, {-1, -1}, {28, 30}, {-1, -1}, {30, -1},
            {-1, 33}, {-1, -1}, {33, 35}, {-1, -1}, {35, 37}, {-1, -1}, {37, 39}, {-1, -1},
            {-1, -1}, {40, 42}, {-1, -1}, {42, 44}, {-1, -1}, {44, 46}, {-1, -1}, {46, -1},
            {-1, 49}, {-1, -1}, {49, 51}, {-1, -1}, {51, 53}, {-1, -1}, {53, 55}, {-1, -1},
            {-1, -1}, {56, 58}, {-1, -1}, {58, 60}, {-1, -1}, {60, 62}, {-1, -1}, {62, -1},
            {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1} };

        private static readonly int[,] AltBlackMoves = new[,] {
            {-1, 9},  {-1, -1}, {9, 11},  {-1, -1}, {11, 13}, {-1, -1}, {13, 15}, {-1, -1},
            {-1, -1}, {16, 18}, {-1, -1}, {18, 20}, {-1, -1}, {20, 22}, {-1, -1}, {22, -1},
            {-1, 25}, {-1, -1}, {25, 27}, {-1, -1}, {27, 29}, {-1, -1}, {29, 31}, {-1, -1},
            {-1, -1}, {32, 34}, {-1, -1}, {34, 36}, {-1, -1}, {36, 38}, {-1, -1}, {38, -1},
            {-1, 41}, {-1, -1}, {41, 43}, {-1, -1}, {43, 45}, {-1, -1}, {45, 47}, {-1, -1},
            {-1, -1}, {48, 50}, {-1, -1}, {50, 52}, {-1, -1}, {52, 54}, {-1, -1}, {54, -1},
            {-1, 57}, {-1, -1}, {57, 59}, {-1, -1}, {59, 61}, {-1, -1}, {61, 63}, {-1, -1},
            {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1} };

        private static readonly int[,] DefRedMoves = new[,] {
            {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1},
            {-1, 1},  {-1, -1}, {1, 3},   {-1, -1}, {3, 5},   {-1, -1}, {5, 7},   {-1, -1},
            {-1, -1}, {8, 10},  {-1, -1}, {10, 12}, {-1, -1}, {12, 14}, {-1, -1}, {14, -1},
            {-1, 17}, {-1, -1}, {17, 19}, {-1, -1}, {19, 21}, {-1, -1}, {21, 23}, {-1, -1},
            {-1, -1}, {24, 26}, {-1, -1}, {26, 28}, {-1, -1}, {28, 30}, {-1, -1}, {30, -1},
            {-1, 33}, {-1, -1}, {33, 35}, {-1, -1}, {35, 37}, {-1, -1}, {37, 39}, {-1, -1},
            {-1, -1}, {40, 42}, {-1, -1}, {42, 44}, {-1, -1}, {44, 46}, {-1, -1}, {46, -1},
            {-1, 49}, {-1, -1}, {49, 51}, {-1, -1}, {51, 53}, {-1, -1}, {53, 55}, {-1, -1} };

        private static readonly int[,] AltRedMoves = new[,] {
            {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}, {-1, -1},
            {-1, -1}, {0, 2},   {-1, -1}, {2, 4},   {-1, -1}, {4, 6},   {-1, -1}, {6, -1},
            {-1, 9},  {-1, -1}, {9, 11},  {-1, -1}, {11, 13}, {-1, -1}, {13, 15}, {-1, -1},
            {-1, -1}, {16, 18}, {-1, -1}, {18, 20}, {-1, -1}, {20, 22}, {-1, -1}, {22, -1},
            {-1, 25}, {-1, -1}, {25, 27}, {-1, -1}, {27, 29}, {-1, -1}, {29, 31}, {-1, -1},
            {-1, -1}, {32, 34}, {-1, -1}, {34, 36}, {-1, -1}, {36, 38}, {-1, -1}, {38, -1},
            {-1, 41}, {-1, -1}, {41, 43}, {-1, -1}, {43, 45}, {-1, -1}, {45, 47}, {-1, -1},
            {-1, -1}, {48, 50}, {-1, -1}, {50, 52}, {-1, -1}, {52, 54}, {-1, -1}, {54, -1}, };
        #endregion

        #region Properties
        public int PieceLocation { get; set; }
        public bool IsKing { get; set; }
        public PlayerColors PlayerColor { get; set; }
        public List<int> PossibleMoveLocations { get; set; }
        public List<int> PossibleJumpLocations { get; set; }
        #endregion

        private int[,] BlackMoves = DefBlackMoves;
        private int[,] RedMoves = DefRedMoves;

        // --------------------------------------------------------------------

        /*
         * Constructor to create a new checker piece tracking instance. There
         * are 12 black and 12 red created at game start.
         */
        public CheckerPiece(PlayerColors playerColor, int pieceLocation, bool isKing,
                            bool useAltPositions)
        {
            PlayerColor = playerColor;
            PieceLocation = pieceLocation;
            IsKing = isKing;
            PossibleMoveLocations = new List<int>();
            PossibleJumpLocations = new List<int>();
            if (useAltPositions) {
                BlackMoves = AltBlackMoves;
                RedMoves = AltRedMoves;
            }
        }

        // --------------------------------------------------------------------

        #region Private Methods
        private void FindMoves(char[] GameboardArray, int[,] PlayerMoves, int[,] OppoMoves)
        {
            if ((PlayerMoves[PieceLocation, 0] >= 0) &&
                (GameboardArray[PlayerMoves[PieceLocation, 0]] == ' ')) {
                PossibleMoveLocations.Add(PlayerMoves[PieceLocation, 0]);
            }

            if ((PlayerMoves[PieceLocation, 1] >= 0) &&
                (GameboardArray[PlayerMoves[PieceLocation, 1]] == ' ')) {
                PossibleMoveLocations.Add(PlayerMoves[PieceLocation, 1]);
            }

            if (IsKing) {
                if ((OppoMoves[PieceLocation, 0] >= 0) &&
                    (GameboardArray[OppoMoves[PieceLocation, 0]] == ' ')) {
                    PossibleMoveLocations.Add(OppoMoves[PieceLocation, 0]);
                }

                if ((OppoMoves[PieceLocation, 1] >= 0) &&
                    (GameboardArray[OppoMoves[PieceLocation, 1]] == ' ')) {
                    PossibleMoveLocations.Add(OppoMoves[PieceLocation, 1]);
                }
            }
        }

        private void FindJumps(char[] GameboardArray, int[,] PlayerMoves, int[,] OppoMoves,
                               char oppo1, char oppo2)
        {
            if (PlayerMoves[PieceLocation, 0] >= 0 &&
                (GameboardArray[PlayerMoves[PieceLocation, 0]] == oppo1 ||
                GameboardArray[PlayerMoves[PieceLocation, 0]] == oppo2) &&
                PlayerMoves[PlayerMoves[PieceLocation, 0], 0] >= 0 &&
                GameboardArray[PlayerMoves[PlayerMoves[PieceLocation, 0], 0]] == ' ') {
                PossibleJumpLocations.Add(PlayerMoves[PlayerMoves[PieceLocation, 0], 0]);
            }

            if (PlayerMoves[PieceLocation, 1] >= 0 &&
                (GameboardArray[PlayerMoves[PieceLocation, 1]] == oppo1 ||
                GameboardArray[PlayerMoves[PieceLocation, 1]] == oppo2) &&
                PlayerMoves[PlayerMoves[PieceLocation, 1], 1] >= 0 &&
                GameboardArray[PlayerMoves[PlayerMoves[PieceLocation, 1], 1]] == ' ') {
                PossibleJumpLocations.Add(PlayerMoves[PlayerMoves[PieceLocation, 1], 1]);
            }

            if (IsKing) {
                if (OppoMoves[PieceLocation, 0] >= 0 &&
                    (GameboardArray[OppoMoves[PieceLocation, 0]] == oppo1 ||
                    GameboardArray[OppoMoves[PieceLocation, 0]] == oppo2) &&
                    OppoMoves[OppoMoves[PieceLocation, 0], 0] >= 0 &&
                    GameboardArray[OppoMoves[OppoMoves[PieceLocation, 0], 0]] == ' ') {
                    PossibleJumpLocations.Add(OppoMoves[OppoMoves[PieceLocation, 0], 0]);
                }

                if (OppoMoves[PieceLocation, 1] >= 0 &&
                    (GameboardArray[OppoMoves[PieceLocation, 1]] == oppo1 ||
                    GameboardArray[OppoMoves[PieceLocation, 1]] == oppo2) &&
                    OppoMoves[OppoMoves[PieceLocation, 1], 1] >= 0 &&
                    GameboardArray[OppoMoves[OppoMoves[PieceLocation, 1], 1]] == ' ') {
                    PossibleJumpLocations.Add(OppoMoves[OppoMoves[PieceLocation, 1], 1]);
                }
            }
        }
        #endregion

        // --------------------------------------------------------------------

        /*
         * Method to return the 'char' representing a particular checker piece
         * while still in the checker board. Each square on the checker board
         * will have either a ' ' (no piece) or one of the 4 characters representing
         * black, crowned black, red or crowned red pieces.
         */
        public char GetPlayCharacter()
        {
            char PlayerPiece;

            if (PlayerColor == PlayerColors.Black)
                PlayerPiece = IsKing ? BLACK_KING_CHAR : BLACK_CHAR;
            else
                PlayerPiece = IsKing ? RED_KING_CHAR : RED_CHAR;

            return PlayerPiece;
        }

        /*
         * Method used by a piece instance to determine if this piece at this
         * location on the board has moves available to make. All possible moves
         * by a checker piece are stored within the PossibleMoveLocations
         * property. If no moves are possible, this property is an empty list.
         */
        public void FindAllMoves(char[] GameboardArray)
        {
            PossibleMoveLocations.Clear();

            if (PlayerColor == PlayerColors.Black)
                FindMoves(GameboardArray, BlackMoves, RedMoves);
            else
                FindMoves(GameboardArray, RedMoves, BlackMoves);
        }

        /*
         * Method used by a piece instance to determine if this piece at this
         * location on the board has jumps available to make. All possible jumps
         * by a checker piece are stored within the PossibleJumpLocations
         * property. If no moves are possible, this property is an empty list.
         */
        public void FindAllJumps(char[] GameboardArray)
        {
            PossibleJumpLocations.Clear();

            if (PlayerColor == PlayerColors.Black)
                FindJumps(GameboardArray, BlackMoves, RedMoves, RED_CHAR, RED_KING_CHAR);
            else
                FindJumps(GameboardArray, RedMoves, BlackMoves, BLACK_CHAR, BLACK_KING_CHAR);
        }

        // --------------------------------------------------------------------

        /*
         * Method used to return a string representing the checker piece
         * instance. Mainly used for debugging and testing.
         */
        public override string ToString()
        {
            return "CheckerPiece: [Color: " + PlayerColor.ToString() +
                ", Location: " + Convert.ToString(PieceLocation) +
                ", Crowned: " + Convert.ToString(IsKing) + "]";
        }
    }
}
