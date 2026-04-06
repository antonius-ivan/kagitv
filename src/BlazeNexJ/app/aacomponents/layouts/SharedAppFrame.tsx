import type { ReactNode } from "react";

type SharedAppFrameProps = {
  header: ReactNode;
  intro?: ReactNode;
  children: ReactNode;
  contentClassName?: string;
};

export default function SharedAppFrame({
  header,
  intro,
  children,
  contentClassName
}: SharedAppFrameProps) {
  return (
    <main className="min-h-screen px-5 py-3 text-stone-900 md:px-8 lg:px-3">{/*px is for padding inline*/}
        <div className="mx-auto flex min-h-[calc(100vh-3rem)] w-full max-w-[1400px] flex-col rounded-[2rem] border border-white/70 bg-white/80 shadow-[0_25px_80px_rgba(73,37,0,0.14)] backdrop-blur">
        <header className="border-b border-stone-200/80 px-6 py-5 md:px-8">
          {header}
        </header>

        {intro ? <section className="border-b border-stone-200/80 px-6 py-8 md:px-8">{intro}</section> : null}

        <section className={`px-6 py-8 md:px-1 ${contentClassName ?? ""}`.trim()}>{children}</section>
      </div>
    </main>
  );
}