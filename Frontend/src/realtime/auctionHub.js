import * as signalR from '@microsoft/signalr'

class AuctionHubService {
  constructor() {
    this.connection = null;
    this.listeners = new Map();
    // Keep track of the actual start promise to avoid race conditions
    this.startPromise = null; 
  }

  async connect() {
    // 1. If connection exists AND is active/connecting, return the existing start promise
    if (this.connection && this.connection.state !== signalR.HubConnectionState.Disconnected) {
      return this.startPromise;
    }

    const hubUrl = `http://localhost:5265/auctionHub`;

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl)
      .withAutomaticReconnect()
      .build();

    this.connection.onclose(() => {
      this.startPromise = null;
    });

    // 2. Store the promise itself. Multiple calls will now wait for the SAME promise.
    this.startPromise = (async () => {
      try {
        await this.connection.start();
        // Re-bind listeners if connection was lost and recreated
        this.listeners.forEach((callbacks, eventName) => {
          callbacks.forEach(callback => this.connection.on(eventName, callback));
        });
        return this.connection;
      } catch (err) {
        this.connection = null;
        this.startPromise = null;
        throw err;
      }
    })();

    return this.startPromise;
  }

  async joinAuctionRoom(artworkId) {
    try {
      const connection = await this.connect();
      if (connection.state === signalR.HubConnectionState.Connected) {
        await connection.invoke('JoinAuctionRoom', artworkId);
      }
    } catch (err) {
      console.error("Failed to join room:", err);
    }
  }

  async leaveAuctionRoom(artworkId) {
    try {
      if (!this.connection) return;

      // Wait for any pending connection attempt to finish
      await this.startPromise; 

      if (this.connection.state === signalR.HubConnectionState.Connected) {
        await this.connection.invoke('LeaveAuctionRoom', artworkId);
      }
    } catch (err) {
      // This catches the error if the server is down or the method fails
      console.warn("Cleanly handled leave attempt:", err.message);
    }
  }

  subscribe(eventName, callback) {
    if (!this.listeners.has(eventName)) {
      this.listeners.set(eventName, new Set());
    }
    this.listeners.get(eventName).add(callback);

    // If already connected, attach immediately
    if (this.connection && this.connection.state === signalR.HubConnectionState.Connected) {
      this.connection.on(eventName, callback);
    }
  }

  unsubscribe(eventName, callback) {
    const callbacks = this.listeners.get(eventName);
    if (!callbacks) return;

    callbacks.delete(callback);
    if (this.connection) {
      this.connection.off(eventName, callback);
    }
  }
}

export const auctionHubService = new AuctionHubService();