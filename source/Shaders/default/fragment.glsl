#version 330 core
in vec3 fPosition;
in vec2 fTexCoords;
in vec3 fNormals;

struct Material {
    vec3 Ka;
    vec3 Kd;
    vec3 Ks;
    float Ns;
};

struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Material material;
uniform Light light;
uniform vec3 cameraPosition;
uniform sampler2D uTexel0;
uniform sampler2D uTexel1;

out vec4 FragColor;

void main()
{
      vec3 viewDirection = normalize(cameraPosition - fPosition);
      vec3 lightDirection = normalize(light.position - fPosition);
      vec3 NormalizedfNormals = normalize(fNormals);

      vec3 ambient = light.ambient * material.Ka;
      vec3 diffuse = light.diffuse * (max(dot(NormalizedfNormals, lightDirection), 0.0) * material.Kd);
      vec3 specular = light.specular * (pow(max(dot(viewDirection, reflect(-lightDirection, NormalizedfNormals)), 0.0), material.Ns) * material.Ks);
      vec4 lightColor = vec4(vec3(ambient + diffuse + specular), 1.0);
      vec4 texelColor = texture2D(uTexel0, fTexCoords);

      FragColor = texelColor * lightColor;
}