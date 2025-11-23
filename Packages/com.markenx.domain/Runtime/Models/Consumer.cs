using System;
using System.Collections.Generic;

public class Consumer
{
  private readonly NationalIdentityDocument _nationalIdentityDocument;
  private readonly HashSet<ProductCategory> _interests;
  private readonly decimal _monthlyIncome;

  public Consumer(
    NationalIdentityDocument nationalID,
    IEnumerable<ProductCategory> interests,
    decimal monthlyIncome)
  {
    _nationalIdentityDocument = RequireNationalIdentityDocument(nationalID);
    _interests = new HashSet<ProductCategory>(interests);
    _monthlyIncome = monthlyIncome;
  }

  public string FirstName => _nationalIdentityDocument.FirstName;
  public string LastName => _nationalIdentityDocument.LastName;
  public IReadOnlyCollection<ProductCategory> Interests => _interests;
  public decimal MonthlyIncome => _monthlyIncome;
  public int Age => CalculateAge(_nationalIdentityDocument.BirthDate);

  private NationalIdentityDocument RequireNationalIdentityDocument(NationalIdentityDocument nationalID)
  {
    return nationalID ?? throw new ArgumentNullException(nameof(nationalID));
  }

  private int CalculateAge(DateTime birthDate)
  {
    var today = DateTime.Today;
    int age = today.Year - birthDate.Year;

    if (birthDate.Date > today.AddYears(-age))
      age--;

    return age;
  }
}
