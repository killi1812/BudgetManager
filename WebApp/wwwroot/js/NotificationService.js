//TODO: Facade Wrapper
export class NotificationService {
    static instance
    constructor(hubUrl, notificationElementId) {
        if (this.instance) {
            return this.instance
        }

        this.hubUrl = hubUrl;
        this.notificationElementId = notificationElementId;
        this.getNotificationsCount().then(data => this.notificationCount = data || 0)
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(this.hubUrl)
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.init();
        
        this.instance = this
    }

    async init() {
        try {
            await this.connection.start();
            console.log("SignalR Connected.");
        } catch (err) {
            console.error(err);
            setTimeout(() => this.init(), 5000);
        }

        this.connection.on("ReceiveNotification", this.receiveNotification.bind(this));

        this.updateNotificationDisplay();
    }

    async reconnect() {
        if (this.connection.state === signalR.HubConnectionState.Disconnected) {
            await this.init();
        }
    }

    receiveNotification(message) {
        this.notificationCount++;
        sessionStorage.setItem('notificationCount', this.notificationCount);
        this.updateNotificationDisplay();
    }

    updateNotificationDisplay() {
        const notificationElement = document.getElementById(this.notificationElementId);
        if (notificationElement) {
            notificationElement.textContent = this.notificationCount;
        }
    }

    getNotificationsCount(){
        return fetch("/Notifications/GetUnreadNotifications")
            .then( r => r.json())
            .then(data => data)
    }
}