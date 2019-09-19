using AsteroidsEngine;
using OpenTK.Graphics.OpenGL;

namespace AsteroidsApp
{
    public class Model : Texture
    {
        private readonly int[] _indices =
        {
            0, 1,
            1, 2,
            2, 3,
            3, 0,

            4, 5,
            5, 9,
            9, 8,
            8, 4,
            5, 8,

            6, 5,
            5, 9,
            
            4, 5,
            5, 7,
            7, 8,
            8, 9,
            
            4, 5,
            5, 6,
            6, 7,
            7, 8,
            
            4, 6,
            6, 7,
            5, 9,
            
            4, 5,
            4, 6,
            6, 7,
            7, 8,
            
            5, 6,
            6, 7,
            7, 9,
            9, 8,
            8, 6,
            
            4, 5,
            5, 6,
            6, 8,
            
            4, 5,
            5, 9,
            9, 8,
            8, 4,
            6, 7,
            
            4, 5,
            5, 7,
            7, 6,
            6, 4,
            7, 8,
            
            4, 5,
            5, 9,
            9, 8,
            8, 4,
            10, 11,
            11, 12,
            
            13, 15,
            14, 16,
            
            17, 18,
            17, 19,
            19, 20,
            20, 22,
            22, 21,
            23, 24,
            23, 25,
            25, 26,
            27, 28,
            28, 30,
            30, 29,
            29, 27,
            31, 32,
            32, 34,
            34, 33,
            33, 36,
            31, 35,
            37, 38,
            37, 41,
            39, 40,
            41, 42,
            43, 45,
            45, 46,
            46, 44,
            44, 43,
            47, 49,
            49, 50,
            50, 48,
            48, 47,
            
            51,52,
            51,53,
            53,54,
            54,55,
            55,56,
            
            57,58,
            57,61,
            58,62,
            59,60,
            
            63,65,
            64,65,
            63,66,
            64,67,
            
            68,69,
            70,71,
            72,73,
            68,72,
            
            74, 75,
            75, 77,
            74, 76,
            76, 77,
            
            78,80,
            79,80,
            
            81,82,
            83,84,
            85,86,
            81,85,
            
            87,88,
            88,90,
            90,89,
            89,92,
            87,91,
            
            93,94,
            93,95,
            95,96,
            96,97,
            
            98,102,
            100,99,
            99,101,
            
            103,104,
            104,105,
            105,103,
            104,106,
            
            107,108,
            108,109,
            109,110,
            
            111, 112,
            112, 113,
            113, 114,
            
            115,116,
            117,118,
            115,119,
            
            120,123,
            121,122,
            123,124,
            
            125, 126,
            126, 128,
            128, 127,
            127, 125,
            
            129,133,
            131,130,
            130,132,
            
            134,135,
            135,136,
            134,136,
            135,137,
            
            138,139,
            139,140,
            140,141,
        };

        private readonly int[] _begin = {
            8,18,22,30,38,44,52,62,68,78,88,100,104,162
        };

        private readonly int[] _lengths =
        {
            10,4,8,8,6,8,10,6,10,10,12,4,58,138
        };

