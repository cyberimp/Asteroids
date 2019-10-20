using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using OpenTK.Graphics.OpenGL;
using AsteroidsEngine;

namespace AsteroidsApp
{
    public class Model : Texture
    {
        private int[] _begin;
        private int[] _lengths;

        public Model(Shader shader, string path) : base(shader, path)
        {
        }

        public override void GenIndices()
        {
            var indices = new List<uint>();
            var vertices = new List<float>();
            var a = Assembly.GetExecutingAssembly();
            var myName = a.GetName().Name;
            using (var stream = a.GetManifestResourceStream(myName + "." + Path + ".points"))
            {
                using var reader = new StreamReader(stream ??
                                                    throw new FileNotFoundException(".points not found"));
                var line = reader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var nums = line.Split(',');
                    vertices.Add(float.Parse(nums[0], NumberFormatInfo.InvariantInfo));
                    vertices.Add(float.Parse(nums[1], NumberFormatInfo.InvariantInfo));
                    for (var i = 0; i < 3; i++)
                        vertices.Add(0f);

                    line = reader.ReadLine();
                }

                Vertices = vertices.ToArray();
            }


            using (var stream = a.GetManifestResourceStream(myName + "." + Path + ".lines"))
            {
                using var reader = new StreamReader(stream ??
                                                    throw new FileNotFoundException(".lines not found"));
                var begin = new List<int>();
                var lengths = new List<int>();
                var start = 0;
                var length = 0;
                var line = reader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    if (char.IsLetter(line[0]))
                    {
                        if (length > 0)
                        {
                            begin.Add(start);
                            lengths.Add(length);
                            start += length;
                            length = 0;
                        }
                    }
                    else
                    {
                        var nums = line.Split(',');
                        indices.Add(uint.Parse(nums[0]));
                        indices.Add(uint.Parse(nums[1]));
                        length += 2;
                    }

                    line = reader.ReadLine();
                }

                begin.Add(start);
                lengths.Add(length);
                _begin = begin.ToArray();
                _lengths = lengths.ToArray();
                Indices = indices.ToArray();
            }
        }


        public void RenderModel(int num)
        {
            GL.DrawElements(PrimitiveType.Lines, _lengths[num],
                DrawElementsType.UnsignedInt, _begin[num] * sizeof(int));
        }
    }
}