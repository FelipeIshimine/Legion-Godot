using Godot;

namespace Legion.Attributes;

public abstract partial class PrimaryAttributeT<T> : PrimaryAttribute where T : PrimaryAttributeT<T>
{
	public static T Instance { get; private set; }
	
	private static readonly int HashCode = typeof(T).GetHashCode();
	public override int Key => HashCode;
	public override int GetHashCode() => HashCode;

	public override void Initialize() => Instance = this as T;
	
}