using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class BunkerBlockBase : Area2D
{
	private uint _devideBy = 3;
	private Sprite2D _block;
	private Godot.Collections.Array<Pixel> _pixels = [];

	public override void _ExitTree()
	{
		base._ExitTree();
		SignalBroadCaster.Instance.OnInvaderBombHit -= OnInvaderBombHit;
		//SignalBroadCaster.Instance.OnInvaderHit -= OnInvaderHit;
	}


	public override void _Ready()
	{
		_block = GetNode<Sprite2D>("Sprite2D");
		Vector2 size = _block.Texture.GetSize();

		int count = 0;
		for(int i = 0; i < size.X; i++)
		{
			for(int j = 0; j < size.Y; j++)
			{
				_pixels.Add(new Pixel(){Index = count++, PixelRow = i, PixelColumn = j});
			}
		}

		AreaEntered += OnInvaderBombHit;
		//SignalBroadCaster.Instance.OnInvaderHit += OnInvaderHit;
		//SignalBroadCaster.Instance.OnInvaderBombHit += OnInvaderBombHit;
	}

	public void SetPixelRandomly()
	{
		Image image = _block.Texture.GetImage();

		List<Pixel> toBeRemoved = [];
		for(int i = 0; i < _pixels.Count / _devideBy; i++)
		{
			Pixel p = _pixels.PickRandom();
			if(toBeRemoved.Contains<Pixel>(p))
			{
				i -= 1;
				continue;
			}
			toBeRemoved.Add(p);	
		}

		foreach(Pixel p in toBeRemoved)
		{
			_pixels.Remove(p);
			Color c = image.GetPixel(p.PixelRow, p.PixelColumn);
			//GD.Print(c);

			image.SetPixel(p.PixelRow, p.PixelColumn, Colors.Transparent);
			_block.Texture = ImageTexture.CreateFromImage(image);
		}

		_devideBy--;
		if(_devideBy == 0) _devideBy = 1;		
	}

	// public void OnAreaEntered(Area2D area)
	// {
	// 	SetPixelRandomly();
	// 	if(_pixels.Count == 0) QueueFree();
	// }

	private void OnInvaderBombHit(Area2D area)
	{
		SetPixelRandomly();		
		if(_pixels.Count == 0) QueueFree();
	}

	// private void OnInvaderHit(GodotObject godotObject)
	// {
	// 	if(godotObject.GetType() == typeof(InvaderBase) == true) QueueFree();
	// 	//GD.Print("GO type = ", godotObject.GetType());
	// }
}