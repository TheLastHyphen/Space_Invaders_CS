using Godot;

public partial class InvaderCrab : InvaderBase
{
	protected override uint Points => 20;
	protected override string InvaderName => "I am Invader Crab";

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
