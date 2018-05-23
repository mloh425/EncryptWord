using System;
using System.Collections;
namespace lohs_p1
{
    /// <summary>
    /// This is the driver class that will automate the testing
    /// process for the EncryptWord and CaesarShiftGame classes.
    /// The purpose of this class is to test for validation
    /// errors, as well as the overal flow of the Caesar Shift Game.
    /// </summary>
    public class p1
    {
        /// <summary>
        /// An array of string values that contain pseudo-user input
        /// for the guesses of the shift value.
        /// </summary>
        public static string[] guessValues = {"", " ", "abc", "#@$@#$", "-1",
            "0", "26", "1", "25", "2", "24", "3", "23", "4", "22", "5", "21",
            "6", "20", "7", "19", "8", "18", "9", "10", "11", "12", "13",
            "14", "15", "16", "17"};

        /// <summary>
        /// An array of string values that contain pseudo-user input
        /// for the words that a user may choose to encrypt.
        /// </summary>
        public static string[] words = {"!@#$%", "ABCD", "ABC", "abcd12",
                                "major lazer", "", " ", "majorlazer"};

        /// <summary>
        /// String containing the welcome message for the user.
        /// This is to be output to the console at the beginning.
        /// </summary>
        public const string welcomeMessage =
            "Welcome to the Caesar Cypher Shift Game! \n\n" +
            "The Caesar Cypher Shift game is a game where \n" +
            "a word is encrypted by shifting each letter forward \n" +
            "in the alphabet by the same number. The number is hidden \n" +
            "from the player and the objective is to guess what that \n" +
            "number is.\n";

        /// <summary>
        /// String containing game rules to be outputted to console
        /// for the user.
        /// </summary>
        public const string rules =
            "Here are a few guidlines to help get the player started. \n" +
            "A word will be shifted by at least one character down \n" +
            "ie if the letter was originally 'a', it will become 'b'. \n" +
            "Each character of the word will be shifted by no more than \n" +
            "25 characters and no less than 1 character. If the letter \n" +
            "shifted down was past the end of the alphabet, then it \n" +
            "will wrap around the alphabet ie (if the letter 'z' was \n" +
            "shifted by one character, it would become 'a'. Hints will be \n" +
            "given if the guess is too 'high' or 'low' meaning if the \n" +
            "guess was over/under the hidden shift number. \n" +
            "Once the correct guess has been made, statistics of the \n" +
            "of the number of guesses made, guesses that went over/under, \n" +
            "as well as the average guess value will be output. \n" +
            "At the end of the round the user may choose to reset the \n" +
            "game by keeping the same word and resetting all statistics \n" +
            "with a new shift number or by changing the word also. \n";

        /// <summary>
        /// This string contains the conditions that must be met when
        /// providing input during the game.
        /// </summary>
        public const string inputRules =
            "When prompted to enter a word for encryption, the word\n" +
            "must be at least four characters long and contain only\n" +
            "lowercase english letters. It cannot contain spaces,\n" +
            "numeric characters, special characters, or capital letters.";

        /// <summary>
        /// Global instance of the CaesarShiftGame.
        /// </summary>
        public static CaesarShiftGame game;

        /// <summary>
        /// This count is used for iterating through the array
        /// of guessValues.
        /// </summary>
        public static int count = 0;

        /// <summary>
        /// This count is used for iterating through the array
        /// of words to be tested/encrypted.
        /// </summary>
        public static int wordCount = 0;

        public static void Main(string[] args)
        {
            game = new CaesarShiftGame();
            Console.WriteLine(welcomeMessage);
            Console.WriteLine(rules);

            //Testing Default State
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Testing Default State of EncryptWord");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Displaying Encrypted Word:" + game.displayWord());
            Console.WriteLine("Shift:" + game.displayShift()); // delete

            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Testing for Valid Guesses of Shift #");
            Console.WriteLine("-------------------------------------");
            playRound();

            //Testing new words being encrypted
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Testing for Valid Words for Encryption");
            Console.WriteLine("--------------------------------------");

            string word = autoWordValidation();
            Console.WriteLine("Current word: " + word);
            game.encryptWord(word);
            Console.WriteLine("Encrypted word " + game.displayWord());
            //Testing guessing
            playRound();

            //Testing Reset 
            Console.WriteLine("-----------------------");
            Console.WriteLine("Testing Reset Function");
            Console.WriteLine("-----------------------");

            game.resetGame();
            Console.WriteLine();
            Console.WriteLine("Same word with new encryption: " 
                              + game.displayWord());
            Console.WriteLine("Statistics are reset: ");
            game.displayStatistics();

            Console.WriteLine("-------------");
            Console.WriteLine("END OF TESTS");
            Console.WriteLine("-------------");
        }

        //This method simply plays a round of the Caesar Cipher
        /// <summary>
        /// This method plays a round of the Caesar Cipher Shift
        /// game with the currently encrypted word
        /// </summary>
        public static void playRound() {
            count = 0;
            do
            {
                int guess = autoGuessValidation();
                //Validate integer to be from 1 - 25
                if (!game.isGameOver())
                {
                    string result = game.guessShiftNumber(guess);
                    if (!result.Equals(game.RIGHT_GUESS))
                    {
                        count++;
                        if (result.Equals(game.HIGH_GUESS))
                        {
                            Console.WriteLine(result);
                        }
                        else
                        {
                            Console.WriteLine(result);
                        }
                    }
                    else
                    {
                        Console.WriteLine(result);
                    }
                    Console.WriteLine();
                }

            } while (!game.isGameOver());
            game.displayStatistics();
        }

