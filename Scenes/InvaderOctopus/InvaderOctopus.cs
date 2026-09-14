using Godot;
using System;

public partial class InvaderOctopus : InvaderBase
{
	public override int InvaderPoints => 30;

	public override string InvaderName => "I am Invader Squid";
	public override AnimatedSprite2D AnimatedSprite => GetNode<AnimatedSprite2D>("AnimatedSprite2D");

	public override void _Ready()
	{
		//GD.Print("Octopus Ready");
		base._Ready();
	}
}
