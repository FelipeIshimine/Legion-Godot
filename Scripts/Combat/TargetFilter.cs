using System.Collections.Generic;
using Godot;
using Legion.Attributes.Derived;
using Legion.Character;
using Legion.Combat.Formation;

namespace Legion.Combat;

public partial class TargetFilter : Node
{
	[Export] public TargetValidator[] Include = new TargetValidator[1] { new All() };

	[Export] public TargetValidator[] Exclude = new TargetValidator[0];
	[Export] public TargetSorter[] Sorters = new TargetSorter[0];

	public TargetFilter()
	{
	}

	public TargetFilter(TargetValidator defaultInclude)
	{
		Include = new TargetValidator[1] { defaultInclude };
	}
	public bool TryProcess(CharacterUnit unit, List<FormationTile> worldCoordinates, out List<FormationTile> results)
	{
		results = new List<FormationTile>(worldCoordinates.Count);

		//Incluimos
		for (int i = worldCoordinates.Count - 1; i >= 0; i--)
		{
			bool include = true;
			for (var index = 0; index < Include.Length; index++)
			{
				var validator = Include[index];
				if (!validator.Validate(unit, worldCoordinates[i]))
				{
					include = false;
					break;
				}
			}
			if (include)
			{
				results.Add(worldCoordinates[i]);
				worldCoordinates.RemoveAt(i);	
			}
		}

		//Excluimos
		for (int i = results.Count - 1; i >= 0; i--)
		{
			bool remove = false;
			for (var index = 0; index < Exclude.Length; index++)
			{
				var validator = Exclude[index];
				remove = true;
				if (!validator.Validate(unit, results[i]))
				{
					remove = false;
					break;
				}
			}
			if(remove)
				results.RemoveAt(i);
		}

		if (results.Count == 0)
			return false;

		int Sorter(FormationTile x, FormationTile y)
		{
			foreach (TargetSorter comparer in Sorters) 
			{
				int comparison = comparer.Compare(unit, x, y);        
				if (comparison != 0) {
					return comparison;
				}
			}
			return 0;
		}

		results.Sort(Sorter);
		return true;
	}

}
