using Godot;

public partial class PlayerMissile : Area2D
{
	[Signal]
	public delegate void OnMissileHitEventHandler(Area2D area);

	[Export]
	public float MissileSpeed { get; set; }= 600;

	private Sprite2D _missile;

	public override void _Ready()
	{
		base._Ready();
		_missile = GetNode<Sprite2D>("Sprite2D");
		AreaEntered += OnAreaEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Vector2 pos = Position;
		pos.Y -= 1 * (float)delta * MissileSpeed;
		Position = pos;		
	}

	public void OnAreaEntered(Area2D area)
	{
		EmitSignal(SignalName.OnMissileHit, area);
	}
}
