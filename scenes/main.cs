using System;
using Godot;
using Godot.Collections;

public partial class main : Node2D
{
	private readonly double SPEED = 3000.0;
	private readonly double TIME_BETWEEN_MOVES = 1000.0;
	private double _timeLastMove = 1000.0;

	private Panel _snakeHead;
	private Array<Panel> _snakeBodyParts;
	private Panel _fruit;
	private Panel _scoreBoard;
	private Panel _playground;
	private Vector2 _nextPosition;
	private PackedScene _snakeBodyPartScene;
	private bool _gameOver;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitElements();
		InitElementsPositions();
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
		if (!_gameOver && _timeLastMove >= TIME_BETWEEN_MOVES)
		{
			_snakeHead.Position += _nextPosition;

			_timeLastMove = 0;
		}

		var snakeCollision = CheckSnakeCollision();

		if (snakeCollision) {

			_gameOver = true;
		}
	}

	private void InitElements()
	{
		_snakeHead = GetNode<Panel>("./SnakeHead");
		_fruit = GetNode<Panel>("./Fruit");
		_scoreBoard = GetNode<Panel>("./ScoreBoard");
		_playground = GetNode<Panel>("./Playground");
		_snakeBodyPartScene = (PackedScene)ResourceLoader.Load("res://scenes/snake_body_part.tscn");

		_snakeBodyParts = new Array<Panel>();
	}

	private void InitElementsPositions()
	{
		_nextPosition = Vector2.Right * _snakeHead.Size.X;
		_snakeHead.Position = new Vector2(205, 255);

		var (x, y) = GetRandomFruitPosition();
		_fruit.Position = new Vector2(x, y);
	}

	private bool CheckSnakeCollision()
	{
		var boardOffset = _scoreBoard.Size.Y;

		// Check collision with border
		if (_snakeHead.Position.X <= 5 || _snakeHead.Position.X >= _playground.Size.X - 5 - _snakeHead.Size.X
			|| _snakeHead.Position.Y == boardOffset + 5 || _snakeHead.Position.Y == _scoreBoard.Size.Y + _playground.Size.Y - 5 - _snakeHead.Size.Y)
		{
			return true;
		}

		return false;
	}

	private (int x, int y) GetRandomFruitPosition()
	{
		var maxX = _playground.Size.X - 10 - _snakeHead.Size.X;
		var maxY = _playground.Size.Y - 10 - _snakeHead.Size.Y;

		var random = new Random();
		var stepsX = maxX / _snakeHead.Size.X;
		var stepsY = maxY / _snakeHead.Size.Y;

		var randomStepX = random.Next((int)stepsX + 1);
		var randomStepY = random.Next((int)stepsY + 1);

		int randomX = (randomStepX * (int)_snakeHead.Size.X) + 5;
		int randomY = (randomStepY * (int)_snakeHead.Size.Y) + (int)_scoreBoard.Size.Y + 5;

		return (randomX, randomY);
	}
}
