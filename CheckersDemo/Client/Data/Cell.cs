using System.Diagnostics.CodeAnalysis;

namespace CheckersDemo.Client.Data;

public struct Cell
{
    public Cell(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public int Row { get; set; }
    public int Col { get; set; }

    public bool Equals(int row, int col)
    {
        return row == Row && col == Col;
    }
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Cell cell)
        {
            return cell.Row == Row && cell.Col == Col;
        }

        return false;
    }

    public static bool operator ==(Cell left, Cell right)
    {
        return left.Col == right.Col && left.Row == right.Row;
    }

    public static bool operator !=(Cell left, Cell right)
    {
        return !(left == right);
    }
}