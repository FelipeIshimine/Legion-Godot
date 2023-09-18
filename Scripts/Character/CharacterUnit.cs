using Godot;

namespace Legion.Character;

public partial class CharacterUnit : Node3D
{
	[Export] private Sprite3D sprite;

	public bool LookingRight => !sprite.FlipH;
	public void Flip() => sprite.FlipH = !sprite.FlipH;

	public void LookAt(Vector3 targetGlobalPosition)
	{
		switch (LookingRight)
		{
			case true when targetGlobalPosition.X  < GlobalPosition.X:
			case false when targetGlobalPosition.X > GlobalPosition.X:
				Flip();
				break;
		}
	}
    

}