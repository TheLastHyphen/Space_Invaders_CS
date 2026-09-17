using Godot;

public partial class PlayerMissile : Area2D
{
	private bool _canMove = true;
	private float _missileSpeed = 250;
	private bool _isFired = false;

	public override void _Ready()
	{
		base._Ready();
		SignalBroadCaster.Instance.OnPlayerFirePressed += OnPlayerFirePressed;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		SignalBroadCaster.Instance.OnPlayerFirePressed -= OnPlayerFirePressed;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if(_isFired == true)
		{
			Vector2 pos = Position;
			pos.Y -= 1 * (float)delta * _missileSpeed;
			Position = pos;
		}
	}

	public void OnPlayerFirePressed(Vector2 pos)
	{
		if(_isFired == true) return;
		Position = pos;
		_isFired = true;
	}

	public void OnAreaEntered(Area2D area)
	{
		if(area.Name == "CeilingBoundary")
		{
			_isFired = false;
		}
	}
}
