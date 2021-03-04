#version 450

layout(binding = 0) uniform sampler2D t_texture;

layout(location = 0) out vec4 fs_color;

in FragmentData
{
	vec3 position;
	vec4 color;
	vec2 uv;
} i;

void main()
{
    fs_color = texture(t_texture, i.uv) * i.color;
}
