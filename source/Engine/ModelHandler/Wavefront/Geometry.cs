using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Cherry.Engine.ModelHandler.Wavefront
{
    public class Face
    {
        public class ElementReference
        {
            public enum ElementType
            {
                Vertex,
                TextureCoordinate,
                VertexNormal
            };

            private int _index;
            private ElementType _type;
            private dynamic _data;

            public int Index { get => _index; set => _index = value; }
            public ElementType Type { get => _type; set => _type = value; }
            public dynamic Data { get => _data; set => _data = value; }
            
            public ElementReference(int index, ElementType type, dynamic data)
            {
                Index = index;
                Type = type;
                Data = data;
            }
        }
        public class Element
        {
            private int _index;
            //private bool __vt, __vn;
            private Vector2 _vt;
            private Vector3 _v, _vn;
            private float[] _buf;

            public int Index { get => _index; set => _index = value; }
            public float[] DataBuffer { get => _buf; set => _buf = value; }
            
            public Element()
            {
                
            }
            
            public Element(int index, List<ElementReference> e)
            {
                Index = index;
                if (e.Count != 3 * 3)
                    return;
                
                List<float[]> BufferList = new List<float[]>();
                for (int i = 0; i < e.Count; i += 3)
                {
                    _v = e[i].Data;
                    _vt = e[i + 1].Data;
                    _vn = e[i + 2].Data;
                    
                    float[] f = new float[8] { _v.X, _v.Y, _v.Z, _vt.X, _vt.Y, _vn.X, _vn.Y, _vn.Z };
                    BufferList.Add(f);
                }
                DataBuffer = BufferList.SelectMany(x => x).ToArray();
            }
        }
        
        private int _index;
        private string _usemtl;
        private bool _smooth_on;
        private List<ElementReference> _erefs;
        private List<Element> _elements;
        
        public int Index { get => _index; set => _index = value; }
        public string CurrentMaterial { get => _usemtl; set => _usemtl = value; }
        public bool SmoothShadingOn { get => _smooth_on; set => _smooth_on = value; }
        public List<ElementReference> ElementReferences { get => _erefs; set => _erefs = value; }
        public List<Element> Elements { get => _elements; set => _elements = value; }


        public Face()
        {
            Index = -1;
            CurrentMaterial = null;
            SmoothShadingOn = false;
            ElementReferences = new List<ElementReference>();
        }
        public Face(Geometry g, int index, string usemtl, bool smooth_on, string f)
        {
            Index = index;
            CurrentMaterial = usemtl;
            SmoothShadingOn = smooth_on;
            ElementReferences = new List<ElementReference>();
            Elements = new List<Element>();
            int vIndex = -1;

            string[] fTokens = f.Split(' ');
            for (int i = 1; i < fTokens.Length; i++)
            {
                string[] iTokens = fTokens[i].Split('/');

                vIndex = int.Parse(iTokens[0]) - 1;
                ElementReferences.Add(new ElementReference(vIndex, ElementReference.ElementType.Vertex, g.Vertices[vIndex]));
                
                if (iTokens.Length > 1) // vt included
                {
                    int vtIndex = -1;
                    if (String.IsNullOrEmpty(iTokens[1]))
                    {
                        vtIndex = -1;
                    }
                    else
                    {
                        vtIndex = int.Parse(iTokens[1]) - 1;
                    }
                    ElementReferences.Add(new ElementReference(vtIndex, ElementReference.ElementType.TextureCoordinate, g.TextureCoordinates[vtIndex]));
                    
                    if (iTokens.Length > 2) // vn included
                    {
                        int vnIndex = -1;
                        if (String.IsNullOrEmpty(iTokens[2]))
                        {
                            vnIndex = -1;
                        }
                        else
                        {
                            vnIndex = int.Parse(iTokens[2]) - 1;
                        }
                        ElementReferences.Add(new ElementReference(vnIndex, ElementReference.ElementType.VertexNormal, g.VertexNormals[vnIndex]));
                    }
                }
            }

            Elements.Add(new Element(vIndex, ElementReferences));
        }
    }
    public class Geometry
    {
        private List<Vector2> _vt;
        private List<Vector3> _v, _vn;
        private List<Face> _f;
        
        public List<Vector3> Vertices { get => _v; set => _v = value; }
        public List<Vector2> TextureCoordinates { get => _vt; set => _vt = value; }
        public List<Vector3> VertexNormals { get => _vn; set => _vn = value; }
        public List<Face> Faces { get => _f; set => _f = value; }

        public Geometry()
        {
            Vertices = new List<Vector3>();
            TextureCoordinates = new List<Vector2>();
            VertexNormals = new List<Vector3>();
            Faces = new List<Face>();
        }

        public Geometry(string[] obj)
        {
            Vertices = new List<Vector3>();
            TextureCoordinates = new List<Vector2>();
            VertexNormals = new List<Vector3>();
            Faces = new List<Face>();

            string usemtl = null;
            bool smooth_on = false;
            int fCount = 0;
            
            for (int i = 0; i < obj.Length; i++)
            {
                string[] src = obj[i].Split(' ');

                switch (src[0])
                {
                    case "v":
                        Vertices.Add(new Vector3(float.Parse(src[1]), float.Parse(src[2]), float.Parse(src[3])));
                        break;
                    case "vt":
                        TextureCoordinates.Add(new Vector2(float.Parse(src[1]), float.Parse(src[2])));
                        break;
                    case "vn":
                        VertexNormals.Add(new Vector3(float.Parse(src[1]), float.Parse(src[2]), float.Parse(src[3])));
                        break;
                    case "usemtl":
                        usemtl = src[1];
                        break;
                    case "s":
                        smooth_on = (src[1] == "on") ? true : false;
                        break;
                    case "f":
                        Faces.Add(new Face(this, fCount++, usemtl, smooth_on, obj[i]));
                        break;
                }
            }
        }
    }
}