        private readonly float[] _vertices =
        {
            0f, 1f, 0f, 0f, 0f,         //0
            1f, -1f, 0f, 0f, 0f,        //1
            0f, -0.5f, 0f, 0f, 0f,      //2
            -1f, -1f, 0f, 0f, 0f,       //3
            
            -0.5f, 0.8f, 0f, 0f, 0f,    //4
            0.5f, 0.8f, 0f, 0f, 0f,     //5
            -0.5f, 0f, 0f, 0f, 0f,      //6
            0.5f, 0f, 0f, 0f, 0f,       //7
            -0.5f, -0.8f, 0f, 0f, 0f,   //8
            0.5f, -0.8f, 0f, 0f, 0f,    //9
            
            -0.25f, 0.5f, 0f, 0f, 0f,    //10
            -0.25f, -0.5f, 0f, 0f, 0f,   //11
            0.25f, -0.5f, 0f, 0f, 0f,    //12
            
            -1f, 1f, 0f, 0f, 0f,         //13
            1f, 1f, 0f, 0f, 0f,          //14
            -1f, -1f, 0f, 0f, 0f,        //15
            1f, -1f, 0f, 0f, 0f,         //16
            
            -2f, 0.65f, 0f, 0f, 0f,    //17
            -1.5f, 0.65f, 0f, 0f, 0f,    //18
            -2f, 0.1f, 0f, 0f, 0f,   //19
            -1.5f, 0.1f, 0f, 0f, 0f,   //20
            -2f, -0.6f, 0f, 0f, 0f,      //21
            -1.5f, -0.6f, 0f, 0f, 0f,      //22
            
            -1f, 0.65f, 0f, 0f, 0f, //23
            -0.5f, 0.65f, 0f, 0f, 0f, //24
            -1f, -0.6f, 0f, 0f, 0f, //25
            -0.5f, -0.6f, 0f, 0f, 0f, //26
            
            -0.16f, 0.65f, 0f, 0f, 0f, //27
            -0.16f, -0.6f, 0f, 0f, 0f, //28
            0.66f, 0.65f, 0f, 0f, 0f, //29
            0.66f, -0.6f, 0f, 0f, 0f, //30
            
            1f, 0.65f, 0f, 0f, 0f, //31
            1.5f, 0.65f, 0f, 0f, 0f,//32
            1f, 0.1f, 0f, 0f, 0f,//33
            1.5f, 0.1f, 0f, 0f, 0f,//34
            1f, -0.6f, 0f, 0f, 0f,//35
            1.5f, -0.6f, 0f, 0f, 0f,//36
            
            1.84f, 0.65f, 0f, 0f, 0f,//37
            2.34f, 0.65f, 0f, 0f, 0f,//38
            1.84f, 0.1f, 0f, 0f, 0f,//39
            2.09f, 0.1f, 0f, 0f, 0f,//40
            1.84f, -0.6f, 0f, 0f, 0f,//41
            2.34f, -0.6f, 0f, 0f, 0f,//42
            
            2.68f, 0.65f, 0f, 0f, 0f,//43
            2.62f, 0.59f, 0f, 0f, 0f,//44
            2.74f, 0.59f, 0f, 0f, 0f,//45
            2.68f, 0.53f, 0f, 0f, 0f,//46

            2.68f, -0.48f, 0f, 0f, 0f,//47
            2.62f, -0.54f, 0f, 0f, 0f,//48
            2.74f, -0.54f, 0f, 0f, 0f,//49
            2.68f, -0.6f, 0f, 0f, 0f,//50
            
            -3.7f, 0.8f, 0f, 0f, 0f,//51
            -3.2f, 0.8f, 0f, 0f, 0f,//52
            -3.7f, -0.1f, 0f, 0f, 0f,//53
            -3.2f, -0.1f, 0f, 0f, 0f,//54
            -3.2f, 0.2f, 0f, 0f, 0f,//55
            -3.3f, 0.2f, 0f, 0f, 0f,//56
            
            -2.7f, 0.8f, 0f, 0f, 0f,//57
            -2.3f, 0.8f, 0f, 0f, 0f,//58
            -2.7f, 0.4f, 0f, 0f, 0f,//59
            -2.3f, 0.4f, 0f, 0f, 0f,//60
            -2.7f, -0.1f, 0f, 0f, 0f,//61
            -2.3f, -0.1f, 0f, 0f, 0f,//62
            
            -1.8f, 0.8f, 0f, 0f, 0f,//63
            -1.1f, 0.8f, 0f, 0f, 0f,//64
            -1.45f, 0.5f, 0f, 0f, 0f,//65
            -1.8f, -0.1f, 0f, 0f, 0f,//66
            -1.1f, -0.1f, 0f, 0f, 0f,//67
            
            -0.6f, 0.8f, 0f, 0f, 0f,//68
            -0.2f, 0.8f, 0f, 0f, 0f,//69
            -0.6f, 0.4f, 0f, 0f, 0f,//70
            -0.3f, 0.4f, 0f, 0f, 0f,//71
            -0.6f, -0.1f, 0f, 0f, 0f,//72
            -0.2f, -0.1f, 0f, 0f, 0f,//73
            
            0.5f, 0.8f, 0f, 0f, 0f,//74
            0.9f, 0.8f, 0f, 0f, 0f,//75
            0.5f, -0.1f, 0f, 0f, 0f,//76
            0.9f, -0.1f, 0f, 0f, 0f,//77
            
            1.4f, 0.8f, 0f, 0f, 0f,//78
            1.9f, 0.8f, 0f, 0f, 0f,//79
            1.65f, -0.1f, 0f, 0f, 0f,//80
            
            2.4f, 0.8f, 0f, 0f, 0f,//81
            2.8f, 0.8f, 0f, 0f, 0f,//82
            2.4f, 0.4f, 0f, 0f, 0f,//83
            2.7f, 0.4f, 0f, 0f, 0f,//84
            2.4f, -0.1f, 0f, 0f, 0f,//85
            2.8f, -0.1f, 0f, 0f, 0f,//86
            
            3.3f, 0.8f, 0f, 0f, 0f,//87
            3.7f, 0.8f, 0f, 0f, 0f,//88
            3.3f, 0.4f, 0f, 0f, 0f,//89
            3.7f, 0.4f, 0f, 0f, 0f,//90
            3.3f, -0.1f, 0f, 0f, 0f,//91
            3.7f, -0.1f, 0f, 0f, 0f,//92
            
            -3.8f, -0.4f, 0f, 0f, 0f,//93
            -3.8f, -1f, 0f, 0f, 0f,//94
            -3.5f, -0.4f, 0f, 0f, 0f,//95
            -3.5f, -0.7f, 0f, 0f, 0f,//96
            -3.8f, -0.7f, 0f, 0f, 0f,//97
            
            -3.3f, -0.5f, 0f, 0f, 0f,//98
            -3.15f, -0.5f, 0f, 0f, 0f,//99
            -3.3f, -0.7f, 0f, 0f, 0f,//100
            -3f, -0.7f, 0f, 0f, 0f,//101
            -3.3f, -1f, 0f, 0f, 0f,//102
            
            -2.6f, -0.5f, 0f, 0f, 0f,//103
            -2.8f, -0.7f, 0f, 0f, 0f,//104
            -2.4f, -0.7f, 0f, 0f, 0f,//105
            -2.6f, -1f, 0f, 0f, 0f,//106
            
            -1.95f, -0.5f, 0f, 0f, 0f,//107
            -2.2f, -0.7f, 0f, 0f, 0f,//108
            -1.9f, -0.7f, 0f, 0f, 0f,//109
            -2.15f, -1f, 0f, 0f, 0f,//110

            -1.45f, -0.5f, 0f, 0f, 0f,//111
            -1.7f, -0.7f, 0f, 0f, 0f,//112
            -1.4f, -0.7f, 0f, 0f, 0f,//113
            -1.65f, -1f, 0f, 0f, 0f,//114
            
            -0.9f, -0.4f, 0f, 0f, 0f,//115
            -0.6f, -0.4f, 0f, 0f, 0f,//116
            -0.9f, -0.7f, 0f, 0f, 0f,//117
            -0.7f, -0.7f, 0f, 0f, 0f,//118
            -0.9f, -1f, 0f, 0f, 0f,//119
            
            -0.3f, -0.4f, 0f, 0f, 0f,//120 t
            -0.3f, -0.6f, 0f, 0f, 0f,//121
            -0.15f, -0.6f, 0f, 0f, 0f,//122
            -0.3f, -0.8f, 0f, 0f, 0f,//123
            -0.15f, -1f, 0f, 0f, 0f,//124
            
            0f, -0.5f, 0f, 0f, 0f,//125
            0.3f, -0.5f, 0f, 0f, 0f,//126
            0f, -1f, 0f, 0f, 0f,//127
            0.3f, -1f, 0f, 0f, 0f,//128

            0.8f, -0.5f, 0f, 0f, 0f,//129
            0.95f, -0.5f, 0f, 0f, 0f,//130
            0.8f, -0.7f, 0f, 0f, 0f,//131
            1.1f, -0.7f, 0f, 0f, 0f,//132
            0.8f, -1f, 0f, 0f, 0f,//133
            
            1.5f, -0.5f, 0f, 0f, 0f,//134
            1.3f, -0.7f, 0f, 0f, 0f,//135
            1.7f, -0.7f, 0f, 0f, 0f,//136
            1.5f, -1f, 0f, 0f, 0f,//137
            
            2.15f, -0.5f, 0f, 0f, 0f,//138
            1.9f, -0.7f, 0f, 0f, 0f,//139
            2.2f, -0.7f, 0f, 0f, 0f,//140
            1.95f, -1f, 0f, 0f, 0f,//141
        };

        public Model(string path) : base(path)
        {
        }

        public override void GenIndices()
        {
        }

        public override void InitBuffers()
        {
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
                BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices,
                BufferUsageHint.StaticDraw);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            var vertexLocation = ServiceLocator.GetShader().GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = ServiceLocator.GetShader().GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float),
                3 * sizeof(float));
        }

        public void RenderModel(int num)
        {
            if (num < 14)
                GL.DrawElements(PrimitiveType.Lines, _lengths[num], 
                    DrawElementsType.UnsignedInt, _begin[num]*sizeof(int));
            else
                GL.DrawElements(PrimitiveType.Lines, 8, DrawElementsType.UnsignedInt, 0);
        }
    }
}