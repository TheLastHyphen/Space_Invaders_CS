using Godot;

public partial class PlayerMissile : Area2D
{
	private bool _canMove = true;
	private float _missileSpeed = 600;
	private bool _isFired = false;
	private Sprite2D _missileExplode;
	private Sprite2D _missile;

	public override void _Ready()
	{
		base._Ready();
		_missile = GetNode<Sprite2D>("Sprite2D");
		_missileExplode = GetNode<Sprite2D>("MissileExplode");
		_missileExplode.Hide();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Vector2 pos = Position;
		pos.Y -= 1 * (float)delta * _missileSpeed;
		Position = pos;
	}

	public void OnAreaEntered(Area2D area)
	{
		if(area.Name == "CeilingBoundary")
		{
			_missileExplode.Modulate = Colors.Red;
		}
		else
		{
			_missileExplode.Modulate = Colors.White;
		}
		GD.Print(area.Name);
		_canMove = false;
		_missile.Hide();
		SignalBroadCaster.Instance.EmitOnPlayerMissileHit(Position, area.Name);
		QueueFree();
	}
}
