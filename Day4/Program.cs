using Common;
using Day4;

string puzzleInput = await Util.GetPuzzleInput(4);
string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
List<Card> cards = puzzleLines.Select(Card.FromLine).ToList();
int sumScore = cards.Sum(c => c.Score);

int[] ownedScratchCards = Enumerable.Repeat(1, puzzleLines.Length).ToArray();
for (int cardNumber = 0; cardNumber < ownedScratchCards.Length; cardNumber++)
{
    Card card = cards[cardNumber];
    int matchingNum = card.MatchingNumbers.Count();
    for (int won = cardNumber + 1; won < Math.Min(cardNumber + 1 + matchingNum, ownedScratchCards.Length); won++)
    {
        ownedScratchCards[won] += ownedScratchCards[cardNumber];
    }
}

Console.WriteLine($"Sum scores: {sumScore}");
Console.WriteLine($"Owned scratch cards: {ownedScratchCards.Sum()}");