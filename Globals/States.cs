using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class States : Node
{
	public static States Instance { get; private set; }


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
