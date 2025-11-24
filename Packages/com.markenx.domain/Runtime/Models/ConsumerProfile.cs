using System.Collections.Generic;

/// <summary>
/// Represents the consumer as a vector of dimension values.
/// This is the final result after applying rule-based transformations on documents.
/// </summary>
public class ConsumerProfile
{
  public Dictionary<DimensionDefinition, DimensionValue> Dimensions { get; }

  public ConsumerProfile()
  {
    Dimensions = new Dictionary<DimensionDefinition, DimensionValue>();
  }

  public void Set(DimensionDefinition def, float value)
  {
    Dimensions[def] = new DimensionValue(def, value);
  }
}
