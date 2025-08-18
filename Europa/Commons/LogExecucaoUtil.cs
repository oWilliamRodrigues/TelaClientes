using System;
using System.Text;
using Europa.Resources;

namespace Europa.Commons
{
    public class LogExecucaoUtil
    {
        private  const string DateHourFormat = "dd/MM/yyyy HH:mm:ss";

        public static string LogInicio(string job)
        {
            var mensagemLog = LogData().Append(string.Format(GlobalMessages.LogIniciandoExecucao,job));
            return mensagemLog.ToString();
        }

        public static string LogMensagensEnvio(int qntd)
        {
            return LogData().Append(string.Format(GlobalMessages.LogMensagensEnvio, qntd)).ToString();
        }

        public static string LogMensagensReEnvio(int qntd)
        {
            return LogData().Append(string.Format(GlobalMessages.LogMensagensReEnvio, qntd)).ToString();
        }

        public static string LogMensagensNovasErroEnvio(int qntd)
        {
            return LogData().Append(string.Format(GlobalMessages.LogMensagensNovasErroEnvio, qntd)).ToString();
        }

        public static string LogMensagensErroReEnvio(int qntd)
        {
            return LogData().Append(string.Format(GlobalMessages.LogMensagensErroReEnvio, qntd)).ToString();
        }

        public static string LogMensagensEnviadasSucesso(int qntd)
        {
            return LogData().Append(string.Format(GlobalMessages.LogMensagensEnviadasSucesso, qntd)).ToString();
        }

        public static string LogMensagensReEnviadasSucesso(int qntd)
        {
            return LogData().Append(string.Format(GlobalMessages.LogMensagensReEnviadasSucesso, qntd)).ToString();
        }

        public static string LogMensagemSucesso(string destinatario)
        {
            return LogData().Append(string.Format(GlobalMessages.LogMensagemSucesso, destinatario)).ToString();
        }

        public static string LogMensagemErro(string destinatario, string erroCode)
        {
            return LogData().Append(string.Format(GlobalMessages.LogMensagemErro, destinatario, erroCode)).ToString();
        }

        public static string LogTokenErro(string velho, string novo)
        {
            return LogData().Append(string.Format(GlobalMessages.LogTokenErro, velho, novo)).ToString();
        }

        public static string LogTokenSucesso(string velho, string novo, int qntd)
        {
            return LogData().Append(string.Format(GlobalMessages.LogTokenSucesso, velho, novo, qntd)).ToString();
        }

        public static string LogFim(string job)
        {
            return LogData().Append(string.Format(GlobalMessages.LogFimExecucao, job)).ToString();
        }

        public static StringBuilder LogData()
        {
            return new StringBuilder(DateTime.Now.ToString(DateHourFormat)).Append(" - ");
        }
    }
}
