"use client";

import { useEffect, useRef, useState, FormEvent, useCallback } from "react";
import { getChatSocket } from "../../lib/socket";
import { useUser } from "../../user-context";
import { useParams, useRouter } from "next/navigation";

const WS_URL = "ws://localhost:5120/api/websocket/ws/";

type Message = {
  id: string;
  chatId: string;
  sender: string;
  content: string;
  timestamp: string;
};


export default function ChatPage() {
  const { userName, setUserName } = useUser();
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState("");
  const socketRef = useRef<ReturnType<typeof getChatSocket> | null>(null);
  const messagesEndRef = useRef<HTMLDivElement | null>(null);
  const params = useParams();
  const slug = params.slug as string;
  const wsConnected = useRef(false);
  const router = useRouter();

  const handleMessage = useCallback((response: any | unknown) => {
    const msg: Message = {
      id: response.Data.Id,
      chatId: response.Data.Metadata.ChatId,
      sender: response.Data.Sender,
      content: response.Data.Content,
      timestamp: response.Timestamp,
    } as Message;

    setMessages((prev) => [...prev, msg]);
  }, [setMessages]);

  const handleWs = useCallback(() => {
    const socket = getChatSocket(`${WS_URL}notifications/${slug}`);
    socketRef.current = socket;

    socket.onMessage(handleMessage);
    return () => {
      socket.close();
    };
  }, [slug, handleMessage]);

  useEffect(() => {
    fetch(`http://localhost:5120/api/chat/${slug}/messages`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    }).then(async (res) => {
      console.log(res);
      const data: { messages: Message[]; chatId: string; name: string } =
        await res.json();

      setMessages(data.messages);
      setUserName(data.name);
      if (!wsConnected.current) {
        handleWs();
        wsConnected.current = true;
      }
    });
  }, [slug, handleWs, setUserName]);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth", block: "start" });
  }, [messages]);


  const handleSend = (e: FormEvent) => {
    e.preventDefault();

    if (input.trim()) {
      setMessages((prev) => [
        ...prev,
        {
          id: crypto.randomUUID(),
          chatId: slug,
          sender: userName,
          content: input.trim(),
          timestamp: new Date().toISOString(),
        } as Message,
       
      ]);
      fetch(`http://localhost:5120/api/chat/${slug}/message`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          sender: userName,
          content: input.trim(),
        }),
      }).then(async (res) => {
        const data: Message = await res.json();
        console.log(data);
        if (input.includes("sair")) {
          setInput("");
          setTimeout(() => {
            router.push("/");
          }, 3 * 1000);
          return;
        }
        setInput("");
      });
    }
  };

  return (
    <div
      style={{
        minHeight: "100vh",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        background: "#f5f6fa",
      }}
    >
      <div
        style={{
          background: "#fff",
          borderRadius: 16,
          boxShadow: "0 4px 24px rgba(0,0,0,0.08)",
          padding: 24,
          width: 400,
          maxWidth: "90vw",
          display: "flex",
          flexDirection: "column",
          height: 500,
        }}
      >
        <div
          style={{
            flex: 1,
            overflowY: "auto",
            marginBottom: 16,
            paddingRight: 8,
            display: "flex",
            flexDirection: "column",
            justifyContent: "flex-start",
          }}
        >
          {messages.map((msg: Message, i: number) => (
            <div key={i} style={{ marginBottom: 8, wordBreak: "break-word" }}>
              <b>{msg.sender}:</b> {msg.content}
            </div>
          ))}
          <div ref={messagesEndRef} />
        </div>
        <form onSubmit={handleSend} style={{ display: "flex", gap: 8 }}>
          <input
            type="text"
            value={input}
            onChange={(e) => setInput(e.target.value)}
            placeholder="Digite sua mensagem..."
            style={{
              flex: 1,
              padding: 10,
              borderRadius: 8,
              border: "1px solid #e0e0e0",
              background: "#f3f4f6",
              fontSize: 16,
            }}
          />
          <button
            type="submit"
            style={{
              padding: "10px 20px",
              borderRadius: 8,
              background: "#2563eb",
              color: "#fff",
              border: "none",
              fontWeight: 600,
              fontSize: 16,
              cursor: "pointer",
            }}
          >
            Enviar
          </button>
        </form>
      </div>
    </div>
  );
}
