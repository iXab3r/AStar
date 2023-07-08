using AStar.Heuristics;

namespace AStar.Options;

public class PathFinderOptions
{
    public PathFinderOptions()
    {
        HeuristicFormula = HeuristicFormula.Manhattan;
        UseDiagonals = true;
        SearchLimit = 2000;
    }

    public HeuristicFormula HeuristicFormula { get; set; }

    public bool UseDiagonals { get; set; }

    public bool PunishChangeDirection { get; set; }

    public int SearchLimit { get; set; }
}