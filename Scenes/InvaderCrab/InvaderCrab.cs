using Godot;

public partial class InvaderCrab : InvaderBase
{
	public override int Points => 20;
	public override string InvaderName => "I am Invader Crab";
	public override float BombDropChance => 0.015f;

	public override void _Ready()
	{
		base._Ready();
	}
}
