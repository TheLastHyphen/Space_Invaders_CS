using Godot;

public partial class PlayerMissile : Area2D
{
	private bool _canMove = true;
	private float _missileSpeed = 600;
	private bool _isFired = false;

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		Vector2 pos = Position;
		pos.Y -= 1 * (float)delta * _missileSpeed;
		Position = pos;
	}

	public void OnAreaEntered(Area2D area)
	{
		QueueFree();

		// if(area.Name == "CeilingBoundary")
		// {
		// 	QueueFree();
		// }
		// else
		// {
		// 	area.QueueFree();
		// 	QueueFree();
		// }
	}
}
