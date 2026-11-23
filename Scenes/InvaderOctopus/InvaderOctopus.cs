using Godot;

public partial class InvaderOctopus : InvaderBase
{
	protected override uint Points => 10;
	protected override string InvaderName => "I and Invader Octopus";

	AnimatedSprite2D _invAnimatedSprite;
	public override AnimatedSprite2D InvAnimatedSprite => _invAnimatedSprite;

	private float _bombDropChance = 0.035f;
	
	public override float BombDropChance
	{
		get => _bombDropChance;
		set => value = _bombDropChance;
	}

	public override void _Ready()
	{
		_invAnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		base._Ready();
	}
}
