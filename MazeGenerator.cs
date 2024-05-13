using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    public Tilemap mazeTilemap;
    public TileBase userProvidedTile;
    public int level = 0;

    public bool test = false;

    public void Start()
    {
        GenFirstLev();
        GenNextLev(2);
    }
    public void DrawSquare(Vector3Int start, int length)
    {
          for (int i = start.x; i <= start.x + length; i++)
        {
                mazeTilemap.SetTile(new Vector3Int(i, start.y, 0), userProvidedTile);
                mazeTilemap.SetTile(new Vector3Int(i, start.y + length, 0), userProvidedTile);
        }
          for (int i = start.y; i <= start.y + length; i++)
        {
                mazeTilemap.SetTile(new Vector3Int(start.x, i, 0), userProvidedTile);
                mazeTilemap.SetTile(new Vector3Int(start.x + length, i, 0), userProvidedTile);
        }
    }
    public void GenFirstLev()
    {
        DrawSquare(new Vector3Int(0, 0, 0), 24);
        level += 1;
    }

    public void GenNextLev()
    {
        DrawSquare(new Vector3Int(24 * level, level, 0), 24);
        level += 1;
    }

    public void GenNextLev(int lev)
    {
        DrawSquare(new Vector3Int(0, 24 * (lev + 1), 0), 24);
        level += 1;
    }

    public void Test(int x)
    {
        Debug.Log(x);
    }

    public void Update(){

    }
}
