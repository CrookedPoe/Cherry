#version 330 core

layout (location = 0) in vec3 vPos;
layout (location = 1) in vec4 vColor;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

out vec3 fPos;
out vec4 fColor;

void main()
{
    
    fPos = vec3(uModel * vec4(vPos, 1.0));
    fColor = vColor;

    gl_Position = uProjection * uView * uModel * vec4(vPos, 1.0);
}