using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to Lucky Larry's Casino!");
        Console.WriteLine("Here, every day is a win and every night is a jackpot!");
        Console.WriteLine("------------------------------------------------------");

        int playerCoins = 1000;
        int jackpot = 5000;

        while (playerCoins > 0)
        {
            Console.WriteLine($"Your coins: {playerCoins} 💰");
            Console.WriteLine($"Jackpot: {jackpot} 💵");

            Console.WriteLine("Choose a game to play:");
            Console.WriteLine("1. Slot Machine");
            Console.WriteLine("2. Blackjack");
            Console.WriteLine("3. Quit");

            string gameChoice = Console.ReadLine();

            switch (gameChoice)
            {
                case "1":
                    PlaySlotMachine(ref playerCoins, ref jackpot);
                    break;
                case "2":
                    PlayBlackjack(ref playerCoins);
                    break;
                case "3":
                    Console.WriteLine("Thanks for playing at Lucky Larry's Casino! Come back soon!");
                    return;
                default:
                    Console.WriteLine("Hold on, it seems like you pressed a button that doesn't exist.");
                    break;
            }

            if (playerCoins <= 0)
            {
                Console.WriteLine("Unfortunately, you've lost all your coins.");
                Console.WriteLine("Do you want to continue playing? (yes/no): ");
                string continuePlaying = Console.ReadLine();

                if (continuePlaying.ToLower() == "yes")
                {
                    playerCoins = 1000; // Reset coins to the starting value
                }
                else
                {
                    Console.WriteLine("Thanks for playing at Lucky Larry's Casino! Come back soon!");
                    return;
                }
            }
        }
    }

    static void PlaySlotMachine(ref int playerCoins, ref int jackpot)
    {
        int bet = 5;
        int numberOfReels = 3;
        string[] symbols = { "🍒", "🍋", "🍇", "🍊", "🍉", "🔔", "💎" };
        int winMultiplier = 5;
        Random random = new Random();

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"\nYour coins: {playerCoins} 💰");
            Console.WriteLine($"Jackpot: {jackpot} 💵");
            Console.WriteLine("Welcome to Lucky Larry's Slot Machine!");
            Console.WriteLine("Press Enter to spin the reels (or type 'quit' to go back to the main menu): ");

            string command = Console.ReadLine();

            if (command.ToLower() == "quit")
                break;

            Console.WriteLine("   _____   ");
            Console.WriteLine("  |     |  ");
            Console.WriteLine($"  |  {symbols[0]}  |  ");
            Console.WriteLine($"  |  {symbols[1]}  |  ");
            Console.WriteLine($"  |  {symbols[2]}  |  ");
            Console.WriteLine("  |_____|  ");

            Console.WriteLine("\nThe reels are spinning...");
            Thread.Sleep(1000);

            string[] result = SpinReels(numberOfReels, symbols, random);

            Console.Clear();
            Console.WriteLine($"\nYour coins: {playerCoins} 💰");
            Console.WriteLine($"Jackpot: {jackpot} 💵");
            Console.WriteLine($"The reels stop at: {string.Join(" | ", result)}");
            Thread.Sleep(500);

            int win = CalculateWin(result, winMultiplier);

            if (win > 0)
            {
                Console.WriteLine($"Congratulations! You won {win} coins! 🎰");
                playerCoins += win;
            }
            else
            {
                Console.WriteLine("Unfortunately, no win this time. Try again! 🍀");
                playerCoins -= bet;
            }

            if (playerCoins >= jackpot)
            {
                Console.WriteLine($"Congratulations! You hit the JACKPOT of {jackpot} coins! 💰💰💰");
                playerCoins += jackpot;
                jackpot = 5000;
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }

    static string[] SpinReels(int numberOfReels, string[] symbols, Random random)
    {
        string[] result = new string[numberOfReels];
        for (int i = 0; i < numberOfReels; i++)
        {
            result[i] = symbols[random.Next(symbols.Length)];
        }
        return result;
    }

    static int CalculateWin(string[] result, int winMultiplier)
    {
        if (result.All(symbol => symbol == result[0]))
        {
            return winMultiplier * result.Length;
        }
        return 0;
    }

    static void PlayBlackjack(ref int playerCoins)
    {
        Console.WriteLine("Welcome to Lucky Larry's Blackjack!");
        Console.WriteLine("Bet and try to beat the dealer without going over 21.");

        while (true)
        {
            Console.WriteLine($"\nYour coins: {playerCoins} 💰");
            Console.Write("Place your bet (whole number, or 0 to quit): ");
            int betAmount = GetValidBet(playerCoins);

            if (betAmount == 0)
            {
                break;
            }

            Deck deck = new Deck();
            deck.Shuffle();

            Hand playerHand = new Hand();
            Hand dealerHand = new Hand();

            playerHand.AddCard(deck.DrawCard());
            playerHand.AddCard(deck.DrawCard());
            dealerHand.AddCard(deck.DrawCard());
            dealerHand.AddCard(deck.DrawCard());

            Console.WriteLine($"Your cards: {playerHand}");
            Console.WriteLine($"Total: {playerHand.CalculatePoints()}");
            Console.WriteLine($"Dealer's open card: {dealerHand.OpenHand()}");

            while (true)
            {
                Console.Write("Do you want to stand or hit? (stand/hit): ");
                string action = Console.ReadLine();

                if (action.ToLower() == "hit")
                {
                    playerHand.AddCard(deck.DrawCard());
                    Console.WriteLine($"Your cards: {playerHand}");
                    Console.WriteLine($"Total: {playerHand.CalculatePoints()}");

                    if (playerHand.CalculatePoints() > 21)
                    {
                        Console.WriteLine("Oh no! You busted! 😭 You lose.");
                        playerCoins -= betAmount;
                        break;
                    }
                }
                else if (action.ToLower() == "stand")
                {
                    while (dealerHand.CalculatePoints() < 17)
                    {
                        dealerHand.AddCard(deck.DrawCard());
                    }

                    Console.WriteLine($"Dealer's cards: {dealerHand}");

                    if (playerHand.CalculatePoints() > 21)
                    {
                        Console.WriteLine("Oh no! You busted! 😭 You lose.");
                        playerCoins -= betAmount;
                    }
                    else if (dealerHand.CalculatePoints() > 21 || playerHand.CalculatePoints() > dealerHand.CalculatePoints())
                    {
                        int winAmount = 2 * betAmount;
                        Console.WriteLine($"Congratulations! You won {winAmount} coins! 🎉");
                        playerCoins += winAmount;
                    }
                    else if (playerHand.CalculatePoints() == dealerHand.CalculatePoints())
                    {
                        Console.WriteLine("It's a tie. You get back your bet.");
                    }
                    else
                    {
                        Console.WriteLine("Oh no! The dealer wins. 😢 You lose.");
                        playerCoins -= betAmount;
                    }

                    break;
                }
                else
                {
                    Console.WriteLine("I didn't quite catch that. Please speak clearly or play with fewer decks.");
                }
            }
        }
    }

    static int GetValidBet(int playerCoins)
    {
        while (true)
        {
            try
            {
                int bet = int.Parse(Console.ReadLine());

                if (bet <= 0 || bet > playerCoins)
                {
                    Console.WriteLine("Lucky Larry thinks you can make a better bet than that. Try again!");
                }
                else
                {
                    return bet;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Lucky Larry isn't great at math, but you need to enter a whole number. Try again!");
            }
        }
    }

    class Deck
    {
        private List<Card> cards = new List<Card>();
        private Random random = new Random();

        public Deck()
        {
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                {
                    cards.Add(new Card { Suit = suit, Value = value });
                }
            }
        }

        public void Shuffle()
        {
            cards = cards.OrderBy(c => random.Next()).ToList();
        }

        public Card DrawCard()
        {
            if (cards.Count == 0)
            {
                throw new InvalidOperationException("The deck is empty. Shuffle and draw again.");
            }

            Card card = cards.First();
            cards.RemoveAt(0);
            return card;
        }
    }

    enum CardSuit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    enum CardValue
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    class Card
    {
        public CardSuit Suit { get; set; }
        public CardValue Value { get; set; }

        public override string ToString()
        {
            return $"{Value} of {Suit}";
        }
    }

    class Hand
    {
        private List<Card> cards = new List<Card>();

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public int CalculatePoints()
        {
            int points = 0;
            int aces = 0;

            foreach (var card in cards)
            {
                if (card.Value == CardValue.Ace)
                {
                    aces++;
                }
                else
                {
                    points += Math.Min(10, (int)card.Value);
                }
            }

            for (int i = 0; i < aces; i++)
            {
                if (points + 11 > 21)
                {
                    points++;
                }
                else
                {
                    points += 11;
                }
            }

            return points;
        }

        public override string ToString()
        {
            return string.Join(", ", cards);
        }

        public string OpenHand()
        {
            return $"{cards.First()} and a face-down card";
        }
    }
}
