using System.Drawing;
using System.Numerics;
using Microsoft.Xna.Framework;
using Nox.Framework;

namespace Nox.Samples;

public class Stage {
    public static Stage Current { get; set; }
    public List<Solid> Solids { get; } = new();
    public List<Actor> Actors { get; } = new();
    public IEnumerable<Entity> Entities => Solids.Cast<Entity>().Concat(Actors);
}


public abstract class Entity {
    public Vector2 Position => new(_hitBox.X + _hitBox.Width/2, _hitBox.Y + _hitBox.Height/2);
    protected Rectangle _hitBox;
    public Entity(Rectangle hitBox)
    {
        this._hitBox = hitBox;
    }
    public Rectangle HitBox => _hitBox;

    public bool CollidesWith(Entity other)
    {
        return _hitBox.IntersectsWith(other.HitBox);
    }
}

public class Solid : Entity {
    public Solid(Rectangle hitBox) : base(hitBox)
    {
    }
}

public class Actor : Entity {
    public Actor(Rectangle hitBox) : base(hitBox)
    {
    }

    private Vector2 _remainder;

    public void MoveX(float x)
    {
        _remainder.X += x;
        var move = MathF.Round(_remainder.X);
        if(move == 0){
            return;
        }
        _remainder.X -= move;
        var sign = MathF.Sign(move);
        while (move != 0)
        {
            _hitBox.X += sign;
            if (Stage.Current.Entities.Any(e => e != this && CollidesWith(e)))
            {
                _hitBox.X -= sign;
                break;
            }
            else
            {
                move -= sign;
            }
        }
    }

    public void MoveY(float y)
    {
        _remainder.Y += y;
        var move = MathF.Round(_remainder.Y);
        if(move == 0){
            return;
        }
        _remainder.Y -= move;
        var sign = MathF.Sign(move);
        while (move != 0)
        {
            _hitBox.Y += sign;
            if (Stage.Current.Entities.Any(e => e != this && CollidesWith(e)))
            {
                _hitBox.Y -= sign;
                break;
            }
            else
            {
                move -= sign;
            }
        }
    }
}


public class PhysicsGame : Game {
    private SpriteBatch _batch = new();
    private Actor _player;
    private Actor _actor;

    public override void Init()
    {
        var stage = Stage.Current = new Stage();
        
        _player = new Actor(new Rectangle(100, 100, 32, 32));
        stage.Actors.Add(_player);
        _actor = new Actor(new Rectangle(200, 200, 32, 32));
        stage.Actors.Add(_actor);
        stage.Solids.Add(new Solid(new Rectangle(300, 300, 200, 200)));
 
       

        base.Init();
    }

    public override void Render()
    {
        _batch.Begin();
        foreach(var item in Stage.Current.Entities){
            var color = item is Actor ? ColorRGBA.Green : ColorRGBA.Blue;
            _batch.DrawRect(item.HitBox, color);
        }
        _batch.End();
        base.Render();
    }

    public override void Update(double time)
    {
        var dir = _player.Position - _actor.Position;
        var len = dir.Length();
        dir = Vector2.Normalize(dir);
        if(len > 48){
            _actor.MoveY(dir.Y);
            _actor.MoveX(dir.X);
        }



        if(Keyboard.IsKeyDown(KeyCode.W)){
            _player.MoveY(-1.3f);
        }
        if(Keyboard.IsKeyDown(KeyCode.S)){
            _player.MoveY(1.3f);
        }
        if(Keyboard.IsKeyDown(KeyCode.A)){
            _player.MoveX(-1.3f);
        }
        if(Keyboard.IsKeyDown(KeyCode.D)){
            _player.MoveX(1.3f);
        }

        base.Update(time);
    }
}
