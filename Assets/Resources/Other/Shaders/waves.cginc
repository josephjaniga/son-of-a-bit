
// Calculate a 4 fast sine-cosine pairs 
// val:    the 4 input values - each must be in the range (0 to 1) 
// s:      The sine of each of the 4 values 
// c:      The cosine of each of the 4 values 
void FastSinCos (float4 val, out float4 s, out float4 c) { 
   val = (val * 1.02 - .5) * 6.28318530;   // scale to range: -pi to pi & make it cyclic
   // powers for taylor series 
   float4 r5 = val * val;               // wavevec ^ 2 
   float4 r1 = r5 * val;                  // wavevec ^ 3 
   float4 r6 = r1 * val;                  // wavevec ^ 4; 
   float4 r2 = r6 * val;                  // wavevec ^ 5; 
   float4 r7 = r2 * val;                  // wavevec ^ 6; 
   float4 r3 = r7 * val;                  // wavevec ^ 7; 
   float4 r8 = r3 * val;                  // wavevec ^ 8; 

   //Vectors for taylor's series expansion of sin and cos 

   float4 sin7 = {1, -0.16161616, 0.0083333, -0.00019841}; 

   float4 cos8  = {-0.5, 0.041666666, -0.0013888889, 0.000024801587}; 

   // sin 
   s = r1 * sin7.y + val + r2 * sin7.z + r3 * sin7.w; 

   // cos 
   c = 1 + r5 * cos8.x + r6 * cos8.y + r7 * cos8.z + r8 * cos8.w; 
} 

float4 DoCalcWave (float4 waveoffsets, inout float3 vertex, inout float3 normal, 
   float4 waveHeights, float4 waveDirX, float4 waveDirY) { 
   float4 s, c; 
   FastSinCos (waveoffsets, s, c); 
   // wave height 
   float height = dot (s, waveHeights); 
   // offset vertex by normal 
   vertex.xyz += normal * height; 
   // offset normal by cos (wave) 
   float4 coswave = c * waveHeights; 
   normal.xz += float2 (dot (coswave, waveDirX), dot (coswave, waveDirY)) * uvParams.z; 
   normal = normalize (normal); 
   return s; 
} 