import Link from "next/link";
import { getSession } from "../server/auth";

type UserMenuProps = {
  tone?: "light" | "dark";
};

function UserIcon() {
  return (
    <svg aria-hidden="true" className="h-4 w-4" fill="none" viewBox="0 0 24 24">
      <circle cx="12" cy="8" r="3.25" stroke="currentColor" strokeWidth="1.6" />
      <path d="M5 19c1.7-3 4.2-4.5 7-4.5s5.3 1.5 7 4.5" stroke="currentColor" strokeLinecap="round" strokeWidth="1.6" />
    </svg>
  );
}

export default async function UserMenu({ tone = "light" }: UserMenuProps) {
  const session = await getSession();
  const shellClassName =
    tone === "dark"
      ? "border-white/10 bg-white/5 text-slate-100 hover:border-cyan-400/70"
      : "border-stone-200 bg-white text-stone-700 hover:border-stone-900 hover:text-stone-950";
  const panelClassName = tone === "dark" ? "border-white/10 bg-slate-950 text-slate-100" : "border-stone-200 bg-white text-stone-900";
  const mutedClassName = tone === "dark" ? "text-slate-400" : "text-stone-500";

  if (!session) {
    return (
      <a
        className={`inline-flex items-center gap-2 rounded-[1rem] border px-4 py-2.5 text-sm font-medium transition ${shellClassName}`}
        href="/auth/login"
      >
        <UserIcon />
        <span>Sign In</span>
      </a>
    );
  }

  return (
    <details className="relative">
      <summary className={`flex list-none items-center gap-3 rounded-[1rem] border px-4 py-2.5 text-sm font-medium transition ${shellClassName}`}>
        <UserIcon />
        <span className="max-w-36 truncate">{session.user.name}</span>
      </summary>
      <div className={`absolute right-0 top-[calc(100%+0.7rem)] z-20 w-64 rounded-[1.35rem] border p-3 shadow-[0_16px_30px_rgba(24,24,27,0.12)] ${panelClassName}`}>
        <p className="px-3 text-sm font-semibold tracking-tight">{session.user.name}</p>
        <p className={`px-3 pt-1 text-xs uppercase tracking-[0.24em] ${mutedClassName}`}>
          Account Menu
        </p>
        {session.user.roles && session.user.roles.length > 0 ? (
          <div className="mt-3 flex flex-wrap gap-2 px-3">
            {session.user.roles.map((role) => (
              <span key={role} className="rounded-full border border-orange-200 bg-orange-50 px-2.5 py-1 text-[11px] font-semibold uppercase tracking-[0.18em] text-orange-700">
                {role}
              </span>
            ))}
          </div>
        ) : null}
        <div className="mt-3 space-y-2">
          <a
            className="block rounded-[1.1rem] border border-transparent px-3 py-3 text-sm transition hover:border-stone-200 hover:bg-stone-50 hover:text-stone-950"
            href="/auth/logout"
          >
            Log out
          </a>
        </div>
      </div>
    </details>
  );
}