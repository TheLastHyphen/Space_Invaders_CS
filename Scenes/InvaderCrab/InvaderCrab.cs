using Godot;

public partial class InvaderCrab : InvaderBase
{
	public override int InvaderPoints => 10;
	public override string InvaderName => "I am Invader Crab";
	public override AnimatedSprite2D AnimatedSprite => GetNode<AnimatedSprite2D>("AnimatedSprite2D");


	public override void _Ready()
	{
		base._Ready();
	}
}
