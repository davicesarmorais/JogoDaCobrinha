using System.Collections.Generic;

namespace JogoDaCobrinha
{
    public enum SnakeDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    public class Snake
    {
        private int size = 0;
        private readonly List<Pos> body = new List<Pos>();
        private bool alive = true;
        public SnakeDirection Direction { get; set; }

        public List<Pos> Body
        { get { return body; } }

        public Pos Head
        { get { return body[0]; } }

        public bool Alive
        { get { return alive; } }

        public int Size
        {
            get { return size; }
            set { if (value >= 0) size = value; }
        }

        public void Move()
        {
            Pos before = new Pos(Head.X, Head.Y);
            if (Direction == SnakeDirection.Left) body[0].X--;
            if (Direction == SnakeDirection.Right) body[0].X++;
            if (Direction == SnakeDirection.Up) body[0].Y--;
            if (Direction == SnakeDirection.Down) body[0].Y++;
            for (int i = 1; i < Size; i++)
            {
                Pos temp = new Pos(body[i].X, body[i].Y);
                body[i].X = before.X; body[i].Y = before.Y;
                before.X = temp.X; before.Y = temp.Y;
            }
        }

        public void IncreaseSize()
        {
            body.Add(new Pos(0, 0));
            size++;
        }

        public bool Collision()
        {
            if (size <= 1)
            {
                return false;
            }
            for (int i = 1; i < size; i++)
            {
                if (Head == body[i])
                {
                    return true;
                }
            }
            return false;
        }

        public void Generate()
        {
            Reset();
            IncreaseSize();
            IncreaseSize();
        }
        public void Kill() { alive = false; }
        public void Reset()
        {
            body.Clear();
            size = 0;
            alive = true;
            Direction = SnakeDirection.Right;
        }
    }
}