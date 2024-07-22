
#ifndef GPUSKINNING_INCLUDE
#define GPUSKINNING_INCLUDE

uniform sampler2D _GPUSkinning_TextureMatrix;

uniform float3 _GPUSkinning_TextureSize_NumPixelsPerFrame;

UNITY_INSTANCING_BUFFER_START(GPUSkinningProperties0)
	UNITY_DEFINE_INSTANCED_PROP(float3, _GPUSkinning_FrameIndex_PixelSegmentation)
UNITY_INSTANCING_BUFFER_END(GPUSkinningProperties0)

inline float4 indexToUV(float index)
{
	int row = (int)(index / _GPUSkinning_TextureSize_NumPixelsPerFrame.x);
	float col = index - row * _GPUSkinning_TextureSize_NumPixelsPerFrame.x;
	return float4(col / _GPUSkinning_TextureSize_NumPixelsPerFrame.x, row / _GPUSkinning_TextureSize_NumPixelsPerFrame.y, 0, 0);
}

inline float4x4 getMatrix(int frameStartIndex, float boneIndex)
{
	float matStartIndex = frameStartIndex + boneIndex * 3;
	float4 row0 = tex2Dlod(_GPUSkinning_TextureMatrix, indexToUV(matStartIndex));
	float4 row1 = tex2Dlod(_GPUSkinning_TextureMatrix, indexToUV(matStartIndex + 1));
	float4 row2 = tex2Dlod(_GPUSkinning_TextureMatrix, indexToUV(matStartIndex + 2));
	float4 row3 = float4(0, 0, 0, 1);
	float4x4 mat = float4x4(row0, row1, row2, row3);
	return mat;
}

inline float getFrameStartIndex()
{
	float3 frameIndex_segment = UNITY_ACCESS_INSTANCED_PROP(GPUSkinningProperties0, _GPUSkinning_FrameIndex_PixelSegmentation);
	float segment = frameIndex_segment.y;
	float frameIndex = frameIndex_segment.x;
	float frameStartIndex = segment + frameIndex * _GPUSkinning_TextureSize_NumPixelsPerFrame.z;
	return frameStartIndex;
}

inline int gpu_Skin_uv()
{
	return UNITY_ACCESS_INSTANCED_PROP(GPUSkinningProperties0, _GPUSkinning_FrameIndex_PixelSegmentation).z;
}

inline float4 gpuSkin1(float4 vertex, float4 uv2, float4 uv3)
{
	float frameStartIndex = getFrameStartIndex();
	float4x4 mat0 = getMatrix(frameStartIndex, uv2.x);
	return mul(mat0, vertex) * uv2.y;
}

inline float4 gpuSkin2(float4 vertex, float4 uv2, float4 uv3)
{
	float frameStartIndex = getFrameStartIndex();
	float4x4 mat0 = getMatrix(frameStartIndex, uv2.x);
	float4x4 mat1 = getMatrix(frameStartIndex, uv2.z);
	return mul(mat0, vertex) * uv2.y +  mul(mat1, vertex) * uv2.w;
}

inline float4 gpuSkin4(float4 vertex, float4 uv2, float4 uv3)
{
	float frameStartIndex = getFrameStartIndex();
	float4x4 mat0 = getMatrix(frameStartIndex, uv2.x);
	float4x4 mat1 = getMatrix(frameStartIndex, uv2.z);
	float4x4 mat2 = getMatrix(frameStartIndex, uv3.x);
	float4x4 mat3 = getMatrix(frameStartIndex, uv3.z);

	return	mul(mat0, vertex) * uv2.y +
			mul(mat1, vertex) * uv2.w +
			mul(mat2, vertex) * uv3.y +
			mul(mat3, vertex) * uv3.w;
}

#endif