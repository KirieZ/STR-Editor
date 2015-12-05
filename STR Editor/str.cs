using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_Editor
{
    public class PairValues
    {
        public float x { get; set; }
        public float y { get; set; }

        public PairValues(StrStream br)
        {
            this.x = br.ReadSingle();
            this.y = br.ReadSingle();
        }
    }

    public class Color
    {
        public float Color1 { get; set; }
        public float Color2 { get; set; }
        public float Color3 { get; set; }
        public int Color4 { get; set; }
        public int Color5 { get; set; }
        public int Color6 { get; set; }

        public Color(StrStream br)
        {
            this.Color1 = br.ReadSingle();
            this.Color2 = br.ReadSingle();
            this.Color3 = br.ReadSingle();
            this.Color4 = br.ReadInt32();
            this.Color5 = br.ReadInt32();
            this.Color6 = br.ReadInt32();
        }

    }

    public abstract class Layer
    {
        public int TexCount { get; set; }

        protected Layer(StrStream br)
        {
            this.TexCount = br.ReadInt32();
        }
    }

    /// <summary>
    /// Represents a Layer Type 0 (Background)
    /// </summary>
    public class Layer0 : Layer
    {
        public int Unknown0 { get; set; }
        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public int Unknown3 { get; set; }
        public int Unknown4 { get; set; }

        public Layer0(StrStream br) : base(br)
        {
            this.Unknown0 = br.ReadInt32();
            this.Unknown1 = br.ReadInt32();
            this.Unknown2 = br.ReadInt32();
            this.Unknown3 = br.ReadInt32();
            this.Unknown4 = br.ReadInt32();
        }
    }

    /// <summary>
    /// Represents a Layer Type 1 (Normal layers)
    /// </summary>
    public class Layer1 : Layer
    {
        public string[] Textures { get; set; }
        public int AniKeyNum { get; set; }
        public AnimKey[] AnimKeys { get; set; }

        public Layer1(StrStream br) : base(br)
        {
            this.Textures = new string[base.TexCount];

            for (int i = 0; i < base.TexCount; i++)
                this.Textures[i] = br.ReadString(127);

            this.AniKeyNum = br.ReadInt32();
            this.AnimKeys = new AnimKey[this.AniKeyNum];

            for (int i = 0; i < this.AniKeyNum; i++)
                this.AnimKeys[i] = new AnimKey(br);

        }
    }

    public class AnimKey
    {
        public int Frame { get; set; }
        public int AniFrame { get; set; }
        public PairValues Pos { get; set; }
        public PairValues Uv { get; set; }
        public PairValues Uvs { get; set; }
        public PairValues Uv2 { get; set; }
        public PairValues Uvs2 { get; set; }
        public float[] Points { get; set; }
        public int MtPreset { get; set; }
        public int AniType { get; set; }
        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public Color Color { get; set; }
        public float Unknown3 { get; set; }

        public AnimKey(StrStream br)
        {
            this.Frame = br.ReadInt32();
            this.AniFrame = br.ReadInt32();
            this.Pos = new PairValues(br);
            this.Uv = new PairValues(br);
            this.Uvs = new PairValues(br);
            this.Uv2 = new PairValues(br);
            this.Uvs2 = new PairValues(br);

            this.Points = new float[8];
            for (int i = 0; i < 8; i++)
                this.Points[i] = br.ReadSingle();

            this.Unknown1 = br.ReadInt32();
            this.Unknown2 = br.ReadInt32();
            this.Color = new Color(br);
            this.Unknown3 = br.ReadSingle();
        }
    }
    /// <summary>
    /// Represents a STR file
    /// </summary>
    public class Str
    {
        public string Header { get; set; }
        public int Unknown1 { get; set; }
        public int Fps { get; set; }
        public int MaxKey { get; set; }
        public int LayerNum { get; set; }
        public Layer0 BackLayer { get; set; }
        public Layer1[] Layers { get; set; }

        public Str(MemoryStream memData)
        {
            StrStream data = new StrStream(memData);

            this.Header = data.ReadString(4);
            this.Unknown1 = data.ReadInt32();
            this.Fps = data.ReadInt32();
            this.MaxKey = data.ReadInt32();
            this.LayerNum = data.ReadInt32();

            this.BackLayer = new Layer0(data);

            this.Layers = new Layer1[this.LayerNum - 1];
            for (int i = 0; i < LayerNum -1; i++)
            {
                this.Layers[i] = new Layer1(data);
            }
        }
    }

    public class StrStream
    {
        private byte[] buffer { get; set; }
        private MemoryStream stream { get; set; }

        public StrStream(MemoryStream pStream)
        {
            this.buffer = new byte[256];
            this.stream = pStream;
        }

        public string ReadString(int size)
        {
            int i;

            for (i = 0; i < size; i++)
            {
                buffer[i] = (byte)stream.ReadByte();
                if (buffer[i] == 0)
                    break;
            }

            stream.Read(buffer, i, size - i);
            
            return Encoding.UTF8.GetString(buffer, 0, i);
        }

        public int ReadInt32()
        {
            stream.Read(buffer, 0, 4);
            return BitConverter.ToInt32(buffer, 0);
        }

        public float ReadSingle()
        {
            stream.Read(buffer, 0, 4);
            return BitConverter.ToSingle(buffer, 0);
        }
    }
}