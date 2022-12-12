namespace CheckersDemo.Client.Data;

public abstract class MoveInfo
{
	public MoveInfo(Cell from, Cell to)
	{
		From = from;
		To = to;
	}

	public Cell From { get; }
	public Cell To { get; }
}

public class SimpleMoveInfo : MoveInfo
{
	public SimpleMoveInfo(Cell from, Cell to) : base(from, to) { }
}

public class JumpedMoveInfo : MoveInfo
{
	public JumpedMoveInfo(Cell from, Cell to, Checker jumpedChecker) : base(from, to)
	{
		JumpedChecker = jumpedChecker;
	}
	public Checker JumpedChecker { get; }
}
