public abstract class Person
{
  private readonly string _firstName;
  private readonly string _lastName;
  private readonly PersonAge _age;

  public string FirstName { get { return _firstName; } }
  public string LastName { get { return _lastName; } }
  public PersonAge Age { get { return _age; } }
}