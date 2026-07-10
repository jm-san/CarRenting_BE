namespace Domain.Entities;

public class Customer
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string LastName { get; set; }
    public string DNI { get; set; }
    public string Telephone { get; set; }
    public DateTime Birthdate { get; set; }
}
