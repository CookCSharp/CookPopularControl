using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CookPopularControl.Tools.Transitions
{
    /// <summary>
    /// 3D转换的基类
    /// </summary>
    public abstract class Transition3DBase : TransitionBase
    {
        protected Transition3DBase()
        {

        }

        static Transition3DBase()
        {
            Model3DGroup defaultLight = new Model3DGroup();

            Vector3D direction = new Vector3D(1, 1, 1);
            direction.Normalize();
            byte ambient = 108; // 108是定向为<256的最小值（对于方向= [1,1,1]）
            byte directional = (byte)Math.Min((255 - ambient) / Vector3D.DotProduct(direction, new Vector3D(0, 0, 1)), 255);

            defaultLight.Children.Add(new AmbientLight(Color.FromRgb(ambient, ambient, ambient)));
            defaultLight.Children.Add(new DirectionalLight(Color.FromRgb(directional, directional, directional), direction));
            defaultLight.Freeze();
            LightProperty = DependencyProperty.Register("Light", typeof(Model3D), typeof(Transition3DBase), new UIPropertyMetadata(defaultLight));
        }

        public double FieldOfView
        {
            get { return (double)GetValue(FieldOfViewProperty); }
            set { SetValue(FieldOfViewProperty, value); }
        }

        public static readonly DependencyProperty FieldOfViewProperty =
            DependencyProperty.Register("FieldOfView", typeof(double), typeof(Transition3DBase), new UIPropertyMetadata(20.0));

        public Model3D Light
        {
            get { return (Model3D)GetValue(LightProperty); }
            set { SetValue(LightProperty, value); }
        }

        public static readonly DependencyProperty LightProperty;

        //设置Viewport3D
        protected internal sealed override void BeginTransition(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            Viewport3D viewport = new Viewport3D();
            viewport.IsHitTestVisible = false;

            viewport.Camera = CreateCamera(transitionElement, FieldOfView);
            viewport.ClipToBounds = false;
            ModelVisual3D light = new ModelVisual3D();
            light.Content = Light;
            viewport.Children.Add(light);

            transitionElement.Children.Add(viewport);
            BeginTransition3D(transitionElement, oldContent, newContent, viewport);
        }

        protected virtual Camera CreateCamera(TransitionPresenter transitionElement, double fov)
        {
            Size size = transitionElement.RenderSize;
            return new PerspectiveCamera(new Point3D(size.Width / 2, size.Height / 2, -size.Width / Math.Tan(fov / 2 * Math.PI / 180) / 2), new Vector3D(0, 0, 1), new Vector3D(0, -1, 0), fov);
        }

        protected virtual void BeginTransition3D(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent, Viewport3D viewport)
        {
            EndTransition(transitionElement, oldContent, newContent);
        }

        //生成等于U和V向量的侧面启动的平网格
        public static MeshGeometry3D CreateMesh(Point3D origin, Vector3D u, Vector3D v, int usteps, int vsteps, Rect textureBounds)
        {
            u = 1.0 / usteps * u;
            v = 1.0 / vsteps * v;

            MeshGeometry3D mesh = new MeshGeometry3D();

            for (int i = 0; i <= usteps; i++)
            {
                for (int j = 0; j <= vsteps; j++)
                {
                    mesh.Positions.Add(origin + i * u + j * v);

                    mesh.TextureCoordinates.Add(new Point(textureBounds.X + textureBounds.Width * i / usteps,
                                                          textureBounds.Y + textureBounds.Height * j / vsteps));

                    if (i > 0 && j > 0)
                    {
                        mesh.TriangleIndices.Add((i - 1) * (vsteps + 1) + (j - 1));
                        mesh.TriangleIndices.Add((i - 0) * (vsteps + 1) + (j - 0));
                        mesh.TriangleIndices.Add((i - 0) * (vsteps + 1) + (j - 1));

                        mesh.TriangleIndices.Add((i - 1) * (vsteps + 1) + (j - 1));
                        mesh.TriangleIndices.Add((i - 1) * (vsteps + 1) + (j - 0));
                        mesh.TriangleIndices.Add((i - 0) * (vsteps + 1) + (j - 0));
                    }
                }
            }
            return mesh;
        }
    }
}
