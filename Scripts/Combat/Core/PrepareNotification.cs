using System.Collections.Generic;
using Fractural.Tasks;

namespace Legion.Combat.Core;

public class PrepareNotification
{
	public readonly CombatAction Value;
	public List<GDTask> Reactions = new List<GDTask>();

	public PrepareNotification(CombatAction value)
	{
		Value = value;
	}

	public bool WasCanceled { get; private set; }
	public void Cancel()
	{
		WasCanceled = true;
	}
}
public class PrepareNotification<T> : PrepareNotification where T : CombatAction<T>
{
	public T Action => (T)Value;
	public PrepareNotification(T value) : base(value)
	{
	}
}
