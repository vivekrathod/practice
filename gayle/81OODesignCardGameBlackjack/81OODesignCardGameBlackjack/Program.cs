using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _81OODesignCardGameBlackjack
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    /// <summary>
    /// OO design for the game of Blackjack 
    /// </summary>
    public interface ICard
    {
	
    }

    /// <summary>
    /// immutable
    /// </summary>
    public class Card
    {
	    private Rank _rank;
	    private Suite _suite;

        public Suite Suite
        {
            get { return _suite; }
        }

        public Rank Rank
        {
            get { return _rank; }
        }

        public Card(Suite suite, Rank rank)
	    {
		    _rank = rank;
		    _suite = suite;
	    }
	
	    public override bool Equals(Object o)
	    {
		    // match rank and suite
	        Card c = ((Card) o);
	        return c.Suite == Suite && c.Rank == Rank;
	    }
	
	    public override int GetHashCode()
	    {
		    return Rank.GetHashCode() ^ Suite.GetHashCode();
	    }
	
	    public static bool operator==(Card a, Card b)
	    {
		    return a.Equals(b);
	    }
	
	    public static bool operator !=(Card a, Card b)
	    {
		    return !(a==b);
	    }
	
	    public static bool operator >(Card a, Card b)
	    {
		    if (a.Suite != b.Suite)
			    throw new ArgumentException("Mismatching suites..");

	        return (a.Rank > b.Rank);
	    }
	
	    public static bool operator <(Card a, Card b)
	    {
		    if (a.Suite != b.Suite)
			    throw new ArgumentException("Mismatching suites..");

	        return (a.Rank < b.Rank);
	    }
    }

    public class CardFactory
    {
        public Card CreateCard()
        {
            throw new NotImplementedException();
        }
    }

    public class Deck
    {
	    private Card[] _cards = new Card[52];
	
	    public Deck()
	    {
            Random random = new Random();
		    foreach (int rank in Enum.GetValues(typeof(Rank)))
		    {
			    foreach	(int suite in Enum.GetValues(typeof(Suite)))
			    {
			        Card card = new Card((Suite)suite, (Rank)rank);
			        int randomIndex = random.Next(0, 52);
			        while (_cards[randomIndex] != null)
			        {
			            randomIndex = random.Next(0, 52);
			        }
			        _cards[randomIndex] = card;
			    }
		    }
	    }
	
	    public void Deal()
	    {
		
	    }
    }

    public class BlackJack
    {
        public BlackJack(Dealer dealer, Player[] players)
        {
            
        }
    }

    public class Dealer
    {
        
        public Dealer()
        {
            
        }

        public void 

    }

    public class Player
    {
        private ICollection<Card> _hand = new List<Card>();
        private PlayerState _state;
        private Dealer _dealer;
        public event EventHandler<Card> AcceptCardEvent; 

        public void JoinGame(Dealer dealer)
        {
            _dealer = dealer;
            _state = PlayerState.Active;
        }

        
        public void PlayTurn()
        {
            
        }
    }

    public enum PlayerActions
    {
        Hit,
        Stand
    }

    public enum PlayerState
    {
        Idle,
        Active, // joined and playing the game
        Busted
    }



    public enum Suite
    {
	    Spade,
	    Heart,
	    Diamond,
	    Club
    }

    public enum Rank
    {
	    Ace,
	    Two,
	    Three,
	    //...
	    Jack,
	    Queen,
	    King
    }
}
