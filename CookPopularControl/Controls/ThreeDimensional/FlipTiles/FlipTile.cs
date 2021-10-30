﻿using CookPopularControl.Expression.Drawing.Core;
using CookPopularControl.Tools.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：FlipTile
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-10 14:43:36
 */
namespace CookPopularControl.Controls.ThreeDimensional
{
    internal class FlipTile : ButtonBase3D
    {
        private readonly TranslateTransform3D m_translate = new TranslateTransform3D();
        private readonly QuaternionRotation3D m_quaternionRotation3D = new QuaternionRotation3D();
        private readonly AxisAngleRotation3D m_verticalFlipRotation = new AxisAngleRotation3D(new Vector3D(1, 0, 0), RandomHelper.Rnd.NextDouble() * 360 + 720);
        private readonly ScaleTransform3D m_scaleTransform = new ScaleTransform3D();
        private readonly DiffuseMaterial m_borderMaterial = new DiffuseMaterial(Brushes.Black);
        private readonly DiffuseMaterial _frontMaterial;
        private const double _diff = 0.000001;
        private readonly double _weird = RandomHelper.Rnd.NextDouble() * 0.1 + 0.85;


        private Point3D _locationDesired;
        private Point3D _locationCurrent;
        private Vector3D m_locationVelocity;
        private Size _size;
        private Vector3D CurrentLocationVector => new Vector3D(_locationCurrent.X, _locationCurrent.Y, _locationCurrent.Z);
        private Quaternion m_rotationCurrent = new Quaternion();
        private Quaternion m_rotationVelocity = new Quaternion();

        private double _scaleDesired = 1;
        private double _scaleCurrent = 1;
        private double _scaleVelocity = 0;
        private double _flipVerticalVelocity = 0;

        public Brush FrontBrush => _frontMaterial.Brush;

        public FlipTile(DiffuseMaterial frontMaterial, Size size, Point center, Material backMaterial, Rect backTextureCoordinates)
        {
            _locationDesired = new Point3D(center.X, center.Y, 0);
            _locationCurrent = new Point3D(0, 0, RandomHelper.Rnd.NextDouble() * 10 - 20);
            _size = size;

            Point3D topLeft = new Point3D(-size.Width / 2, size.Height / 2, 0);
            Point3D topRight = new Point3D(size.Width / 2, size.Height / 2, 0);
            Point3D bottomLeft = new Point3D(-size.Width / 2, -size.Height / 2, 0);
            Point3D bottomRight = new Point3D(size.Width / 2, -size.Height / 2, 0);

            _frontMaterial = frontMaterial;

            Model3DGroup quad = new Model3DGroup();
            quad.Children.Add(CreateTile(frontMaterial, backMaterial, m_borderMaterial, new Size3D(size.Width, size.Height, .01), backTextureCoordinates));

            Transform3DGroup group = new Transform3DGroup();

            group.Children.Add(new RotateTransform3D(m_verticalFlipRotation));
            group.Children.Add(new RotateTransform3D(m_quaternionRotation3D));

            group.Children.Add(m_scaleTransform);
            group.Children.Add(m_translate);

            quad.Transform = group;

            this.Visual3DModel = quad;
        }

