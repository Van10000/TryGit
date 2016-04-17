using System.ComponentModel;

namespace PudgeClient
{
    [TypeConverter(typeof(RuneConverter))]
    public class Rune: Node
    {
        public bool visited;
        public double prior = 1;

        public Rune(double x, double y): base(x, y)
        {
            visited = false;
        }

        public override string ToString()
        {
            return x + " " + y;
        }
    }
}
