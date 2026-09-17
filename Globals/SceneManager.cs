using Godot;

/*
* The singleton approach for Godot
* This script still needs adding to the Global/AutoLoad
*/
public partial class SceneManager : Node
{
	public static SceneManager Instance {get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	public void LoadGameScene()
	{
		GetTree().ChangeSceneToPacked(PackedScenes.Instance.Game);
	}

	public void LoadMainUIScene()
	{
		GetTree().ChangeSceneToPacked(PackedScenes.Instance.MainUI);
	}
}
