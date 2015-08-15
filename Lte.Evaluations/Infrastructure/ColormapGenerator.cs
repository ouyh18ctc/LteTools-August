using System;
using Lte.Evaluations.Entities;

namespace Lte.Evaluations.Infrastructure
{
    public abstract class ColormapGenerator
    {
        protected readonly int colormapLength;

        protected Color[] cmap;

        public Color[] Cmap
        {
            get { return cmap; }
        }

        protected readonly double[] array;
        protected readonly byte alphaValue;

        protected ColormapGenerator(int colormapLength, byte alphaValue)
        {
            this.colormapLength = colormapLength;
            cmap = new Color[colormapLength];
            array = new double[colormapLength];
            this.alphaValue = alphaValue;
            for (int i = 0; i < colormapLength; i++)
            {
                cmap[i] = new Color();
            }
            GenerateMap();
        }

        protected abstract void GenerateMap();
    }

    public class CoolColormapGenerator : ColormapGenerator
    {
        public CoolColormapGenerator(int colormapLength, byte alphaValue = 255)
            : base(colormapLength, alphaValue)
        { }

        protected override void GenerateMap()
        {
            for (int i = 0; i < colormapLength; i++)
            {
                array[i] = 1.0 * i / (colormapLength - 1);
                cmap[i].ColorA = alphaValue;
                cmap[i].ColorR = (byte)(255 * array[i]);
                cmap[i].ColorG = (byte)(255 * (1 - array[i]));
                cmap[i].ColorB = 255;
            }
        }
    }

    public class GrayColormapGenerator : ColormapGenerator
    {
        public GrayColormapGenerator(int colormapLength, byte alphaValue = 255)
            : base(colormapLength, alphaValue)
        { }

        protected override void GenerateMap()
        {
            for (int i = 0; i < colormapLength; i++)
            {
                array[i] = 1.0 * i / (colormapLength - 1);
                cmap[i].ColorA = alphaValue;
                cmap[i].ColorR = (byte)(255 * array[i]);
                cmap[i].ColorG = (byte)(255 * array[i]);
                cmap[i].ColorB = (byte)(255 * array[i]);
            }
        }
    }

    public class HotColormapGenerator : ColormapGenerator
    {
        public HotColormapGenerator(int colormapLength, byte alphaValue = 255)
            : base(colormapLength, alphaValue)
        { }

        protected override void GenerateMap()
        {
            int n1 = 3 * colormapLength / 8;
            double[] red1 = new double[colormapLength];
            double[] green1 = new double[colormapLength];
            double[] blue1 = new double[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                if (i < n1)
                    red1[i] = 1.0 * (i + 1.0) / n1;
                else
                    red1[i] = 1.0;
                if (i < n1)
                    green1[i] = 0.0;
                else if (i >= n1 && i < 2 * n1)
                    green1[i] = 1.0 * (i + 1 - n1) / n1;
                else
                    green1[i] = 1.0;
                if (i < 2 * n1)
                    blue1[i] = 0.0;
                else
                    blue1[i] = 1.0 * (i + 1 - 2 * n1) / (colormapLength - 2 * n1);

                cmap[i].ColorA = alphaValue;
                cmap[i].ColorR = (byte)(255 * red1[i]);
                cmap[i].ColorG = (byte)(255 * green1[i]);
                cmap[i].ColorB = (byte)(255 * blue1[i]);
            }
        }
    }

    public class JetColormapGenerator : ColormapGenerator
    {
        public JetColormapGenerator(int colormapLength, byte alphaValue = 255)
            : base(colormapLength, alphaValue)
        { }

