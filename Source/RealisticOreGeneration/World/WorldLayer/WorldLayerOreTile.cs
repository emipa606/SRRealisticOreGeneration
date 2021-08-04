// ******************************************************************
//       /\ /|       @file       WorldLayerOreTile.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-08-04 05:56:22
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration
{
    [UsedImplicitly]
    [StaticConstructorOnStartup]
    public class WorldLayerOreTile : WorldLayer
    {
        private static readonly Material OreInfoTile =
            MaterialPool.MatFrom("World/SelectedTile", ShaderDatabase.WorldOverlayAdditive, 3559);

        private readonly List<Vector3> _verts = new List<Vector3>();

        private static Material DepletionInfoMaterial
        {
            get
            {
                OreInfoTile.color = Color.red;
                return OreInfoTile;
            }
        }

        private static Material HalfInfoMaterial
        {
            get
            {
                OreInfoTile.color = Color.cyan;
                return OreInfoTile;
            }
        }

        private static Material CompleteInfoMaterial
        {
            get
            {
                OreInfoTile.color = Color.green;
                return OreInfoTile;
            }
        }


        public override IEnumerable Regenerate()
        {
            foreach (var obj in base.Regenerate())
            {
                yield return obj;
            }

            DrawDepletionInfo();
            FinalizeMesh(MeshParts.All);
        }

        private void DrawDepletionInfo()
        {
            Log.Warning(
                $"WorldOreInfoRecorder.Instance.WorldOreInfoTile:{WorldOreInfoRecorder.Instance.WorldOreInfoTile.Count()}");
            foreach (var oreInfoTileId in WorldOreInfoRecorder.Instance.WorldOreInfoTile)
            {
                if (WorldOreInfoRecorder.Instance.IsTileAbandoned(oreInfoTileId))
                {
                    DrawOreInfoCursor(oreInfoTileId, DepletionInfoMaterial);
                    Log.Warning("1");
                    continue;
                }

                if (WorldOreInfoRecorder.Instance.IsTileScannedSurface(oreInfoTileId) &&
                    WorldOreInfoRecorder.Instance.IsTileScannedUnderground(oreInfoTileId))
                {
                    DrawOreInfoCursor(oreInfoTileId, CompleteInfoMaterial);
                    Log.Warning("2");
                    continue;
                }

                if (WorldOreInfoRecorder.Instance.IsTileScannedSurface(oreInfoTileId) ||
                    WorldOreInfoRecorder.Instance.IsTileScannedUnderground(oreInfoTileId))
                {
                    DrawOreInfoCursor(oreInfoTileId, HalfInfoMaterial);
                    Log.Warning("3");
                }
            }
        }

        private void DrawOreInfoCursor(int tileId, Material material)
        {
            Log.Warning($"tileId:{tileId}");
            var subMesh = GetSubMesh(material);
            Find.WorldGrid.GetTileVertices(tileId, _verts);
            var subMeshVertsCount = subMesh.verts.Count;
            var num = 0;
            for (var count2 = _verts.Count; num < count2; ++num)
            {
                subMesh.verts.Add(_verts[num] + _verts[num].normalized * 0.012f);
                subMesh.uvs.Add((GenGeo.RegularPolygonVertexPosition(count2, num) + Vector2.one) / 2f);
                if (num >= count2 - 2) continue;
                subMesh.tris.Add(subMeshVertsCount + num + 2);
                subMesh.tris.Add(subMeshVertsCount + num + 1);
                subMesh.tris.Add(subMeshVertsCount);
            }
        }
    }
}