        /// <summary>
        /// This function automates the guess validation process.
        /// It will utilize this driver class's array int[] guessValues
        /// to simulate user input when playing a round of the 
        /// Caesar Cipher Shift game. This function will be called
        /// by the driving program and not by the user.
        /// 
        /// Precondition: Do not call this yourself.
        /// Postcondition: None
        /// </summary>
        /// <returns>A validated guess integer value</returns>
        public static int autoGuessValidation()
        {
            int guess;
            do
            {
                while (!int.TryParse(guessValues[count], out guess))
                {
                    Console.WriteLine("Enter your guess: " 
                                      + guessValues[count]);
                    Console.WriteLine("Error: Please Enter a valid numerical " +
                                      "value!\n");
                    count++;
                }
                Console.WriteLine("Enter your guess: " + guessValues[count]);
                if (guess > game.VALID_GUESS_UPPERBOUND ||
                    guess < game.VALID_GUESS_LOWERBOUND)
                {
                    Console.WriteLine("Error: Your guess can only be in the " +
                                      "range of 1 to 25 inclusive\n");
                    count++;
                }
            } while (guess > game.VALID_GUESS_UPPERBOUND ||
                     guess < game.VALID_GUESS_LOWERBOUND);
            return guess;
        }

        /// <summary>
        /// This method is not used in testing but it is
        /// meant to validate input directly from users through
        /// the keyboard.
        /// </summary>
        /// <returns>A valid guess input</returns>
        public static int guessValidation() 
        {
            int guess;
            do
            {
                while (!int.TryParse(Console.ReadLine(), out guess))
                {
                    Console.WriteLine("Error: Please Enter a valid numerical " +
                                      "value!\n");
                    Console.Write("Enter your guess:");
                }
                if (guess >  game.VALID_GUESS_UPPERBOUND ||
                    guess < game.VALID_GUESS_LOWERBOUND)
                {
                    Console.WriteLine("Error: Your guess can only be in the " +
                                      "range of 1 to 25 inclusive\n");
                }
            } while (guess > game.VALID_GUESS_UPPERBOUND ||
                     guess < game.VALID_GUESS_LOWERBOUND);
            return guess;
        }

        /// <summary>
        /// This function automates the word validation process.
        /// 
        /// It will utilize this driver class's array string[] words
        /// to simulate user input when playing a round of the 
        /// Caesar Cipher Shift game. This function will be called
        /// by the driving program and not by the user.
        /// Words will be 
        /// 
        /// Precondition: Do not call this yourself.
        /// Postcondition: None
        /// </summary>
        /// <returns>A validated guess integer value</returns>
        public static string autoWordValidation()
        {
            string word;
            bool validWordLength;
            bool validCharacters;
            do
            {
                validWordLength = true;
                validCharacters = true;
                word = words[wordCount];
                Console.WriteLine("Please enter a word to encrypt: " + word);
                if (word.Length < game.MINIMUM_WORD_LENGTH)
                {
                    Console.WriteLine("Error: The word must contain at least" +
                                      " 4 characters!\n");
                    validWordLength = false;
                }
                foreach (char c in word)
                {
                    int asciiVal = (int)c;
                    if (asciiVal < game.VALID_ASCII_LETTER_LOWERBOUND
                        || asciiVal > game.VALID_ASCII_LETTER_UPPERBOUND)
                    {
                        validCharacters = false;
                    }
                }
                if (!validCharacters)
                {
                    Console.WriteLine("Error: Please enter only lowercase\n" +
                                      "letters from the english alphabet!\n" +
                                      "No numbers or special characters " +
                                      "allowed!\n");
                }
                wordCount++;
            } while (!validWordLength || !validCharacters);
            return word;
        }

        public static string wordValidation()
        {
            string word;
            bool validWordLength;
            bool validCharacters;
            do
            {
                validWordLength = true;
                validCharacters = true;
                Console.Write("Please enter a word to encrypt: ");
                word = Console.ReadLine();
                Console.WriteLine();
                if (word.Length < game.MINIMUM_WORD_LENGTH) {
                    Console.WriteLine("Error: The word must contain at least" +
                                      " 4 characters!\n");
                    validWordLength = false;
                }
                foreach (char c in word) {
                    int asciiVal = (int)c;
                    if (asciiVal < game.VALID_ASCII_LETTER_LOWERBOUND
                        || asciiVal > game.VALID_ASCII_LETTER_UPPERBOUND) {
                        validCharacters = false;
                    }
                }
                if (!validCharacters) {
                    Console.WriteLine("Error: Please enter only lowercase\n" +
                                      "letters from the english alphabet!\n" +
                                      "No numbers or special characters " +
                                      "allowed!\n");
                }
            } while (!validWordLength || !validCharacters);
            return word;
        }
    }
}
