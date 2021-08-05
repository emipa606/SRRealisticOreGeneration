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
        private readonly List<Vector3> _verts = new List<Vector3>();

        private static readonly Material OreInfoTileDepletion =
            MaterialPool.MatFrom("World/SelectedTile", ShaderDatabase.WorldOverlayAdditive, Color.red, 3559);

        private static readonly Material OreInfoTileComplete =
            MaterialPool.MatFrom("World/SelectedTile", ShaderDatabase.WorldOverlayAdditive, Color.green, 3559);

        private static readonly Material OreInfoTileHalf =
            MaterialPool.MatFrom("World/SelectedTile", ShaderDatabase.WorldOverlayAdditive, Color.cyan, 3559);


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
            if (Prefs.DevMode)
            {
                Log.Warning($"{MsicDef.LogTag}total mesh: {WorldOreInfoRecorder.Instance.WorldOreInfoTile.Count()}");
            }

            foreach (var oreInfoTileId in WorldOreInfoRecorder.Instance.WorldOreInfoTile)
            {
                if (WorldOreInfoRecorder.Instance.IsTileAbandoned(oreInfoTileId))
                {
                    DrawOreInfoCursor(oreInfoTileId, OreInfoTileDepletion);
                    continue;
                }

                if (WorldOreInfoRecorder.Instance.IsTileScannedSurface(oreInfoTileId) &&
                    WorldOreInfoRecorder.Instance.IsTileScannedUnderground(oreInfoTileId))
                {
                    DrawOreInfoCursor(oreInfoTileId, OreInfoTileComplete);
                    continue;
                }

                if (WorldOreInfoRecorder.Instance.IsTileScannedSurface(oreInfoTileId) ||
                    WorldOreInfoRecorder.Instance.IsTileScannedUnderground(oreInfoTileId))
                {
                    DrawOreInfoCursor(oreInfoTileId, OreInfoTileHalf);
                }
            }
        }

        private void DrawOreInfoCursor(int tileId, Material material)
        {
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