using Godot;

public partial class States : Node
{
	public static States Instance { get; private set; }

	bool _debug = false;
	public bool Debug
	{
		get => _debug;
		set => _debug = value;
	}

	private bool _toggleMuteSound = false;
	public bool IsSoundsMute => _toggleMuteSound;

	public void MuteSound()
	{
		_toggleMuteSound = !_toggleMuteSound;
	}

	public override void _Ready()
	{
		Instance = this;
	}
}
