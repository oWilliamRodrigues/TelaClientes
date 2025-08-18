Europa.Components.SignalR = function () {
    this.chatProxy = undefined;
    this.connectionId = undefined;
};

Europa.Components.SignalR.prototype = {
    StartConnection: function () {
        var self = this;

        self.chatProxy = $.connection.HubAtendimento;

        $.connection.hub.start().done(function () {
            self.connectionId = $.connection.hub.id;
        }).fail(function (err) {
            console.log(err);
        });
        return self;
    },

    RegisterFunctionsProxy: function () {
        var self = this;
        self.chatProxy.client.notificar = function (msg) {
            console.log(msg);
            new Europa.Notification()
                .Content(msg.Mensagem)
                .WithIcon(msg.Icone)
                .WithDismiss(msg.DeveDispensarAutomaticamente)
                .WithDismissDelay(msg.TempoParaDispensar)
                .Show();
        };
    },

    AcceptAction: function (context, url) {
        console.log('AcceptAction')
        Spinner.Show();
        window.location.href = url;
        context.RejectAction(context);
    },

    RejectAction: function (context) {
        console.log('RejectAction')
        context.chatProxy.server.RejectRequest(context.connectionId, null);
    },

    SendMessage: function (nameMethod, connectionId, userName, message) {
        console.log('SendMessage')
        this.chatProxy.invoke(nameMethod, connectionId, userName, message);
    }
}

Europa.Components.SignalR.GetInstance = function () {
    var chat = new Europa.Components.SignalR();
    chat.StartConnection();
    chat.RegisterFunctionsProxy();
    return chat;
}