using Godot;

public partial class LivesRemaining : Node2D
{
	private Texture2D _lifeTexture;
	private Texture2D _lifeTextureEllipsis;
	private Label _liveLabel;
	private HBoxContainer _hBoxContainer;

	private int _lives = 3;
	public int Lives
	{
		get => _lives;
		set => _lives = value;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		SignalBroadCaster.Instance.OnPlayerHit -= OnAreaEntered;
		SignalBroadCaster.Instance.OnExtraLife -= OnExtraLife;
	}

	public override void _Ready()
	{
		_lifeTexture = GD.Load<Texture2D>("res://Assets/Player/PlayerBase.png");
		_lifeTextureEllipsis = GD.Load<Texture2D>("res://Assets/Player/PlayerBaseEllipsis.png");
		_liveLabel = GetNode<Label>("HBoxContainer/LivesLabel");
		_liveLabel.Text = $"{Lives}";
		_hBoxContainer = GetNode<HBoxContainer>("HBoxContainer");
		SignalBroadCaster.Instance.OnPlayerHit += OnAreaEntered;
		SignalBroadCaster.Instance.OnExtraLife += OnExtraLife;

		InitialiseHBoxItems();
	}

	private void InitialiseHBoxItems()
	{
		for(int i = 0; i < Lives - 1; i++)
		{
			AddItemToHbox(_lifeTexture);
		}
	}

	private void OnExtraLife()
	{
		Lives += 1;
		UpdateLivesLabel();
		AddExtraSprite();
	}

	private void UpdateLivesLabel()
	{
		_liveLabel.Text = $"{Lives}";
	}

	private void AddExtraSprite()
	{
		if(Lives <= 10)
		{
			if(Lives == 10)
			{
				AddItemToHbox(_lifeTextureEllipsis);
			}
			else
			{
				AddItemToHbox(_lifeTexture);
			}
		}
	}

	private void AddItemToHbox(Texture2D texture)
	{
		TextureRect item =  new TextureRect()
		{
			Texture = texture,
			ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional,
			StretchMode = TextureRect.StretchModeEnum.KeepAspect,
			OffsetTransformEnabled = true,
			OffsetTransformPosition = new Vector2(0, -2.0f),
			Modulate = Colors.Green
		};
		_hBoxContainer.AddChild(item);
	}

	private void RemoveItemFromHbox()
	{
		int count = _hBoxContainer.GetChildCount();
		_hBoxContainer.RemoveChild(_hBoxContainer.GetChildren()[count -1 ]);
	}

	private void UpdateLives()
	{
		Lives -= 1;

		if(Lives == 0)
		{
			SignalBroadCaster.Instance.EmitOnPlayerZeroLives();
		}
		else if(Lives < 10)
		{
			RemoveItemFromHbox();
		}

		UpdateLivesLabel();
	}

	private void OnAreaEntered()
	{
		UpdateLives();
	}
}
