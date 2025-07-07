// Remove @ts-ignore and @ts-expect-error
// Define a generic type for message payloads, or use 'unknown' if the structure is not known

type MessageCallback = (msg: unknown) => void;

class ChatSocket {
  private url: string;
  private ws: WebSocket | null;
  private listeners: MessageCallback[];

  constructor(url: string) {
    this.url = url;
    this.ws = null;
    this.listeners = [];
  }

  connect() {
    this.ws = new WebSocket(this.url);
    this.ws.onmessage = (event: MessageEvent) => {
      const data = JSON.parse(event.data);
      this.listeners.forEach((cb) => cb(data));
    };
  }

  send(message: unknown) {
    if (this.ws && this.ws.readyState === WebSocket.OPEN) {
      this.ws.send(JSON.stringify(message));
    }
  }

  onMessage(cb: MessageCallback) {
    this.listeners.push(cb);
  }

  close() {
    if (this.ws) this.ws.close();
  }
}

let socketInstance: ChatSocket | null = null;

export function getChatSocket(url: string): ChatSocket {
  if (!socketInstance) {
    socketInstance = new ChatSocket(url);
    socketInstance.connect();
  }
  return socketInstance;
} 