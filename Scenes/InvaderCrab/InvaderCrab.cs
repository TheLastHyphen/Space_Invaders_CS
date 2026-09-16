using Godot;

public partial class InvaderCrab : InvaderBase
{
	public override int Points => 10;
	public override string Name => "I am Invader Crab";

	public override void _Ready()
	{
		base._Ready();
	}
}
