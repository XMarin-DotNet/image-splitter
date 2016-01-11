using System;
using System.Drawing;
using System.IO;

namespace ImageSplitter
{
    internal class ImageSplitter : IDisposable
    {
        private readonly int _sizeX;
        private readonly int _sizeY;
        private readonly Image _source;
        private readonly string _format;
        private readonly int _tileSizeX;
        private readonly int _tileSizeY;
        private readonly int _marginX;
        private readonly int _marginY;
        private readonly int _innerTilesX;
        private readonly int _innerTilesY;
        private readonly int _lastTileSizeX;
        private readonly int _lastTileSizeY;
        private readonly Bitmap _combinedBitmap;
        private readonly Graphics _combinedG;
        private readonly string _dirName;
        private readonly Color _background;
        private readonly Brush _foreground;

        public ImageSplitter(Image source, string format, int sizeX, int sizeY, int marginX, int marginY, Color background, Brush foreground)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            _source = source;
            _format = format;
            _marginX = marginX;
            _marginY = marginY;
            _background = background;

            _tileSizeX = _sizeX + 1;
            _tileSizeY = _sizeY + 1;
            _innerTilesX = (_source.Width - 1)/_sizeX;
            _innerTilesY = (_source.Height - 1)/_sizeY;
            _lastTileSizeX = (_source.Width - 1)%_sizeX + 1;
            _lastTileSizeY = (_source.Height - 1)%_sizeY + 1;
            _dirName = "out_" + _sizeX + "_" + _sizeY;

            _combinedBitmap = new Bitmap(
                _marginX + _innerTilesX * _tileSizeX + _lastTileSizeX,
                _marginY + _innerTilesY * _tileSizeY + _lastTileSizeY);

            _combinedG = Graphics.FromImage(_combinedBitmap);
            _foreground = foreground;
        }

        public void Split()
        {
            Directory.CreateDirectory(_dirName);
            _combinedG.Clear(_background);

            DrawInnerTiles();

            for (int i = 0; i < _innerTilesX; i++)
            {
                DrawTile(i, _innerTilesY, _sizeX, _lastTileSizeY);
            }

            for (int j = 0; j < _innerTilesY; j++)
            {
                DrawTile(_innerTilesX, j, _lastTileSizeX, _sizeY);
            }

            DrawTile(_innerTilesX, _innerTilesY, _lastTileSizeX, _lastTileSizeY);

            for (int i = 0; i <= _innerTilesX; i++)
            {
                _combinedG.DrawString((i + 1).ToString(), SystemFonts.DefaultFont, _foreground, _marginX + i * _tileSizeX, 0.0f);
            }

            for (int j = 0; j <= _innerTilesY; j++)
            {
                _combinedG.DrawString((j + 1).ToString(), SystemFonts.DefaultFont, _foreground, 0.0f, _marginY + j*_tileSizeY);
            }

            _combinedBitmap.Save(string.Format("{0}\\_Result.{1}", _dirName, _format));
        }

        private void DrawInnerTiles()
        {
            for (int i = 0; i < _innerTilesX; i++)
            {
                for (int j = 0; j < _innerTilesY; j++)
                {
                    DrawTile(i, j, _sizeX, _sizeY);
                }
            }
        }

        private void DrawTile(int i, int j, int sizeX, int sizeY)
        {
            var srcRect = new Rectangle(i*_sizeX, j*_sizeY, sizeX, sizeY);

            using (var tileBitmap = new Bitmap(sizeX, sizeY))
            using (var g = Graphics.FromImage(tileBitmap))
            {
                g.Clear(_background);

                var destRect = new Rectangle(0, 0, sizeX, sizeY);
                g.DrawImage(_source, destRect, srcRect, GraphicsUnit.Pixel);

                tileBitmap.Save(string.Format("{0}\\{1}_{2}.{3}", _dirName, i + 1, j + 1, _format));
            }

            _combinedG.DrawImage(_source,
                new Rectangle(_marginX + i*_tileSizeX, _marginY + j*_tileSizeY, sizeX, sizeY),
                srcRect, GraphicsUnit.Pixel);
        }

        public void Dispose()
        {
            _combinedG.Dispose();
            _combinedBitmap.Dispose();
        }
    }
}