using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace NeoTestApp.Code
{
	public class Camera
	{
		#region fields
		public Vector3 Position { get; private set; } = new Vector3(0, 0, 0);
		public Vector3 LookAtVector { get; private set; } = new Vector3(0, 0, 0);
		public Vector3 UpVector { get; private set; } = Vector3.UnitZ;
		public float AspectRatio { get; set; } = 1;
		public float NearClipPlane { get; private set; } = 0.001f;
		public float FarClipPlane { get; private set; } = 10000;

		public Matrix Projection { get; private set; }

		public Matrix View
		{
			get
			{
				return Matrix.CreateLookAt(Position, LookAtVector, UpVector);
			}
		}

		public float FieldOfView
		{
			get
			{
				return _fieldOfView;
			}
			set
			{
				if (value < 2 && value > 0.1f)
					_fieldOfView = value;
			}
		}
		#endregion

		#region properties
		float _circlePos = (float)Math.PI * 2;
		float _distance = 11;
		float _heigth = 6;
		float _fieldOfView = MathHelper.ToRadians(30);
		int _previousScrollValue;
		float _targetDistance = 6;
		float _targetHeight = 4;
		float _targetCirclePos;
		#endregion

		#region methods

		#endregion
		public Camera(float aspectRatio)
		{
			this.AspectRatio = aspectRatio;
			Position = getCirclePos(_circlePos);
			UpdateProjection();
		}

		private Vector3 getCirclePos(float circlePos)
		{
			return new Vector3((float)Math.Sin(circlePos) * _distance, (float)Math.Cos(circlePos) * _distance + 1, _heigth);
		}

		private void UpdateProjection()
		{
			Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, this.AspectRatio, NearClipPlane, FarClipPlane);
		}

		public void Update(GameTime gameTime)
		{

				MouseState m = Mouse.GetState();
				KeyboardState k = Keyboard.GetState();

				if (k.IsKeyDown(Keys.Up))
					_targetHeight += 0.4f;

				if (k.IsKeyDown(Keys.Down))
					_targetHeight -= 0.4f;

				if (k.IsKeyDown(Keys.Left))
					_targetCirclePos += 0.04f;

				if (k.IsKeyDown(Keys.Right))
					_targetCirclePos -= 0.04f;


				if (m.ScrollWheelValue < _previousScrollValue)
				{
					if (k.IsKeyDown(Keys.LeftShift))
						_targetHeight += 2f;
					else if (k.IsKeyDown(Keys.LeftControl))
						_targetCirclePos += 0.4f;
					else
					{
						_targetDistance += 2f;
						_targetHeight += 2f;
					}
				}
				else if (m.ScrollWheelValue > _previousScrollValue)
				{
					if (k.IsKeyDown(Keys.LeftShift))
						_targetHeight -= 1f;
					else if (k.IsKeyDown(Keys.LeftControl))
						_targetCirclePos -= 0.2f;
					else
					{
						_targetDistance -= 1f;
						_targetHeight -= 1f;
					}
				}
				_previousScrollValue = m.ScrollWheelValue;
			

			if (_targetDistance != _distance)
			{
				_distance += (_targetDistance * (float)gameTime.ElapsedGameTime.TotalSeconds * 5);
				_targetDistance -= (_targetDistance * (float)gameTime.ElapsedGameTime.TotalSeconds * 5);
				_heigth += (_targetHeight * (float)gameTime.ElapsedGameTime.TotalSeconds * 5);
				_targetHeight -= (_targetHeight * (float)gameTime.ElapsedGameTime.TotalSeconds * 5);

				UpdateProjection();
			}

			if (_targetCirclePos != _circlePos)
			{
				_circlePos += (_targetCirclePos * (float)gameTime.ElapsedGameTime.TotalSeconds * 5);
				_targetCirclePos -= (_targetCirclePos * (float)gameTime.ElapsedGameTime.TotalSeconds * 5);
				Position = getCirclePos(_circlePos);
			}

			
		}
	}
}
