using System;
using System.Collections.Generic;
using System.IO;

namespace Cherry.Engine.ModelHandler.Wavefront
{
    public class Object
    {
        private MaterialLibrary _mtllib;
        private Dictionary<string, Geometry> _g;

        public MaterialLibrary MaterialLibrary { get => _mtllib; set => _mtllib = value; }
        public Dictionary<string, Geometry> Groups { get => _g; set => _g = value; }
        public Object()
        {

        }
        public Object(string filepath)
        {
            string[] obj = File.ReadAllLines(filepath);
            Groups = new Dictionary<string, Geometry>();
            for (int i = 0; i < obj.Length; i++)
            {
                string[] src = obj[i].Split(' ');

                switch (src[0])
                {
                    case "mtllib":
                        MaterialLibrary = new MaterialLibrary(Path.GetFullPath(Path.GetDirectoryName(filepath) + "\\" + src[1]));
                        break;
                    case "g":
                        {
                            string groupName = src[1];
                            int j = i;
                            List<string> groupLines = new List<string>();
                            while (!String.IsNullOrWhiteSpace(obj[j++].Split(' ')[0]) && j <= obj.Length - 1)
                            {
                                groupLines.Add(obj[j]);
                            }
                            Groups.Add(groupName, new Geometry(groupLines.ToArray()));
                        }
                        break;
                    case "o":
                        break;
                }
            }
        }
    }
}