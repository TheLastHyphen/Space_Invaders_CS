using Godot;

public partial class DebugMessages : Node
{
	public static DebugMessages Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	public void PrintMessage(string message)
	{
		if(States.Instance.Debug)
		{
			GD.Print($"{message}\n");
		}
	}
}
