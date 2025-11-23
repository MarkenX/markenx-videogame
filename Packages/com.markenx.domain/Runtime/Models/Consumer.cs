public class Consumer
{
  private readonly NationalIdentityDocument _nationalIdentityDocument;

  public Consumer(NationalIdentityDocument nationalIdentityDocument, decimal monthlyIncome)
  {
    _nationalIdentityDocument = nationalIdentityDocument
        ?? throw new ArgumentNullException(nameof(nationalIdentityDocument));

    MonthlyIncome = monthlyIncome;
  }

  public string FirstName => _nationalIdentityDocument.FirstName;
  public string LastName => _nationalIdentityDocument.LastName;
  public int Age => CalculateAge(_nationalIdentityDocument.BirthDate);
  public decimal MonthlyIncome { get; }

  private int CalculateAge(DateTime birthDate)
  {
    var today = DateTime.Today;
    int age = today.Year - birthDate.Year;

    if (birthDate.Date > today.AddYears(-age))
      age--;

    return age;
  }
}
