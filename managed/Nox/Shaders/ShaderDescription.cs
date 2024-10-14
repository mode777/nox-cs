using Nox.Native;

namespace Nox.Shaders;

public class ShaderDescription
{
    public string slang { get; set; }
    public List<ProgramDescription> programs { get; set; }
}