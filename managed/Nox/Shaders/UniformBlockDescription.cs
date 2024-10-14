using System.Collections.Generic;

namespace Nox.Shaders;

public class UniformBlockDescription
{
    public int slot { get; set; }
    public int size { get; set; }
    public string struct_name { get; set; }
    public string inst_name { get; set; }
    public List<UniformDescription> uniforms { get; set; }
}