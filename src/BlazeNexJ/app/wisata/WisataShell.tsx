import Link from "next/link";

type WisataShellProps = {
  title: string;
  subtitle: string;
  children: React.ReactNode;
  actions?: React.ReactNode;
};

export default function WisataShell({ title, subtitle, children, actions }: WisataShellProps) {
  return (
    <main className="min-h-screen bg-[radial-gradient(circle_at_top,_rgba(59,130,246,0.18),_transparent_30%),linear-gradient(180deg,#fffdf7_0%,#eef5ff_100%)] px-4 py-8 text-stone-900 sm:px-6 lg:px-8">
      <div className="mx-auto flex max-w-6xl flex-col gap-6">
        <div className="flex flex-col gap-4 rounded-[2rem] border border-sky-100 bg-white/85 p-6 shadow-[0_24px_60px_rgba(34,89,155,0.12)] backdrop-blur sm:flex-row sm:items-end sm:justify-between">
          <div>
            <p className="text-xs font-semibold uppercase tracking-[0.3em] text-sky-600">TestWisata</p>
            <h1 className="mt-2 text-3xl font-semibold tracking-tight text-stone-950 sm:text-4xl">{title}</h1>
            <p className="mt-3 max-w-3xl text-sm leading-7 text-stone-600">{subtitle}</p>
          </div>

          <div className="flex flex-wrap gap-3">{actions}</div>
        </div>

        <div className="flex items-center justify-between rounded-[1.5rem] border border-white/70 bg-sky-950 px-5 py-4 text-sm text-sky-50 shadow-[0_18px_42px_rgba(8,47,73,0.18)]">
          <div className="flex flex-wrap gap-4">
            <Link href="/wisata" className="font-medium text-white transition hover:text-sky-200">List Wisata</Link>
            <Link href="/wisata/tambah" className="font-medium text-white transition hover:text-sky-200">Tambah Wisata</Link>
            <Link href="/wisata/laporan" className="font-medium text-white transition hover:text-sky-200">Report</Link>
          </div>
          <span className="text-xs uppercase tracking-[0.24em] text-sky-200">React 19 + .NET 8</span>
        </div>

        {children}
      </div>
    </main>
  );
}