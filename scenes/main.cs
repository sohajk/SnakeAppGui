using Godot;
using Godot.Collections;

public partial class main : Node2D
{
	private readonly double SPEED = 3000.0;
	private readonly double TIME_BETWEEN_MOVES = 1000.0;
	private double _timeLastMove = 1000.0;

	private Panel _snakeHead;
	private Array<Panel> _snakeBodyParts;
	private Panel _playground;
	private Vector2 _nextPosition;
	private PackedScene _snakeBodyPartScene;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitElements();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Up))
		{
			_nextPosition = Vector2.Up * _snakeHead.Size.Y;
		}
		else if (Input.IsKeyPressed(Key.Right))
		{
			_nextPosition = Vector2.Right * _snakeHead.Size.X;
		}
		else if (Input.IsKeyPressed(Key.Down))
		{
			_nextPosition = Vector2.Down * _snakeHead.Size.Y;
		}
		else if (Input.IsKeyPressed(Key.Left))
		{
			_nextPosition = Vector2.Left * _snakeHead.Size.X;
		}

		_timeLastMove += delta * SPEED;
		if (_timeLastMove >= TIME_BETWEEN_MOVES)
		{
			_snakeHead.Position += _nextPosition;

			_timeLastMove = 0;
		}
	}

	private void InitElements()
	{
		_snakeHead = GetNode<Panel>("./SnakeHead");
		_playground = GetNode<Panel>("./Playground");
		_snakeBodyPartScene = (PackedScene)ResourceLoader.Load("res://scenes/snake_body_part.tscn");

		_snakeBodyParts = new Array<Panel>();
		_snakeHead.Position = new Vector2(205, 205);
		_nextPosition = Vector2.Right * _snakeHead.Size.X;
	}
}
