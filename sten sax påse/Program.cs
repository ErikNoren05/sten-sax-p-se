using System;
using System.Reflection.Metadata.Ecma335;

class Game
{
    static void Main(string[] args)
    {
        while (true) 
        { 
            //skapar spelare
            Entity player = new Entity(1);

            //skapar ai
            Entity opponent = new Entity(2);

            choiceComponent playerChoiceComponent = new choiceComponent();
            choiceComponent opponentChoiceComponent = new choiceComponent();

            player.addComponent(playerChoiceComponent);
            opponent.addComponent(opponentChoiceComponent);
            
            
            gameSystem game = new gameSystem();
            WinnerSystem winner = new WinnerSystem();

            //startar spelet
            game.Input(player, "");

            //slumpa motståndarens val, buggar mycket om det är system
            choiceComponent opponentChoice = opponent.getComponent<choiceComponent>();
            Random random = new Random();
            int index = random.Next(opponentChoice.choices.Length);

            //Får motståndarens val
            opponentChoice.opponentChoice = opponentChoice.choices[index];

            //får spelarens val
            choiceComponent playerChoice = player.getComponent<choiceComponent>();

            //simpel kod för att skriva ut val
            Console.WriteLine("spelarens val: " + playerChoice.playerChoice);
            Console.WriteLine("motståndarens val: " + opponentChoice.opponentChoice);

            //hämtar vinnaren och skriver ut vinnaren 
            string vinnaren = winner.getWinner(playerChoice, opponentChoice);
            Console.WriteLine(vinnaren);

            //simpel loop för att 
            Console.WriteLine("\nVill du spela en runda till? ja/nej");
            string playAgain = Console.ReadLine().ToLower();
            Console.Write("\n");
            if (playAgain != "ja") 
            {
                break;
            }

        }
    }
}

public class choiceComponent
{
    
    public string playerChoice { get; set; }
    
    //osäker på om denna behövs
    public string opponentChoice { get; set; }

    //valen som slumpas för motståndaren
    public string[] choices = { "sten", "sax", "påse" };
}


public class Entity
{
    public int id { get; }
    private Dictionary <Type, object> components = new Dictionary <Type, object> ();

    public Entity(int id)
    {
        this.id = id;
    }

    public void addComponent<T>(T component)
    {
        components[typeof(T)] = component;
    }

    public T getComponent<T>()where T : class
    {
        if(components.TryGetValue(typeof(T), out var component))
        {
            return component as T;
        }
        return null;
    }

}

public class gameSystem
{
    public void Input(Entity entity, string input)
    {
        var choiceComponent = entity.getComponent<choiceComponent>();
        if(choiceComponent != null )
        {
            //frågar användaren och tar input från användaren
            Console.WriteLine("välj sten sax eller påse");
            input = Console.ReadLine().ToLower();

            //kollar så input är godkänt
            if(input != "sten" && input != "sax" && input != "påse") 
            {
                //om det inte är godkänt
                Console.WriteLine("inte ett godkänt alternativ");
                choiceComponent.playerChoice = "diskad";
            }
            else
            {
                //sparar input till playerchoice i choicecomponent
                choiceComponent.playerChoice = input;


            }
            
        }
    }
}

public class WinnerSystem
{
    //hämtar playerchoihce och opponentChoice från choicecomponent
    public string getWinner(choiceComponent playerChoice, choiceComponent opponentChoice)
    {
        //simpel if för att kolla vem som vann
        if (playerChoice.playerChoice == opponentChoice.opponentChoice)
        {
            return "oavgjort, det var inget kul";
        }
        else if ((playerChoice.playerChoice == "sten" && opponentChoice.opponentChoice == "sax") ||
                 (playerChoice.playerChoice == "sax" && opponentChoice.opponentChoice == "påse") ||
                 (playerChoice.playerChoice == "påse" && opponentChoice.opponentChoice == "sten"))
        {
            return "Vilken tur du har, du vann!";
        }
        else
        {
            return "japp, du förlorade";
        }
    }
}