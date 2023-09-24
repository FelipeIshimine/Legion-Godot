namespace Legion.Attributes;

public abstract partial class DerivedAttributeT<T> : DerivedAttribute where T : DerivedAttributeT<T>
{
	public static T Instance { get; private set; }
	private static readonly int HashCode = typeof(T).GetHashCode();
	public override int GetHashCode() => HashCode;
	public override int Key => HashCode;
	
	public override void Initialize() => Instance = this as T;
}