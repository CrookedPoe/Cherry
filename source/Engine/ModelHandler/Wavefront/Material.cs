using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace Cherry.Engine.ModelHandler.Wavefront
{
    // https://en.wikipedia.org/wiki/Wavefront_.obj_file#Material_template_library
    public class Material
    {
        private MaterialLibrary _mtllib;
        private string _newmtl;
        private string _map_Ka, _map_Kd, _map_Ks;
        private Vector3 _Ka, _Kd, _Ks;
        private float _Ns, _d;
        private int _illum;

        public MaterialLibrary MaterialLibrary { get => _mtllib; set => _mtllib = value; }
        public string Name { get => _newmtl; set => _newmtl = value; }
        public Vector3 AmbientColor { get => _Ka; set => _Ka = value; }
        public string AmbientTextureMap { get => _map_Ka; set => _map_Ka = value; }
        public Vector3 DiffuseColor { get => _Kd; set => _Kd = value; }
        public string DiffuseTextureMap { get => _map_Kd; set => _map_Kd = value; }
        public Vector3 SpecularColor { get => _Ks; set => _Ks = value; }
        public string SpecularTextureMap { get => _map_Ks; set => _map_Ks = value; }
        public float Shininess { get => _Ns; set => _Ns = value; }
        public float Transparency { get => _d; set => _d = value; }
        public int Illumination { get => _illum; set => _illum = value; }
        public Material()
        {
            
        }
    }
    public class MaterialLibrary
    {
        private Dictionary<string, Material> _materials;
        private string _filepath;
        public Dictionary<string, Material> Materials { get => _materials; set => _materials = value; }
        public string Filepath { get => _filepath; set => _filepath = value; }
        public MaterialLibrary(string filepath)
        {
            _filepath = filepath;
            _materials = new Dictionary<string, Material>();

            string[] mtl = File.ReadAllLines(_filepath);
            for (int i = 0; i < mtl.Length; i++)
            {
                string[] l = mtl[i].Split(' ');
                if (l[0] == "newmtl")
                {
                    string newmtl = l[1];
                    _materials.Add(newmtl, new Material());
                    _materials[newmtl].MaterialLibrary = this;
                    int j = i;
                    while (!String.IsNullOrWhiteSpace(mtl[j++].Split(' ')[0]) && j <= mtl.Length - 1)
                    {
                        string[] src = mtl[j].Split(' ');
                        switch (src[0])
                        {
                            case "Ka":
                                _materials[newmtl].AmbientColor = new Vector3(float.Parse(src[1]), float.Parse(src[2]), float.Parse(src[3]));
                                break;
                            case "Kd":
                                _materials[newmtl].DiffuseColor = new Vector3(float.Parse(src[1]), float.Parse(src[2]), float.Parse(src[3]));
                                break;
                            case "Ks":
                                _materials[newmtl].SpecularColor = new Vector3(float.Parse(src[1]), float.Parse(src[2]), float.Parse(src[3]));
                                break;
                            case "Ns":
                                _materials[newmtl].Shininess = float.Parse(src[1]);
                                break;
                            case "d":
                                _materials[newmtl].Transparency = float.Parse(src[1]);
                                break;
                            case "illum":
                                _materials[newmtl].Illumination = int.Parse(src[1]);
                                break;
                            case "map_Ka":
                                _materials[newmtl].AmbientTextureMap = ResolveMaterialPath(_materials[newmtl], src[1]);
                                break;
                            case "map_Kd":
                                _materials[newmtl].DiffuseTextureMap = ResolveMaterialPath(_materials[newmtl], src[1]);
                                break;
                            case "map_Ks":
                                _materials[newmtl].SpecularTextureMap = ResolveMaterialPath(_materials[newmtl], src[1]);
                                break;
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
        }
        private static string ResolveMaterialPath(Material mtl, string path)
        {
            string _path = path;

            if (!path.Contains(":")) // Can we reasonably assume this means there's a drive letter in the file path?
            {
                _path = Path.Combine(Path.GetDirectoryName(mtl.MaterialLibrary.Filepath), path);
            }

            _path = Path.GetFullPath(_path);

            if (File.Exists(_path))
            {
                // Good to go.
            }
            else
            {
                throw new Exception($"{_path} does not exist.");
            }

            return _path;
        }
    }
}