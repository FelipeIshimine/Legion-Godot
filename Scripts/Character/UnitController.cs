using Fractural.Tasks;
using Godot;
using Legion.Combat;

namespace Legion.Character;

public abstract partial class UnitController : Node
{
	public abstract GDTask TakeTurn(CharacterUnit unit, CombatSystemsContainer combatContainer);
}