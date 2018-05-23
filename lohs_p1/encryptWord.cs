using System;
namespace lohs_p1
{
    /// <summary>
    /// EncryptWord
    /// Author: Sau Chung Loh
    /// Filename: EncryptWord.h
    /// Date: April 14th, 2017
    /// Version: 1.0
    /// Description: The EncryptWord class handles the logic of Encrypting a 
    /// string by shifting each character by a randomly generated integer value
    /// down the alphabet.
    ///
    /// Initially once the EncryptWord object is initialized, a randomly
    /// generated shift number is created and it is also given a default
    /// word. The encapsulated word will have each of its letters shifted
    /// by that randomly generated shift number and the object is in an
    /// 'on' state and is useable. The user may choose to enter a different
    /// word for encryption, in which a once a valid word is chosen, it will
    /// be encrypted by a newly generated shift number.
    /// 
    /// For example: If the randomly generated shift number was 3, and the
    /// string was 'abcz' the result would be 'defc'.
    /// 
    /// When the EncryptWord object is 'on' meaning it it is in its 'ENCRYPTED'
    /// state, the user can choose to 'decrypt' (using the verifyShiftQuery()
    /// method) the word by passing in a valid shift number query. The query
    /// should be an integer. If the query matches the encapsulated shift
    /// number, then the EncryptWord object's state will be in a 'DECRYPTED'
    /// state which is its 'off' state. From here the user can either encrypt
    /// a new word, or 'reset' the state of the class by keeping the same word
    /// but using a different, randomly generated shift number to re-encrypt
    /// the word.
    ///
    ///
    /// Assumption:
    /// -This class should only be used for encrypting words made up of
    /// lowercased letters from the English alphabet. They should not
    /// contain spaces, numbers, or other special characters.
    /// 
    /// -Words should contain at a minimum of four characters. 
    ///
    /// -The shift number will be an integer from 1 to 25 inclusive. This
    /// ensures the maximum amount of characters each letter can shift down
    /// the alphabet without shifting back to its original form. 
    /// 
    /// -Should the characters be shifted past the last letter of the alphabet,
    /// the shift will wrap around to the beginning of the alphabet.
    /// 
    /// For example, with a shift of 3, the letter 'z' will become 'c'.
    ///
    /// Interface Invariants
    /// -The state of this object is to be handled by the object internally
    /// and not toggled explicitly by the user or driving program.
    ///
    ///
    /// Classes and Libraries used:
    /// Random class.
    /// 
    /// </summary>
    public class EncryptWord
    {
        /*** Class Constants ***/


        /// <summary>
        /// The valid ASCII letter lowerbound value is 96
        /// even though the letter 'a' is at ASCII value 
        /// 97. This is due to a wrap around calculation
        /// when characters are shifted past 122, and need
        /// to wrap around to the beginning of the alphabet
        /// properly.
        /// </summary>
        public readonly int VALID_ASCII_LETTER_LOWERBOUND = 96;

        /// <summary>
        /// The valid ASCII letter upperbound is 122
        /// as letters shifting past the letter 'z' should
        /// wrap around to 'a'.
        /// </summary>
        public readonly int VALID_ASCII_LETTER_UPPERBOUND = 122;

        /// <summary>
        /// The valid guess lowerbound is 1 and cannot be lower as
        /// that would indicate no shift if the shift number is 0, and
        /// we only shift characters forward.
        /// </summary>
        public readonly int VALID_GUESS_LOWERBOUND = 1;

        /// <summary>
        /// The valid guess upperbound is 25 and cannot be higher
        /// as characters will only start wrapping around the alphabet
        /// when shifted forward.
        /// </summary>
        public readonly int VALID_GUESS_UPPERBOUND = 25;

        /// <summary>
        /// The default word when an instance of
        /// EncryptWord is created.
        /// </summary>
        private const string DEFAULT_WORD = "catt";

        /// <summary>
        /// A boolean that is used to define the state
        /// of the EncryptWord object as 'off' if the
        /// shift number has been identified.
        /// </summary>
        private const bool DECRYPTED_STATE = false;

