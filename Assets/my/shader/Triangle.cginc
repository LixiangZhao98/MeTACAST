#ifndef BODY_CGINC
#define BODY_CGINC

struct Vertex
{
	float3 vPosition;
	float3 vNormal;
};

struct Triangle
{
	Vertex v[3];
};

#endif