using Godot;
using System;
using Legion.addons.MathSpring;

public partial class SpringVector2 : Sprite2D
{

	[Export, ExportGroup("Movement")] private float damping = .5f;
	[Export, ExportGroup("Movement")] private float angularFrequency = .5f;
	
	[Export, ExportGroup("Deformation")] private float scaleDamping = .5f;
	[Export, ExportGroup("Deformation")] private float scaleAngularFrequency = .5f;
	[Export, ExportGroup("Deformation")] private float scaleMultiplier = .5f;
	
	[Export] private float scaleAmount = .5f;
	[Export] private Vector2 velToScaleRange = new Vector2(5,10);
	
	private double timeStep;

	private Vector2 posVel,scaleVel;
	private Vector2 targetPosition,targetScale;

	private Vector2 globalPosition;
	
	public override void _Process(double delta)
	{
		timeStep = delta;

		var oldPosition = globalPosition;
		globalPosition = GlobalPosition;
		MathSpring.Spring(
			ref globalPosition,
			ref posVel,
			targetPosition,
			damping,
			angularFrequency,
			(float)timeStep
		);

		GlobalPosition = globalPosition;


		var diff = (globalPosition - oldPosition);
		float vel = (diff / (float)delta).Length();

		if (vel != 0)
		{
			LookAt(globalPosition + diff);
		}

		GD.Print($"vel: {vel}");



		var scaleDeformation = Mathf.Clamp(Mathf.InverseLerp(velToScaleRange.X, velToScaleRange.Y, vel),0,1);
		targetScale = new Vector2(
			1 + scaleMultiplier * (scaleDeformation) * scaleAmount,
			1 - scaleMultiplier * (scaleDeformation) * scaleAmount);
		
		var scale = Scale;

		MathSpring.Spring(
			ref scale,
			ref scaleVel,
			targetScale,
			damping,
			angularFrequency,
			(float)timeStep
		);

		Scale = scale;

	}

	public override void _Input(InputEvent input)
	{
		base._Input(input);
		if (input is InputEventMouseMotion mouseMotion)
		{
			targetPosition = mouseMotion.GlobalPosition;
		}
	}

   
}
