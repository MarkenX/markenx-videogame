using System.Collections.Generic;

/// <summary>
/// Represents a product as a vector of dimension values.
/// These values can be modified by market actions (promotions, events, campaigns).
/// </summary>
public class ProductProfile
{
  public Dictionary<DimensionDefinition, DimensionValue> Dimensions { get; }

  public ProductProfile()
  {
    Dimensions = new Dictionary<DimensionDefinition, DimensionValue>();
  }

  public void Set(DimensionDefinition def, float value)
  {
    Dimensions[def] = new DimensionValue(def, value);
  }

  public void ApplyAction(MarketAction action)
  {
    action.Apply(this);
  }
}
