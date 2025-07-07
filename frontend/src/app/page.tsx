"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { useUser } from "./user-context";

export default function Home() {
  const [name, setName] = useState("");
  const router = useRouter();
  const { setUserName } = useUser();

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (name.trim()) {
      fetch("http://localhost:5120/api/chat", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ name: name.trim() }),
      }).then(async (res) => {
        console.log(res);
        const data: { id: string; name: string } = await res.json();

        setUserName(data.name);
        router.push(`/chat/${data.id}`);
      });
    }
  };

  return (
    <div
      style={{
        minHeight: "100vh",
        display: "flex",
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
          padding: 32,
          minWidth: 320,
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <h2 style={{ marginBottom: 24, fontWeight: 600, fontSize: 24 }}>
          Bem-vindo ao Chat
        </h2>
        <form
          onSubmit={handleSubmit}
          style={{
            width: "100%",
            display: "flex",
            flexDirection: "column",
            gap: 16,
          }}
        >
          <label htmlFor="name" style={{ fontWeight: 500, marginBottom: 4 }}>
            Nome
          </label>
          <input
            id="name"
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
            placeholder="Digite seu nome"
            style={{
              padding: "12px 16px",
              fontSize: 16,
              borderRadius: 8,
              border: "1px solid #e0e0e0",
              background: "#f3f4f6",
              outline: "none",
              marginBottom: 8,
            }}
          />
          <button
            type="submit"
            style={{
              padding: "12px 0",
              fontSize: 16,
              borderRadius: 8,
              background: "#2563eb",
              color: "#fff",
              border: "none",
              fontWeight: 600,
              cursor: "pointer",
              transition: "background 0.2s",
            }}
          >
            Entrar
          </button>
        </form>
      </div>
    </div>
  );
}
