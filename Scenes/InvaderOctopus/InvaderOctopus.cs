using Godot;

public partial class InvaderOctopus : InvaderBase
{
	public override int Points => 10;
	public override string InvaderName => "I and Invader Octopus";

	private float _bombDropChance = 0.035f;
	public override float BombDropChance
	{
		get => _bombDropChance;
		set => value = _bombDropChance;
	}

	public override void _Ready()
	{
		base._Ready();
	}
}
