using System;

/// <summary>
/// Converts real-world documents into psychological/product dimensions.
/// Each rule maps document data to specific dimensions using formulas.
/// </summary>
public static class RuleEngine
{
  public static void ApplyDemographicRules(DNI dni, ConsumerProfile consumer,
                                           DimensionDefinition priceSensitivity,
                                           DimensionDefinition socialRecognition)
  {
    // Example rule: older people tend to have lower social recognition need
    float ageFactor = Math.Clamp((100 - dni.Age) / 100f, 0f, 1f);

    consumer.Set(socialRecognition, ageFactor);

    // Example rule: lower income increases price sensitivity
    float incomeFactor = Math.Clamp(1f - (dni.MonthlyIncome / 3000f), 0f, 1f);
    consumer.Set(priceSensitivity, incomeFactor);
  }
}
