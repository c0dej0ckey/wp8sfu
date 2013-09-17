using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using wp8sfu.VMs;

namespace wp8sfu.Pages
{
    public partial class MapDetailsPage : PhoneApplicationPage
    {
        private bool _needToUpdateMaxZoom = false;
        private int _imageHeight = 0;
        private int _imageWidth = 0;
        // Reference
        // these two fields fully define the zoom state:
        private double _totalImageScale = 1.0;
        private Point _imagePosition = new Point(0, 0);

        private double _maxImageZoom = 1;
        private Point _oldFinger1;
        private Point _oldFinger2;
        private double _oldScaleFactor;

        public MapDetailsPage()
        {
            InitializeComponent();

            this.DataContext = new MapDetailsVM();
            MapDetailsVM vm = this.DataContext as MapDetailsVM;
            WriteableBitmap bi = vm.GetBitmapForCampus();

            _imageHeight = bi.PixelHeight;
            _imageWidth = bi.PixelWidth;
            _imagePosition = new Point(0, 0);
            _totalImageScale = 1;

            
            if (MapImage.ActualWidth == 0.0 || MapImage.ActualHeight == 0.0)
            {
                _needToUpdateMaxZoom = true;
            }
            else
            {
                UpdateMaxZoom();
                UpdateImageScale(1.0);
                UpdateImagePosition(new Point(0, 0));
            }

            //MapImage.Source = bi;

        }

        private void MapImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_needToUpdateMaxZoom)
            {
                if (MapImage.ActualHeight != 0.0 && MapImage.ActualWidth != 0.0)
                {
                    UpdateMaxZoom();
                }
            }
        }

        private void UpdateMaxZoom()
        {
            // this is already stretched, so this gets tricky
            _maxImageZoom = Math.Min(_imageHeight / MapImage.ActualHeight,
                _imageWidth / MapImage.ActualWidth);
            _maxImageZoom *= Math.Max(1.0,
                Math.Max(_imageHeight / MapImage.ActualHeight, _imageWidth / MapImage.ActualWidth));
            const double MAX_ZOOM_FACTOR = 2;
            _maxImageZoom *= MAX_ZOOM_FACTOR;
            _maxImageZoom = Math.Max(1.0, _maxImageZoom);
            _needToUpdateMaxZoom = false;
            UpdateImageScale(1.0);
            UpdateImagePosition(new Point(0, 0));
        }

        private void GestureListener_PinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            _oldFinger1 = e.GetPosition(MapImage, 0);
            _oldFinger2 = e.GetPosition(MapImage, 1);
            _oldScaleFactor = 1;
        }

        private void GestureListener_PinchDelta(object sender, PinchGestureEventArgs e)
        {
            var scaleFactor = e.DistanceRatio / _oldScaleFactor;
            if (!IsScaleValid(scaleFactor))
                return;

            var currentFinger1 = e.GetPosition(MapImage, 0);
            var currentFinger2 = e.GetPosition(MapImage, 1);

            var translationDelta = GetTranslationDelta(currentFinger1, currentFinger2,
                _oldFinger1, _oldFinger2, _imagePosition, scaleFactor);

            _oldFinger1 = currentFinger1;
            _oldFinger2 = currentFinger2;
            _oldScaleFactor = e.DistanceRatio;

            UpdateImageScale(scaleFactor);
            UpdateImagePosition(translationDelta);
        }

        private void GestureListener_DragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            var translationDelta = new Point(e.HorizontalChange, e.VerticalChange);

            if (IsDragValid(1, translationDelta))
                UpdateImagePosition(translationDelta);
        }

        private void GestureListener_DoubleTap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            if (Math.Abs(_totalImageScale - 1) < .0001)
            {
                const double DOUBLE_TAP_ZOOM_IN = 3;
                double imageScale = Math.Min(DOUBLE_TAP_ZOOM_IN, _maxImageZoom);

                Point imagePositionTapped = e.GetPosition(MapImage);
                // we want this point to be centered.
                double x = imagePositionTapped.X * imageScale - (MapImage.ActualWidth / 2);
                double y = imagePositionTapped.Y * imageScale - (MapImage.ActualHeight / 2);
                Point imageDelta = new Point(-1 * x, -1 * y);
                // FFV - animation?
                UpdateImageScale(imageScale);
                UpdateImagePosition(imageDelta);
            }
            else
            {
                ResetImagePosition();
            }
        }

        private Point GetTranslationDelta(Point currentFinger1, Point currentFinger2,
              Point oldFinger1, Point oldFinger2, Point currentPosition, double scaleFactor)
        {
            var newPos1 = new Point(currentFinger1.X + (currentPosition.X - oldFinger1.X) * scaleFactor,
                currentFinger1.Y + (currentPosition.Y - oldFinger1.Y) * scaleFactor);
            var newPos2 = new Point(currentFinger2.X + (currentPosition.X - oldFinger2.X) * scaleFactor,
                currentFinger2.Y + (currentPosition.Y - oldFinger2.Y) * scaleFactor);
            var newPos = new Point((newPos1.X + newPos2.X) / 2, (newPos1.Y + newPos2.Y) / 2);
            return new Point(newPos.X - currentPosition.X, newPos.Y - currentPosition.Y);
        }

        private void UpdateImageScale(double scaleFactor)
        {
            _totalImageScale *= scaleFactor;
            ApplyScale();
        }

        private void ApplyScale()
        {
            ((CompositeTransform)MapImage.RenderTransform).ScaleX = _totalImageScale;
            ((CompositeTransform)MapImage.RenderTransform).ScaleY = _totalImageScale;
        }

        private void UpdateImagePosition(Point delta)
        {
            var newPosition = new Point(_imagePosition.X + delta.X, _imagePosition.Y + delta.Y);
            if (newPosition.X > 0) newPosition.X = 0;
            if (newPosition.Y > 0) newPosition.Y = 0;

            if ((MapImage.ActualWidth * _totalImageScale) + newPosition.X < MapImage.ActualWidth)
                newPosition.X = MapImage.ActualWidth - (MapImage.ActualWidth * _totalImageScale);

            if ((MapImage.ActualHeight * _totalImageScale) + newPosition.Y < MapImage.ActualHeight)
                newPosition.Y = MapImage.ActualHeight - (MapImage.ActualHeight * _totalImageScale);

            _imagePosition = newPosition;

            ApplyPosition();
        }

        private void ApplyPosition()
        {
            ((CompositeTransform)MapImage.RenderTransform).TranslateX = _imagePosition.X;
            ((CompositeTransform)MapImage.RenderTransform).TranslateY = _imagePosition.Y;
        }

        private void ResetImagePosition()
        {
            _totalImageScale = 1;
            _imagePosition = new Point(0, 0);
            ApplyScale();
            ApplyPosition();
        }

        private bool IsDragValid(double scaleDelta, Point translateDelta)
        {
            if (_imagePosition.X + translateDelta.X > 0 || _imagePosition.Y + translateDelta.Y > 0)
                return false;
            if ((MapImage.ActualWidth * _totalImageScale * scaleDelta) +
                (_imagePosition.X + translateDelta.X) < MapImage.ActualWidth)
                return false;
            if ((MapImage.ActualHeight * _totalImageScale * scaleDelta) +
                (_imagePosition.Y + translateDelta.Y) < MapImage.ActualHeight)
                return false;
            return true;
        }

        private bool IsScaleValid(double scaleDelta)
        {
            return (_totalImageScale * scaleDelta >= 1) &&
                   (_totalImageScale * scaleDelta <= _maxImageZoom);
        }
        

    }
}