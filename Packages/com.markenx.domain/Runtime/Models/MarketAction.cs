using System.Collections.Generic;

/// <summary>
/// Represents a market action that modifies product attributes.
/// Example: "Advertising Campaign" → +0.1 brand appeal, -0.05 price perception.
/// </summary>
public class MarketAction
{
  public string Name { get; }
  public string Description { get; }

  // Effects per dimension
  private Dictionary<DimensionDefinition, float> _effects;

  public MarketAction(string name, string description)
  {
    Name = name;
    Description = description;
    _effects = new Dictionary<DimensionDefinition, float>();
  }

  public void AddEffect(DimensionDefinition def, float delta)
  {
    _effects[def] = delta;
  }

  public void Apply(ProductProfile profile)
  {
    foreach (var kvp in _effects)
    {
      if (profile.Dimensions.TryGetValue(kvp.Key, out var dim))
        dim.Add(kvp.Value);   // modify existing
      else
        profile.Set(kvp.Key, kvp.Value); // auto-create if missing
    }
  }
}
