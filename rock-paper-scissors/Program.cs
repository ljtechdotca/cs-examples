
int score = 0;
string[] answers = {"ROCK", "PAPER", "SCISSORS"};

while(true) {
  Console.ForegroundColor = ConsoleColor.Cyan;
  Console.WriteLine("Pick:");
  
  foreach (string answer in answers) {
    Console.WriteLine(answer);
  }
  
  Console.ForegroundColor = ConsoleColor.Green;
  string? you = Console.ReadLine();
  
  if (answers.Contains(you)) {
    string opponent = answers[Random.Shared.Next(0,3)];
    Console.Write($"YOU: {you}");
    Console.ResetColor();
    Console.Write(" vs ");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write($"OPPONENT: {opponent}");
    Console.WriteLine("");
    Console.ForegroundColor = ConsoleColor.Yellow;

    Game mode = (you, opponent) switch {
      ("ROCK", "SCISSORS") => Game.Win,
      ("ROCK", "PAPER") => Game.Lose,
      ("ROCK", "ROCK") => Game.Draw,
      ("PAPER", "ROCK") => Game.Win,
      ("PAPER", "SCISSORS") => Game.Lose,
      ("PAPER", "PAPER") => Game.Draw,
      ("SCISSORS", "PAPER") => Game.Win,
      ("SCISSORS", "ROCK") => Game.Lose,
      ("SCISSORS", "SCISSORS") => Game.Draw,
      _ => Game.Error
    };

    switch(mode) {
      case Game.Win:
      score += 1;
      Console.WriteLine($"YOU WON! {score}");
      break;

      case Game.Lose:
      score -= 1;
      Console.WriteLine($"YOU LOST! {score}");
      break;

      case Game.Draw:
      Console.WriteLine($"Game draw. {score}");
      break;

      case Game.Error:
      Console.WriteLine("Something bugged out.");
      break;

      default:
      break;
    }
  } else {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Thats not fair! You can't use that!");
  }
  
  Console.ResetColor();
}

enum Game {
    Win,
    Lose,
    Draw,
    Error
};