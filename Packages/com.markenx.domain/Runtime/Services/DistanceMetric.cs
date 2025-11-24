using System;
using System.Collections.Generic;

/// <summary>
/// Computes similarity (acceptance) between consumer and product.
/// Uses normalized Manhattan distance.
/// Returns a value between 0 and 1.
/// </summary>
public static class DistanceMetric
{
  public static float ComputeAcceptance(ConsumerProfile consumer, ProductProfile product)
  {
    float sum = 0f;
    int count = 0;

    foreach (var dim in consumer.Dimensions)
    {
      if (product.Dimensions.TryGetValue(dim.Key, out var pValue))
      {
        float distance = Math.Abs(dim.Value.Value - pValue.Value);
        sum += distance;
        count++;
      }
    }

    if (count == 0) return 0f;

    float normalized = sum / count;
    return 1f - normalized; // 1 = perfect match, 0 = totally opposite
  }
}
