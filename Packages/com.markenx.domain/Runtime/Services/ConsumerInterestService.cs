using System.Linq;

public class ConsumerInterestService
{
  public bool IsInterestedIn(Consumer consumer, Product product)
  {
    return consumer.Interests.Contains(product.Category);
  }
}
