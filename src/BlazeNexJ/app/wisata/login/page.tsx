"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { getWisataSession, loginWisata } from "../auth-client";

export default function WisataLoginPage() {
  const router = useRouter();
  const [username, setUsername] = useState("admin");
  const [password, setPassword] = useState("P@ssw0rd!123");
  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    if (getWisataSession()) {
      router.replace("/wisata");
    }
  }, [router]);

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setIsSubmitting(true);
    setError(null);

    try {
      await loginWisata(username, password);
      router.replace("/wisata");
    } catch (submitError) {
      setError(submitError instanceof Error ? submitError.message : "Login gagal.");
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <main className="flex min-h-screen items-center justify-center bg-[radial-gradient(circle_at_top,_rgba(59,130,246,0.18),_transparent_26%),linear-gradient(180deg,#fffef8_0%,#e8f0ff_100%)] px-4 py-10">
      <div className="grid w-full max-w-5xl gap-8 lg:grid-cols-[1.1fr_0.9fr]">
        <section className="rounded-[2rem] border border-white/70 bg-white/90 p-8 shadow-[0_25px_70px_rgba(26,71,120,0.18)] backdrop-blur">
          <p className="text-xs font-semibold uppercase tracking-[0.28em] text-sky-600">Soal WST System Developer</p>
          <h1 className="mt-3 text-4xl font-semibold tracking-tight text-stone-950">Login Wisata</h1>
          <p className="mt-4 max-w-xl text-sm leading-7 text-stone-600">
            Halaman ini memakai JWT untuk modul wisata, menyimpan access token di localStorage, dan refresh token dikelola lewat backend.
          </p>

          <form className="mt-10 max-w-md space-y-5" onSubmit={handleSubmit}>
            <label className="block">
              <span className="mb-2 block text-sm font-medium text-stone-700">Username</span>
              <input
                className="w-full rounded-2xl border border-sky-100 bg-sky-50/60 px-4 py-3 outline-none transition focus:border-sky-400 focus:bg-white"
                value={username}
                onChange={(event) => setUsername(event.target.value)}
                placeholder="Masukkan username"
              />
            </label>

            <label className="block">
              <span className="mb-2 block text-sm font-medium text-stone-700">Password</span>
              <input
                type="password"
                className="w-full rounded-2xl border border-sky-100 bg-sky-50/60 px-4 py-3 outline-none transition focus:border-sky-400 focus:bg-white"
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                placeholder="Masukkan password"
              />
            </label>

            {error ? <p className="rounded-2xl bg-rose-50 px-4 py-3 text-sm text-rose-700">{error}</p> : null}

            <button
              type="submit"
              disabled={isSubmitting}
              className="w-full rounded-2xl bg-sky-600 px-4 py-3 text-sm font-semibold text-white transition hover:bg-sky-700 disabled:cursor-not-allowed disabled:opacity-60"
            >
              {isSubmitting ? "Memproses..." : "Login"}
            </button>
          </form>
        </section>

        <aside className="rounded-[2rem] border border-sky-100 bg-sky-950 p-8 text-sky-50 shadow-[0_25px_70px_rgba(15,23,42,0.24)]">
          <p className="text-xs font-semibold uppercase tracking-[0.3em] text-sky-300">Demo Credential</p>
          <h2 className="mt-3 text-2xl font-semibold">Akses cepat untuk evaluasi</h2>
          <div className="mt-8 space-y-4 rounded-[1.5rem] bg-white/8 p-5">
            <div>
              <p className="text-xs uppercase tracking-[0.2em] text-sky-200">Username</p>
              <p className="mt-1 text-lg font-semibold text-white">admin</p>
            </div>
            <div>
              <p className="text-xs uppercase tracking-[0.2em] text-sky-200">Password</p>
              <p className="mt-1 text-lg font-semibold text-white">P@ssw0rd!123</p>
            </div>
          </div>
          <p className="mt-6 text-sm leading-7 text-sky-100/80">
            User sample lain yang juga tersedia dari seed adalah <strong>operator</strong> dan <strong>viewer</strong> dengan password yang sama.
          </p>
        </aside>
      </div>
    </main>
  );
}