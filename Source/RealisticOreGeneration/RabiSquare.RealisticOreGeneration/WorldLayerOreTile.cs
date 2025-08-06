using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RabiSquare.RealisticOreGeneration;

[UsedImplicitly]
[StaticConstructorOnStartup]
public class WorldLayerOreTile : WorldDrawLayerBase
{
    private static readonly Material OreInfoTileDepletion =
        MaterialPool.MatFrom("World/SelectedTile", ShaderDatabase.WorldOverlayAdditive, Color.red, 3559);

    private static readonly Material OreInfoTileComplete =
        MaterialPool.MatFrom("World/SelectedTile", ShaderDatabase.WorldOverlayAdditive, Color.green, 3559);

    private static readonly Material OreInfoTileHalf =
        MaterialPool.MatFrom("World/SelectedTile", ShaderDatabase.WorldOverlayAdditive, Color.cyan, 3559);

    private readonly List<Vector3> _verts = [];

    public override IEnumerable Regenerate()
    {
        foreach (var item in base.Regenerate())
        {
            yield return item;
        }

        DrawDepletionInfo();
        FinalizeMesh(MeshParts.All);
    }

    private void DrawDepletionInfo()
    {
        if (Prefs.DevMode)
        {
            Log.Message(
                $"[RabiSquare.RealisticOreGeneration]total mesh: {BaseSingleTon<WorldOreInfoRecorder>.Instance.WorldOreInfoTile.Count()}");
        }

        foreach (var item in BaseSingleTon<WorldOreInfoRecorder>.Instance.WorldOreInfoTile)
        {
            if (BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileAbandoned(item))
            {
                DrawOreInfoCursor(item, OreInfoTileDepletion);
            }
            else if (BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileScannedSurface(item) &&
                     BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileScannedUnderground(item))
            {
                DrawOreInfoCursor(item, OreInfoTileComplete);
            }
            else if (BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileScannedSurface(item) ||
                     BaseSingleTon<WorldOreInfoRecorder>.Instance.IsTileScannedUnderground(item))
            {
                DrawOreInfoCursor(item, OreInfoTileHalf);
            }
        }
    }

    private void DrawOreInfoCursor(PlanetTile tileId, Material material)
    {
        var subMesh = GetSubMesh(material);
        Find.WorldGrid.GetTileVertices(tileId, _verts);
        var count = subMesh.verts.Count;
        var i = 0;
        for (var count2 = _verts.Count; i < count2; i++)
        {
            subMesh.verts.Add(_verts[i] + (_verts[i].normalized * 0.012f));
            subMesh.uvs.Add((GenGeo.RegularPolygonVertexPosition(count2, i) + Vector2.one) / 2f);
            if (i >= count2 - 2)
            {
                continue;
            }

            subMesh.tris.Add(count + i + 2);
            subMesh.tris.Add(count + i + 1);
            subMesh.tris.Add(count);
        }
    }
}