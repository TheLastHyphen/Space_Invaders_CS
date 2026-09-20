using Godot;

public partial class InvaderSquid : InvaderBase
{
	public override int Points => 30;
	public override string InvaderName => "I am an Invader Squid";
	public override float BombDropChance => 0.075f;

	public override void _Ready()
	{		
		base._Ready();
	}
}