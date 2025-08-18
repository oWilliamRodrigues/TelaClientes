using System;
using System.Text;
using Europa.Extensions;

namespace Europa.Commons
{
    public static class PasswordUtil
    {
        private const string VALID_CHARS = "abcdefghijklmnopqrstuvwxyz1234567890@#!?";
        public const string VALID_CHARS_NON_ESPECIAL_WITHOUT_I_L = "abcdefghijkmnopqrstuvwxyz1234567890ABCDEFGHJKMNOPQRSTUVWXYZ";
        public const string VALID_CHARS_NON_ESPECIAL = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const int MIN_PASSWORD_LENGTH = 6;
        private const int MAX_PASSWORD_LENGTH = 16;

        public static string CreatePassword(int passwordLength,string caracteresPossiveis="")
        {
            if (caracteresPossiveis == "")
            {
                caracteresPossiveis = VALID_CHARS;
            }

            //Aqui pego o valor máximo de caracteres para gerar a senha
            int maxlength = caracteresPossiveis.Length;

            //Criamos um objeto do tipo randon
            Random random = new Random(DateTime.Now.Millisecond);

            //Criamos a string que montaremos a senha
            StringBuilder senha = new StringBuilder(passwordLength);

            //Fazemos um for adicionando os caracteres a senha
            for (int i = 0; i < passwordLength; i++)
            {
                senha.Append(caracteresPossiveis[random.Next(0, maxlength)]);
            }

            //retorna a senha
            return senha.ToString();            
        }

        public static string CreatePassword()
        {
            return CreatePassword(MIN_PASSWORD_LENGTH);
        }

        public static bool ValidatePassword(string password)
        {
            if (password.IsEmpty())
            {
                return false;
            }

            bool hasLetter = false;
            bool hasNumber = false;

            if (password.Length < MIN_PASSWORD_LENGTH || password.Length > MAX_PASSWORD_LENGTH)
            {
                return false;
            }

            foreach (char letra in password)
            {
                if (char.IsLetter(letra))
                {
                    hasLetter = true;
                }
                else if (char.IsDigit(letra))
                {
                    hasNumber = true;
                }
                else
                {
                    //Do Nothing. Just to compliance SonarQube
                }
            }
            return hasLetter && hasNumber;
        }
    }
}
