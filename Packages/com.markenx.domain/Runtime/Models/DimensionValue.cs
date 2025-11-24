using System;

/// <summary>
/// Stores the value of a dimension (0–1) for a consumer or a product.
/// It references the dimension definition to maintain consistency.
/// </summary>
public class DimensionValue
{
  public DimensionDefinition Definition { get; }
  public float Value { get; private set; }

  public DimensionValue(DimensionDefinition definition, float value)
  {
    Definition = definition;
    Value = Math.Clamp(value, 0f, 1f);
  }

  public void Add(float delta)
  {
    Value = Math.Clamp(Value + delta, 0f, 1f);
  }
}
