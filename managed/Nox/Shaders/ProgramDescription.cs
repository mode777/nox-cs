namespace Nox.Shaders;

using Nox.Native;
using static Nox.Native.LibNox;

public class ProgramDescription
{
    public string name { get; set; }
    public StageDescription vs { get; set; }
    public StageDescription fs { get; set; }

    internal sg_shader_desc CreateShaderDesc()
    {
        var shaderDesc = new sg_shader_desc
        {
            attrs = new sg_shader_attr_desc[SG_MAX_VERTEX_ATTRIBUTES],
            vs = new sg_shader_stage_desc
            {
                uniform_blocks = new sg_shader_uniform_block_desc[SG_MAX_SHADERSTAGE_UBS],
                storage_buffers = new sg_shader_storage_buffer_desc[SG_MAX_SHADERSTAGE_STORAGEBUFFERS],
                images = new sg_shader_image_desc[SG_MAX_SHADERSTAGE_IMAGES],
                samplers = new sg_shader_sampler_desc[SG_MAX_SHADERSTAGE_SAMPLERS],
                image_sampler_pairs = new sg_shader_image_sampler_pair_desc[SG_MAX_SHADERSTAGE_IMAGESAMPLERPAIRS]
            },
            fs = new sg_shader_stage_desc
            {
                uniform_blocks = new sg_shader_uniform_block_desc[SG_MAX_SHADERSTAGE_UBS],
                storage_buffers = new sg_shader_storage_buffer_desc[SG_MAX_SHADERSTAGE_STORAGEBUFFERS],
                images = new sg_shader_image_desc[SG_MAX_SHADERSTAGE_IMAGES],
                samplers = new sg_shader_sampler_desc[SG_MAX_SHADERSTAGE_SAMPLERS],
                image_sampler_pairs = new sg_shader_image_sampler_pair_desc[SG_MAX_SHADERSTAGE_IMAGESAMPLERPAIRS]
            }
        };

        // Fill vertex shader stage
        shaderDesc.vs.source = vs.source;
        shaderDesc.vs.entry = vs.entry_point;
        FillStageDesc(ref shaderDesc.vs, vs);
        shaderDesc.vs.d3d11_target = "vs_5_0";

        // Fill fragment shader stage
        shaderDesc.fs.source = fs.source;
        shaderDesc.fs.entry = fs.entry_point;
        FillStageDesc(ref shaderDesc.fs, fs);
        shaderDesc.fs.d3d11_target = "ps_5_0";

        // Fill attributes
        foreach (var input in vs.inputs)
        {
            shaderDesc.attrs[input.slot] = new sg_shader_attr_desc
            {
                name = input.name,
                sem_name = input.sem_name,
                sem_index = input.sem_index,
            };
        }

        shaderDesc.label = name;

        return shaderDesc;
    }

    private static sg_uniform_type GetUniformType(string type)
    {
        switch (type)
        {
            case "float": return sg_uniform_type.SG_UNIFORMTYPE_FLOAT;
            case "vec2": return sg_uniform_type.SG_UNIFORMTYPE_FLOAT2;
            case "vec3": return sg_uniform_type.SG_UNIFORMTYPE_FLOAT3;
            case "vec4": return sg_uniform_type.SG_UNIFORMTYPE_FLOAT4;
            case "mat4": return sg_uniform_type.SG_UNIFORMTYPE_MAT4;
            default: throw new InvalidOperationException($"Unknown uniform type {type}");
        }

    }

    private static sg_image_type GetImageType(string type)
    {
        switch (type)
        {
            case "2d": return sg_image_type.SG_IMAGETYPE_2D;
            case "cube": return sg_image_type.SG_IMAGETYPE_CUBE;
            case "3d": return sg_image_type.SG_IMAGETYPE_3D;
            case "array": return sg_image_type.SG_IMAGETYPE_ARRAY;
            default: throw new InvalidOperationException($"Unknown image type {type}");
        }
    }

    private static sg_image_sample_type GetImageSampleType(string type)
    {
        switch (type)
        {
            case "float": return sg_image_sample_type.SG_IMAGESAMPLETYPE_FLOAT;
            case "depth": return sg_image_sample_type.SG_IMAGESAMPLETYPE_DEPTH;
            case "sint": return sg_image_sample_type.SG_IMAGESAMPLETYPE_SINT;
            case "uint": return sg_image_sample_type.SG_IMAGESAMPLETYPE_UINT;
            case "unfilterable_float": return sg_image_sample_type.SG_IMAGESAMPLETYPE_UNFILTERABLE_FLOAT;
            default: throw new InvalidOperationException($"Unknown image sample type {type}");
        }
    }

    private static sg_sampler_type GetSamplerType(string type)
    {
        switch (type)
        {
            case "filtering": return sg_sampler_type.SG_SAMPLERTYPE_FILTERING;
            case "nonfiltering": return sg_sampler_type.SG_SAMPLERTYPE_NONFILTERING;
            case "comparison": return sg_sampler_type.SG_SAMPLERTYPE_COMPARISON;
            default: throw new InvalidOperationException($"Unknown sampler type {type}");
        }
    }

    private static void FillStageDesc(ref sg_shader_stage_desc stageDesc, StageDescription stage)
    {
        // Fill uniform blocks
        for (int i = 0; i < (stage.uniform_blocks?.Count ?? 0) && i < SG_MAX_SHADERSTAGE_UBS; i++)
        {
            var ub = stage.uniform_blocks[i];
            
            stageDesc.uniform_blocks[i] = new sg_shader_uniform_block_desc
            {
                size = (IntPtr)ub.size,
                layout = sg_uniform_layout.SG_UNIFORMLAYOUT_STD140,
                uniforms = new sg_shader_uniform_desc[SG_MAX_UB_MEMBERS]
            };
            var uniforms = ub.uniforms.Select(u => new sg_shader_uniform_desc
            {
                name = u.name,
                type = GetUniformType(u.type),
                array_count = u.array_count
            }).ToArray();
            for(int j = 0; j < uniforms.Length; j++)
            {
                stageDesc.uniform_blocks[i].uniforms[j] = uniforms[j];
            }
        }

        // Fill images
        for (int i = 0; i < (stage.images?.Count ?? 0) && i < SG_MAX_SHADERSTAGE_IMAGES; i++)
        {
            var img = stage.images[i];
            stageDesc.images[i] = new sg_shader_image_desc
            {
                used = true,
                multisampled = img.multisampled,
                image_type = GetImageType(img.type),
                sample_type = GetImageSampleType(img.sample_type)
            };
        }

        // Fill samplers
        for (int i = 0; i < (stage.samplers?.Count ?? 0) && i < SG_MAX_SHADERSTAGE_SAMPLERS; i++)
        {
            var sampler = stage.samplers[i];
            stageDesc.samplers[i] = new sg_shader_sampler_desc
            {
                used = true,
                sampler_type = GetSamplerType(sampler.sampler_type)
            };
        }

        // Fill image sampler pairs
        for (int i = 0; i < (stage.image_sampler_pairs?.Count ?? 0) && i < SG_MAX_SHADERSTAGE_IMAGESAMPLERPAIRS; i++)
        {
            var isp = stage.image_sampler_pairs[i];
            stageDesc.image_sampler_pairs[i] = new sg_shader_image_sampler_pair_desc
            {
                used = true,
                image_slot = stage.images?.FirstOrDefault(x => x.name == isp.image_name)?.slot ?? 0,
                sampler_slot = stage.samplers?.FirstOrDefault(x => x.name == isp.sampler_name)?.slot ?? 0,
                glsl_name = isp.name
            };
        }
    }
}