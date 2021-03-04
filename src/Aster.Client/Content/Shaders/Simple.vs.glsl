#version 450

out gl_PerVertex
{
    vec4 gl_Position;
};

layout (location = 0) in vec3 i_position;
layout (location = 1) in vec4 i_color;
layout (location = 2) in vec2 i_uv;

layout (std140, binding = 0) uniform Matrices
{
	mat4 u_mvp;
};

out FragmentData
{
	vec3 position;
	vec4 color;
	vec2 uv;
} o;

void main()
{
	//o.normal = mat3(transpose(inverse(model))) * i_normal;
	o.color = i_color;
	o.uv = i_uv;
    gl_Position = u_mvp * vec4(i_position, 1.0);
}
