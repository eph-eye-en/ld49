using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum TileDirection
{
    Up,
    Right,
    Down,
    Left
}

public class QuestableTile : Tile
{
    public Questable QuestablePrefab;
    public TileDirection Direction;

    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
    {
        Debug.Log("StartUp");

        //var c = go.GetComponent<PolygonCollider2D>();
        //var points = new List<Vector2>();
        //sprite.GetPhysicsShape(0, points);
        //c.points = points.ToArray();

        return true;
    }

    // This refreshes itself and other RoadTiles that are orthogonally and diagonally adjacent
    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        //for (int yd = -1; yd <= 1; yd++)
        //    for (int xd = -1; xd <= 1; xd++)
        //    {
        //        Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
        //        if (HasQuestableTile(tilemap, position))
        //            tilemap.RefreshTile(position);
        //    }
    }

    // This determines which sprite is used based on the RoadTiles that are adjacent to it and rotates it to fit the other tiles.
    // As the rotation is determined by the RoadTile, the TileFlags.OverrideTransform is set for the tile.
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
        tileData.color = Color.white;
        var m = tileData.transform;
        m.SetTRS(Vector3.zero, GetRotation(Direction), Vector3.one);
        tileData.transform = m;
        tileData.flags = TileFlags.LockTransform;
        tileData.colliderType = ColliderType.None;

        //tileData.gameObject = QuestablePrefab.gameObject;
    }

    // This determines if the Tile at the position is the same RoadTile.
    private bool HasQuestableTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

    // The following determines which sprite to use based on the number of adjacent RoadTiles
    private int GetIndex(byte mask)
    {
        switch (mask)
        {
            case 0: return 0;
            case 3:
            case 6:
            case 9:
            case 12: return 1;
            case 1:
            case 2:
            case 4:
            case 5:
            case 10:
            case 8: return 2;
            case 7:
            case 11:
            case 13:
            case 14: return 3;
            case 15: return 4;
        }
        return -1;
    }

    // The following determines which rotation to use based on the positions of adjacent RoadTiles
    private Quaternion GetRotation(TileDirection dir)
    {
        var a = 0;
        switch (dir)
        {
            case TileDirection.Up:
                a = 0;
                break;
            case TileDirection.Right:
                a = 270;
                break;
            case TileDirection.Down:
                a = 180;
                break;
            case TileDirection.Left:
                a = 90;
                break;
        }
        return Quaternion.Euler(0, 0, a);
        //switch (mask)
        //{
        //    case 9:
        //    case 10:
        //    case 7:
        //    case 2:
        //    case 8:
        //        return Quaternion.Euler(0f, 0f, -90f);
        //    case 3:
        //    case 14:
        //        return Quaternion.Euler(0f, 0f, -180f);
        //    case 6:
        //    case 13:
        //        return Quaternion.Euler(0f, 0f, -270f);
        //}
        //return Quaternion.Euler(0f, 0f, 0f);
    }

#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/QuestableTile")]
    public static void CreateQuestableTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Questable Tile", "New Questable Tile", "Asset", "Save Questable Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<QuestableTile>(), path);
    }
#endif
}
