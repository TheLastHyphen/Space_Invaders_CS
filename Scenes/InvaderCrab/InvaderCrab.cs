using Godot;

public partial class InvaderCrab : InvaderBase
{
	public override int Points => 20;
	public override string InvaderName => "I am Invader Crab";

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
