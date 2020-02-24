using System.Drawing;

namespace JumpingPlatformGame {
	class Entity {
        public WorldPoint Location;
		public virtual Color Color => Color.Black;
		
		public void Update(Seconds deltaSeconds)
		{
			if(this is MovableEntity movableEntity)
			{
				Meters horizontalDelta = deltaSeconds * movableEntity.Horizontal.Speed;
				Meters newXlocation = movableEntity.Location.X + horizontalDelta;

				//if you are leaving the screen 
				if (newXlocation >= movableEntity.Horizontal.UpperBound)					
				{
					//change direction
					movableEntity.Horizontal.Speed *=(-1);
					//move as far as you can
					movableEntity.Location.X = movableEntity.Horizontal.UpperBound;
				}
				else if(newXlocation <= movableEntity.Horizontal.LowerBound)
				{
					//change direction
					movableEntity.Horizontal.Speed *= (-1);
					//move as far as you can
					movableEntity.Location.X = movableEntity.Horizontal.LowerBound;
				}
				else
				{
					//just move
					movableEntity.Location.X += horizontalDelta;
				}
				
				
			}
			if (this is MovableJumpingEntity movableJumpingEntity)
			{
				Meters verticalDelta = deltaSeconds * movableJumpingEntity.Vertical.Speed;
				Meters newYlocation = movableJumpingEntity.Location.Y + verticalDelta;
				//if you are tryint to fly away from the screen
				if (newYlocation >= movableJumpingEntity.Vertical.UpperBound)
				{
					//change direction
					movableJumpingEntity.Vertical.Speed *= (-1);
					//move as far as you can
					movableJumpingEntity.Location.Y = movableJumpingEntity.Vertical.UpperBound;
				}
				//if you reach the ground
				else if (newYlocation <= movableJumpingEntity.Vertical.LowerBound)
				{
					//just stop falling
					movableJumpingEntity.Location.Y = movableJumpingEntity.Vertical.LowerBound;
				}
				else
				{
					//just move
					movableJumpingEntity.Location.Y += verticalDelta;
				}

			}
		}
	}

	class MovableEntity : Entity {
        public Movement Horizontal;
		public MovableEntity()
		{
			this.Horizontal = new Movement();
		}
	}

	class MovableJumpingEntity : MovableEntity {
        public Movement Vertical;
		public MovableJumpingEntity():base()
		{
			this.Vertical = new Movement();
		}
	}

	class Joe : MovableEntity {
		public override string ToString() => "Joe";
		public override Color Color => Color.Blue;
	}

	class Jack : MovableEntity {
		public override string ToString() => "Jack";
		public override Color Color => Color.LightBlue;
	}

	class Jane : MovableJumpingEntity {
		public override string ToString() => "Jane";
		public override Color Color => Color.Red;
	}

	class Jill : MovableJumpingEntity {
		public override string ToString() => "Jill";
		public override Color Color => Color.Pink;
	}

    public class WorldPoint
    {
        public Meters X;
        public Meters Y;
    }

    public class Movement
    {
        public Meters LowerBound;
        public Meters UpperBound;
        public MeterPerSeconds Speed;
    }
}