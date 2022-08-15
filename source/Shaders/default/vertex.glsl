#version 330 core
layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec2 vTexCoords;
layout (location = 2) in vec3 vNormals;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

out vec3 fPosition;
out vec2 fTexCoords;
out vec3 fNormal;

void main()
{   
    fPosition = vec3(uModel * vec4(vPosition, 1.0));
    fTexCoords = vTexCoords;
    fNormal = mat3(transpose(inverse(uModel))) * vNormals;

    gl_Position = uProjection * uView * uModel * vec4(vPosition, 1.0);
}