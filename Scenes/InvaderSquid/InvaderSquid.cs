using Godot;

public partial class InvaderSquid : InvaderBase
{
	public override int Points => 20;
	public override string InvaderName => "I am an Invader Squid";

	public override void _Ready()
	{		
		base._Ready();
	}
}
