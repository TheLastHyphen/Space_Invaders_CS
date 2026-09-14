using Godot;

/*
* The singleton approach for Godot
* This script still needs adding to the Global/AutoLoad
*/
public partial class SceneManager : Node
{
	public static SceneManager Instance {get; private set; }

	// Preload the Scenes so we can switch them out
	private PackedScene Game = ResourceLoader.Load<PackedScene>("res://Scenes/Game/Game.tscn");
	private PackedScene MainUI = ResourceLoader.Load<PackedScene>("res://Scenes/MainUI/MainUi.tscn");

	public override void _Ready()
	{
		Instance = this;
	}

	public void LoadGameScene()
	{
		GetTree().ChangeSceneToPacked(Game);
	}

	public void LoadMainUIScene()
	{
		GetTree().ChangeSceneToPacked(MainUI);
	}
}
