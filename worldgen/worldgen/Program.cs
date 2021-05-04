using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
            
            // Window setup
            
            InitWindow(GENWIDTH,GENHEIGHT,"Knallkorn's World Generator");

            // Create and configure FastNoise object
            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

            // Gather noise data
            int[] noiseData = new int[GENWIDTH * GENHEIGHT];
            int index = 0;

            for (int y = 0; y < GENHEIGHT; y++)
            {
                for (int x = 0; x < GENWIDTH; x++)
                {
                    noiseData[index++] = Convert.ToInt32(map(noise.GetNoise(x, y),-1.0,1.0,0.0,255.0));
                }
            }
            
            // Create colour array

            Color[] displayColors = new Color[GENWIDTH * GENHEIGHT];
            index = 0;
            
            for (int y = 0; y < GENHEIGHT; y++)
            {
                for (int x = 0; x < GENWIDTH; x++)
                {
                    displayColors[y * GENWIDTH + x] = new Color(noiseData[index],noiseData[index],noiseData[index++],255);
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