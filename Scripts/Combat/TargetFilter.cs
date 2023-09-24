using System.Collections.Generic;
using Godot;
using Legion.Character;
using Legion.Combat.Formation;
using Legion.Combat.Targeting.Validation;

namespace Legion.Combat;

public partial class TargetFilter : Node
{
	[Export] public BaseTargetValidator[] Include = new BaseTargetValidator[1] { new AnythingValidator() };

	[Export] public BaseTargetValidator[] Exclude = new BaseTargetValidator[0];
	[Export] public Targeting.Sorting.TargetSorter[] Sorters = new Targeting.Sorting.TargetSorter[0];

	public TargetFilter() { }

	public TargetFilter(BaseTargetValidator defaultInclude)
	{
		Include = new BaseTargetValidator[1] { defaultInclude };
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
			foreach (Targeting.Sorting.TargetSorter comparer in Sorters) 
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
