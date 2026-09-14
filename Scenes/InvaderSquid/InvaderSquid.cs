using Godot;

public partial class InvaderSquid : InvaderBase
{
	public override int InvaderPoints => 20;
	public override string InvaderName => "I am an Invader Squid";
	public override AnimatedSprite2D AnimatedSprite => GetNode<AnimatedSprite2D>("AnimatedSprite2D");

	public override void _Ready()
	{
		//GD.Print("Squid Ready");
		base._Ready();		
	}
}
