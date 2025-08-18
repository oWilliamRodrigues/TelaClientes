namespace Europa.Data.FluentConfigurationHelpers{
    public enum ConnectionStringConfigurationTag{
        /// <summary>
        /// A definição da connection String deve estar contida no App/Web.Config, na tag <connectionStrings/>
        /// <example> <add connectionString="Server=localhost;Database=exemplo;Uid=root;Pwd=exemplo123;Port=3306" name="CS_EXEMPLO" />
        /// </example> 
        /// </summary>
        ConnectionString = 0,

        /// <summary>
        /// A definição da connection String deve estar contida no App/Web.Config, na tag <appSettings/>
        /// <example>
        /// <add value="Server=localhost;Database=exemplo;Uid=root;Pwd=exemplo123;Port=3306" key="CS_EXEMPLO"/> 
        ///  </example>
        /// </summary>
        AppSettings = 1
    }
}