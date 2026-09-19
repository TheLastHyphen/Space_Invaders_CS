using Godot;

public partial class InvaderOctopus : InvaderBase
{
	public override int Points => 30;
	public override string InvaderName => "I and Invader Octopus";
	public override float BombDropChance => 0.035f;


	public override void _Ready()
	{
		base._Ready();
	}
}
