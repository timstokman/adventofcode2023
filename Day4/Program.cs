// See https://aka.ms/new-console-template for more information

using Common;
using Day4;

string puzzleInput = await Util.GetPuzzleInput(4);
string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
var cards = puzzleLines.Select(Card.FromLine).ToList();
int sumScore = cards.Sum(c => c.Score);
Console.WriteLine(sumScore);

int[] ownedScratchCards = Enumerable.Repeat(1, puzzleLines.Length).ToArray();

for (int cardNumber = 0; cardNumber < ownedScratchCards.Length; cardNumber++)
{
    var card = cards[cardNumber];
    var matchingNum = card.MatchingNumbers.Count();
    for (int won = cardNumber + 1; won < Math.Min(cardNumber + 1 + matchingNum, ownedScratchCards.Length); won++)
    {
        ownedScratchCards[won] += ownedScratchCards[cardNumber];
    }
}

Console.WriteLine(ownedScratchCards.Sum());