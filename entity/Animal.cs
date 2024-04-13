namespace AnimalRestAPI.entity;

public class Animal
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Category Category { get; set; }
    public string Breed { get; set; }
    public string Color { get; set; }
    public List<Visit> Visits { get; set; }
}