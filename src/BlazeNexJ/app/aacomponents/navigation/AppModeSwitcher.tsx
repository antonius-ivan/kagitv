"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useState } from "react";

type AppModeSwitcherProps = {
  tone?: "light" | "dark";
};

const modes = [
  {
    id: "catalog",
    label: "Catalog",
    description: "Catalog browsing and sign-in access",
    href: "/salesitem"
  },
  {
    id: "dashboard",
    label: "Dashboard",
    description: "Dashboard menu and admin surfaces",
    href: "/dashboard"
  }
] as const;

function getCurrentMode(pathname: string) {
  return pathname.startsWith("/dashboard")
    ? modes[1]
    : modes[0];
}

export default function AppModeSwitcher({ tone = "light" }: AppModeSwitcherProps) {
  const pathname = usePathname();
  const [open, setOpen] = useState(false);
  const currentMode = getCurrentMode(pathname);

  const shellClassName =
    tone === "dark"
      ? "border-white/12 bg-white/6 text-slate-100"
      : "border-stone-200 bg-stone-50/90 text-stone-900";

  const mutedClassName = tone === "dark" ? "text-slate-400" : "text-stone-500";
  const panelClassName =
    tone === "dark"
      ? "border border-white/12 bg-slate-950/96 text-slate-100"
      : "border border-stone-200 bg-white text-stone-900";
  const itemClassName =
    tone === "dark"
      ? "border-white/10 hover:border-cyan-400/60 hover:bg-white/6"
      : "border-stone-200 hover:border-stone-900 hover:bg-stone-50";
  const activeClassName =
    tone === "dark"
      ? "border-cyan-400/70 bg-cyan-400/12 text-white"
      : "border-orange-300 bg-orange-50 text-stone-950";

  return (
    <div className="relative">
      <button
        aria-expanded={open}
        aria-haspopup="menu"
        className={`flex min-w-[13.5rem] items-center justify-between gap-4 rounded-[1.4rem] border px-4 py-3 text-left transition ${shellClassName}`}
        onClick={() => setOpen((value) => !value)}
        type="button"
      >
        <span className="flex min-w-0 flex-col">
          <span className={`text-[0.65rem] font-semibold uppercase tracking-[0.3em] ${mutedClassName}`}>
            Mode
          </span>
          <span className="mt-1 text-lg font-semibold tracking-tight">{currentMode.label}</span>
          {/*<span className={`truncate text-xs ${mutedClassName}`}>{currentMode.description}</span>*/}
        </span>
        <span className={`text-xs uppercase tracking-[0.34em] ${mutedClassName}`}>
          {open ? "|" : "-"}
        </span>
      </button>

      {open ? (
        <div
          className={`absolute right-0 top-[calc(100%+0.7rem)] z-20 w-80 rounded-[1.5rem] p-2.5 shadow-[0_16px_30px_rgba(24,24,27,0.12)] ${panelClassName}`}
        >
          <div className="space-y-2">
            {modes.map((mode) => {
              const isCurrentMode = mode.id === currentMode.id;

              return (
                <Link
                  key={mode.id}
                  className={`block rounded-[1.05rem] border px-4 py-3 transition ${isCurrentMode ? activeClassName : itemClassName}`}
                  href={mode.href}
                  onClick={() => setOpen(false)}
                >
                  <div className="flex items-start justify-between gap-4">
                    <div>
                      <p className="text-base font-semibold tracking-tight">{mode.label}</p>
                    </div>
                    <span className="text-xs font-semibold uppercase tracking-[0.24em]">
                      {isCurrentMode ? "Live" : "Go"}
                    </span>
                  </div>
                </Link>
              );
            })}
          </div>
        </div>
      ) : null}
    </div>
  );
}