namespace Application.Costumers.Dtos;

public class CreateCustomerDto
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string DNI { get; set; }
    public string Telephone { get; set; }
    public DateTime Birthdate { get; set; }
}
