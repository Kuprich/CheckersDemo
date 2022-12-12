using CheckersDemo.Client.Extensions;

namespace CheckersDemo.Client.Data;

public class CheckersBoard
{
    public List<Checker> Checkers { get; set; } = new();

    public List<Cell> CellsPossible = new();
    public Checker? ActiveChecker { get; set; }
    public Cell[]? ActiveCells => GetPossibleMoves(ActiveChecker).Select(x => x.To).Distinct().ToArray();
    public Checker? AttaсkingChecker { get; set; }
    public List<Checker> JumpedCheckers { get; private set; } = new();
    public bool WhiteTurn { get; set; } = true;
    public List<Checker> EnabledCheckers { get; private set; } = new();
    public Checker? GetChecker(int row, int col) => Checkers.FirstOrDefault(x => x.Cell.Equals(row, col));
    public Checker? GetChecker(Cell cell) => GetChecker(cell.Row, cell.Col);

    public CheckersBoard()
    {
        CreateNewGame();
        //InitializeBoard();
        //UpdateEnabledCheckers();
    }

    public void InitializeBoard()
    {
        Checkers.Clear();

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
    public void MoveActiveCheckerTo(Cell cell)
    {
        if (ActiveChecker == null) return;

        MoveInfo? move = GetPossibleMoves(ActiveChecker).FirstOrDefault(x => x.From == ActiveChecker.Cell && x.To == cell);

        if (move == null) return;

        ActiveChecker.Cell = cell;

        if (cell.Row == 0 && ActiveChecker.IsWhite ||
            cell.Row == 7 && !ActiveChecker.IsWhite)
        {
            ActiveChecker.Direction = CheckerDirection.Both;
        }

        if (move is JumpedMoveInfo jumpedMove)
        {
            JumpedCheckers.Add(jumpedMove.JumpedChecker);
            if (GetPossibleMoves(ActiveChecker).Any(x => x is JumpedMoveInfo))
            {
                return;
            }
        }


        RemoveJumpedCheckers();

        WhiteTurn = !WhiteTurn;
        ActiveChecker = null;

        if (GameIsEnd())
        {
            CreateNewGame();
        }

        UpdateEnabledCheckers();
    }

    private void RemoveJumpedCheckers()
    {
        if (JumpedCheckers != null && JumpedCheckers.Any())
        {
            foreach (var jumpedChecker in JumpedCheckers!)
                Checkers.Remove(jumpedChecker);
            JumpedCheckers.Clear();
        }
    }
    private void CreateNewGame()
    {
        InitializeBoard();
        UpdateEnabledCheckers();
        WhiteTurn = true;
    }

    private bool GameIsEnd()
    {
        if (!Checkers.Any(x => x.IsWhite) || !Checkers.Any(x => !x.IsWhite))
        {
            return true;
        }
        return false;
    }
    private List<MoveInfo> GetPossibleMoves(Checker? checker)
    {
        if (checker == null) return new();

        List<MoveInfo> result = new();

        List<Cell> possibleMoveCells = new();
        List<Cell> possibleJumpCells = new();

        //simple checker
        if (checker.Direction != CheckerDirection.Both)
        {
            //jump: 
            possibleJumpCells.AddCellsRange(new[]
            {
                new Cell(checker.Cell.Row + 2, checker.Cell.Col + 2),
                new Cell(checker.Cell.Row + 2, checker.Cell.Col - 2),
                new Cell(checker.Cell.Row - 2, checker.Cell.Col + 2),
                new Cell(checker.Cell.Row - 2, checker.Cell.Col - 2),
            });

            foreach (var cell in possibleJumpCells)
            {
                if (GetChecker(cell) != null) continue;

                var possibleJumpedChecker = GetChecker((checker.Cell.Row + cell.Row) / 2, (checker.Cell.Col + cell.Col) / 2);
                if (possibleJumpedChecker != null && checker.IsWhite != possibleJumpedChecker.IsWhite && !JumpedCheckers.Contains(possibleJumpedChecker))
                {
                    result.Add(new JumpedMoveInfo(checker.Cell, cell, possibleJumpedChecker));
                }

            }

            if (result.Any()) return result;

            // move: 
            int row = checker.Cell.Row + (checker.Direction == CheckerDirection.Down ? 1 : -1);
            possibleMoveCells.AddCellsRange(new[] { new Cell(row, checker.Cell.Col + 1), new Cell(row, checker.Cell.Col - 1) });

            foreach (var cell in possibleMoveCells)
                if (GetChecker(cell) == null)
                    result.Add(new SimpleMoveInfo(checker.Cell, cell));
        }

        //King checker (both checker direction)
        else
        {

            var cellLines = new List<List<Cell>>
            {
                new List<Cell>().AddCellsRange(Enumerable.Range(1, 7).Select(i => new Cell(checker.Cell.Row - i, checker.Cell.Col - i)).ToArray()),
                new List<Cell>().AddCellsRange(Enumerable.Range(1, 7).Select(i => new Cell(checker.Cell.Row - i, checker.Cell.Col + i)).ToArray()),
                new List<Cell>().AddCellsRange(Enumerable.Range(1, 7).Select(i => new Cell(checker.Cell.Row + i, checker.Cell.Col - i)).ToArray()),
                new List<Cell>().AddCellsRange(Enumerable.Range(1, 7).Select(i => new Cell(checker.Cell.Row + i, checker.Cell.Col + i)).ToArray())
            };

            //jump: 
            foreach (var cellLine in cellLines)
            {
                if (cellLine.Count <= 1) continue;

                for (int i = 0; i < cellLine.Count - 1; i++)
                {
                    var possibleJumpedChecker = GetChecker(cellLine[i]);

                    if (possibleJumpedChecker == null) continue;
                    if (possibleJumpedChecker.IsWhite == checker.IsWhite) break;

                    if (JumpedCheckers.Contains(possibleJumpedChecker)) continue;

                    if (GetChecker(cellLine[i + 1]) != null) break;

                    if (i + 1 >= cellLine.Count - 1)
                        result.Add(new JumpedMoveInfo(checker.Cell, cellLine[i + 1], possibleJumpedChecker));
                    else
                    {
                        for (int j = i + 1; j < cellLine.Count; j++)
                        {
                            if (GetChecker(cellLine[j]) != null) break;
                            if (CheckJumpPossibilityForKingChecker(cellLine[j], possibleJumpedChecker))
                                result.Add(new JumpedMoveInfo(checker.Cell, cellLine[j], possibleJumpedChecker));
                        }


                        if (!result.Any())
                            for (int j = i + 1; j < cellLine.Count; j++)
                            {
                                if (GetChecker(cellLine[j]) != null) break;
                                result.Add(new JumpedMoveInfo(checker.Cell, cellLine[j], possibleJumpedChecker));
                            }

                    }

                    break;
                }
            }

            if (result.Any()) return result;

            //simple move: 
            foreach (var cellLine in cellLines)
                foreach (var cell in cellLine)
                {
                    if (GetChecker(cell) != null) break;
                    result.Add(new SimpleMoveInfo(checker.Cell, cell));
                }

        }

        return result;
    }
    private bool CheckJumpPossibilityForKingChecker(Cell cell, Checker jumpedChecker)
    {

        var cellLines = new List<List<Cell>>
            {
                new List<Cell>().AddCellsRange(Enumerable.Range(1, 7).Select(i => new Cell(cell.Row - i, cell.Col - i)).ToArray()),
                new List<Cell>().AddCellsRange(Enumerable.Range(1, 7).Select(i => new Cell(cell.Row - i, cell.Col + i)).ToArray()),
                new List<Cell>().AddCellsRange(Enumerable.Range(1, 7).Select(i => new Cell(cell.Row + i, cell.Col - i)).ToArray()),
                new List<Cell>().AddCellsRange(Enumerable.Range(1, 7).Select(i => new Cell(cell.Row + i, cell.Col + i)).ToArray())
            };

        //jump:
        foreach (var cellLine in cellLines)
        {
            if (cellLine.Count <= 1) continue;



            if (JumpedCheckers.Any() && cellLine.Contains(JumpedCheckers.Last().Cell) || cellLine.Contains(jumpedChecker.Cell)) continue;

            for (int i = 0; i < cellLine.Count - 1; i++)
            {
                var possibleJumpedChecker = GetChecker(cellLine[i]);

                if (possibleJumpedChecker == null) continue;
                if (possibleJumpedChecker.IsWhite == WhiteTurn) break;

                if (JumpedCheckers.Contains(possibleJumpedChecker)) continue;

                if (GetChecker(cellLine[i + 1]) != null) break;

                return true;
            }
        }
        return false;
    }
    private void UpdateEnabledCheckers()
    {
        EnabledCheckers.Clear();

        Checker[] checkersToJump = Checkers.Where(checker => GetPossibleMoves(checker).Any(x => x is JumpedMoveInfo && checker.IsWhite == WhiteTurn)).ToArray();

        foreach (var cheker in Checkers)
        {
            if (!GetPossibleMoves(cheker).Any()) continue;

            if (cheker.IsWhite && !WhiteTurn ||
                !cheker.IsWhite && WhiteTurn) continue;

            if (checkersToJump.Any())
            {
                if (checkersToJump.Contains(cheker))
                    EnabledCheckers.Add(cheker);
            }

            else
                EnabledCheckers.Add(cheker);
        }
    }


}
