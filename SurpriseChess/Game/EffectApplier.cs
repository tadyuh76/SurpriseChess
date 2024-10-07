using SurpriseChess;

public class EffectApplier
{
    private readonly Board board;
    private readonly Random random = new();

    private const float MorphChance = 0.25f;
    private const float InvisibilityChance = 0.1f;
    private const float ParalysisChance = 0.5f;
    private const float ShieldChance = 0.5f;

    public EffectApplier(Board board)
    {
        this.board = board;
    }
}