        protected override void GenerateMap()
        {
            int n = (int)Math.Ceiling(colormapLength / 4.0);
            double[,] cMatrix = new double[colormapLength, 3];
            int nMod = 0;
            double[] array1 = new double[3 * n - 1];
            int[] red = new int[array1.Length];
            int[] green = new int[array1.Length];
            int[] blue = new int[array1.Length];

            if (colormapLength % 4 == 1)
                nMod = 1;

            for (int i = 0; i < array1.Length; i++)
            {
                if (i < n)
                    array1[i] = (i + 1.0) / n;
                else if (i >= n && i < 2 * n - 1)
                    array1[i] = 1.0;
                else if (i >= 2 * n - 1)
                    array1[i] = (3.0 * n - 1.0 - i) / n;
                green[i] = (int)Math.Ceiling(n / 2.0) - nMod + i;
                red[i] = green[i] + n;
                blue[i] = green[i] - n;
            }

            int nb = 0;
            for (int i = 0; i < blue.Length; i++)
            {
                if (blue[i] > 0)
                    nb++;
            }

            for (int i = 0; i < colormapLength; i++)
            {
                for (int j = 0; j < red.Length; j++)
                {
                    if (i == red[j] && red[j] < colormapLength)
                        cMatrix[i, 0] = array1[i - red[0]];
                }
                for (int j = 0; j < green.Length; j++)
                {
                    if (i == green[j] && green[j] < colormapLength)
                        cMatrix[i, 1] = array1[i - green[0]];
                }
                for (int j = 0; j < blue.Length; j++)
                {
                    if (i == blue[j] && blue[j] >= 0)
                        cMatrix[i, 2] = array1[array1.Length - 1 - nb + i];
                }
            }

            for (int i = 0; i < colormapLength; i++)
            {
                cmap[i].ColorA = alphaValue;
                cmap[i].ColorR = (byte)(cMatrix[i, 0] * 255);
                cmap[i].ColorG = (byte)(cMatrix[i, 1] * 255);
                cmap[i].ColorB = (byte)(cMatrix[i, 2] * 255);
            }
        }
    }

    public class AutumnColormapGenerator : ColormapGenerator
    {
        public AutumnColormapGenerator(int colormapLength, byte alphaValue = 255)
            : base(colormapLength, alphaValue)
        { }

        protected override void GenerateMap()
        {
            for (int i = 0; i < colormapLength; i++)
            {
                array[i] = 1.0 * i / (colormapLength - 1);
                cmap[i].ColorA = alphaValue;
                cmap[i].ColorR = 255;
                cmap[i].ColorG = (byte)(255 * array[i]);
                cmap[i].ColorB = 0;
            }
        }
    }

    public class SpringColormapGenerator : ColormapGenerator
    {
        public SpringColormapGenerator(int colormapLength, byte alphaValue = 255)
            : base(colormapLength, alphaValue)
        { }

        protected override void GenerateMap()
        {
            for (int i = 0; i < colormapLength; i++)
            {
                array[i] = 1.0 * i / (colormapLength - 1);
                cmap[i].ColorA = alphaValue;
                cmap[i].ColorR = 255;
                cmap[i].ColorG = (byte)(255 * array[i]);
                cmap[i].ColorB = (byte)(255 - cmap[i].ColorG);
            }
        }
    }

    public class SummerColormapGenerator : ColormapGenerator
    {
        public SummerColormapGenerator(int colormapLength, byte alphaValue = 255)
            : base(colormapLength, alphaValue)
        { }

        protected override void GenerateMap()
        {
            for (int i = 0; i < colormapLength; i++)
            {
                array[i] = 1.0 * i / (colormapLength - 1);
                cmap[i].ColorA = alphaValue;
                cmap[i].ColorR = (byte)(255 * array[i]);
                cmap[i].ColorG = (byte)(255 * 0.5 * (1 + array[i]));
                cmap[i].ColorB = (byte)(255 * 0.4);
            }
        }
    }

    public class WinterColormapGenerator : ColormapGenerator
    {
        public WinterColormapGenerator(int colormapLength, byte alphaValue = 255)
            : base(colormapLength, alphaValue)
        { }

        protected override void GenerateMap()
        {
            for (int i = 0; i < colormapLength; i++)
            {
                array[i] = 1.0 * i / (colormapLength - 1);
                cmap[i].ColorA = alphaValue;
                cmap[i].ColorR = 0;
                cmap[i].ColorG = (byte)(255 * array[i]);
                cmap[i].ColorB = (byte)(255 * (1.0 - 0.5 * array[i]));
            }
        }
    }
}
