namespace worldgen
{
    public class Generation
    {
        // Useful Math methods
        
        public static double map(double x, double in_min, double in_max, double out_min, double out_max) {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
        
        // Noise functions

        public static double GetNoiseOctave(ref FastNoiseLite noise, float x, float y, int o, float p, float l)
        {
            double total = 0;
            float freq = 1;
            double amp = 1;
            double max = 0;
            
            for (int i = 0; i < o; i++)
            {
                total += noise.GetNoise(x * freq, y * freq) * amp;
                max += amp;
                amp *= p;
                freq *= l;
            }

            return total / max;
        }
        
        // Generation methods
    }
}