using Godot;

public partial class BunkerBlock : BunkerBlockBase
{
	public override void _Ready()
	{
		base._Ready();
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		if( area is InvaderBase)
			QueueFree();		
	}
}
