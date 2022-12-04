using AOC_2k22_Common;

namespace AOC_2k22_2;

internal class Solver : ChallangeSolver
{
    private const int DrawPoints = 3;
    private const int WinPoints = 6;

    private static Dictionary<string, GameSign> opponentMapToGameSing = new Dictionary<string, GameSign>
    {
        { "A", GameSign.Rock },
        { "B", GameSign.Paper },
        { "C", GameSign.Scissors }
    };

    private static Dictionary<string, GameSign> myMapToGameSign = new Dictionary<string, GameSign>
    {
        { "X", GameSign.Rock },
        { "Y", GameSign.Paper },
        { "Z", GameSign.Scissors }
    };

    private static Dictionary<string, GameResult> letterToGameResult = new Dictionary<string, GameResult>
    {
        { "X", GameResult.Lose },
        { "Y", GameResult.Draw },
        { "Z", GameResult.Win }
    };

    private static Dictionary<GameSign, GameSign> beatingSignsMap = new Dictionary<GameSign, GameSign>
    {
        { GameSign.Rock, GameSign.Scissors },
        { GameSign.Paper, GameSign.Rock },
        { GameSign.Scissors, GameSign.Paper }
    };

    private static Dictionary<GameSign, GameSign> gettingBeatBySignsMap = new Dictionary<GameSign, GameSign>
    {
        { GameSign.Scissors, GameSign.Rock },
        { GameSign.Rock, GameSign.Paper },
        { GameSign.Paper, GameSign.Scissors }
    };


    protected override void SolvePart1(
        string[] input)
    {
        var totalScore = 0;

        foreach (var game in input)
        {
            var pickedSigns = game.Split(' ');

            var opponentPick = pickedSigns[0];
            var myPick = pickedSigns[1];

            var gameState = new GameState(
                opponentMapToGameSing[opponentPick],
                myMapToGameSign[myPick]);

            totalScore += EvaluateGameScore(gameState);
        }

        Console.WriteLine(totalScore);
    }

    protected override void SolvePart2(
        string[] input)
    {
        var totalScore = 0;

        foreach (var game in input)
        {
            var gameInfo = game.Split(' ');

            var opponentPick = gameInfo[0];
            var myGameResult = gameInfo[1];

            var gameState = new GameState(
                opponentMapToGameSing[opponentPick],
                GetCorrectSign(opponentMapToGameSing[opponentPick], letterToGameResult[myGameResult]));

            totalScore += EvaluateGameScore(gameState);
        }

        Console.WriteLine(totalScore);
    }

    private GameSign GetCorrectSign(
        GameSign opponentSign,
        GameResult gameResult)
    {
        if (gameResult == GameResult.Draw)
        {
            return opponentSign;
        }
        else if (gameResult == GameResult.Win)
        {
            return gettingBeatBySignsMap[opponentSign];
        }
        else
        {
            return beatingSignsMap[opponentSign];
        }
    }

    private int EvaluateGameScore(
        GameState gameState)
    {
        if (gameState.OpponentSign == gameState.MySign)
        {
            return (int)gameState.MySign + DrawPoints;
        }
        else if (beatingSignsMap[gameState.MySign] == gameState.OpponentSign)
        {
            return (int)gameState.MySign + WinPoints;
        }
        else
        {
            return (int)gameState.MySign;
        }
    }

    private record GameState(
        GameSign OpponentSign,
        GameSign MySign);

    private enum GameSign
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    private enum GameResult
    {
        Lose = 1,
        Draw = 2,
        Win = 3
    }
}
