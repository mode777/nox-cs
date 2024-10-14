namespace Nox.Shaders;

public class StageDescription
{
    public string source { get; set; }
    public string path { get; set; }
    public bool? is_binary { get; set; }
    public string entry_point { get; set; }
    public List<IOVariableDescription> inputs { get; set; }
    public List<IOVariableDescription> outputs { get; set; }
    public List<UniformBlockDescription> uniform_blocks { get; set; }
    public List<ImageDescription> images { get; set; }
    public List<SamplerDescription> samplers { get; set; }
    public List<ImageSamplerPairDescription> image_sampler_pairs { get; set; }

    public int GetAttributeSlot(string name){
        return inputs.FirstOrDefault(x => x.name == name)?.slot ?? -1;
    }

    public int GetTextureSlot(string name){
        return images.FirstOrDefault(x => x.name == name)?.slot ?? -1;
    }

    public int GetSamplerSlot(string name){
        return samplers.FirstOrDefault(x => x.name == name)?.slot ?? -1;
    }

    public int GetUniformSlot(string name){
        return uniform_blocks.FirstOrDefault(x => x.struct_name == name)?.slot ?? -1;
    }
}