using Godot;
using System.Threading.Tasks;

public partial class GameOver : Control
{
	private string _gameText = "GAME";
	private string _overText = "OVER";
	private Label _gameLabel;
	private Label _overLabel;

	[Export]
	public float CharacterDelay { get; set; }

	public async override void _Ready()
	{
		_gameLabel = GetNode<Label>("HBoxContainer/GameLabel");
		_overLabel = GetNode<Label>("HBoxContainer/OverLabel");

		_gameLabel.Text = "";
		_overLabel.Text = "";

		await WaitDrawChar(_gameLabel, CharacterDelay, _gameText);
		await WaitDrawChar(_overLabel, CharacterDelay, _overText);
	}

	private async Task WaitDrawChar(Label label, float delay, string text)
	{	
		foreach(char c in text)
		{
			label.Text += c;
			await Delay(delay);
		}
	}

	private async Task Delay(float delay)
	{
		await ToSignal(GetTree().CreateTimer(delay), "timeout");
	}
}
