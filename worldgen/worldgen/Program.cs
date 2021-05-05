using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.PixelFormat;
using static worldgen.Generation;

namespace worldgen
{
    class Program
    {
        static int Main(string[] args)
        {
            //Control variables

            const int GENWIDTH = 1024;
            const int GENHEIGHT = 1024;

            const float SCALE = 1.0f;
            const int OCTAVES = 2;
            const float PERSISTANCE = 1.0f;
            const float LACUNARITY = 2.0f;

            // Window setup
            
            InitWindow(GENWIDTH,GENHEIGHT,"Knallkorn's World Generator");

            // Create and configure FastNoise object
            FastNoiseLite noise = new FastNoiseLite(RandomNumberGenerator.GetInt32(9999));
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

            // Gather noise data
            double[] noiseData = new double[GENWIDTH * GENHEIGHT];
            int index = 0;

            index = 0;
            for (int y = 0; y < GENHEIGHT; y++)
            {
                for (int x = 0; x < GENWIDTH; x++)
                {
                    noiseData[index++] = GetNoiseOctave(ref noise, x / SCALE, y / SCALE, OCTAVES, PERSISTANCE, LACUNARITY);
                }
            }

            // Create colour array

            Color[] displayColors = new Color[GENWIDTH * GENHEIGHT];
            index = 0;
            double minVal = noiseData.Min();
            double maxVal = noiseData.Max();
            
            for (int y = 0; y < GENHEIGHT; y++)
            {
                for (int x = 0; x < GENWIDTH; x++)
                {
                    int val = Convert.ToInt32(Math.Round(map(noiseData[index++],minVal,maxVal,0.0,255.0)));
                    displayColors[y * GENWIDTH + x] = new Color(val,val,val,255);
                }
            }

            // Convert to texture
            
            GCHandle pinnedArray = GCHandle.Alloc(displayColors, GCHandleType.Pinned);
            IntPtr displayColorsPtr = pinnedArray.AddrOfPinnedObject();

            Image displayImg = new Image
            {
                data = displayColorsPtr,
                width = GENWIDTH,
                height = GENHEIGHT,
                format = PixelFormat.UNCOMPRESSED_R8G8B8A8,
                mipmaps = 1,
            };

            Texture2D displayTex = LoadTextureFromImage(displayImg);
            pinnedArray.Free();

            //Main loop

            while (!WindowShouldClose())
            {
                // Update-------------
                
                
                
                // Draw---------------
                
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);
                
                DrawTexture(displayTex,0,0,Color.WHITE);
                
                EndDrawing();
                
            }
            
            UnloadTexture(displayTex);
            
            CloseWindow();

            return 0;

        }
    }
}