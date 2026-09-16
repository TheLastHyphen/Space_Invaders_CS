using Godot;

public partial class InvaderOctopus : InvaderBase
{
	public override int Points => 30;
	public override string Name => "I and Invader Octopus";

	public override void _Ready()
	{
		//GD.Print("Octopus Ready");
		base._Ready();
	}
}
