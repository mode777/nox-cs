namespace Nox.Shaders;

public class UniformDescription
{
    public string name { get; set; }
    public string type { get; set; }
    public int array_count { get; set; }
    public int offset { get; set; }
}