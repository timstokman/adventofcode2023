using Common;
using Day4;

int[] OwnedScratchCards(List<Card> cards)
{
    int[] ownedScratchCards = Enumerable.Repeat(1, cards.Count).ToArray();
    for (int cardNumber = 0; cardNumber < ownedScratchCards.Length; cardNumber++)
    {
        Card card = cards[cardNumber];
        int matchingNum = card.MatchingNumbers.Count();
        for (int won = cardNumber + 1; won < Math.Min(cardNumber + 1 + matchingNum, ownedScratchCards.Length); won++)
        {
            ownedScratchCards[won] += ownedScratchCards[cardNumber];
        }
    }

    return ownedScratchCards;
}

string puzzleInput = await Util.GetPuzzleInput(4);
List<Card> cards = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(Card.FromLine).ToList();

int sumScore = cards.Sum(c => c.Score);
int[] ownedScratchCards = OwnedScratchCards(cards);
int numOwned = ownedScratchCards.Sum();

Console.WriteLine($"Sum scores: {sumScore}");
Console.WriteLine($"Owned scratch cards: {numOwned}");