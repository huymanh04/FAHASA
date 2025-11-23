window.adminChat = {
    connection: null,
    keyListeners: {},
    init: function (dotNetRef) {
        if (this.connection) {
            console.log('AdminChat: Connection already exists, reusing');
            return;
        }
        console.log('AdminChat: Initializing new connection');
        this.connection = new signalR.HubConnectionBuilder().withUrl('/chathub').build();
        this.connection.on('ReceiveMessage', function (fromUserId, user, message, isAdminMsg, time) {
            dotNetRef.invokeMethodAsync('ReceiveFromHub', fromUserId, user, message, isAdminMsg, time);
        });
        this.connection.start()
            .then(() => console.log('AdminChat: Connected'))
            .catch(function (err) { console.error('AdminChat error:', err.toString()); });
    },
    setupAdminKeySend: function (dotNetRef, textareaId) {
        var el = document.getElementById(textareaId);
        if (!el) {
            console.warn('AdminChat: Element not found:', textareaId);
            return;
        }
        var handler = function (e) {
            if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                dotNetRef.invokeMethodAsync('OnAdminEnterPressed');
            }
        };
        el.addEventListener('keydown', handler);
        this.keyListeners[textareaId] = handler;
    },
    stopAdminKeySend: function (textareaId) {
        var el = document.getElementById(textareaId);
        var h = this.keyListeners[textareaId];
        if (el && h) {
            el.removeEventListener('keydown', h);
        }
        delete this.keyListeners[textareaId];
    },
    stop: function () {
        if (this.connection) {
            console.log('AdminChat: Stopping connection');
            this.connection.stop();
            this.connection = null;
        }
    }
};

window.initAdminSignalR = function (dotNetRef) { 
    window.adminChat.init(dotNetRef); 
};

window.stopAdminSignalR = function () { 
    window.adminChat.stop(); 
};
