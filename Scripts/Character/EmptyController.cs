using Fractural.Tasks;
using Godot;
using Legion.Combat;

namespace Legion.Character;

[GlobalClass]
public partial class EmptyController : UnitController
{
	public override GDTask TakeTurn(CharacterUnit unit, CombatSystemsContainer combatContainer)
	{
		return GDTask.CompletedTask;
	}
}