        /// <summary>
        /// A boolean that is used to define the state
        /// of the EncryptWord object as 'on' if the
        /// shift number has been newly generated
        /// or not identified.
        /// </summary>
        private const bool ENCRYPTED_STATE = true;

        /// <summary>
        /// A constant string to identify whether the
        /// shift value guessed by the user is higher
        /// than the encapsulated shift number.
        /// </summary>
        public const string GREATER_THAN_SHIFT = "HIGH";

        /// <summary>
        /// A constant string to identify whether the
        /// shift value guessed by the user is lower
        /// than the encapsulated shift number.
        /// </summary>
        public readonly string LOWER_THAN_SHIFT = "LOW";

        /// <summary>
        /// A constant string to identify whether the
        /// shift value guessed by the user is equal
        /// to the encapsulated shift number.
        /// </summary>
        public readonly string EQUAL_TO_SHIFT = "EQUAL";


        /*** Class Variables ***/


        /// <summary>
        /// string word - contains the word to be encrypted in this class
        /// </summary>
        private string word;

        /// <summary>
        /// int shift - stores the shift number that will be used for
        ///             encryption process. Each letter of the word will have
        ///             an ASCII value that will be shifted by this number.
        ///             This number will be randomly generated for each word.
        /// </summary>
        private int shift;

        /// <summary>
        /// bool isDecrypted - contains....
        /// </summary>
        private bool decryptionState;

        /// <summary>
        /// Random generator - will store a Random object that will be used
        ///                    to generate a random integer for the shift 
        /// </summary>
        Random generator;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:lohs_p1.EncryptWord"/> class.
        /// 
        /// Description: This instance will contain a default word 'catt' and
        /// will encapsulate a randomly generate a shift number that each 
        /// character of the default word will shift by.
        /// 
        /// Precondition: None
        /// Postcondition: The initialized instance of the EncryptWord
        /// object will be in an 'ENCRYPTED' state by default. 
        /// </summary>
        public EncryptWord()
        {
            this.generator = new Random();
            this.word = DEFAULT_WORD;
            generateShift();
            encryptWord();
        }

        /// <summary>
        /// Description: This method simply checks the state of
        /// the EncryptWord object. It will notify the user of whether
        /// it is in its 'ENCRYPTED' or 'DECRYPTED' state. Certain 
        /// functionality will be available to the user ONLY if the
        /// EncryptWord object is in its 'ENCRYPTED' state. 
        /// 
        /// Precondition: None
        /// Postcondition: The EncryptWord object may or may not be on
        /// but the state is unchanged by calling this method.
        /// </summary>
        /// <returns><c>true</c>, if the shift number has not been
        /// identified, meaning the user has not decrypted the number
        /// by which each letter of the encapsulated word has been shifted
        /// by, meaning the EncryptWord is int its 'ENCRYPTED' state.
        /// 
        /// <c>false</c> - if the shift number has been identified meaning
        /// the EncryptWord object is in its 'DECRYPTED' state.</returns>
        public bool isOn()
        {
            return decryptionState;
        }

        /// <summary>
        /// Description: This method allows the user to encrypt a new
        /// word. The word chosen for encryption must be at least 4 characters
        /// long and contain ONLY lowercase English letters from the
        /// alphabet. The chosen word will be encrypted by a newly, randomly
        /// generated shift number.
        /// 
        /// Precondition: None
        /// Postcondition: The EncryptWord object will be in its 'ENCRYPTED'
        /// state with a new word and randomly generated shift number.
        /// </summary>
        /// <param name="word">
        /// A string containing the word to be encrypted.
        /// </param>
        public void encryptNewWord(string word)
        {
            //VALIDATE WORD
            this.word = word;
            reset();
        }

        /// <summary>
        /// Description: This method generates an integer that represents
        /// the number of letters the word will shift down the alphabet by.
        /// The generated shift number will be from 1 - 25 as it cannot
        /// be 0 or 26 because that would shift the character back
        /// to its original letter, and any value greater than 25 will 
        /// simply wrap around the beginning of the alphabet.
        /// 
        /// Precondition: None
        /// Postcondition: None
        /// </summary>
        private void generateShift()
        {
            shift = generator.Next(VALID_GUESS_LOWERBOUND,
                                   VALID_GUESS_UPPERBOUND);
        }

