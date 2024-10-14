namespace Nox.Shaders;

public class ImageDescription
{
    public int slot { get; set; }
    public string name { get; set; }
    public bool multisampled { get; set; }
    public string type { get; set; }
    public string sample_type { get; set; }
}