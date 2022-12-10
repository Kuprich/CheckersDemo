﻿using CheckersDemo.Client.Data;

namespace CheckersDemo.Client.Extensions;


public static class ListExtensions
{
    public static void AddCellsRange(this List<Cell> list, IEnumerable<Cell> cells)
    {
        foreach (var cell in cells)
        {
            list.AddCell(cell);
        }
    }

    public static void AddCell(this List<Cell> list, Cell cell)
    {
        if (cell.Row >= 0 && cell.Row <= 7 && 
            cell.Col >=0 && cell.Col <=7)
        {
            list.Add(cell);
        }
    }
}