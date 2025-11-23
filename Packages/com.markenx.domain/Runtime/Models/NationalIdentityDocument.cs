using System;

public class NationalIdentityDocument
{
  private readonly string _firstName;
  private readonly string _lastName;
  private readonly DateTime _birthDate;

  public string FirstName { get { return _firstName; } }
  public string LastName { get { return _lastName; } }
  public DateTime BirthDate { get { return _birthDate; } }
}