using System;

namespace lohs_p1
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            EncryptWord encryptor = new EncryptWord();
            encryptor.generateShift();
            encryptor.setWord("cat");
            encryptor.encryptWord();
            Console.WriteLine("Encrypted word is: " + encryptor.word);
            Console.WriteLine("Shift number is: " + encryptor.shift);
        }
    }
}
