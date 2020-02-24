using System.Drawing;

namespace JumpingPlatformGame {
	class Entity {
        public WorldPoint Location;
		public virtual Color Color => Color.Black;
		
		public void Update(Seconds deltaSeconds)
		{
			if(this is MovableEntity movableEntity)
			{
				//if you are leaving the screen 
				if (movableEntity.Location.X >= movableEntity.Horizontal.UpperBound ||
					movableEntity.Location.X <=movableEntity.Horizontal.LowerBound)
				{
					//change direction
					movableEntity.Horizontal.Speed *=(-1);
				}
				//update location
				Meters horizontalDelta = deltaSeconds * movableEntity.Horizontal.Speed;
				this.Location.X += horizontalDelta;
			}
			if (this is MovableJumpingEntity movableJumpingEntity)
			{
				//if you are tryint to fly away from the screen
				if (movableJumpingEntity.Location.Y >= movableJumpingEntity.Vertical.UpperBound)
				{
					//change direction
					movableJumpingEntity.Vertical.Speed *= (-1);
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