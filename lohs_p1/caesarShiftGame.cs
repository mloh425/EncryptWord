using System;
namespace lohs_p1
{
    /// <summary>
    /// CaesarShiftGame
    /// Author: Sau Chung Loh
    /// Filename: EncryptWord.h
    /// Date: April 14th, 2017
    /// Version: 1.0
    /// Description: The CaesarShiftGame class handles the game logic
    /// of guessing the number of characters a word's letters has been
    /// shifted forward by. The class will keep track of statistics of
    /// each round played and can be outputted to console at the end of
    /// a round.
    /// 
    /// Dependencies: This class depends on the EncryptWord object's
    /// state and functionality in order to facilitate a round of the
    /// guessing game. 
    /// 
    /// Assumptions:
    /// -A round begins when a new word is selected for encryption and ends
    /// only when the shift number has been identified by the user. 
    /// 
    /// -A round can be reset in two ways:
    ///     -1) By resetting the statistics and beginning a new round
    ///      with a new word encrypted with a new shift number
    ///     -2) By resetting the statistics and beginning a new round
    ///      with the same word that is encrypted by a new shift number.
    /// 
    /// -The state of the EncryptWord object should be checked before
    /// calling methods within the EncryptWord class.
    /// 
    /// Validation:
    /// -Only integers from 1 to 25 should be passed in to the 
    /// guessShiftNumber(int guess) method.
    /// 
    /// -Only strings containing the following rules can be encrypted:
    ///     -Lower cased english letters
    ///     -Minimum of 4 characters
    ///     -No numbers/special characters/spaces
    ///     
    /// </summary>
    public class CaesarShiftGame
    {
        /// <summary>
        /// A counter that keeps track of the number
        /// of guesses the user has made. This will be
        /// output as a statistic to the user at the
        /// end of the round.
        /// </summary>
        private int guessCount;

        /// <summary>
        /// A counter that keeps track of the number
        /// of guesses the user has made that were
        /// higher than the encapsulated shift value.
        /// This will be output as a statistic to the 
        /// user at the end of the round.
        /// </summary>
        private int highGuessCount;

        /// <summary>
        /// A counter that keeps track of the number
        /// of guesses the user has made that were
        /// lower than the encapsulated shift value.
        /// This will be output as a statistic to the 
        /// user at the end of the round.
        /// </summary>
        private int lowGuessCount;

        /// <summary>
        /// This is a sum of all the guesses made by the
        /// user. This variable will be used to calculate
        /// the approximate average guess value the user
        /// made. 
        /// </summary>
        private int guessValueSum;

        /// <summary>
        /// This boolean is used to identify the state
        /// of the game.
        /// </summary>
        private bool gameStatus;

        /// <summary>
        /// An instance of the EncryptWord object
        /// that will be used for encapsulating a shift
        /// number that a word's character will be shifted by.
        /// </summary>
        private EncryptWord encryptor;

        /// <summary>
        /// The default counter value of 0 initially.
        /// </summary>
        private const int DEFAULT_COUNTER_VALUE = 0;

        /// <summary>
        /// The minimum valid number of characters in a word.
        /// </summary>
        public readonly int MINIMUM_WORD_LENGTH = 4;

        /// <summary>
        /// The valid guess lowerbound is 1 and cannot be lower as
        /// that would indicate no shift if the shift number is 0, and
        /// we only shift characters forward.
        /// </summary>
        public readonly int VALID_GUESS_LOWERBOUND;

        /// <summary>
        /// The valid guess upperbound is 25 and cannot be higher
        /// as characters will only start wrapping around the alphabet
        /// when shifted forward.
        /// </summary>
        public readonly int VALID_GUESS_UPPERBOUND;

        /// <summary>
        /// The valid ASCII letter lowerbound value is 96
        /// even though the letter 'a' is at ASCII value 
        /// 97. This is due to a wrap around calculation
        /// when characters are shifted past 122, and need
        /// to wrap around to the beginning of the alphabet
        /// properly.
        /// </summary>
        public readonly int VALID_ASCII_LETTER_LOWERBOUND;

        /// <summary>
        /// The valid ASCII letter upperbound is 122
        /// as letters shifting past the letter 'z' should
        /// wrap around to 'a'.
        /// </summary>
        public readonly int VALID_ASCII_LETTER_UPPERBOUND;

        /// <summary>
        /// This variable is used to help notify
        /// the user of whether his or her guess
        /// is too high when trying to identify
        /// the shift number.
        /// </summary>
        public readonly string HIGH_GUESS =
            "Your guess is too high!";

        /// <summary>
        /// This variable is used to help notify
        /// the user of whether his or her guess
        /// is too low when trying to identify
        /// the shift number.
        /// </summary>
        public readonly string LOW_GUESS =
            "Your guess is too low!";

        /// <summary>
        /// This variable is used to help notify
        /// the user of whether his or her guess
        /// is just right when trying to identify
        /// the shift number.
        /// </summary>
        public readonly string RIGHT_GUESS =
            "Your guess is right!";

        /// <summary>
        /// An 'ACTIVE' state means that a round
        /// is in play, and the user has yet to identify the shift number.
        /// This also means that the class is keeping track of statistics
        /// of each user guess.
        /// </summary>
        private const bool ACTIVE = true;

        /// <summary>
        /// The 'INACTIVE' state means that a round is over.
        /// This means that the shift number has been identified
        /// by the user and the game will no longer continue to
        /// keep track of statistics.
        /// </summary>
        private const bool INACTIVE = false;

