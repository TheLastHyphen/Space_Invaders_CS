using Godot;

public partial class InvaderSquid : InvaderBase
{
	public override int Points => 30;
	public override string InvaderName => "I am an Invader Squid";

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