        public bool TickData(Vector lastMouse, bool isFlipped)
        {
            bool somethingChanged = false;

            //active means nothing in the "flipped" mode
            bool isActiveItem = IsMouseOver && !isFlipped;
            bool goodMouse = lastMouse.IsValid();

            #region rotation

            Quaternion rotationTarget = new Quaternion(new Vector3D(1, 0, 0), 0);

            //apply forces
            rotationTarget.Normalize();
            m_rotationCurrent.Normalize();

            double angle = 0;
            Vector3D axis = new Vector3D(0, 0, 1);
            if (lastMouse.IsValid() && !isFlipped)
            {
                Point3D mouse = new Point3D(lastMouse.X, lastMouse.Y, 1);
                Vector3D line = mouse - _locationCurrent;
                Vector3D straight = new Vector3D(0, 0, 1);

                angle = Vector3D.AngleBetween(line, straight);
                axis = Vector3D.CrossProduct(line, straight);
            }
            Quaternion rotationForceTowardsMouse = new Quaternion(axis, -angle);

            Quaternion rotationForceToDesired = rotationTarget - m_rotationCurrent;

            Quaternion rotationForce = rotationForceToDesired + rotationForceTowardsMouse;

            m_rotationVelocity *= new Quaternion(rotationForce.Axis, rotationForce.Angle * .2);

            //dampenning
            m_rotationVelocity = new Quaternion(m_rotationVelocity.Axis, m_rotationVelocity.Angle * (_weird - .3));

            //apply terminal velocity
            m_rotationVelocity = new Quaternion(m_rotationVelocity.Axis, m_rotationVelocity.Angle);

            m_rotationVelocity.Normalize();

            //apply to position
            m_rotationCurrent *= m_rotationVelocity;
            m_rotationCurrent.Normalize();

            //see if there is any real difference between what we calculated and what actually exists
            if (AnyDiff(m_quaternionRotation3D.Quaternion.Axis, m_rotationCurrent.Axis, _diff) ||
                AnyDiff(m_quaternionRotation3D.Quaternion.Angle, m_rotationCurrent.Angle, _diff))
            {
                //if the angles are both ~0, the axis may be way off but the result is basically the same
                //check for this and forget animating in this case
                if (AnyDiff(m_quaternionRotation3D.Quaternion.Angle, 0, _diff) || AnyDiff(m_rotationCurrent.Angle, 0, _diff))
                {
                    m_quaternionRotation3D.Quaternion = m_rotationCurrent;
                    somethingChanged = true;
                }
            }

            #endregion

            #region flip

            double verticalFlipTarget = isFlipped ? 180 : 0;
            double verticalFlipCurrent = m_verticalFlipRotation.Angle;

            //force
            double verticalFlipForce = verticalFlipTarget - verticalFlipCurrent;

            //velocity
            _flipVerticalVelocity += 0.3 * verticalFlipForce;

            //dampening
            _flipVerticalVelocity *= (_weird - 0.3);

            //terminal velocity
            _flipVerticalVelocity = LimitDouble(_flipVerticalVelocity, 10);

            //apply
            verticalFlipCurrent += _flipVerticalVelocity;

            if (AnyDiff(verticalFlipCurrent, m_verticalFlipRotation.Angle, _diff) && AnyDiff(_flipVerticalVelocity, 0, _diff))
            {
                m_verticalFlipRotation.Angle = verticalFlipCurrent;
            }

            #endregion

            #region scale

            if (isActiveItem && !isFlipped)
            {
                this._scaleDesired = 2;
            }
            else
            {
                this._scaleDesired = 1;
            }

            double scaleForce = this._scaleDesired - this._scaleCurrent;
            this._scaleVelocity += 0.1 * scaleForce;
            //dampening
            this._scaleVelocity *= 0.8;
            //terminal velocity
            this._scaleVelocity = LimitDouble(this._scaleVelocity, 0.05);
            this._scaleCurrent += this._scaleVelocity;

            if (AnyDiff(m_scaleTransform.ScaleX, _scaleCurrent, _diff) || AnyDiff(m_scaleTransform.ScaleY, _scaleCurrent, _diff))
            {
                this.m_scaleTransform.ScaleX = this._scaleCurrent;
                this.m_scaleTransform.ScaleY = this._scaleCurrent;
                somethingChanged = true;
            }

            #endregion

            #region location

            Vector3D locationForce;

            //apply forces
            if (isActiveItem)
            {
                _locationDesired.Z = .1;
            }
            else
            {
                _locationDesired.Z = 0;
            }
            locationForce = _locationDesired - _locationCurrent;

            //only repel the non-active items
            if (!isActiveItem && goodMouse && !isFlipped)
            {
                locationForce += 0.025 * InvertVector(this.CurrentLocationVector - new Vector3D(lastMouse.X, lastMouse.Y, 0));
            }

            m_locationVelocity += 0.1 * locationForce;

            //apply dampenning
            m_locationVelocity *= (_weird - 0.3);

            //apply terminal velocity
            m_locationVelocity = LimitVector3D(m_locationVelocity, 0.3);

            //apply velocity to location
            _locationCurrent += m_locationVelocity;

            if ((GetVector(m_translate) - (Vector3D)_locationCurrent).Length > _diff)
            {
                m_translate.OffsetX = _locationCurrent.X;
                m_translate.OffsetY = _locationCurrent.Y;
                m_translate.OffsetZ = _locationCurrent.Z;
                somethingChanged = true;
            }
            #endregion

            return somethingChanged;
        }


        private static bool AnyDiff(double d1, double d2, double diff)
        {
            Debug.Assert(AreGoodNumbers(d1, d2, diff));
            Debug.Assert(diff >= 0);
            return Math.Abs(d1 - d2) > diff;
        }
        private static bool AnyDiff(Vector3D v1, Vector3D v2, double diff)
        {
            Debug.Assert(diff.IsValid());
            Debug.Assert(diff >= 0);
            double angleBetween = Vector3D.AngleBetween(v1, v2);
            return angleBetween > diff;
        }