        /// <summary>
        /// Initializes a new instance of the
        ///  <see cref="T:lohs_p1.CaesarShiftGame"/> class.
        /// Where all statistics are set to the default value of 0
        /// and there will be an EncryptWord object that contains a
        /// default word. The default word will be encrypted with
        /// a randomly generated shift number that will be encapsulated
        /// by the EncryptWord object.
        /// 
        /// Precondition: None 
        /// Postcondition: The game will be an an 'ACTIVE' state where
        /// the user can play the game with the default word or they can
        /// choose to enter a new word to play with.
        /// </summary>
        public CaesarShiftGame()
        {
            this.encryptor = new EncryptWord();
            this.guessCount = DEFAULT_COUNTER_VALUE;
            this.highGuessCount = DEFAULT_COUNTER_VALUE;
            this.lowGuessCount = DEFAULT_COUNTER_VALUE;
            this.guessValueSum = DEFAULT_COUNTER_VALUE;
            this.gameStatus = ACTIVE;
            this.VALID_GUESS_LOWERBOUND = encryptor.VALID_GUESS_LOWERBOUND;
            this.VALID_GUESS_UPPERBOUND = encryptor.VALID_GUESS_UPPERBOUND;
            this.VALID_ASCII_LETTER_LOWERBOUND 
                = encryptor.VALID_ASCII_LETTER_LOWERBOUND + 1;
            this.VALID_ASCII_LETTER_UPPERBOUND
                = encryptor.VALID_ASCII_LETTER_UPPERBOUND;
        }

        /// <summary>
        /// Description: Encrypts the word chosen by the user.
        /// The word must contain a minimum of four characters,
        /// only english alphabet letters that are lowercase, and
        /// no special characters, spaces, or numbers. 
        /// 
        /// Precondition: None.
        /// Postcondition: The game status will be 'ACTIVE' as a 
        /// new word with a newly encapsulated shift number will be 
        /// generated.
        /// </summary>
        /// <param name="word">word - a string to be encrypted.</param>
        public void encryptWord(string word)
        {
            resetGame();
            encryptor.encryptNewWord(word);
        }

        /// <summary>
        /// Description: This method allows the
        /// user to attempt to guess what the encapsulated
        /// shift number is in the EncryptWord object. 
        ///
        /// Precondition: The CaesarShiftGame must be in an
        /// active state.
        /// Postcondition: The CaesarShiftGame may or may not be in an
        /// 'ACTIVE' or 'INACTIVE' state depending on if the user
        /// properly identifies the shift number encapsulated in the
        /// EncryptWord object.
        /// </summary>
        /// <returns>The shift number.</returns>
        /// <param name="guess">Guess.</param>
        public string guessShiftNumber(int guess)
        {
            //VALIDATE GUESS

            //if number is not of type int
            //Error: Must be a number
            //if number is greater than 25 or less than 1
            //Error: Out of range 1-25

            //Check state before use
            guessCount++;
            guessValueSum += guess;
            string result = encryptor.verifyShiftQuery(guess);
            if (result.Equals(encryptor.EQUAL_TO_SHIFT))
            {
                gameStatus = INACTIVE;
                return RIGHT_GUESS;
            }
            else if (result.Equals(encryptor.LOWER_THAN_SHIFT))
            {
                lowGuessCount++;
                return LOW_GUESS;
            }
            else
            {
                highGuessCount++;
                return HIGH_GUESS;
            }
        }

        /// <summary>
        /// Description: Resets the round of the CaesarShiftGame
        /// by resetting all statistics and setting
        /// the game to an 'ACTIVE' state.
        /// 
        /// Preconditon: None
        /// Postconditon: The CaesarShiftGame object
        /// will be in an active state.
        /// </summary>
        public void resetGame()
        {
            guessCount = DEFAULT_COUNTER_VALUE;
            highGuessCount = DEFAULT_COUNTER_VALUE;
            lowGuessCount = DEFAULT_COUNTER_VALUE;
            gameStatus = ACTIVE;
            encryptor.reset();
        }

        /// <summary>
        /// Description: This method retrieves the word
        /// in its Encrypted or Decrypted form from the 
        /// EncryptWord object and returns it.
        /// 
        /// Precondition: None
        /// Postcondition: Does not change the state.
        /// </summary>
        /// <returns>The word in its encrypted or decrypted
        /// form.</returns>
        public string displayWord() 
        {
            string word = encryptor.getWord();
            return word;
        }

        /// <summary>
        /// TEST FUNCTION
        /// </summary>
        /// <returns>The shift.</returns>
        public int displayShift()
        {
            return encryptor.getShift();
        }

        /// <summary>
        /// Displaies the statistics of the round.
        /// 
        /// Precondition: The caesarShiftGame should be in an
        /// 'INACTIVE' state.
        /// Postcondition: Does not change the state.
        /// </summary>
        public void displayStatistics() 
        {
            Console.WriteLine(
                "Number of guesses made: " + guessCount
                + "\nNumber of high guesses: " + highGuessCount
                + "\nNumber of low guesses: " + lowGuessCount
                + "\nAverage guess value: " + calculateAverageGuessValue()
                + "\n"
            );
        }

        /// <summary>
        /// This method calculates the average guess value.
        /// 
        /// Precondition: The caesarShiftGame should be in an
        /// 'INACTIVE' state.
        /// Postcondition: Does not change the state.
        /// </summary>
        /// <returns>The average guess count.</returns>
        private double calculateAverageGuessValue()
        {
            if (guessCount != 0)
            {
                double averageGuessValue = guessValueSum / guessCount;
                return averageGuessValue;
            } else {
                return 0;
            }

        }

        /// <summary>
        /// Returns the state of the game.
        /// </summary>
        /// <returns><c>true</c> Returns true if the
        /// game is in an active state, false otherwise
        /// <c>false</c> otherwise.</returns>
        public bool isGameOver() {
            if (gameStatus == ACTIVE) {
                return false;
            } else {
                return true;
            }
        }
    }
}
