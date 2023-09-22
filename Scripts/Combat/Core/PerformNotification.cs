using System.Collections.Generic;
using Fractural.Tasks;

namespace Legion.Combat.Core;

public class PerformNotification
{
	public readonly CombatAction Value;
	public List<GDTask> Reactions = new List<GDTask>();

	public PerformNotification(CombatAction value)
	{
		Value = value;
	}
}
public class PerformNotification<T> : PerformNotification where T : CombatAction<T>
{
	public T Action => (T)Value;

	public PerformNotification(T value) : base(value)
	{
	}
}
