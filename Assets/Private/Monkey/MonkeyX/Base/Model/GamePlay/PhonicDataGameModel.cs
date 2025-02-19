
public class PhonicDataGameModel 
{
    public string Name { get; set; }
    public string Positions { get; set; }
    public PhonicDataGameModel(string name, string positions)
    {
        Name = name;
        Positions = positions;
    }
}
