using CheckersDemo.Client.Extensions;

namespace CheckersDemo.Client.Data;

public class CheckersBoard
{
    public List<Checker> Checkers { get; set; } = new();

    public Checker? BeingAttackedChecker { get; set; }

    public List<Cell> CellsPossible = new();

    public Checker? ActiveChecker { get; set; }
    public Cell[]? ActiveCells => GetPossibleMoves(ActiveChecker).ToArray();

    public Checker? AttaсkingChecker { get; set; }

    public bool WhiteTurn { get; set; } = true;

    public Checker[] EnabledCheckers => GetEnabledCheckers().ToArray();

    public Checker? GetChecker(int row, int col) => Checkers.FirstOrDefault(x => x.Cell.Equals(row, col));
    public Checker? GetChecker(Cell cell) => GetChecker(cell.Row, cell.Col);


    public CheckersBoard()
    {
        InitializeBoard();
    }

    public void InitializeBoard()
    {
        for (int row = 0; row < 8; row++)
            for (int col = (row + 1) % 2; col < 8; col += 2)
            {
                var checker = row switch
                {
                    < 3 => new Checker(row, col, CheckerDirection.Down, isWhite: false),
                    > 4 => new Checker(row, col, CheckerDirection.Up, isWhite: true),
                    _ => default,
                };

                if (checker != null)
                    Checkers.Add(checker);
            }
    }

    public List<Cell> GetPossibleMoves(Checker? checker)
    {
        if (checker == null) return new();

        List<Cell> result = new();

        List<Cell> possibleCells = new();

        if (checker.Direction != CheckerDirection.Both)
        {

            int row = checker.Cell.Row + (checker.Direction == CheckerDirection.Down ? 1 : -1);
            possibleCells.AddCellsRange(new[] { new Cell(row, checker.Cell.Col + 1), new Cell(row, checker.Cell.Col - 1) });

            foreach (var cell in possibleCells)
                if (GetChecker(cell) == null)
                    result.Add(cell);
        }

        else
        {
            //TODO define possible moves for Both direction; (King checker)
        }

        return result;

    }

    private IEnumerable<Checker> GetEnabledCheckers()
    {
        foreach (var cheker in Checkers)
        {
            if (!GetPossibleMoves(cheker).Any()) continue;

            if (cheker.IsWhite && !WhiteTurn ||
                !cheker.IsWhite && WhiteTurn) continue;

             yield return cheker;
        }
    }

    //public List<Cell> EvaluateSpotForMove(Checker checker)
    //{
    //    // for simple 
    //    List<Cell> result = new();

    //    List<int> rowsPossibleForMove = new();
    //    List<int> colsPossibleForMove = new();

    //    if (checker.Direction == CheckerDirection.Both)
    //    {
    //        rowsPossibleForMove.AddBoardValues(new[] { checker.Cell.Row + 1, checker.Cell.Row - 1 });
    //    }
    //    else
    //    {
    //        rowsPossibleForMove.AddBoardValue(checker.Cell.Row + (1 * (checker.Direction == CheckerDirection.Down ? 1 : -1)));
    //    }

    //    colsPossibleForMove.AddBoardValues(new[] { checker.Cell.Col + 1, checker.Cell.Col - 1 });

    //    foreach (int row in rowsPossibleForMove)
    //        foreach (int col in colsPossibleForMove)
    //        {
    //            if (Math.Abs(checker.Cell.Col - col) != Math.Abs(checker.Cell.Row - row)) continue;

    //            if (GetChecker(row, col) != null) continue;

    //            result.Add(new(row, col));
    //        }
    //    return result;
    //}

    //public List<Cell> EvaluateSpotForJump(Checker checker)
    //{
    //    List<Cell> result = new();
    //    BeingAttackedChecker = null;

    //    List<int> rowsPossibleForJump = new();
    //    List<int> colsPossibleForJump = new();

    //    rowsPossibleForJump.AddBoardValues(new[] { checker.Cell.Row + 2, checker.Cell.Row - 2 });
    //    colsPossibleForJump.AddBoardValues(new[] { checker.Cell.Col + 2, checker.Cell.Col - 2 });

    //    foreach (int row in rowsPossibleForJump)
    //        foreach (int col in colsPossibleForJump)
    //        {
    //            if (Math.Abs(checker.Cell.Col - col) != Math.Abs(checker.Cell.Row - row)) continue;

    //            if (GetChecker(row, col) != null) continue;

    //            var possibleAttackedChecker = Checkers.FirstOrDefault(x => x.Cell.Row == (checker.Cell.Row + row) / 2 && x.Cell.Col == (checker.Cell.Col + col) / 2);
    //            if (possibleAttackedChecker != null && checker.IsWhite != possibleAttackedChecker.IsWhite)
    //            {
    //                BeingAttackedChecker = possibleAttackedChecker;
    //                result.Add(new(row, col));
    //            }
    //        }

    //    return result;
    //}

    //public void MoveChecker(int row, int col)
    //{

    //    bool canMoveHere = CellsPossible.Contains(new(row, col));
    //    if (!canMoveHere) return;

    //    bool isContinueJump = false;

    //    if (ActiveChecker != null)
    //    {
    //        ActiveChecker.Cell.Col = col;
    //        ActiveChecker.Cell.Row = row;

    //        if (ActiveChecker.Cell.Row == 0 && ActiveChecker.IsWhite ||
    //            ActiveChecker.Cell.Row == 7 && !ActiveChecker.IsWhite)
    //        {
    //            ActiveChecker.Direction = CheckerDirection.Both;
    //        }

    //        if (BeingAttackedChecker != null)
    //        {
    //            Checkers.Remove(BeingAttackedChecker);

    //            // Check the possibility of continuing the jump
    //            isContinueJump = EvaluateSpotForJump(ActiveChecker).Any();

    //            if (isContinueJump)
    //            {
    //                AttaсkingChecker = ActiveChecker;
    //                CellsPossible = EvaluateSpotForJump(AttaсkingChecker);
    //                return;
    //            }
    //        }


    //    }

    //    AttaсkingChecker = null;
    //    WhiteTurn = !WhiteTurn;
    //    ActiveChecker = null;
    //    CellsPossible.Clear();

    //}


}
