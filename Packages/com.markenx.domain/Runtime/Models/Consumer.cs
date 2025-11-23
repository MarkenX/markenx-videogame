public class Consumer
{
  private readonly NationalIdentityDocument _nationalIdentityDocument;
  private readonly decimal _monthlyIncome;

  public string FirstName { get { return _nationalIdentityDocument.FirstName; } }

  public decimal MonthlyIncome { get { return _monthlyIncome; } }
}