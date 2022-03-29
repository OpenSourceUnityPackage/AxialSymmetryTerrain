using UnityEngine;

namespace AST
{
    public class RotateScript : MonoBehaviour
    {
        // horizontal flip
        public static void HorizontalFlip(Terrain activeTerrain)
        {
            int i, j;
            TerrainData td = activeTerrain.terrainData;

            //rotate heightmap
            float[,] hgts = td.GetHeights(0, 0, td.heightmapResolution, td.heightmapResolution);
            float[,] newhgts = new float[hgts.GetLength(0), hgts.GetLength(1)];
            for (j = 0; j < td.heightmapResolution; j++)
            {
                for (i = 0; i < td.heightmapResolution; i++)
                {
                    newhgts[td.heightmapResolution - 1 - i, j] = hgts[i, j];
                }
            }

            td.SetHeights(0, 0, newhgts);
            activeTerrain.Flush();

            //rotate splatmap
            float[,,] alpha = td.GetAlphamaps(0, 0, td.alphamapWidth, td.alphamapHeight);
            float[,,] newalpha = new float[alpha.GetLength(0), alpha.GetLength(1), alpha.GetLength(2)];
            for (j = 0; j < td.alphamapHeight; j++)
            {
                for (i = 0; i < td.alphamapWidth; i++)
                {
                    for (int k = 0; k < td.terrainLayers.Length; k++)
                    {
                        newalpha[td.alphamapHeight - 1 - i, j, k] = alpha[i, j, k];
                    }
                }
            }

            td.SetAlphamaps(0, 0, newalpha);

            //rotate trees
            Vector3 size = td.size;
            TreeInstance[] trees = td.treeInstances;
            for (i = 0; i < trees.Length; i++)
            {
                trees[i].position = new Vector3(trees[i].position.x, 0, 1 - trees[i].position.z);
                trees[i].position.y = td.GetInterpolatedHeight(trees[i].position.x, trees[i].position.z) / size.y;
            }

            td.treeInstances = trees;

            //rotate detail layers
            int num = td.detailPrototypes.Length;
            for (int k = 0; k < num; k++)
            {
                int[,] map = td.GetDetailLayer(0, 0, td.detailWidth, td.detailHeight, k);
                int[,] newmap = new int[map.GetLength(0), map.GetLength(1)];
                for (j = 0; j < td.detailHeight; j++)
                {
                    for (i = 0; i < td.detailWidth; i++)
                    {
                        newmap[td.detailHeight - 1 - i, j] = map[i, j];
                    }
                }

                td.SetDetailLayer(0, 0, k, newmap);
            }
        }

        public static void VerticalFlip(Terrain activeTerrain)
        {
            int i, j;
            TerrainData td = activeTerrain.terrainData;

            //rotate heightmap
            float[,] hgts = td.GetHeights(0, 0, td.heightmapResolution, td.heightmapResolution);
            float[,] newhgts = new float[hgts.GetLength(0), hgts.GetLength(1)];
            for (j = 0; j < td.heightmapResolution; j++)
            {
                for (i = 0; i < td.heightmapResolution; i++)
                {
                    newhgts[i, td.heightmapResolution - 1 - j] = hgts[i, j];
                }
            }

            td.SetHeights(0, 0, newhgts);
            activeTerrain.Flush();

            //rotate splatmap
            float[,,] alpha = td.GetAlphamaps(0, 0, td.alphamapWidth, td.alphamapHeight);
            float[,,] newalpha = new float[alpha.GetLength(0), alpha.GetLength(1), alpha.GetLength(2)];
            for (j = 0; j < td.alphamapHeight; j++)
            {
                for (i = 0; i < td.alphamapWidth; i++)
                {
                    for (int k = 0; k < td.terrainLayers.Length; k++)
                    {
                        newalpha[i, td.alphamapHeight - 1 - j, k] = alpha[i, j, k];
                    }
                }
            }

            td.SetAlphamaps(0, 0, newalpha);

            //rotate trees
            Vector3 size = td.size;
            TreeInstance[] trees = td.treeInstances;
            for (i = 0; i < trees.Length; i++)
            {
                trees[i].position = new Vector3(1 - trees[i].position.x, 0, trees[i].position.z);
                trees[i].position.y = td.GetInterpolatedHeight(trees[i].position.x, trees[i].position.z) / size.y;
            }

            td.treeInstances = trees;

            //rotate detail layers
            int num = td.detailPrototypes.Length;
            for (int k = 0; k < num; k++)
            {
                int[,] map = td.GetDetailLayer(0, 0, td.detailWidth, td.detailHeight, k);
                int[,] newmap = new int[map.GetLength(0), map.GetLength(1)];
                for (j = 0; j < td.detailHeight; j++)
                {
                    for (i = 0; i < td.detailWidth; i++)
                    {
                        newmap[i, td.detailHeight - 1 - j] = map[i, j];
                    }
                }

                td.SetDetailLayer(0, 0, k, newmap);
            }
        }
    }
}