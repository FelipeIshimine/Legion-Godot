#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using Extensions.Godot;
using Legion.Attributes;

[Tool]
public partial class attributesautomationplugin : EditorPlugin
{
	private string mainAttributesPath = "res://Resources/Attributes/Main";
	private string derivedAttributesPath = "res://Resources/Attributes/Derived";
	private string singletonPath = "res://Scenes/Singletons/AttributesTypes.tscn";
	private readonly List<Resource> attributes = new List<Resource>();
	public override void _EnterTree()
	{
		attributes.Clear();
		
		GD.Print("_EnterTree");

		CreateFromBaseClass(typeof(PrimaryAttribute), mainAttributesPath);
		CreateFromBaseClass(typeof(DerivedAttribute), derivedAttributesPath);

	}

	private void CreateFromBaseClass(Type baseClass, string path)
	{
		var subclassesOfBaseAttribute = GetSubclasses(baseClass);
		foreach (var subclassType in subclassesOfBaseAttribute)
		{
			//GD.Print("Class inheriting from BaseAttribute: " + subclassType.FullName);
			var filePath = $"{path}/{subclassType.Name}.tres";
			attributes.Add(FindOrCreateResourceOf(subclassType, filePath));
		}
	}

	private static List<Type> GetSubclasses(Type baseClassType)
	{
		// Get all loaded assemblies in the current AppDomain.
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

		// Create a list to store the found classes.
		var subclassesOfBaseAttribute = new System.Collections.Generic.List<Type>();

		foreach (var assembly in assemblies)
		foreach (Type type in assembly.GetTypes())
		{
			if (!type.IsAbstract && baseClassType.IsAssignableFrom(type))
			{
				subclassesOfBaseAttribute.Add(type);
			}
		}

		return subclassesOfBaseAttribute;
	}

	private Resource FindOrCreateResourceOf(Type type, string filePath)
	{
		if (!ResourceLoader.Exists(filePath))
		{
			if (Activator.CreateInstance(type) is Resource nResource)
			{
				ResourceSaver.Save(nResource, filePath);
				GD.Print($"{type.Name} created");
			}
		}

		var loadedAttribute = ResourceLoader.Load(filePath);
		
		/*
		GD.Print($"LoadedAttribute:{loadedAttribute} " +
			$"{loadedAttribute is BaseAttribute} "     +
			$"{loadedAttribute is DerivedAttribute} "     +
			$"{loadedAttribute is MainAttribute}");*/
		
		return loadedAttribute;
	}

	public override void _ExitTree()
	{
	}
	
	private List<string> FindAttributes(string directory)
	{
		List<string> scenePaths = new List<string>();

		// Get a list of all files in the directory
		string[] files = Directory.GetFiles(directory);

		foreach (string file in files)
		{
			//file = file.Replace("res:\")
			// Check if the file has a ".tscn" extension (Godot scene file)
			if (file.EndsWith(".tres"))
			{
				// Convert the file path to a resource path
				string resourcePath = ProjectSettings.GlobalizePath(file);
				scenePaths.Add(resourcePath);
			}
		}

		return scenePaths;
	}
}
#endif
