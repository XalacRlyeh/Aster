using System.Collections.Generic;
using System.Drawing;
using Aster.Client.Base;
using OpenTK.Mathematics;

namespace Aster.Client
{
    public class TextureAtlas
    {
        private readonly int _tileSize;
        private readonly string _atlasTextureFilePath;
        private readonly IDictionary<int, Vector2[]> _tiles;

        public TextureAtlas(int tileSize, string atlasTextureFilePath)
        {
            _tileSize = tileSize;
            _atlasTextureFilePath = atlasTextureFilePath;
            _tiles = new Dictionary<int, Vector2[]>();
        }

        public Texture AtlasTexture { get; private set; }

        public Vector2[] GetTileUvs(int tileId)
        {
            if (_tiles.TryGetValue(tileId, out var uvs))
            {
                return uvs;
            }

            var tileWidthOnAtlas = 1 / (float)AtlasTexture.Width * _tileSize;
            var tileHeightOnAtlas = 1 / (float)AtlasTexture.Height * _tileSize;

            var tilesPerRow = AtlasTexture.Width / _tileSize;
            var tileY = tileId / tilesPerRow;
            var tileX = tileId % tilesPerRow;

            var u = tileX * tileWidthOnAtlas;
            var v = tileY * tileHeightOnAtlas;

            uvs = new Vector2[4];

            uvs[0] = new Vector2(u, v);
            uvs[1] = new Vector2(u + tileWidthOnAtlas, v);
            uvs[2] = new Vector2(u, v + tileHeightOnAtlas);
            uvs[3] = new Vector2(u + tileWidthOnAtlas, v + tileHeightOnAtlas);

            _tiles.Add(tileId, uvs);
            return uvs;
        }

        public void Load()
        {
            using var bitmap = Image.FromFile(_atlasTextureFilePath);
            AtlasTexture = new Texture("T_LandTiles", (Bitmap)bitmap, false, false);
        }
    }
}
