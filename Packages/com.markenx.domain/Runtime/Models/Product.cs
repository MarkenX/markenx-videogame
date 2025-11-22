public class Product
{
  private readonly float _price;

  public Product(float price)
  {
    _price = price;
  }

  public float Price { get { return _price; } }
}