using UnityEngine;
using System.Collections.Generic;
using Spine;
using System;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class NGUISpine : SkeletonAnimation
	{
		public Action<Vector3[],Color32[],Vector2[]> fillDrawCall;
		public const int X1 = 0;
		public const int Y1 = 1;
		public const int X2 = 2;
		public const int Y2 = 3;
		public const int X3 = 4;
		public const int Y3 = 5;
		public const int X4 = 6;
		public const int Y4 = 7;

		protected override Mesh newMesh ()
		{
			return null;
		}

		public override void LateUpdate ()
		{
			if (!valid)
				return;
			// Count vertices and submesh triangles.
			int vertexCount = 0;
			int submeshTriangleCount = 0, submeshFirstVertex = 0, submeshStartSlotIndex = 0;
			Material lastMaterial = null;
			submeshMaterials.Clear ();
			List<Slot> drawOrder = skeleton.DrawOrder;
			int drawOrderCount = drawOrder.Count;
			bool renderMeshes = this.renderMeshes;
			for (int i = 0; i < drawOrderCount; i++) {
				Slot slot = drawOrder [i];
				Attachment attachment = slot.attachment;
				
				object rendererObject;
				int attachmentVertexCount, attachmentTriangleCount;
				
				if (attachment is RegionAttachment) {
					rendererObject = ((RegionAttachment)attachment).RendererObject;
					attachmentVertexCount = 4;
					attachmentTriangleCount = 6;
				} else {
					if (!renderMeshes)
						continue;
					if (attachment is MeshAttachment) {
						MeshAttachment meshAttachment = (MeshAttachment)attachment;
						rendererObject = meshAttachment.RendererObject;
						attachmentVertexCount = meshAttachment.vertices.Length >> 1;
						attachmentTriangleCount = meshAttachment.triangles.Length;
					} else if (attachment is SkinnedMeshAttachment) {
						SkinnedMeshAttachment meshAttachment = (SkinnedMeshAttachment)attachment;
						rendererObject = meshAttachment.RendererObject;
						attachmentVertexCount = meshAttachment.uvs.Length >> 1;
						attachmentTriangleCount = meshAttachment.triangles.Length;
					} else
						continue;
				}
				
				// Populate submesh when material changes.
				Material material = (Material)((AtlasRegion)rendererObject).page.rendererObject;
				if ((lastMaterial != material && lastMaterial != null) || slot.Data.name [0] == '*') {
					AddSubmesh (lastMaterial, submeshStartSlotIndex, i, submeshTriangleCount, submeshFirstVertex, false);
					submeshTriangleCount = 0;
					submeshFirstVertex = vertexCount;
					submeshStartSlotIndex = i;
				}
				lastMaterial = material;
				
				submeshTriangleCount += attachmentTriangleCount;
				vertexCount += attachmentVertexCount;
			}
			AddSubmesh (lastMaterial, submeshStartSlotIndex, drawOrderCount, submeshTriangleCount, submeshFirstVertex, true);
			
			// Set materials.
			if (submeshMaterials.Count == sharedMaterials.Length)
				submeshMaterials.CopyTo (sharedMaterials);
			else
				sharedMaterials = submeshMaterials.ToArray ();
//			GetComponent<Renderer>().sharedMaterials = sharedMaterials;
			
			// Ensure mesh data is the right size.
			Vector3[] vertices = this.vertices;
			bool newTriangles = vertexCount > vertices.Length;
			if (newTriangles) {
				// Not enough vertices, increase size.
				this.vertices = vertices = new Vector3[vertexCount];
				this.colors = new Color32[vertexCount];
				this.uvs = new Vector2[vertexCount];
//				mesh1.Clear ();
//				mesh2.Clear ();
			} else {
				// Too many vertices, zero the extra.
				Vector3 zero = Vector3.zero;
				for (int i = vertexCount, n = lastVertexCount; i < n; i++)
					vertices [i] = zero;
			}
			lastVertexCount = vertexCount;
			
			// Setup mesh.
			float[] tempVertices = this.tempVertices;
			Vector2[] uvs = this.uvs;
			Color32[] colors = this.colors;
			int vertexIndex = 0;
			Color32 color = new Color32 ();
			float zSpacing = this.zSpacing;
			float a = skeleton.a * 255, r = skeleton.r, g = skeleton.g, b = skeleton.b;
			for (int i = 0; i < drawOrderCount; i++) {
				Slot slot = drawOrder [i];
				Attachment attachment = slot.attachment;
				if (attachment is RegionAttachment) {
					RegionAttachment regionAttachment = (RegionAttachment)attachment;
					regionAttachment.ComputeWorldVertices (slot.bone, tempVertices);
					
					float z = i * zSpacing;
					vertices [vertexIndex] = new Vector3 (tempVertices [X1], tempVertices [Y1], z);
					vertices [vertexIndex + 1] = new Vector3 (tempVertices [X2], tempVertices [Y2], z);
					vertices [vertexIndex + 2] = new Vector3 (tempVertices [X3], tempVertices [Y3], z);
					vertices [vertexIndex + 3] = new Vector3 (tempVertices [X4], tempVertices [Y4], z);
					
					color.a = (byte)(a * slot.a * regionAttachment.a);
					color.r = (byte)(r * slot.r * regionAttachment.r * color.a);
					color.g = (byte)(g * slot.g * regionAttachment.g * color.a);
					color.b = (byte)(b * slot.b * regionAttachment.b * color.a);
					if (slot.data.additiveBlending)
						color.a = 0;
					colors [vertexIndex] = color;
					colors [vertexIndex + 1] = color;
					colors [vertexIndex + 2] = color;
					colors [vertexIndex + 3] = color;
					
					float[] regionUVs = regionAttachment.uvs;
					uvs [vertexIndex] = new Vector2 (regionUVs [X1], regionUVs [Y1]);
					uvs [vertexIndex + 1] = new Vector2 (regionUVs [X2], regionUVs [Y2]);
					uvs [vertexIndex + 2] = new Vector2 (regionUVs [X3], regionUVs [Y3]);
					uvs [vertexIndex + 3] = new Vector2 (regionUVs [X4], regionUVs [Y4]);
					
					vertexIndex += 4;
				} else {
					if (!renderMeshes)
						continue;
					if (attachment is MeshAttachment) {
						MeshAttachment meshAttachment = (MeshAttachment)attachment;
						int meshVertexCount = meshAttachment.vertices.Length;
						if (tempVertices.Length < meshVertexCount)
							this.tempVertices = tempVertices = new float[meshVertexCount];
						meshAttachment.ComputeWorldVertices (slot, tempVertices);
						
						color.a = (byte)(a * slot.a * meshAttachment.a);
						color.r = (byte)(r * slot.r * meshAttachment.r * color.a);
						color.g = (byte)(g * slot.g * meshAttachment.g * color.a);
						color.b = (byte)(b * slot.b * meshAttachment.b * color.a);
						if (slot.data.additiveBlending)
							color.a = 0;
						
						float[] meshUVs = meshAttachment.uvs;
						float z = i * zSpacing;
						for (int ii = 0; ii < meshVertexCount; ii += 2, vertexIndex++) {
							vertices [vertexIndex] = new Vector3 (tempVertices [ii], tempVertices [ii + 1], z);
							colors [vertexIndex] = color;
							uvs [vertexIndex] = new Vector2 (meshUVs [ii], meshUVs [ii + 1]);
						}
					} else if (attachment is SkinnedMeshAttachment) {
						SkinnedMeshAttachment meshAttachment = (SkinnedMeshAttachment)attachment;
						int meshVertexCount = meshAttachment.uvs.Length;
						if (tempVertices.Length < meshVertexCount)
							this.tempVertices = tempVertices = new float[meshVertexCount];
						meshAttachment.ComputeWorldVertices (slot, tempVertices);
						
						color.a = (byte)(a * slot.a * meshAttachment.a);
						color.r = (byte)(r * slot.r * meshAttachment.r * color.a);
						color.g = (byte)(g * slot.g * meshAttachment.g * color.a);
						color.b = (byte)(b * slot.b * meshAttachment.b * color.a);
						if (slot.data.additiveBlending)
							color.a = 0;
						
						float[] meshUVs = meshAttachment.uvs;
						float z = i * zSpacing;
						for (int ii = 0; ii < meshVertexCount; ii += 2, vertexIndex++) {
							vertices [vertexIndex] = new Vector3 (tempVertices [ii], tempVertices [ii + 1], z);
							colors [vertexIndex] = color;
							uvs [vertexIndex] = new Vector2 (meshUVs [ii], meshUVs [ii + 1]);
						}
					}
				}
			}

			if(fillDrawCall!=null)
				fillDrawCall(vertices,colors,uvs);
		}
	}
} // namespace RO
