using System.Collections.Generic;

namespace Legion.Attributes;

public abstract partial class MainAttribute : BaseAttribute
{
	public override int Calculate(Dictionary<int, int> dictionary) => dictionary.TryGetValue(this, out var value) ? value : 0;
}

public abstract partial class MainAttribute<T> : MainAttribute where T : MainAttribute<T>
{
	private static readonly int HashCode = typeof(T).GetHashCode();
	public override int Key => HashCode;
	public override int GetHashCode() => HashCode;
}