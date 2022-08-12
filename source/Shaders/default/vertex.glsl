varying vec3 vPosition;
varying vec2 vTexCoords;
varying vec3 vNormals;

void main() {
    vPosition = (modelViewMatrix * vec4(position, 1.0)).xyz;
    vTexCoords = uv;
    vNormals = (modelViewMatrix * vec4(normals, 0.0)).xyz

    gl_Position = projectionMatrix * vec4(vPosition, 1.0);
}