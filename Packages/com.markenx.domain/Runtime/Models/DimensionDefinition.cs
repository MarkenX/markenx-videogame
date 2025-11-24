using System;

/// <summary>
/// Represents a single psychological or product dimension.
/// This class defines *what* the dimension means.
/// Example: "PriceSensitivity", "NeedForRecognition".
/// </summary>
public class DimensionDefinition
{
  public string Name { get; }
  public string Description { get; }

  public DimensionDefinition(string name, string description)
  {
    Name = name;
    Description = description;
  }
}