        private static bool AreGoodNumbers(params double[] d)
        {
            return d.All(item => item.IsValid());
        }

        public static Vector3D GetVector(TranslateTransform3D transform)
        {
            return new Vector3D(transform.OffsetX, transform.OffsetY, transform.OffsetZ);
        }

        private static Vector3D InvertVector(Vector3D input)
        {
            double invertLength = 1 / input.Length;
            input.Normalize();
            return invertLength * input;
        }

        private static Vector3D LimitVector3D(Vector3D input, double max)
        {
            Debug.Assert(max > 0);
            Debug.Assert(max.IsValid());
            Debug.Assert(input.IsValid());

            if (input.Length > max)
            {
                input.Normalize();
                return input * max;
            }
            else
            {
                return input;
            }
        }

        private static double LimitDouble(double input, double max)
        {
            Debug.Assert(max >= 0);

            if (Math.Abs(input) > max)
            {
                return Math.Sign(input) * max;
            }
            else
            {
                return input;
            }
        }


        private static Model3DGroup CreateTile(Material frontMaterial, Material backMaterial, Material sideMaterial, Size3D size, Rect backMaterialCoordiantes)
        {
            //these are represent half the width, height, depth of the quads, since everything is from zero
            double w = size.X / 2;
            double h = size.Y / 2;
            double d = size.Z / 2;

            //front
            GeometryModel3D front = GetQuad(
                new Point3D(-w, -h, d),
                new Point3D(w, -h, d),
                new Point3D(w, h, d),
                new Point3D(-w, h, d),
                frontMaterial);

            //back
            GeometryModel3D back = GetQuad(
                new Point3D(-w, -h, d),
                new Point3D(w, -h, d),
                new Point3D(w, h, d),
                new Point3D(-w, h, d),
                backMaterial, backMaterialCoordiantes);

            RotateTransform3D backRotate =
                new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 180), new Point3D());
            RotateTransform3D backFlip =
                new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 180), new Point3D());

            Transform3DGroup backTransformGroup = new Transform3DGroup();
            backTransformGroup.Children.Add(backRotate);
            backTransformGroup.Children.Add(backFlip);

            back.Transform = backTransformGroup;

            GeometryModel3D bottom, left, top, right;
            //sides
            {
                //right
                right = GetQuad(
                    new Point3D(w, -h, d),
                    new Point3D(w, -h, -d),
                    new Point3D(w, h, -d),
                    new Point3D(w, h, d), sideMaterial);

                //left
                left = GetQuad(
                    new Point3D(-w, -h, -d),
                    new Point3D(-w, -h, d),
                    new Point3D(-w, h, d),
                    new Point3D(-w, h, -d), sideMaterial);

                //top
                top = GetQuad(
                    new Point3D(-w, h, d),
                    new Point3D(w, h, d),
                    new Point3D(w, h, -d),
                    new Point3D(-w, h, -d), sideMaterial);

                //bottom
                bottom = GetQuad(
                    new Point3D(-w, -h, -d),
                    new Point3D(w, -h, -d),
                    new Point3D(w, -h, d),
                    new Point3D(-w, -h, d), sideMaterial);
            }

            Model3DGroup group = new Model3DGroup();
            group.Children.Add(front);
            group.Children.Add(back);
            group.Children.Add(right);
            group.Children.Add(left);
            group.Children.Add(bottom);
            group.Children.Add(top);

            return group;
        }

        private static GeometryModel3D GetQuad(Point3D bottomLeft, Point3D bottomRight, Point3D topRight, Point3D topLeft, Material material)
        {
            return GetQuad(bottomLeft, bottomRight, topRight, topLeft, material, new Rect(0, 0, 1, 1));
        }

        private static GeometryModel3D GetQuad(Point3D bottomLeft, Point3D bottomRight, Point3D topRight, Point3D topLeft, Material material, Rect textureCoordinates)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(bottomLeft);
            mesh.Positions.Add(bottomRight);
            mesh.Positions.Add(topRight);
            mesh.Positions.Add(topLeft);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(0);

            mesh.TextureCoordinates.Add(textureCoordinates.BottomLeft);
            mesh.TextureCoordinates.Add(textureCoordinates.BottomRight);
            mesh.TextureCoordinates.Add(textureCoordinates.TopRight);
            mesh.TextureCoordinates.Add(textureCoordinates.TopLeft);

            GeometryModel3D gm3d = new GeometryModel3D(mesh, material);
            gm3d.BackMaterial = material;

            return gm3d;
        }
    }
}