        /// <summary>
        /// Description: This method encrypts the word using a randomly
        /// generated shift number, and the ASCII property that each
        /// character has. The method will iterate through each character
        /// of the string and convert each of them into an ASCII value
        /// which will be in the form of an integer. The randomly generated
        /// shift number will be added to each character in the process and
        /// then converted back to the new ASCII value's letter form. It
        /// uses a stringbuilder methodology to construct the shifted string.
        /// 
        /// Precondition: None, EncryptWord can be in an 'ENCRYPTED' or
        /// 'DECRYPTED' state as there will always be a word and shift number
        /// to be used for the encryption process.
        /// Postcondition: The EncryptWord object will be in an 'ENCRYPTED'
        /// state meaning 
        /// </summary>
        public void encryptWord()
        {
            string encryptedWord = "";
            char currentChar;
            int initialAscii;
            int encryptedAscii;

            for (int i = 0; i < word.Length; i++)
            {
                currentChar = word[i];
                initialAscii = (int)currentChar;
                encryptedAscii = initialAscii + shift;
                if (encryptedAscii > VALID_ASCII_LETTER_UPPERBOUND)
                {
                    encryptedAscii = VALID_ASCII_LETTER_LOWERBOUND
                    + (encryptedAscii - VALID_ASCII_LETTER_UPPERBOUND);
                }
                currentChar = (char)encryptedAscii;
                encryptedWord += currentChar;
            }
            word = encryptedWord;
            decryptionState = ENCRYPTED_STATE;
        }

        /// <summary>
        /// Description: This method resets the instance of the EncryptWord
        /// object by setting the shift number to a newly generated one
        /// before re-encrypting the current word with the new shift number.
        /// 
        /// Precondition: None, the instance can be reset at any time.
        /// Postcondition: The instance of the EncryptWord object will be
        /// in an 'ENCRYPTED' state. 
        /// </summary>
        public void reset()
        {
            generateShift();
            encryptWord();
            decryptionState = ENCRYPTED_STATE;
        }

        /// <summary>
        /// Description: This method resets the instance of the EncryptWord
        /// object by setting the shift number to a newly generated one
        /// before encrypting the DEFAULT_WORD with it.
        /// 
        /// Precondition: None, the instance can be reset at any time.
        /// Postcondition: The instance of the EncryptWord object will be
        /// in an 'ENCRYPTED' state. 
        /// </summary>
        public void resetDefault()
        {
            word = DEFAULT_WORD;
            generateShift();
            encryptWord();
        }

        /// <summary>
        /// Description: This method provides the user the ability to
        /// verify / decipher what the shift number is by allowing the
        /// user to pass in a query in the form of an integer. This method
        /// simply compares the query to the encapsulated shift number.
        /// Once compared, feedback will be given to the user of whether
        /// the query was higher, lower, or equal to the shift number.
        /// It is only when the user queries the right number that the 
        /// object changes to its 'DECRYPTED' state. 
        /// 
        /// Precondition: EncryptWord must be in its 'ENCRYPTED' state in
        /// order to allow the user query what the encapsulated shift is.
        /// 
        /// Postcondition: EncryptWord object MAY be in its 'DECRYPTED' state
        /// if the query is equal to the encapsulated shift number. Otherwise
        /// it remains 'ENCRYPTED'
        /// 
        /// </summary>
        /// <returns>A string containing information regarding
        /// whether the query was higher, lower, or equal to the
        /// encapsulated shift number.</returns>
        /// <param name="shift">int shift - An integer containing
        /// the user's query of what the encapsulated shift number
        /// may be.</param>
        public string verifyShiftQuery(int shift)
        {
            if (this.shift == shift)
            {
                decryptionState = DECRYPTED_STATE;
                return EQUAL_TO_SHIFT;
            }
            else if (this.shift < shift)
            {
                return GREATER_THAN_SHIFT;
            } else { //this.shift > shift
                return LOWER_THAN_SHIFT;
            }
        }

        /* METHODS FOR TEST PURPOSES */
        public int getShift() {
            return shift;
        }

        public string getWord() {
            return word;
        }
    }
}
