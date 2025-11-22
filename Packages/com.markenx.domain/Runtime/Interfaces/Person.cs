public abstract class Person
{
  private readonly string _firstName;
  private readonly string _lastName;

  public string FirstName { get { return _firstName; } }
  public string LastName { get { return _lastName; } }
}