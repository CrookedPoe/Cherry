precision highp float;

varying vec3 vPosition;
varying vec2 vTexCoords;
varying vec3 vNormals;

struct PointLight {
    vec3 color;
    vec3 position;
};

uniform PointLight pointLights[NUM_POINT_LIGHTS];
uniform sampler2D texture;
uniform vec2 offset;

void main() {
    
    vec4 lightMultiplier = vec4(0.1, 0.1, 0.1, 1.0);

    for(int l = 0; l < NUM_POINT_LIGHTS; l++) {
        vec3 adjustedLight = pointLights[l].position + cameraPosition;
        vec3 lightDirection = normalize(vecPos - adjustedLight);
        lightMultiplier.rgb += clamp(dot(-lightDirection, vecNormal), 0.0, 1.0 * pointLights[l].color;
    }

    vec4 texelColor = texture2D( texture, vUv + offset );
    gl_FragColor = texelColor * lightMultiplier;
}